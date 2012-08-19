using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace DistributedConcurrency.DM.Journaling
{
    class PersistantFolderQueue<T> : IJournalStorage<T>
    {
        private String JOURNAL_FORMAT = "yyyy-MM-dd H-mm-ss-fffffZzz"; //http://msdn.microsoft.com/en-us/library/az4se3k1.aspx
	    private String _directory;
	    private Queue<FileStream> _queue;

        public PersistantFolderQueue(String journalDirectory)
        {
            _directory = journalDirectory; //let directory be journalDirectory
            Directory.CreateDirectory(journalDirectory);
		    _queue = new Queue<FileStream>(); //init queue
		    Recover(); //call recover()
        }

        //~PersistantFolderQueue()
        //{
        //    RemoveAll();
        //}

        //private FileStream GetHead()
        //{
        //    FileStream stream = _queue.Peek();
        //    return stream;
        //}

        public void Recover()
        {
            RefreshQueueWithFiles();
        }
        private void RefreshQueueWithFiles()
        {
            IEnumerable<String> journalFiles = GetAllJournalFiles();
            FillUpQueueWith(journalFiles);
        }
        private IEnumerable<string> GetAllJournalFiles()
        {
            String[] allFiles = Directory.GetFiles(_directory); //let allFiles be all the files inside of directory
            var fileNames = allFiles.Select(fullPath => Path.GetFileName(fullPath));

            //let journalFiles be allFiles filtered based on JOURNAL_FORMAT
            Dictionary<DateTime, String> fileNameMap = new Dictionary<DateTime, string>();
            foreach (String fileName in fileNames)
            {
                DateTime dt;
                if(DateTime.TryParse(fileName, out dt))
                {
                    fileNameMap.Add(dt, fileName);
                }
            }

            var orderedMapping = fileNameMap.OrderBy(keyValuePair => keyValuePair.Key); //sort journalFiles a-z
            return orderedMapping.Select(keyValuePair => keyValuePair.Value);
        }
        private void FillUpQueueWith(IEnumerable<string> journalFiles)
        {
            _queue.Clear(); //clear the queue
		    foreach(String fileName in journalFiles) //for each fileName in journalFiles,
            {
                FileStream file = OpenJournalFile(fileName); //let file be the result of createJournalFile() passing in fileName
                _queue.Enqueue(file); //push() file on the queue
            }
        }

        public void Push(T change)
        {
            FileStream file = CreateJournalFile(); //let file be the result of createJournalFile()
            WriteChangeToStream(file, change); //call writeChangeToStream() passing in file & change
            _queue.Enqueue(file); //push() file on the queue
        }
        private void WriteChangeToStream(Stream stream, T change)
        {
            Serializer<T>.Serialize(change, stream); //call _serializer's serialize() passing in stream & change
            stream.Flush(); //call stream's flush()
            stream.Position = 0;//let stream's position be 0
            Debug.Assert(stream.CanWrite);
        }

        public T Peek()
        {
            FileStream file = _queue.Peek(); //let file be the call to queue's peek()
            T journaledChange = ReadChangeFromStream(file); //let journaledChange be the result of the call to readChangeFromStream() passing in file
            return journaledChange; //return journaledChange
        }
        private T ReadChangeFromStream(Stream stream)
        {
            T change = Serializer<T>.Deserialize(stream); //call _serializer's deserialze() passing in stream //let change be that result
            stream.Position = 0; //let stream's position be 0
            Debug.Assert(stream.CanRead);
            return change; //return change
        }

        public void Pop()
        {
            FileStream file = _queue.Dequeue(); //let file be the call to queue's peek() //pop() queue
            //Console.WriteLine("Closing: " + file.Name.Substring(34, 5) + " on thread: " + Thread.CurrentThread.ManagedThreadId);
            file.Flush(); //flush() file //just to be sure
            file.Close(); //close() file (?)
            File.Delete(file.Name); //delete file
        }

        public void RemoveAll()
        {
            DeleteEntireQueue();

            RefreshQueueWithFiles();

            DeleteEntireQueue();
        }
        private void DeleteEntireQueue()
        {
            while (_queue.Count > 0)
            {
                Pop();
            }
        }

        private FileStream OpenJournalFile(String fileName)
        {
            return new FileStream(_directory + fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }
        private FileStream CreateJournalFile()
        {
            string currentTimestamp = System.DateTime.Now.ToString(JOURNAL_FORMAT); //let currentTimestamp be DateTime's now, toString'd passing in JOURNAL_FORMAT
            FileStream file = null;
            while (file == null)
            {
                try
                {
                    file = new FileStream(_directory + currentTimestamp, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
                }
                catch (IOException ioEx)
                {
                    if (!ioEx.Message.Contains("it is being used by another process"))
                        throw;
                    Thread.Sleep(0);
                }
            }
            //Console.WriteLine("Creating: " + file.Name.Substring(34, 5) + " on thread: " + Thread.CurrentThread.ManagedThreadId);
            return file;
        }
    }
}