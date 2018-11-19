using Model.Manage_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebManager.Model
{
    [Serializable]
    public class ServiceList_Model
    {
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public int RowsCount { get; set; }
        public int PageCount { get; set; }

        public List<Service_Model> Data { get; set; }
        public int Status { get; set; }

    }

    [Serializable]
    public class ServiceDetail_Model
    {

        public Service_Model Data { get; set; }

        public string ServiceCode { get; set; }

    }

    [Serializable]
    public class ServiceDoctorList_Model
    {

        public List<ServiceDoctor_Model> Data { get; set; }
        public string ServiceCode { get; set; }
        public string Name { get; set; }
        public int IsBargain { get; set; }

    }

    [Serializable]
    public class MemberServiceList_Model
    {
        public List<MemberService_Model> Data { get; set; }
        public string ServiceCode { get; set; }
        public string Name { get; set; }

    }




    [Serializable]
    public class ServiceImage_Model
    {
        public List<ServiceImg_Model> Data { get; set; }
        public string ServiceCode { get; set; }
        public string Name { get; set; }

    }


}