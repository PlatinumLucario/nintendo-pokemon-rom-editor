namespace DSDecmp.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class SimpleReversedPrioQueue<TPrio, TValue>
    {
        private int itemCount;
        private SortedDictionary<TPrio, LinkedList<TValue>> items;

        public SimpleReversedPrioQueue()
        {
              items = new SortedDictionary<TPrio, LinkedList<TValue>>();
              itemCount = 0;
        }

        public TValue Dequeue(out TPrio priority)
        {
            if (  itemCount == 0)
            {
                throw new IndexOutOfRangeException();
            }
            LinkedList<TValue> list = null;
            priority = default(TPrio);
            foreach (KeyValuePair<TPrio, LinkedList<TValue>> pair in   items)
            {
                list = pair.Value;
                priority = pair.Key;
                break;
            }
            TValue local = list.First.Value;
            list.RemoveFirst();
            if (list.Count == 0)
            {
                  items.Remove(priority);
            }
              itemCount--;
            return local;
        }

        public void Enqueue(TPrio priority, TValue value)
        {
            if (!  items.ContainsKey(priority))
            {
                  items.Add(priority, new LinkedList<TValue>());
            }
              items[priority].AddLast(value);
              itemCount++;
        }

        public TValue Peek(out TPrio priority)
        {
            if (  itemCount == 0)
            {
                throw new IndexOutOfRangeException();
            }
            foreach (KeyValuePair<TPrio, LinkedList<TValue>> pair in   items)
            {
                priority = pair.Key;
                return pair.Value.First.Value;
            }
            throw new IndexOutOfRangeException();
        }

        public int Count
        {
            get
            {
                return   itemCount;
            }
        }
    }
}

