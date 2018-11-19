using Model.Manage_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebManager.Model
{
    [Serializable]
    public class MemberActList_Model
    {
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public int RowsCount { get; set; }
        public int PageCount { get; set; }

        public List<MemberAct_Model> Data { get; set; }
        public int Status { get; set; }

    }

    [Serializable]
    public class MemberActDetail_Model
    {

        public MemberAct_Model Data { get; set; }

        public List<Level_Model> listLevel { get; set; }

        public int ID { get; set; }

    }
    [Serializable]
    public class MemberActInfoView_Model
    {
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public int RowsCount { get; set; }
        public int PageCount { get; set; }

        public List<MemberActInfo_Model> Data { get; set; }
        public int ID { get; set; }
        public int HandleSts { get; set; }

    }
}