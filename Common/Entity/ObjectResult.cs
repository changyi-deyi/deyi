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
    public class ObjectResult<T> : BaseResult
    {
        /// <summary>
        /// 记录集
        /// </summary>
        [DataMember]
        public T Data { get; set; }
    }
}
