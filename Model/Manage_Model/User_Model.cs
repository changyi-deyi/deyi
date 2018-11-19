using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Manage_Model
{
    [Serializable]
    public class User_Model
    {
        public int UserID { get; set; }
        /// <summary>
        /// 1：客户 2：医生 3：运营
        /// </summary>
        public int Type { get; set; }

        public string LoginUserName { get; set; }
        public string Password { get; set; }
        /// <summary>
        /// 1：有效 2：无效
        /// </summary>
        public int Status { get; set; }

        public int Creator { get;set; }

        public DateTime CreatetTime { get; set; }

        public int? Updater { get; set; }

        public DateTime? UpdateTime { get; set; } 

    }

    [Serializable]
    public class Doctor_Model
    {
        public int UserID { get; set; }
        public string DoctorCode { get; set; }

        public string Name { get; set; }
        /// <summary>
        /// 0：未输入 1：男 2：女
        /// </summary>
        public int Gender { get; set; }
        public DateTime Birthday { get; set; }

        public int HospitalID { get; set; }

        public int DepartmentID { get; set; }

        public int TitleID { get; set; }

        public string GoodAt { get; set; }
        public string Introduction { get; set; }
        public string Phone { get; set; }
        public string ImageURL { get; set; }
        /// <summary>
        /// 1：合伙人 2：服务人
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 1：有效 2：无效
        /// </summary>
        public int Status { get; set; }
        public int Creator { get; set; }

        public DateTime CreatetTime { get; set; }

        public int? Updater { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string HospitalName { get; set; }

        public string DepartmentName { get; set; }
        public string LoginUserName { get; set; }
        public List<int> listTag { get; set; }
        public int Weights { get; set; }
    }

    [Serializable]
    public class Staff_Model {
        public int UserID { get; set; }
        public string StaffCode { get; set; }

        public string Name { get; set; }

        public string Mobile { get; set; }
        /// <summary>
        /// 0：未输入 1：男 2：女
        /// </summary>
        public int Gender { get; set; }
        /// <summary>
        /// 1：管理员 2：客服
        /// </summary>
        public int Role { get; set; }
        /// <summary>
        /// 1：有效 2：无效
        /// </summary>
        public int Status { get; set; }
        public int Creator { get; set; }

        public DateTime CreatetTime { get; set; }

        public int? Updater { get; set; }

        public DateTime? UpdateTime { get; set; }
        public string LoginUserName { get; set; }
    }

    [Serializable]
    public class Customer_Model {
        public int UserID { get; set; }
        public string MemberCode { get; set; }
        public string CustomerCode { get; set; }

        public string Name { get; set; }

        public string Mobile { get; set; }
        public DateTime Birthday { get; set; }
        /// <summary>
        /// 0：未输入 1：男 2：女
        /// </summary>
        public int Gender { get; set; }
        public decimal Balance { get; set; }
        public decimal IsMember { get; set; }
        public DateTime? LastSignInDate { get; set; }
        public int CycleDate { get; set; }

        public int ContinuousDate { get;set; }
        public int SignStatus { get; set; }
        /// <summary>
        /// 1：有效 2：无效
        /// </summary>
        public int Status { get; set; }
        public int Creator { get; set; }

        public DateTime CreatetTime { get; set; }

        public int? Updater { get; set; }

        public DateTime? UpdateTime { get; set; }
        public string IDNumber { get; set; }
        public string LevelName { get; set; }
    }

    [Serializable]
    public class UserOperate_Model
    {
        public int UserID { get; set; }
        public string UserCode { get; set; }
        public User_Model User { get; set; }
        public Doctor_Model Doctor { get; set; }
        public Staff_Model Staff { get; set; }

    }

    [Serializable]
    public class UserDeleteOperate_Model {
        public int UserID { get; set; }
        public string UserCode { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }

        public int Updater { get; set; }

        public DateTime UpdateTime { get; set; }



    }


    [Serializable]
    public class Member_Model {

        public string MemberCode { get; set; }
        public string CustomerCode { get; set; }
        public int ChannelID { get; set; }
        public string ChannelName { get; set; }
        public int LevelID { get; set; }
        public string LevelName { get; set; }
        public int Type { get; set; }
        public string FamilyCode { get; set; }
        public int MaxQty { get; set; }
        public int FreeQty { get; set; }
        public DateTime ExpiredDate { get; set; }
        public string IDNumber { get; set; }
        public int Status { get; set; }
    }


    [Serializable]
    public class Balance_Model {
        public string CustomerCode { get; set; }
        public int BalanceID { get; set; }
        public int InOutType { get; set; }
        public int Type { get; set; }
        public decimal ChangeBalance { get; set; }
        public decimal Balance { get; set; }
        public int RelatedType { get; set; }
        public String RelatedID { get; set; }
        public string Remark { get; set; }
        /// <summary>
        /// 1：有效 2：无效
        /// </summary>
        public int Status { get; set; }
        public int Creator { get; set; }

        public DateTime CreatetTime { get; set; }

        public int? Updater { get; set; }

        public DateTime? UpdateTime { get; set; }
    }




    public class CustomerProfile_Model {
        public int ID { get; set; }
        public string CustomerCode { get; set; }
        public string RelatedCustomerCode { get; set; }
        public int Type { get; set; }
        public DateTime UploadTime { get; set; }
        public int VerifyStatus { get; set; }
        public int Status { get; set; }
        public DateTime CreatetTime { get; set; }
        public int Creator { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int Updater { get; set; }
        public string CustomerName { get; set; }

    }


    public class CustomerProfileImg_Model
    {
        public int ID { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public int Status { get; set; }
        public DateTime CreatetTime { get; set; }
        public int Creator { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int Updater { get; set; }
    }




}
