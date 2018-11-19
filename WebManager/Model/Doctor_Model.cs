using Model.Manage_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebManager.Model
{
    [Serializable]
    public class DoctorList_Model
    {
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public int RowsCount { get; set; }
        public int PageCount { get; set; }

        public List<Doctor_Model> Data { get; set; }
        public string DoctorName { get; set; }
        public int HostpitalID { get; set; }
        public int DepartmentID { get; set; }

        public List<Hospital_Model> listHospital { get; set; }

        public List<Department_Model> listDepartment { get; set; }
    }

    [Serializable]
    public class DoctorDetail_Model
    {
        public string DoctorCode { get; set; }

        public Doctor_Model Data { get; set; }

        public List<Hospital_Model> listHospital { get; set; }

        public List<Department_Model> listDepartment { get; set; }
        public List<Title_Model> listTitle { get; set; }

        public List<Tag_Model> listTag { get; set; }
    }

}