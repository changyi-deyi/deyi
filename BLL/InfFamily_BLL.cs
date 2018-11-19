using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model.Table_Model;

namespace BLL
{
    public class InfFamily_BLL
    {
        #region 构造类实例
        public static InfFamily_BLL Instance
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
            internal static readonly InfFamily_BLL instance = new InfFamily_BLL();
        }
        #endregion
        public List<InfFamily_Model> GetFamilyList(string FamilyCode)
        {
            return InfFamily_DAL.Instance.GetFamilyList(FamilyCode);
        }
        public int UpdateFamily(int ID, string Name, string IDNumber, string Relationship, int UserID)
        {
            return InfFamily_DAL.Instance.UpdateFamily(ID, Name, IDNumber, Relationship, UserID);
        }
        public int InsertFamily(string FamilyCode, string Name, string IDNumber, string Relationship, int UserID)
        {
            return InfFamily_DAL.Instance.InsertFamily(FamilyCode, Name, IDNumber, Relationship, UserID);
        }
        public int DeleteFamily(int ID, int UserID)
        {
            return InfFamily_DAL.Instance.DeleteFamily(ID, UserID);
        }
    }
}
