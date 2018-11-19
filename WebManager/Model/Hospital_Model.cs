using Model.Manage_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebManager.Model
{
    [Serializable]
    public class HospitalList_Model
    {
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public int RowsCount { get; set; }
        public int PageCount { get; set; }

        public List<Hospital_Model> Data { get; set; }
        public string HospitalName { get; set; }
    }


    [Serializable]
    public class HospitalDetail_Model
    {
        public int HospitalID { get; set; }

        public Hospital_Model Data { get; set; }
    }

}