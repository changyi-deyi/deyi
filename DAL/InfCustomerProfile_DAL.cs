using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Operate_Model;
using Model.Table_Model;
using BLToolkit.Data;

namespace DAL
{
    public class InfCustomerProfile_DAL
    {
        #region 构造类实例
        public static InfCustomerProfile_DAL Instance
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
            internal static readonly InfCustomerProfile_DAL instance = new InfCustomerProfile_DAL();
        }

        #endregion
        public List<InfCustomerProfile_Model> GetCustomerProfile(string CustomerCode)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT  `ID`
                                          ,`CustomerCode`
                                          ,`RelatedCustomerCode`
                                          ,`Type`
                                          ,`UploadTime`
                                          ,`Status`
                                    FROM  `Inf_Coupon`
                                   WHERE  `Status` = 1 
                                     AND  `CustomerCode` = @CustomerCode
                                ORDER BY  `Weights` DESC";
                List<InfCustomerProfile_Model> list = db.SetCommand(strSql, db.Parameter("@CustomerCode", CustomerCode, DbType.String)).ExecuteList<InfCustomerProfile_Model>();
                return list;
            }
        }

        public List<string> GetUploadDate(string CustomerCode, int type)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT  DATE_FORMAT(`UploadTime`,'%Y-%m-%d') as UploadDate
                                    FROM  `Inf_CustomerProfile`
                                   WHERE   `Status` = 1 
                                     AND   `CustomerCode` = @CustomerCode ";
                if (type != 0)
                {
                    strSql += " AND `Type` = @type ";
                }
                strSql += " GROUP BY `UploadDate` ORDER BY `UploadDate` DESC";
                List<string> list = db.SetCommand(strSql
                    , db.Parameter("@CustomerCode", CustomerCode, DbType.String)
                    , db.Parameter("@type", type, DbType.Int32)).ExecuteScalarList<string>();

                return list;
            }
        }

        public List<CustomerProfile_Model> GetProfileAndImaCount(string CustomerCode, string UploadDate, int type)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT  A.`ID`
                                          ,A.`CustomerCode`
                                          ,A.`RelatedCustomerCode`
                                          ,A.`Type`
                                          ,A.`UploadTime`
                                          ,A.`Status`
                                          ,(SELECT COUNT(1) 
                                              FROM `Ima_CustomerProfile` B 
                                             WHERE A.`ID` = B.`ID` AND B.`Status` = 1 ) AS `ImaCount`
                                    FROM  `Inf_CustomerProfile` A
                                   WHERE   A.`Status` = 1 
                                     AND   A.`CustomerCode` = @CustomerCode 
                                     AND   DATE_FORMAT(A.`UploadTime`,'%Y-%m-%d') = @UploadDate";
                if (type != 0)
                {
                    strSql += " AND A.`Type` = @type ";
                }
                strSql += " ORDER BY A.`UploadTime` DESC";
                List<CustomerProfile_Model> list = db.SetCommand(strSql
                    , db.Parameter("@CustomerCode", CustomerCode, DbType.String)
                    , db.Parameter("@UploadDate", UploadDate, DbType.String)
                    , db.Parameter("@type", type, DbType.Int32)).ExecuteList<CustomerProfile_Model>();
                return list;
            }
        }
        
        public int ProfileUpload(ProfileUpload_Model model, List<ImageURL_Model> list)
        {
            using (DbManager db = new DbManager())
            {
                db.BeginTransaction();
                //新增客户健康档案表记录
                string strProfileIns = @" INSERT INTO `Inf_CustomerProfile` (`CustomerCode`, `RelatedCustomerCode`, `Type`, `UploadTime`,`VerifyStatus`, `Status`, `CreatetTime`, `Creator`)
                                            VALUES (@CustomerCode, @RelatedCustomerCode, @Type, @nowtime,1, 1, @nowtime, @UserID);select @@IDENTITY";
                int ProfileID = db.SetCommand(strProfileIns
                    , db.Parameter("@CustomerCode", model.CustomerCode, DbType.String)
                    , db.Parameter("@RelatedCustomerCode", model.RelatedCustomerCode, DbType.String)
                    , db.Parameter("@Type", model.Type, DbType.Int32)
                    , db.Parameter("@nowtime", DateTime.Now, DbType.DateTime)
                    , db.Parameter("@UserID", model.UserID, DbType.Int32)).ExecuteScalar<int>();

                if (ProfileID <= 0)
                {
                    db.RollbackTransaction();
                    return 0;
                }
                //新增健康档案图片表记录
                if (list != null && list.Count > 0)
                {
                    string strImaIns = @" INSERT INTO `Ima_CustomerProfile` (`ID`, `Path`, `FileName`, `Status`, `CreatetTime`, `Creator`)
                                            VALUES (@ID, @Path, @FileName, 1, @nowtime, @UserID) ";
                    foreach(ImageURL_Model item in list)
                    {
                        int row = db.SetCommand(strImaIns
                            , db.Parameter("@ID", ProfileID, DbType.String)
                            , db.Parameter("@Path",item.ImageURL, DbType.String)
                            , db.Parameter("@FileName", item.FileName, DbType.String)
                            , db.Parameter("@nowtime", DateTime.Now, DbType.DateTime)
                            , db.Parameter("@UserID", model.UserID, DbType.Int32)).ExecuteNonQuery();

                        if (row <= 0)
                        {
                            db.RollbackTransaction();
                            return 0;
                        }
                    }
                }
                db.CommitTransaction();
                return 1;
            }
        }
        public int ProfileDelete(int ID, int UserID)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE  `Inf_CustomerProfile` 
                                      SET  `Status` = 2
                                          ,`UpdateTime` = @now
                                          ,`Updater` = @UserID
                                    WHERE  `ID` = @ID ";
                int row = db.SetCommand(strSql
                    , db.Parameter("@ID", ID, DbType.Int32)
                    , db.Parameter("@UserID", UserID, DbType.Int32)
                    , db.Parameter("@now", DateTime.Now, DbType.DateTime)).ExecuteNonQuery();
                if (row <= 0)
                {
                    return 0;
                }
                return 1;
            }
        }
    }
}
