using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model.Table_Model;
using Model.View_Model;
using Model.Operate_Model;

namespace BLL
{
    public class InfDoctor_BLL
    {
        #region 构造类实例
        public static InfDoctor_BLL Instance
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
            internal static readonly InfDoctor_BLL instance = new InfDoctor_BLL();
        }
        #endregion
        public List<DoctorList_Model> GetDoctorList(Doctor_Model model)
        {
            return InfDoctor_DAL.Instance.GetDoctorList(model);
        }

        public DoctorList_Model GetDoctorDetail(DoctorDetailIn_Model model)
        {
            return InfDoctor_DAL.Instance.GetDoctorDetail(model);
        }

        public List<DoctorService_Model> GetDoctorService(string DoctorCode)
        {
            return InfDoctor_DAL.Instance.GetDoctorService(DoctorCode);
        }

    }
}
