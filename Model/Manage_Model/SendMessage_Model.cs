using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Manage_Model
{
    [Serializable]
    public class SendCustomer_Model
    {
        public string customer { get; set; }
        public string hospital { get; set; }
        public string department { get; set; }
        public string doctor { get; set; }
        public string datetime { get; set; }
        public string address { get; set; }
        public string code { get; set; }
    }

    [Serializable]
    public class SendDoctor_Model
    {
        public string doctor { get; set; }
        public string customer { get; set; }
        public string datetime { get; set; }
        public string address { get; set; }
        public string code { get; set; }
    }
}
