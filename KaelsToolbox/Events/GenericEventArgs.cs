using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KaelsToolbox.Events
{
    public class GenericEventArgs<T> : EventArgs
    {
        public T Data { get; }

        public GenericEventArgs(T data)
        {
            Data = data;
        }
    }
}
