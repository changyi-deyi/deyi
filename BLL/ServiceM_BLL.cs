using DAL;
using Model.Manage_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ServiceM_BLL
    {
        #region 构造类实例
        public static ServiceM_BLL Instance
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
            internal static readonly ServiceM_BLL instance = new ServiceM_BLL();
        }

        #endregion

        public List<Service_Model> getServiceList(int Status, int StartCount = 0, int EndCount = 999999999)
        {
            return ServiceM_DAL.Instance.getServiceList(Status, StartCount, EndCount);
        }


        public Service_Model getServiceDetail(string ServiceCode)
        {
            return ServiceM_DAL.Instance.getServiceDetail(ServiceCode);
        }

        public int addService(Service_Model model)
        {
            return ServiceM_DAL.Instance.addService(model);
        }

        public int updateService(Service_Model model)
        {
            return ServiceM_DAL.Instance.updateService(model);
        }

        public int deleteService(Service_Model model)
        {
            return ServiceM_DAL.Instance.deleteService(model);
        }

        public List<ServiceDoctor_Model> getServiceDoctorList(string ServiceCode)
        {
            return ServiceM_DAL.Instance.getServiceDoctorList(ServiceCode);
        }

        public int addServiceDoctor(string ServiceCode, List<ServiceDoctor_Model> DoctorList, int UserID)
        {
            return ServiceM_DAL.Instance.addServiceDoctor(ServiceCode, DoctorList, UserID);
        }


        public List<MemberService_Model> getMemberServiceList(string ServiceCode)
        {
            return ServiceM_DAL.Instance.getMemberServiceList(ServiceCode);
        }

        public int addMemberService(MemberService_Model model)
        {
            return ServiceM_DAL.Instance.addMemberService(model);
        }

        public int UpdatedMemberService(MemberService_Model model)
        {
            return ServiceM_DAL.Instance.UpdatedMemberService(model);
        }


        #region img
        public List<ServiceImg_Model> getServiceImg(string ServiceCode)
        {
            return ServiceM_DAL.Instance.getServiceImg(ServiceCode);
        }


        public int addServiceImg(ServiceImg_Model model)
        {
            return ServiceM_DAL.Instance.addServiceImg(model);
        }

        public int DeleteServiceImg(ServiceImg_Model model)
        {
            return ServiceM_DAL.Instance.DeleteServiceImg(model);
        }


        #endregion

        public int UpdateSort(string ServiceCode, int Sort) {
            return ServiceM_DAL.Instance.UpdateSort(ServiceCode,Sort);
        }


    }
}
