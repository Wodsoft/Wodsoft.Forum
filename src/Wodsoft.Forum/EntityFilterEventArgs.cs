using System;
using System.Collections.Generic;
using System.Text;

namespace Wodsoft.Forum
{
    public class EntityFilterEventArgs<T> : EventArgs
    {
        public EntityFilterEventArgs(T item)
        {
            Item = item;
        }

        public T Item { get; private set; }
    }
}
