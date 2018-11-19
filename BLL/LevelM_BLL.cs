using DAL;
using Model.Manage_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public  class LevelM_BLL
    {
        #region 构造类实例
        public static LevelM_BLL Instance
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
            internal static readonly LevelM_BLL instance = new LevelM_BLL();
        }

        #endregion

        public List<Level_Model> getLevelList(int Status , int StartCount = 0, int EndCount = 999999999)
        {
            return LevelM_DAL.Instance.getLevelList(Status,StartCount, EndCount);
        }


        public Level_Model getLevelDetail(int LevelID)
        {
            return LevelM_DAL.Instance.getLevelDetail(LevelID);
        }

        public int addLevel(Level_Model model)
        {
            return LevelM_DAL.Instance.addLevel(model);
        }

        public int updateLevel(Level_Model model)
        {
            return LevelM_DAL.Instance.updateLevel(model);
        }

        public int deleteLevel(Level_Model model)
        {
            return LevelM_DAL.Instance.deleteLevel(model);
        }
    }
}
