using Aspose.Cells;
using Common.Util;
using DAL;
using Model.Manage_Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class MemberActM_BLL
    {
        #region 构造类实例
        public static MemberActM_BLL Instance
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
            internal static readonly MemberActM_BLL instance = new MemberActM_BLL();
        }

        #endregion

        public List<MemberAct_Model> getMemberActList(int Status, int StartCount = 0, int EndCount = 999999999)
        {
            return MemberActM_DAL.Instance.getMemberActList(Status, StartCount, EndCount);
        }


        public MemberAct_Model getMemberActDetail(int ID)
        {
            return MemberActM_DAL.Instance.getMemberActDetail(ID);
        }

        public int addMemberAct(MemberAct_Model model)
        {
            return MemberActM_DAL.Instance.addMemberAct(model);
        }

        public int updateMemberAct(MemberAct_Model model)
        {
            return MemberActM_DAL.Instance.updateMemberAct(model);
        }

        public int deleteMemberAct(MemberAct_Model model)
        {
            return MemberActM_DAL.Instance.deleteMemberAct(model);
        }


        public List<MemberActInfo_Model> getMemberInfoList(int ActID, int HandleSts)
        {
            return MemberActM_DAL.Instance.getMemberInfoList(ActID, HandleSts);
        }

        public int updateHandleSts(MemberActInfo_Model model)
        {
            return MemberActM_DAL.Instance.updateHandleSts(model);
        }


        public string ExportMemberActInfo(MemberAct_Model memberAct, List<MemberActInfo_Model> listInfo)
        {
            Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
            Aspose.Cells.Worksheet ws = wb.Worksheets[0];
            ws.Name = memberAct.Name;

            ws.Cells[0, 0].PutValue("会员编号");
            ws.Cells.SetColumnWidth(0, 20);
            ws.Cells[0, 1].PutValue("姓名");
            ws.Cells.SetColumnWidth(1, 20);
            ws.Cells[0, 2].PutValue("性别");
            ws.Cells.SetColumnWidth(2, 20);
            ws.Cells[0, 3].PutValue("年龄");
            ws.Cells.SetColumnWidth(3, 20);
            ws.Cells[0, 4].PutValue("联系电话");
            ws.Cells.SetColumnWidth(4, 20);
            ws.Cells[0, 5].PutValue("身份证号码");
            ws.Cells.SetColumnWidth(5, 20);
            ws.Cells[0, 6].PutValue("地址");
            ws.Cells.SetColumnWidth(6, 20);
            ws.Cells[0, 7].PutValue("处理状态");
            ws.Cells.SetColumnWidth(7, 20);

            Range range = ws.Cells.CreateRange(0, 0, 1, 6);

            if (listInfo != null && listInfo.Count > 0)
            {
                for (int rows = 0; rows < listInfo.Count; rows++)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                ws.Cells[rows + 1, i].PutValue(listInfo[rows].MemberCode);
                                break;
                            case 1:
                                ws.Cells[rows + 1, i].PutValue(listInfo[rows].Name);
                                break;
                            case 2:
                                int Gender = StringUtils.GetDbInt(listInfo[rows].Gender);
                                switch (Gender) {
                                    case 0:
                                        ws.Cells[rows + 1, i].PutValue("未输入");
                                        break;
                                    case 1:
                                        ws.Cells[rows + 1, i].PutValue("男");
                                        break;
                                    case 2:
                                        ws.Cells[rows + 1, i].PutValue("女");
                                        break;

                                }
                                
                                break;
                            case 3:
                                ws.Cells[rows + 1, i].PutValue(listInfo[rows].Age);
                                break;
                            case 4:
                                ws.Cells[rows + 1, i].PutValue(listInfo[rows].Phone);
                                break;
                            case 5:
                                ws.Cells[rows + 1, i].PutValue(listInfo[rows].IDNumber);
                                break;
                            case 6:
                                ws.Cells[rows + 1, i].PutValue(listInfo[rows].Address);
                                break;
                            case 7:
                                ws.Cells[rows + 1, i].PutValue(listInfo[rows].HandleSts == 1? "未处理" : "已处理");
                                break;
                        }
                    }
                }
            }
            wb.CalculateFormula(true);
            string path = System.AppDomain.CurrentDomain.BaseDirectory + System.Configuration.ConfigurationManager.AppSettings["ImageFolder"] + "temp/report/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName = memberAct.Name + DateTime.Now.ToLocalTime().ToString("yyyyMMddHHmmss") + ".xls";
            wb.Save(path + fileName, SaveFormat.Excel97To2003);
            string url = System.Configuration.ConfigurationManager.AppSettings["ImageDoMain"] + System.Configuration.ConfigurationManager.AppSettings["ImageFolder"] + "temp/report/" + fileName;
            return url;
        }

    }
}
