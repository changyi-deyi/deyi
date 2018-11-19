using Model.Manage_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebManager.Model
{
    [Serializable]
    public class AdvisoryList_Model
    {
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public int RowsCount { get; set; }
        public int PageCount { get; set; }

        public List<Advisory_Model> Data { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public int IsDone { get; set; }
    }

    [Serializable]
    public class AdvisoryDetail_Model
    {
        public List<Advisory_Model> Data { get; set; }
        public int ID { get; set; }
    }

}