using Model.Manage_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class TagM_BLL
    {
        #region 构造类实例
        public static TagM_BLL Instance
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
            internal static readonly TagM_BLL instance = new TagM_BLL();
        }

        #endregion

        public List<Tag_Model> getTagList(int StartCount = 0, int EndCount = 999999999)
        {
            return TagM_DAL.Instance.getTagList(StartCount, EndCount);
        }


        public Tag_Model getTagDetail(int TagID)
        {
            return TagM_DAL.Instance.getTagDetail(TagID);
        }

        public int addTag(Tag_Model model)
        {
            return TagM_DAL.Instance.addTag(model);
        }

        public int updateTag(Tag_Model model)
        {
            return TagM_DAL.Instance.updateTag(model);
        }

        public int deleteTag(Tag_Model model)
        {
            return TagM_DAL.Instance.deleteTag(model);
        }


        public List<Tag_Model> getDoctorTagList(string DoctorCode) {
            return TagM_DAL.Instance.getDoctorTagList(DoctorCode);
        }

    }
}
