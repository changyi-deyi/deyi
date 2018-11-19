using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model.Manage_Model;

namespace BLL
{
    public class UserM_BLL
    {
        #region 构造类实例
        public static UserM_BLL Instance
        {
            get
            {
                return Nested.instance;
            }
        }

        class Nested
        {
            static Nested()
            {
            }
            internal static readonly UserM_BLL instance = new UserM_BLL();
        }

        #endregion

        public User_Model getUserByAccountPassword(string LoginUserName, string Password)
        {
            return UserM_DAL.Instance.getUserByAccountPassword(LoginUserName, Password);
        }


        public Doctor_Model getDoctorDetail(int UserID)
        {
            return UserM_DAL.Instance.getDoctorDetail(UserID);
        }

        public Staff_Model getStaffDetail(int UserID)
        {
            return UserM_DAL.Instance.getStaffDetail(UserID);
        }

        public int addUser(UserOperate_Model model)
        {
            return UserM_DAL.Instance.addUser(model);
        }



        public int deleteUser(UserDeleteOperate_Model model)
        {
            return UserM_DAL.Instance.deleteUser(model);
        }

        #region docotr
        public List<Doctor_Model> getDoctorList(string Name, int HospitalID, int DepartmentID, int StartCount = 0, int EndCount = 999999999)
        {
            return UserM_DAL.Instance.getDoctorList(Name, HospitalID, DepartmentID, StartCount, EndCount);
        }


        public Doctor_Model getDoctorDetail(string DoctorCode)
        {
            return UserM_DAL.Instance.getDoctorDetail(DoctorCode);
        }


        public int updateDoctor(Doctor_Model model)
        {
            return UserM_DAL.Instance.updateDoctor(model);
        }


        #endregion

        #region Staff
        public List<Staff_Model> getStaffList(string Name, int Role, int StartCount = 0, int EndCount = 999999999)
        {
            return UserM_DAL.Instance.getStaffList(Name, Role, StartCount, EndCount);

        }


        public Staff_Model getStaffDetail(string StaffCode)
        {
            return UserM_DAL.Instance.getStaffDetail(StaffCode);
        }


        public int updateStaff(Staff_Model model)
        {
            return UserM_DAL.Instance.updateStaff(model);
        }


        #endregion

        #region Customer
        public List<Customer_Model> getCustomerList(string Name, int LevelID, int ChannelID, int Status, int StartCount = 0, int EndCount = 999999999)
        {
            return UserM_DAL.Instance.getCustomerList(Name, LevelID, ChannelID, Status, StartCount, EndCount);
        }


        public Customer_Model getCustomerDetail(string CustomerCode)
        {
            return UserM_DAL.Instance.getCustomerDetail(CustomerCode);
        }


        public Member_Model getMemberDetail(string MemberCode)
        {
            return UserM_DAL.Instance.getMemberDetail(MemberCode);
        }
        public int UpdateCustomer(Customer_Model model)
        {

            return UserM_DAL.Instance.UpdateCustomer(model);
        }

        public List<CustomerProfile_Model> getCustomerProfile(string CustomerCode, int VerifyStatus, int StartCount = 0, int EndCount = 99999999)
        {
            return UserM_DAL.Instance.getCustomerProfile( CustomerCode, VerifyStatus,  StartCount,  EndCount);
        }

        public List<CustomerProfileImg_Model> getCustomerProfileImg(string CustomerCode, int VerifyStatus)
        {
            return UserM_DAL.Instance.getCustomerProfileImg( CustomerCode, VerifyStatus);
        }


        public int changeVerifyStatus(CustomerProfile_Model model)
        {
            return UserM_DAL.Instance.changeVerifyStatus(model);

        }

        public List<Balance_Model> getBalanceList(string CustomerCode, string StartDate, string EndDate, int StartCount = 0, int EndCount = 999999999)
        {
            return UserM_DAL.Instance.getBalanceList(CustomerCode, StartDate, EndDate, StartCount, EndCount);
        }


        #endregion


    }
}
