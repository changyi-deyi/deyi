using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entity
{
    [Serializable]
    [DataContract]
    public abstract class BaseResult
    {
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}
