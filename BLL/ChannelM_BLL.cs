using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model.Manage_Model;

namespace BLL
{
    public  class ChannelM_BLL
    {
        #region 构造类实例
        public static ChannelM_BLL Instance
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
            internal static readonly ChannelM_BLL instance = new ChannelM_BLL();
        }

        #endregion

        public List<Channel_Model> getChannelList(int StartCount = 0, int EndCount = 999999999)
        {
            return ChannelM_DAL.Instance.getChannelList(StartCount, EndCount);
        }


        public Channel_Model getChannelDetail(int ChannelID)
        {
            return ChannelM_DAL.Instance.getChannelDetail(ChannelID);
        }

        public int addChannel(Channel_Model model)
        {
            return ChannelM_DAL.Instance.addChannel(model);
        }

        public int updateChannel(Channel_Model model)
        {
            return ChannelM_DAL.Instance.updateChannel(model);
        }

        public int deleteChannel(Channel_Model model)
        {
            return ChannelM_DAL.Instance.deleteChannel(model);
        }
    }
}
