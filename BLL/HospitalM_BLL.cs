using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model.Manage_Model;

namespace BLL
{
    public  class HospitalM_BLL
    {
        #region 构造类实例
        public static HospitalM_BLL Instance
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
            internal static readonly HospitalM_BLL instance = new HospitalM_BLL();
        }

        #endregion

        public List<Hospital_Model> getHospitalList(string HospitalName, int StartCount = 0, int EndCount = 999999999) {
            return HospitalM_DAL.Instance.getHospitalList(HospitalName, StartCount, EndCount);
        }


        public Hospital_Model getHospitalDetail(int HospitalID)
        {
            return HospitalM_DAL.Instance.getHospitalDetail(HospitalID);
        }


        public int addHospital(Hospital_Model model) {
            return HospitalM_DAL.Instance.addHospital(model);
        }


        public int updateHospital(Hospital_Model model)
        {
            return HospitalM_DAL.Instance.updateHospital(model);
        }


        public int deleteHospital(Hospital_Model model)
        {
            return HospitalM_DAL.Instance.deleteHospital(model);
        }

    }
}
