using Model.Manage_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebManager.Model
{
    [Serializable]
    public class StaffList_Model
    {
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public int RowsCount { get; set; }
        public int PageCount { get; set; }

        public List<Staff_Model> Data { get; set; }
        public string StaffName { get; set; }
        public int Role { get; set; }
        
    }

    [Serializable]
    public class StaffDetail_Model
    {
        public string StaffCode { get; set; }

        public Staff_Model Data { get; set; }

    }
}