using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model.Table_Model;

namespace BLL
{
    public class SettingM_BLL
    {
        #region 构造类实例
        public static SettingM_BLL Instance
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
            internal static readonly SettingM_BLL instance = new SettingM_BLL();
        }

        #endregion


        public SetGetpoint_Model getSetPoint()
        {
            return SettingM_DAL.Instance.getSetPoint();
             }


        public int UpdateSetPoint(string key, string value)
        {
            return SettingM_DAL.Instance.UpdateSetPoint(key,  value);
        }
        }
}
