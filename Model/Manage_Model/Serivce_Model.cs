using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Manage_Model
{
    [Serializable]
    public class Service_Model
    {
        public string ServiceCode { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Introduct { get; set; }
        public decimal OriginPrice { get; set; }
        public decimal PromPrice { get; set; }
        public decimal ExchangePrice { get; set; }
        public string ListImageURL { get; set; }
        public int Status { get; set; }
        public int Creator { get; set; }

        public DateTime CreatetTime { get; set; }

        public int? Updater { get; set; }

        public DateTime? UpdateTime { get; set; }
        public int Sort { get; set; }
        public int IsVisible { get; set; }
    }

    [Serializable]
    public class ServiceImg_Model {

        public int ID { get; set; }

        public string ServiceCode { get; set; }

        public string FileName { get; set; }

        public int Status { get; set; }
        public int Creator { get; set; }

        public DateTime CreatetTime { get; set; }

        public int? Updater { get; set; }

        public DateTime? UpdateTime { get; set; }
    }

}
