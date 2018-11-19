using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Table_Model;
using Model.Operate_Model;
using BLToolkit.Data;

namespace DAL
{
    public class BasUser_DAL
    {
        #region 构造类实例
        public static BasUser_DAL Instance
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
            internal static readonly BasUser_DAL instance = new BasUser_DAL();
        }

        #endregion
        public BasUser_Model GetUser(string LoginUserName)
        {
            using (DbManager db = new DbManager())
            {
                string strSqlSel = @" SELECT  `UserID` 
                                             ,`Type`
                                             ,`LoginUserName`
                                             ,`Password`
                                             ,`WechatOpenID`
                                             ,`WecharUnionID`
                                             ,`LastLogin`
                                             ,`Status`
                                        FROM  `Bas_User` 
                                       WHERE  `Status` = 1
                                         AND  `Type` = 1
                                         AND  `LoginUserName` = @LoginUserName ";
                BasUser_Model model = db.SetCommand(strSqlSel
                    , db.Parameter("@LoginUserName", LoginUserName, DbType.String)).ExecuteObject<BasUser_Model>();
                return model;
            }
        }

        public BasUser_Model GetUserWithOpenID(string WechatOpenID)
        {
            using (DbManager db = new DbManager())
            {
                string strSqlSel = @" SELECT  `UserID` 
                                             ,`Type`
                                             ,`LoginUserName`
                                             ,`Password`
                                             ,`WechatOpenID`
                                             ,`WecharUnionID`
                                             ,`LastLogin`
                                             ,`Status`
                                        FROM  `Bas_User` 
                                       WHERE  `Status` = 1
                                         AND  `Type` = 1
                                         AND  `WechatOpenID` = @WechatOpenID ";
                BasUser_Model model = db.SetCommand(strSqlSel
                    , db.Parameter("@WechatOpenID", WechatOpenID, DbType.String)).ExecuteObject<BasUser_Model>();
                return model;
            }
        }

        public int GetUserCount(string LoginUserName)
        {
            using (DbManager db = new DbManager())
            {
                string strSqlSel = @" SELECT  COUNT(1)
                                        FROM  `Bas_User` 
                                       WHERE  `Status` = 1
                                         AND  `Type` = 1
                                         AND  `LoginUserName` = @LoginUserName ";
                int rows = db.SetCommand(strSqlSel
                    , db.Parameter("@LoginUserName", LoginUserName, DbType.String)).ExecuteScalar<int>();
                return rows;
            }
        }

        public int Login(BasUser_Model model, Register_Model info)
        {
            using (DbManager db = new DbManager())
            {
                db.BeginTransaction();
                //更新用户表
                string strUserUpd = @" UPDATE  `Bas_User`
                                          SET  `LastLogin` = @nowtime,
                                               `WechatOpenID` = @WechatOpenID,
                                               `UpdateTime` = @nowtime,
                                               `Updater` = @UserID
                                        WHERE  `UserID` = @UserID ";
                int rows = db.SetCommand(strUserUpd
                    , db.Parameter("@nowtime", DateTime.Now, DbType.DateTime)
                    , db.Parameter("@UserID", model.UserID, DbType.Int32)
                    , db.Parameter("@WechatOpenID", info.OpenID, DbType.String)).ExecuteNonQuery();
                
                if (rows <= 0)
                {
                    db.RollbackTransaction();
                    return 0;
                }

                //新增用户登陆记录表记录
                string strHistoryIns = @" INSERT  `Ope_LoginHistory` (`UserID`, `LoginTime`, `BrowserInfo`)
                                            VALUES  (@UserID, @nowtime, @BrowserInfo)";
                rows = db.SetCommand(strHistoryIns
                    , db.Parameter("@nowtime", DateTime.Now, DbType.DateTime)
                    , db.Parameter("@UserID", model.UserID, DbType.Int32)
                    , db.Parameter("@BrowserInfo", info.BrowserInfo, DbType.String)).ExecuteNonQuery();

                if (rows <= 0)
                {
                    db.RollbackTransaction();
                    return 0;
                }

                db.CommitTransaction();
                return 1;
            }
        }

        public int Register(BasUser_Model model, Register_Model info)
        {
            using (DbManager db = new DbManager())
            {
               
                db.BeginTransaction();

                //存储过程获取顾客编号
                var outParmeter = db.OutputParameter("@Result", DbType.String);
                var sf = db.SetCommand(CommandType.StoredProcedure, "GetSerialNo"
                                  , db.Parameter("@TN", "Inf_Customer", DbType.String)
                                  , outParmeter).ExecuteScalar<string>();
                string CustomerCode = outParmeter.Value.ToString();
                if (string.IsNullOrEmpty(CustomerCode))
                {
                    return 0;
                }
                
                //新增用户表记录
                string strUserIns = @" INSERT  `Bas_User` (`Type`, `LoginUserName`, `WechatOpenID`, `LastLogin`, `Status`, `CreatetTime`,`Creator`)
                                        VALUES  (1, @LoginUserName, @WechatOpenID, @nowtime, 1, @nowtime, 1);select @@IDENTITY";
                int UserID = db.SetCommand(strUserIns
                    , db.Parameter("@nowtime", DateTime.Now, DbType.DateTime)
                    , db.Parameter("@LoginUserName", info.Mobile, DbType.String)
                    , db.Parameter("@WechatOpenID", info.OpenID, DbType.String)).ExecuteScalar<int>();

                if (UserID <= 0)
                {
                    db.RollbackTransaction();
                    return 0;
                }

                string strUserUpd = @" UPDATE  `Bas_User` 
                                          SET  `Creator` = @UserID
                                        WHERE  `UserID` = @UserID";
                int rows = db.SetCommand(strUserUpd
                    , db.Parameter("@UserID", UserID, DbType.Int32)).ExecuteNonQuery();

                if (rows <= 0)
                {
                    db.RollbackTransaction();
                    return 0;
                }

                //新增客户基本信息表记录
                string strCustomerIns = @" INSERT `Inf_Customer` (`UserID`, `CustomerCode`, `Name`, `Mobile`, `Gender`, `IsMember`, `SignStatus`, `Status`, `CreatetTime`, `Creator`)
                                            VALUES  (@UserID, @CustomerCode, @Name, @Mobile, @Gender, 1, 1, 1, @nowtime, @UserID)";
                rows = db.SetCommand(strCustomerIns
                    , db.Parameter("@UserID", UserID, DbType.Int32)
                    , db.Parameter("@CustomerCode", CustomerCode, DbType.String)
                    , db.Parameter("@Name", info.Name, DbType.String)
                    , db.Parameter("@Mobile", info.Mobile, DbType.String)
                    , db.Parameter("@Gender", info.Gender, DbType.Int32)
                    , db.Parameter("@nowtime", DateTime.Now, DbType.DateTime)).ExecuteNonQuery();

                if (rows <= 0)
                {
                    db.RollbackTransaction();
                    return 0;
                }

                //新增用户登陆记录表记录
                string strHistoryIns = @" INSERT  `Ope_LoginHistory` (`UserID`, `LoginTime`, `BrowserInfo`)
                                            VALUES  (@UserID, @nowtime, @BrowserInfo)";
                rows = db.SetCommand(strHistoryIns
                    , db.Parameter("@nowtime", DateTime.Now, DbType.DateTime)
                    , db.Parameter("@UserID", UserID, DbType.Int32)
                    , db.Parameter("@BrowserInfo", info.BrowserInfo, DbType.String)).ExecuteNonQuery();

                if (rows <= 0)
                {
                    db.RollbackTransaction();
                    return 0;
                }

                db.CommitTransaction();
                return UserID;
            }
        }


        public int ChangeMobile(string Mobile, int UserID)
        {
            using (DbManager db = new DbManager())
            {
                db.BeginTransaction();
                //更新用户表
                string strUserUpd = @" UPDATE  `Bas_User`
                                          SET  `LoginUserName` = @Mobile,
                                               `UpdateTime` = @nowtime,
                                               `Updater` = @UserID
                                        WHERE  `UserID` = @UserID ";
                int rows = db.SetCommand(strUserUpd
                    , db.Parameter("@Mobile", Mobile, DbType.String)
                    , db.Parameter("@nowtime", DateTime.Now, DbType.DateTime)
                    , db.Parameter("@UserID", UserID, DbType.Int32)).ExecuteNonQuery();

                if (rows <= 0)
                {
                    db.RollbackTransaction();
                    return 0;
                }

                //更新客户基本信息表
                string strCusUpd = @" UPDATE  `Inf_Customer`
                                         SET  `Mobile` = @Mobile,
                                              `UpdateTime` = @nowtime,
                                              `Updater` = @UserID
                                       WHERE  `UserID` = @UserID ";
                rows = db.SetCommand(strCusUpd
                    , db.Parameter("@Mobile", Mobile, DbType.String)
                    , db.Parameter("@nowtime", DateTime.Now, DbType.DateTime)
                    , db.Parameter("@UserID", UserID, DbType.Int32)).ExecuteNonQuery();

                if (rows <= 0)
                {
                    db.RollbackTransaction();
                    return 0;
                }

                db.CommitTransaction();
                return 1;
            }
        }
    }
}
