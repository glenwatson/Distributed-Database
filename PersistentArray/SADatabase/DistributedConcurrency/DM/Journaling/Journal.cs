﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DistributedConcurrency.DM.Journaling
{
    class Journal<T>
    {
        private IJournalStorage<T> _journalStorage;
	
	    public Journal(Uri journalLocation)
        {
		    _journalStorage = new PersistantFolderQueue<T>(journalLocation.AbsolutePath);
        }
	
	    public void AddChange(T change)
        {
            Console.WriteLine("Journaling " + change.GetHashCode());
            _journalStorage.Push(change);
        }
	
	    public void RemoveChange()
        {
		    _journalStorage.Pop();
        }

        public void Recover()
        {
            _journalStorage.Recover();
        }

        internal void RemoveAll()
        {
            Console.WriteLine("Un-Journaling all");
            _journalStorage.RemoveAll();
        }
    }
}
