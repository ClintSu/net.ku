using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Communication
{
    public class WebChannel : IChannel
    {
        public WebChannel()
        {
            this.Name = "Web";
            this.Priority = 0;
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
