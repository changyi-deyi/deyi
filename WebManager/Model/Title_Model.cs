using Model.Manage_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebManager.Model
{
    [Serializable]
    public class TitleList_Model
    {
        public string TitleName { get; set; }
        public List<Title_Model> Data { get; set; }

        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public int RowsCount { get; set; }
        public int PageCount { get; set; }

    }
    [Serializable]
    public class TitleDetail_Model
    {
        public int TitleID { get; set; }
        public Title_Model Data { get; set; }


    }
}