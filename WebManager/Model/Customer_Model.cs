using Model.Manage_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebManager.Model
{
    [Serializable]
    public class CustomerList_Model
    {
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public int RowsCount { get; set; }
        public int PageCount { get; set; }

        public List<Customer_Model> Data { get; set; }
        public string CustomerName { get; set; }
        public int LevelID { get; set; }
        public int ChannelID { get; set; }
        public int Status { get; set; }

        public List<Level_Model> listLevel { get; set; }

        public List<Channel_Model> listChannel { get; set; }
    }

    [Serializable]
    public class CustomerDetail_Model
    {
        public string CustomerCode { get; set; }

        public Customer_Model Customer { get; set; }

        public Member_Model Member { get; set; }
        
    }

    [Serializable]
    public class BalanceList_Model {

        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public int RowsCount { get; set; }
        public int PageCount { get; set; }

        public List<Balance_Model> Data { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string CustomerCode { get; set; }
    }


    [Serializable]
    public class CustomerProfileList_Model {
        public List<CustomerProfile_Model> Data { get; set; }
        public List<CustomerProfileImg_Model> listImg { get; set; }

        public int VerifyStatus { get; set; }
        public string CustomerCode { get; set; }
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public int RowsCount { get; set; }
        public int PageCount { get; set; }
    }
}