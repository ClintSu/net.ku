using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Communication
{
    public class LocalChannel:IChannel
    {
        public LocalChannel()
        {
            this.Name = "Local";
            this.Priority = 1;
        }
        public string Name { get; private set; }
        public int Priority { get; private set; }

        public T Read<T>()
        {
            throw new NotImplementedException();
        }

        public T Write<T>()
        {
            throw new NotImplementedException();
        }
    }
}
