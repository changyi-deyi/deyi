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
    public class InfCustomer_DAL
    {
        #region 构造类实例
        public static InfCustomer_DAL Instance
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
            internal static readonly InfCustomer_DAL instance = new InfCustomer_DAL();
        }

        #endregion
        public InfCustomer_Model GetMark(CustomerMessage_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT  `UserID`
                                          ,`MemberCode`
                                          ,`CustomerCode`
                                          ,`Name`
                                          ,`Mobile`
                                          ,`Gender`
                                          ,`Birthday`
                                          ,`Balance`
                                          ,`IsMember`
                                          ,`IDNumber`
                                          ,`LastSignInDate`
                                          ,`CycleDate`
                                          ,`ContinuousDate`
                                          ,`SignStatus`
                                          ,`Status`
                                     FROM  `inf_customer`
                                    WHERE  `UserID` = @ID 
                                      AND  `Status` = 1";
                InfCustomer_Model result = db.SetCommand(strSql, db.Parameter("@ID", model.UserID, DbType.Int32)).ExecuteObject<InfCustomer_Model>();
                return result;
            }
        }

        public InfCustomer_Model GetBalance(int UserID, decimal ExchangeAmount)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT  `Balance`
                                     FROM  `inf_customer`
                                    WHERE  `UserID` = @ID 
                                      AND  `Status` = 1
                                      AND  `Balance` >= @ExchangeAmount";
                InfCustomer_Model result = db.SetCommand(strSql
                    , db.Parameter("@ID", UserID, DbType.Int32)
                    , db.Parameter("@ExchangeAmount", ExchangeAmount, DbType.Decimal)).ExecuteObject<InfCustomer_Model>();
                return result;
            }
        }

        public int SignIn(InfCustomer_Model model)
        {
            string yesterday = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            using (DbManager db = new DbManager())
            {
                int CycleDate = 0;
                int ContinueDate = 0;
                decimal changeBalance = decimal.Zero;
                if (model.SignStatus == 1)
                {
                    string strSignInSel = @" SELECT  `SingleReward`
                                                    ,`Cycle`
                                                    ,`CycleReward`
                                               FROM  `Set_GetPoint` ";
                    SetGetpoint_Model signReward = db.SetCommand(strSignInSel).ExecuteObject<SetGetpoint_Model>();

                    if (signReward == null)
                    {
                        return 0;
                    }
                    
                    //判断是否连续签到
                    if (model.LastSignInDate.ToString("yyyy-MM-dd") == yesterday)
                    {
                        CycleDate = model.CycleDate + 1;
                        ContinueDate = model.ContinuousDate + 1;
                    }
                    else
                    {
                        CycleDate = 1;
                        ContinueDate = 1;
                    }
                    
                    //判断是否周期签到最后一天
                    if (CycleDate % signReward.Cycle == 0)
                    {
                        changeBalance = signReward.SingleReward + signReward.CycleReward;
                        CycleDate = signReward.Cycle;
                    }
                    else
                    {
                        changeBalance = signReward.SingleReward;
                        CycleDate = CycleDate % signReward.Cycle;
                    }

                    db.BeginTransaction();

                    //新增健康币流水表记录
                    string strBalanceIns = @" INSERT INTO `Ope_Balance` (`CustomerCode`, `InOutType`, `Type`, `ChangeBalance`, `Balance`, `RelatedType`, `Status`, `CreatetTime`, `Creator`)
                                                    VALUES (@CustomerCode, 1, 11, @ChangeBalance, `Balance` + @ChangeBalance, 3, 1, @nowtime, @UserID);select @@IDENTITY";
                    int BalanceID = db.SetCommand(strBalanceIns
                        , db.Parameter("@CustomerCode", model.CustomerCode, DbType.String)
                        , db.Parameter("@ChangeBalance", changeBalance, DbType.Decimal)
                        , db.Parameter("@nowtime", DateTime.Now, DbType.DateTime)
                        , db.Parameter("@UserID", model.UserID, DbType.Int32)).ExecuteScalar<int>();
                    
                    if (BalanceID <= 0)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }

                    //新增签到表记录
                    string strSignInIns = @" INSERT INTO `Ope_SignIn` (`CustomerCode`, `Date`, `DateTime`, `BalanceID`, `Status`, `CreatetTime`, `Creator`)
                                                  VALUES (@CustomerCode, @Date, @DateTime, @BalanceID, 1, @nowtime, @UserID)";
                    int rows = db.SetCommand(strSignInIns
                        , db.Parameter("@CustomerCode", model.CustomerCode, DbType.String)
                        , db.Parameter("@Date", DateTime.Now, DbType.Date)
                        , db.Parameter("@DateTime", DateTime.Now, DbType.DateTime)
                        , db.Parameter("@BalanceID", BalanceID, DbType.Int32)
                        , db.Parameter("@nowtime", DateTime.Now, DbType.DateTime)
                        , db.Parameter("@UserID", model.UserID, DbType.Int32)).ExecuteNonQuery();

                    if (rows <= 0)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }

                    //更新客户基本信息表
                    string strCustomerUpd = @" UPDATE  `Inf_Customer`
                                                  SET  `Balance` = `Balance` + @ChangeBalance,
                                                       `LastSignInDate` = @Date,
                                                       `ContinuousDate` = @ContinuousDate,
                                                       `CycleDate` = @CycleDate,
                                                       `SignStatus` = 2,
                                                       `UpdateTime` = @UpdateTime,
                                                       `Updater` = @UserID
                                                 WHERE `UserID` = @UserID ";
                    rows = db.SetCommand(strCustomerUpd
                        , db.Parameter("@ChangeBalance", changeBalance, DbType.String)
                        , db.Parameter("@Date", DateTime.Now, DbType.Date)
                        , db.Parameter("@ContinuousDate", ContinueDate, DbType.Int32)
                        , db.Parameter("@CycleDate", CycleDate, DbType.Int32)
                        , db.Parameter("@UpdateTime", DateTime.Now, DbType.DateTime)
                        , db.Parameter("@UserID", model.UserID, DbType.Int32)).ExecuteNonQuery();

                    if (rows <= 0)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }
                    db.CommitTransaction();
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
        }
        
        public InfCustomer_Model GetCustomerMember(string CustomerCode)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT  `UserID`
                                          ,`MemberCode`
                                          ,`CustomerCode`
                                          ,`Name`
                                          ,`Mobile`
                                          ,`Gender`
                                          ,`Birthday`
                                          ,`Balance`
                                          ,`IsMember`
                                          ,`IDNumber`
                                          ,`LastSignInDate`
                                          ,`CycleDate`
                                          ,`ContinuousDate`
                                          ,`SignStatus`
                                          ,`Status`
                                     FROM  `inf_customer`
                                    WHERE  `CustomerCode` = @CustomerCode
                                      AND  `Status` = 1";
                InfCustomer_Model result = db.SetCommand(strSql, db.Parameter("@CustomerCode", CustomerCode, DbType.String)).ExecuteObject<InfCustomer_Model>();
                return result;
            }
        }

        public int NameAuthenticate(string CustomerCode, string Name, string IDNumber, int UserID)
        {
            DateTime now = DateTime.Now;
            using (DbManager db = new DbManager())
            {
                db.BeginTransaction();
                string strCusUpd = @" UPDATE  `Inf_Customer`
                                         SET  `Name` = @Name
                                             ,`IDNumber` = @IDNumber
                                             ,`UpdateTime` = @now
                                             ,`Updater` = @UserID
                                       WHERE  `CustomerCode` = @CustomerCode ";
                int row = db.SetCommand(strCusUpd
                    , db.Parameter("@CustomerCode", CustomerCode, DbType.String)
                    , db.Parameter("@Name", Name, DbType.String)
                    , db.Parameter("@IDNumber", IDNumber, DbType.String)
                    , db.Parameter("@now", now, DbType.DateTime)
                    , db.Parameter("@UserID", UserID, DbType.Int32)).ExecuteNonQuery();

                if (row <= 0)
                {
                    db.RollbackTransaction();
                    return 0;
                }

                string strMemSel = @" SELECT COUNT(1) FROM `Inf_Member` WHERE `CustomerCode` = @CustomerCode ";
                row = db.SetCommand(strMemSel
                    , db.Parameter("@CustomerCode", CustomerCode, DbType.String)).ExecuteScalar<int>();

                if (row > 0)
                {
                    string strMemUpd = @" UPDATE  `Inf_Member`
                                         SET  `IDNumber` = @IDNumber
                                             ,`UpdateTime` = @now
                                             ,`Updater` = @UserID
                                       WHERE  `CustomerCode` = @CustomerCode ";
                    row = db.SetCommand(strMemUpd
                        , db.Parameter("@CustomerCode", CustomerCode, DbType.String)
                        , db.Parameter("@Name", Name, DbType.String)
                        , db.Parameter("@IDNumber", IDNumber, DbType.String)
                        , db.Parameter("@now", now, DbType.DateTime)
                        , db.Parameter("@UserID", UserID, DbType.Int32)).ExecuteNonQuery();

                    if (row <= 0)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }
                }
                db.CommitTransaction();
                return 1;
            }
        }
        public CustomerInfo_Model GetCustomerInfo(string CustomerCode)
        {
            using (DbManager db = new DbManager())
            {
                string strSqlCus = @" SELECT  `UserID`
                                          ,`MemberCode`
                                          ,`CustomerCode`
                                          ,`Name`
                                          ,`Mobile`
                                          ,`Gender`
                                          ,`Birthday`
                                          ,`Balance`
                                          ,`IsMember`
                                          ,`IDNumber`
                                          ,`LastSignInDate`
                                          ,`CycleDate`
                                          ,`ContinuousDate`
                                          ,`SignStatus`
                                          ,`Status`
                                     FROM  `inf_customer`
                                    WHERE  `CustomerCode` = @CustomerCode
                                      AND  `Status` = 1";
                CustomerInfo_Model result = db.SetCommand(strSqlCus, db.Parameter("@CustomerCode", CustomerCode, DbType.String)).ExecuteObject<CustomerInfo_Model>();
                if (result.IsMember == 2)
                {
                    string strSqlMem = @" SELECT  A.`LevelID`
                                                 ,B.`Name`
                                            FROM  `Inf_Member` A, `Set_MemberLevel` B
                                           WHERE  A.`CustomerCode` = @CustomerCode 
                                             AND  B.`LevelID` = A.`LevelID`
                                             AND  A.`Status` = 1
                                             AND  B.`Status` = 1  ";
                    SetMemberLevel_Model level = db.SetCommand(strSqlMem
                                    , db.Parameter("@CustomerCode", CustomerCode, DbType.String)).ExecuteObject<SetMemberLevel_Model>();
                    result.LevelID = level.LevelID;
                    result.LevelName = level.Name;
                }
                else
                {
                    result.LevelID = 0;
                }
                return result;
            }
        }

        public int CheckCustomer(string IDNumber, int LevelID, List<string> ChannelList)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT COUNT(1)
                                        FROM  `inf_customer` A, `inf_member` B
                                       WHERE  A.`MemberCode` = B.`MemberCode`
                                         AND  A.`Status` = 1
                                         AND  B.`Status` = 1
                                         AND  A.`IDNumber` = @IDNumber
                                         AND  B.`LevelID` >= @LevelID ";

                if (ChannelList != null && ChannelList.Count > 0)
                {
                    int index = 1;
                    foreach (string item in ChannelList)
                    {
                        if (index == 1)
                        {
                            strSql += " AND B.`ChannelID` IN (" + item;
                            index++;
                        }
                        else
                        {
                            strSql += " ," + item;
                        }
                    }
                    strSql += ") ";
                }
                int result = db.SetCommand(strSql
                    , db.Parameter("@IDNumber", IDNumber, DbType.String)
                    , db.Parameter("@LevelID", LevelID, DbType.Int32)).ExecuteScalar<int>();

                if (result != 1)
                {
                    return 0;
                }
                else if (result > 1)
                {
                    return 2;
                }

                return 1;
            }
        }
    }
}
