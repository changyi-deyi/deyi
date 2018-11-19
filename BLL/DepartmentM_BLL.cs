using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model.Manage_Model;

namespace BLL
{
    public class DepartmentM_BLL
    {
        #region 构造类实例
        public static DepartmentM_BLL Instance
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
            internal static readonly DepartmentM_BLL instance = new DepartmentM_BLL();
        }

        #endregion
        

        public List<Department_Model> getDepartmentList(string DepartmentName, int UpperID, int StartCount = 0, int EndCount = 999999999)
        {
            return DepartmentM_DAL.Instance.getDepartmentList(DepartmentName, UpperID, StartCount, EndCount);
        }


        public Department_Model getDepartmentDetail(int DepartmentID)
        {
            return DepartmentM_DAL.Instance.getDepartmentDetail(DepartmentID);
        }

        public int addDepartment(Department_Model model)
        {
            return DepartmentM_DAL.Instance.addDepartment(model);
        }

        public int updateDepartment(Department_Model model)
        {

            return DepartmentM_DAL.Instance.updateDepartment(model);
        }

        public int deleteDepartment(Department_Model model)
        {
            return DepartmentM_DAL.Instance.deleteDepartment(model);
        }
    }
}
