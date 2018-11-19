using Model.Manage_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebManager.Model
{
    [Serializable]
    public class DepartmentList_Model
    {
        public string DepartmentName { get; set; }
        public int UpperID { get; set; }

        public List<Department_Model> Data { get; set; }

        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public int RowsCount { get; set; }
        public int PageCount { get; set; }

        public List<Department_Model> UpperList { get; set; }

    }

    [Serializable]
    public class DepartmentDetail_Model {

        public int DepartmentID { get; set; }

        public Department_Model Data { get; set; }
        public List<Department_Model> UpperList { get; set; }
    }

}