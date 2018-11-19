using Model.Manage_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebManager.Model
{
    [Serializable]
    public class ChannelList_Model
    {
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public int RowsCount { get; set; }
        public int PageCount { get; set; }

        public List<Channel_Model> Data { get; set; }

    }

    [Serializable]
    public class ChannelDetail_Model
    {

        public Channel_Model Data { get; set; }

        public int ChannelID { get; set; }

    }
}