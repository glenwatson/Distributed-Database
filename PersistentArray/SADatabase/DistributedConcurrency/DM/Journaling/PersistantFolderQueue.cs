using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DistributedConcurrency.DM.Journaling
{
    class PersistantFolderQueue<T> : IJournalStorage<T>
    {
        private String JOURNAL_FORMAT = "s"; //http://msdn.microsoft.com/en-us/library/az4se3k1.aspx
	    private String _directory;
	    private Queue<FileStream> _queue;

        public PersistantFolderQueue(String journalDirectory)
        {
            _directory = journalDirectory; //let directory be journalDirectory
		    _queue = new Queue<FileStream>(); //init queue
		    Recover(); //call recover()
        }

        private FileStream GetHead()
        {
            FileStream stream = _queue.Peek();
            return stream;
        }

        public void Recover()
        {
            RefreshQueueWithFiles();
        }

        private void RefreshQueueWithFiles()
        {
            List<String> journalFiles = GetAllJournalFiles();
            FillUpQueueWith(journalFiles);
        }

        private List<string> GetAllJournalFiles()
        {
            String[] allFiles = Directory.GetFiles(_directory); //let allFiles be all the files inside of directory
            //let journalFiles be allFiles filtered based on JOURNAL_FORMAT
            //TODO: I am converting each string twice
            var journalFiles = allFiles.Where(s => { DateTime dt; return DateTime.TryParse(s, out dt); });

            journalFiles.OrderBy(s => DateTime.Parse(s)); //sort journalFiles a-z
		    //return journalFiles
            return journalFiles.ToList();
        }

        private void FillUpQueueWith(List<string> journalFiles)
        {
            _queue.Clear(); //clear the queue
		    foreach(String fileName in journalFiles) //for each fileName in journalFiles,
            {
                FileStream file = CreateJournalFile(fileName); //let file be the result of createJournalFile() passing in fileName
                _queue.Enqueue(file); //push() file on the queue
            }
        }

        public void Push(T change)
        {
            string currentTimestamp = DateTime.Now.ToString(JOURNAL_FORMAT); //let currentTimestamp be DateTime's now, toString'd passing in JOURNAL_FORMAT
            FileStream file = CreateJournalFile(currentTimestamp); //let file be the result of createJournalFile() passing in currentTimestamp
            WriteChangeToStream(file, change); //call writeChangeToStream() passing in file & change
            _queue.Enqueue(file); //add file to queue
        }

        private FileStream CreateJournalFile(String fileName)
        {
            //return a new FileStream with no sharing & r/w permissions passing in fileName
            return new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
        }

        public T Peek() //could cache the file for use in pop() since 99% of the time peek() will be called before pop()
        {
            FileStream file = _queue.Dequeue(); //let file be the call to queue's pop()
            T journaledChange = ReadChangeFromStream(file); //let journaledChange be the result of the call to readChangeFromStream() passing in file
            return journaledChange; //return journaledChange
        }

        public void Pop()
        {
            FileStream file = _queue.Dequeue(); //let file be the call to queue's peek() //pop() queue
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

        private void WriteChangeToStream(Stream stream, T change)
        {
            Serializer<T>.Serialize(change, stream); //call _serializer's serialize() passing in stream & change
            stream.Flush(); //call stream's flush()
            stream.Position = 0;//let stream's position be 0
        }

        private T ReadChangeFromStream(Stream stream)
        {
            T change = Serializer<T>.Deserialize(stream); //call _serializer's deserialze() passing in stream //let change be that result
            stream.Position = 0; //let stream's position be 0
            return change; //return change
        }
    }
}
