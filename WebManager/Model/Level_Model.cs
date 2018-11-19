using Model.Manage_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebManager.Model
{
    [Serializable]
    public class LevelList_Model
    {
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public int RowsCount { get; set; }
        public int PageCount { get; set; }

        public List<Level_Model> Data { get; set; }
        public int Status { get; set; }

    }

    [Serializable]
    public class LevelDetail_Model
    {

        public Level_Model Data { get; set; }

        public int LevelID { get; set; }

    }
}