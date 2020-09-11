using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    internal struct VolatileBool
    {
        public volatile bool m_value;

        public VolatileBool(bool value)
        {
            this.m_value = value;
        }
    }

    [DebuggerDisplay("Count = {Count}")]
    [HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
    public class ThreadSafeList<T> : IEnumerable<T>, ICollection<T>, IDisposable
    {

        private readonly List<T> _Items;
        private readonly ReaderWriterLockSlim _Lock;

        public T this[int index]
        {
            get
            {
                _Lock.EnterReadLock();
                try
                {
                    return _Items[index];
                }
                finally
                {
                    _Lock.ExitReadLock();
                }
            }
            set
            {
                _Lock.EnterWriteLock();
                try
                {
                    _Items[index] = value;
                }
                finally
                {
                    _Lock.ExitWriteLock();
                }
            }
        }

        public int Count
        {
            get
            {
                _Lock.EnterReadLock();
                try
                {
                    return _Items.Count;
                }
                finally
                {
                    _Lock.ExitReadLock();
                }
            }
        }

        public bool IsReadOnly => false;

        /// <summary>
        /// Initializes a new instance of the ThreadSafeList class.
        /// </summary>
        public ThreadSafeList()
        {
            _Items = new List<T>();
            _Lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        }

        public ThreadSafeList(IEnumerable<T> items)
        {
            _Items = new List<T>(items);
            _Lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        }

        ~ThreadSafeList()
        {
            if (_Lock != null)
                _Lock.Dispose();
        }

        public void Add(T item)
        {
            _Lock.EnterWriteLock();
            try
            {
                _Items.Add(item);
            }
            finally
            {
                _Lock.ExitWriteLock();
            }
        }

        public bool Remove(T item)
        {
            _Lock.EnterWriteLock();
            try
            {
                return _Items.Remove(item);
            }
            finally
            {
                _Lock.ExitWriteLock();
            }
        }

        public int IndexOf(T item)
        {
            _Lock.EnterReadLock();
            try
            {
                return _Items.IndexOf(item);
            }
            finally
            {
                _Lock.ExitReadLock();
            }
        }

        public void Clear()
        {
            _Lock.EnterWriteLock();
            try
            {
                _Items.Clear();
            }
            finally
            {
                _Lock.ExitWriteLock();
            }
        }

        public bool Contains(T item)
        {
            _Lock.EnterReadLock();
            try
            {
                return _Items.Contains(item);
            }
            finally
            {
                _Lock.ExitReadLock();
            }

        }

        public T Find(Predicate<T> match)
        {
            _Lock.EnterReadLock();
            try
            {
                return _Items.Find(match);
            }
            finally
            {
                _Lock.ExitReadLock();
            }
        }

        public int CountTS(Func<T, bool> predicate)
        {
            _Lock.EnterReadLock();
            try
            {
                return _Items.Count(predicate);
            }
            finally
            {
                _Lock.ExitReadLock();
            }
        }

        public void Dispose()
        {
            if (_Lock != null)
                _Lock.Dispose();
            GC.SuppressFinalize(this);
        }

        public IEnumerator<T> GetEnumerator()
        {
            _Lock.EnterReadLock();
            try
            {
                return _Items.GetEnumerator();
            }
            finally
            {
                _Lock.ExitReadLock();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            _Lock.EnterReadLock();
            try
            {
                return _Items.GetEnumerator();
            }
            finally
            {
                _Lock.ExitReadLock();
            }
        }

        public System.Collections.ObjectModel.ReadOnlyCollection<T> AsReadOnly()
        {
            _Lock.EnterReadLock();
            List<T> cloneList = null;
            try
            {
                cloneList = new List<T>(_Items);
            }
            finally
            {
                _Lock.ExitReadLock();
            }
            return cloneList.AsReadOnly();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _Lock.EnterReadLock();
            try
            {
                _Items.CopyTo(array, arrayIndex);
            }
            finally
            {
                _Lock.ExitReadLock();
            }
        }

        #region LinQ & Helper functions

        public void ForEach(Action<T> predicate)
        {
            foreach (var item in this)
            {
                predicate(item);
            }
        }

        public void AddRange(IEnumerable<T> items)
        {
            _Lock.EnterWriteLock();
            try
            {
                _Items.AddRange(items);
            }
            finally
            {
                _Lock.ExitWriteLock();
            }
        }

        public void RemoveAll(Predicate<T> match)
        {
            _Lock.EnterWriteLock();
            try
            {
                _Items.RemoveAll(match);
            }
            finally
            {
                _Lock.ExitWriteLock();
            }
        }

        public IEnumerable<T> Where(Func<T, bool> predicate)
        {
            _Lock.EnterWriteLock();
            try
            {
                foreach (var item in _Items.Where(predicate))
                    yield return item;
            }
            finally
            {
                _Lock.ExitWriteLock();
            }
        }

        public IEnumerable<TResult> OfType<TResult>() where TResult : T
        {
            _Lock.EnterWriteLock();
            try
            {
                foreach (var item in _Items.OfType<TResult>())
                    yield return item;
            }
            finally
            {
                _Lock.ExitWriteLock();
            }
        }

        #endregion
        
    }

}
