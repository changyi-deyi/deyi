using BLToolkit.Data;
using Model.Manage_Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public  class UserM_DAL
    {
        #region 构造类实例
        public static UserM_DAL Instance
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
            internal static readonly UserM_DAL instance = new UserM_DAL();
        }

        #endregion

        public User_Model getUserByAccountPassword(string LoginUserName, string Password)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT * FROM `bas_user` WHERE `LoginUserName`=@LoginUserName AND `Password` =@Password and status = 1 ";

                User_Model result = db.SetCommand(strSql
                     , db.Parameter("@LoginUserName", LoginUserName, DbType.String)
                     , db.Parameter("@Password", Password, DbType.String)).ExecuteObject<User_Model>();



                return result;
            }
        }




        public Doctor_Model getDoctorDetail(int UserID)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT * FROM `inf_doctor` WHERE `UserID`=@UserID ";

                Doctor_Model result = db.SetCommand(strSql
                     , db.Parameter("@UserID", UserID, DbType.Int32)).ExecuteObject<Doctor_Model>();

                return result;
            }
        }


        public Staff_Model getStaffDetail(int UserID)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT * FROM `inf_staff` WHERE `UserID`=@UserID ";

                Staff_Model result = db.SetCommand(strSql
                     , db.Parameter("@UserID", UserID, DbType.Int32)).ExecuteObject<Staff_Model>();

                return result;
            }
        }


        public int addUser(UserOperate_Model model)
        {
            using (DbManager db = new DbManager())
            {
                db.BeginTransaction();
                string strSqlUser = @" INSERT INTO `bas_user` 
                                    (`Type`,`LoginUserName`,`Password`,`Status`,`CreatetTime`,`Creator`) 
                                      VALUES
                                    (@Type,@LoginUserName,@Password,@Status,@CreatetTime,@Creator) ; SELECT @@identity";

                int UserID = db.SetCommand(strSqlUser
                     , db.Parameter("@Type", model.User.Type, DbType.Int32)
                     , db.Parameter("@Status", model.User.Status, DbType.Int32)
                     , db.Parameter("@LoginUserName", model.User.LoginUserName, DbType.String)
                     , db.Parameter("@Password", model.User.Password, DbType.String)
                     , db.Parameter("@CreatetTime", model.User.CreatetTime, DbType.DateTime)
                     , db.Parameter("@Creator", model.User.Creator, DbType.Int32)).ExecuteScalar<int>();

                if (UserID < 1)
                {
                    db.RollbackTransaction();
                    return 0;
                }
                int rows = 0;
                if (model.User.Type == 2)
                {
                    var outParmeter = db.OutputParameter("@Result", DbType.String);
                    var dc = db.SetCommand(CommandType.StoredProcedure, "GetSerialNo"
                                      , db.Parameter("@TN", "inf_doctor", DbType.String)
                                      , outParmeter).ExecuteScalar<string>();
                    string DoctorCode = outParmeter.Value.ToString();

                    if (string.IsNullOrEmpty(DoctorCode))
                    {

                        db.RollbackTransaction();
                        return 0;
                    }

                    string strSql = @" INSERT INTO `inf_doctor` 
                                    (`UserID`,`DoctorCode`,`Name`,`Gender`,`Birthday`,`HospitalID`,`DepartmentID`,`TitleID`,`GoodAt`,`Introduction`,`Phone`,`ImageURL`,`Type`,`Status`,`CreatetTime`,`Weights`,`Creator`) 
                                    VALUES
                                    (@UserID,@DoctorCode,@Name,@Gender,@Birthday,@HospitalID,@DepartmentID,@TitleID,@GoodAt,@Introduction,@Phone,@ImageURL,@Type,@Status,@CreatetTime,@Creator,@Weights)   ";



                    rows = db.SetCommand(strSql
                         , db.Parameter("@UserID", UserID, DbType.Int32)
                         , db.Parameter("@DoctorCode", DoctorCode, DbType.String)
                         , db.Parameter("@Name", model.Doctor.Name, DbType.String)
                         , db.Parameter("@Gender", model.Doctor.Gender, DbType.Int32)
                         , db.Parameter("@Birthday", model.Doctor.Birthday, DbType.DateTime)
                         , db.Parameter("@HospitalID", model.Doctor.HospitalID, DbType.Int32)
                         , db.Parameter("@DepartmentID", model.Doctor.DepartmentID, DbType.Int32)
                         , db.Parameter("@TitleID", model.Doctor.TitleID, DbType.String)
                         , db.Parameter("@GoodAt", model.Doctor.GoodAt, DbType.String)
                         , db.Parameter("@Introduction", model.Doctor.Introduction, DbType.String)
                         , db.Parameter("@Phone", model.Doctor.Phone, DbType.String)
                         , db.Parameter("@ImageURL", model.Doctor.ImageURL, DbType.String)
                         , db.Parameter("@Type", model.Doctor.Type, DbType.Int32)
                         , db.Parameter("@Status", model.Doctor.Status, DbType.Int32)
                         , db.Parameter("@CreatetTime", model.Doctor.CreatetTime, DbType.DateTime)
                         , db.Parameter("@Creator", model.Doctor.Creator, DbType.Int32)
                         , db.Parameter("@Weights", model.Doctor.Weights, DbType.Int32)).ExecuteNonQuery();

                    if (rows != 1)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }

                    if (model.Doctor.listTag != null && model.Doctor.listTag.Count > 0) {
                        string strSqlTag = @" INSERT INTO `ope_doctortag` (`DoctorCode`, `TagID`) VALUES (@DoctorCode, @TagID)  ";

                        foreach (int item in model.Doctor.listTag)
                        {
                            rows = db.SetCommand(strSqlTag
                                , db.Parameter("@DoctorCode", DoctorCode, DbType.String)
                                , db.Parameter("@TagID", item, DbType.Int32)).ExecuteNonQuery();

                            if (rows != 1)
                            {
                                db.RollbackTransaction();
                                return 0;
                            }
                        }
                    }

                }
                else if (model.User.Type == 3) {
                    var outParmeter = db.OutputParameter("@Result", DbType.String);
                    var sf = db.SetCommand(CommandType.StoredProcedure, "GetSerialNo"
                                      , db.Parameter("@TN", "inf_staff", DbType.String)
                                      , outParmeter).ExecuteScalar<string>();
                    string StaffCode = outParmeter.Value.ToString();

                    if (string.IsNullOrEmpty(StaffCode))
                    {

                        db.RollbackTransaction();
                        return 0;
                    }

                    string strSql = @" INSERT INTO `inf_staff` 
                                (`UserID`,`StaffCode`,`Name`,`Mobile`,`Gender`,`Role`,`Status`,`CreatetTime`,`Creator`) 
                                VALUES
                                (@UserID,@StaffCode,@Name,@Mobile,@Gender,@Role,1,@CreatetTime,@Creator)   ";



                    rows = db.SetCommand(strSql
                         , db.Parameter("@UserID", UserID, DbType.Int32)
                         , db.Parameter("@StaffCode", StaffCode, DbType.String)
                         , db.Parameter("@Name", model.Staff.Name, DbType.String)
                         , db.Parameter("@Gender", model.Staff.Gender, DbType.Int32)
                         , db.Parameter("@Mobile", model.Staff.Mobile, DbType.String)
                         , db.Parameter("@Role", model.Staff.Role, DbType.Int32)
                         , db.Parameter("@CreatetTime", model.Staff.CreatetTime, DbType.DateTime)
                         , db.Parameter("@Creator", model.Staff.Creator, DbType.Int32)).ExecuteNonQuery();

                    if (rows != 1)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }
                }

                db.CommitTransaction();
                return 1;
            }
        }



        public int deleteUser(UserDeleteOperate_Model model )
        {
            using (DbManager db = new DbManager())
            {
                db.BeginTransaction();
                string strSqlUser = @"Update `bas_user` set  
                                  `Status` =@Status,
                                  `UpdateTime` =@UpdateTime,
                                  `Updater` =@Updater 
                                  where  `UserID` =@UserID   ";

                int rows = db.SetCommand(strSqlUser
                     , db.Parameter("@Status", model.Status, DbType.Int32)
                     , db.Parameter("@UserID", model.UserID, DbType.Int32)
                     , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                     , db.Parameter("@Updater", model.Updater, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    db.RollbackTransaction();
                    return 0;
                }


                if (model.Type == 1)
                {
                    string strSql = @" UPDATE 
                                  `inf_customer` 
                                SET
                                  `Status` =@Status,
                                  `UpdateTime` =@UpdateTime,
                                  `Updater` =@Updater 
                                WHERE `CustomerCode` =@CustomerCode and  `UserID` =@UserID   ";

                    rows = db.SetCommand(strSql
                     , db.Parameter("@Status", model.Status, DbType.Int32)
                     , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                     , db.Parameter("@Updater", model.Updater, DbType.Int32)
                     , db.Parameter("@CustomerCode", model.UserCode, DbType.String)
                     , db.Parameter("@UserID", model.UserID, DbType.Int32)).ExecuteNonQuery();

                    if (rows != 1)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }
                }
                else if (model.Type == 2)
                {
                    string strSql = @" UPDATE 
                                  `Inf_Doctor` 
                                SET
                                  `Status` =@Status,
                                  `UpdateTime` =@UpdateTime,
                                  `Updater` =@Updater 
                                WHERE `DoctorCode` =@DoctorCode and  `UserID` =@UserID   ";

                    rows = db.SetCommand(strSql
                     , db.Parameter("@Status", model.Status, DbType.Int32)
                     , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                     , db.Parameter("@Updater", model.Updater, DbType.Int32)
                     , db.Parameter("@DoctorCode", model.UserCode, DbType.String)
                     , db.Parameter("@UserID", model.UserID, DbType.Int32)).ExecuteNonQuery();

                    if (rows != 1)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }
                }
                else if (model.Type == 3) {
                    string strSql = @" UPDATE 
                                  `inf_staff` 
                                SET
                                  `Status` =@Status,
                                  `UpdateTime` =@UpdateTime,
                                  `Updater` =@Updater 
                                WHERE `StaffCode` =@StaffCode and  `UserID` =@UserID   ";

                    rows = db.SetCommand(strSql
                     , db.Parameter("@Status", model.Status, DbType.Int32)
                     , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                     , db.Parameter("@Updater", model.Updater, DbType.Int32)
                     , db.Parameter("@StaffCode", model.UserCode, DbType.String)
                     , db.Parameter("@UserID", model.UserID, DbType.Int32)).ExecuteNonQuery();

                    if (rows != 1)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }
                }


                db.CommitTransaction();
                return 1;
            }
        }

        #region docotr
        public List<Doctor_Model> getDoctorList(string Name,int HospitalID,int DepartmentID, int StartCount, int EndCount)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT a.*,b.`HospitalName`,c.`DepartmentName` FROM `inf_doctor` a
                                LEFT JOIN `set_hospital` b
                                ON a.`HospitalID` = b.`HospitalID`
                                LEFT JOIN `set_department` c
                                ON a.`DepartmentID` = c.`DepartmentID` where 1=1 and a.`status` = 1 {0} ORDER BY a.Weights DESC,a.`Name` LIMIT @StartCount,@EndCount ";

                string strWhere = "";

                if (!string.IsNullOrEmpty(Name))
                {
                    strWhere += " and a.`Name` like @Name";
                }

                if (HospitalID > 0)
                {
                    strWhere += " and a.`HospitalID`=@HospitalID ";
                }

                if (DepartmentID > 0)
                {
                    strWhere += " and a.`DepartmentID`=@DepartmentID ";
                }

                strSql = string.Format(strSql, strWhere);

                List<Doctor_Model> result = db.SetCommand(strSql
                     , db.Parameter("@Name", "%" + Name + "%", DbType.String)
                     , db.Parameter("@HospitalID", HospitalID, DbType.Int32)
                     , db.Parameter("@DepartmentID", DepartmentID, DbType.Int32)
                     , db.Parameter("@StartCount", StartCount, DbType.Int32)
                     , db.Parameter("@EndCount", EndCount, DbType.Int32)).ExecuteList<Doctor_Model>();



                return result;
            }
        }


        public Doctor_Model getDoctorDetail(string DoctorCode)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT * FROM `inf_doctor` WHERE `DoctorCode`=@DoctorCode ";

                Doctor_Model result = db.SetCommand(strSql
                     , db.Parameter("@DoctorCode", DoctorCode, DbType.String)).ExecuteObject<Doctor_Model>();

                return result;
            }
        }


        public int updateDoctor(Doctor_Model model)
        {
            using (DbManager db = new DbManager())
            {
                db.BeginTransaction();
                string strSql = @" UPDATE 
                                  `Inf_Doctor` 
                                SET
                                  `Name` =@Name,
                                  `Gender` =@Gender,
                                  `Birthday` =@Birthday,
                                  `HospitalID` =@HospitalID,
                                  `DepartmentID` =@DepartmentID,
                                  `TitleID` =@TitleID,
                                  `GoodAt` =@GoodAt,
                                  `Introduction` =@Introduction,
                                  `Phone` =@Phone,
                                  `ImageURL` =@ImageURL,
                                  `Type` =@Type,
                                  `Status` =@Status,
                                  `UpdateTime` =@UpdateTime,
                                  `Updater` =@Updater ,
                                  `Weights` =@Weights 
                                WHERE `DoctorCode` =@DoctorCode and  `UserID` =@UserID  ";

                int rows = db.SetCommand(strSql
                         , db.Parameter("@Name", model.Name, DbType.String)
                         , db.Parameter("@Gender", model.Gender, DbType.Int32)
                         , db.Parameter("@Birthday", model.Birthday, DbType.DateTime)
                         , db.Parameter("@HospitalID", model.HospitalID, DbType.Int32)
                         , db.Parameter("@DepartmentID", model.DepartmentID, DbType.Int32)
                         , db.Parameter("@TitleID", model.TitleID, DbType.String)
                         , db.Parameter("@GoodAt", model.GoodAt, DbType.String)
                         , db.Parameter("@Introduction", model.Introduction, DbType.String)
                         , db.Parameter("@Phone", model.Phone, DbType.String)
                         , db.Parameter("@ImageURL", model.ImageURL, DbType.String)
                         , db.Parameter("@Type", model.Type, DbType.Int32)
                         , db.Parameter("@Status", model.Status, DbType.Int32)
                         , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                         , db.Parameter("@Updater", model.Updater, DbType.Int32)
                         , db.Parameter("@UserID", model.UserID, DbType.Int32)
                         , db.Parameter("@DoctorCode", model.DoctorCode, DbType.String)
                         , db.Parameter("@Weights", model.Weights, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    db.RollbackTransaction();
                    return 0;
                }

                string strSqldelete = @" delete from ope_doctortag where DoctorCode=@DoctorCode ";

                rows = db.SetCommand(strSqldelete
                         , db.Parameter("@DoctorCode", model.DoctorCode, DbType.String)).ExecuteNonQuery();

                //if (rows != 1)
                //{
                //    db.RollbackTransaction();
                //    return 0;
                //}

                if (model.listTag != null && model.listTag.Count > 0)
                {
                    string strSqlTag = @" INSERT INTO `ope_doctortag` (`DoctorCode`, `TagID`) VALUES (@DoctorCode, @TagID)  ";

                    foreach (int item in model.listTag)
                    {
                        rows = db.SetCommand(strSqlTag
                            , db.Parameter("@DoctorCode", model.DoctorCode, DbType.String)
                            , db.Parameter("@TagID", item, DbType.Int32)).ExecuteNonQuery();

                        if (rows != 1)
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


        #endregion




        #region Customer
        public List<Customer_Model> getCustomerList(string Name,int LevelID, int ChannelID,int Status, int StartCount, int EndCount)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT a.*,CASE a.`IsMember` WHEN 2 THEN  c.`Name` ELSE '非会员' END AS LevelName  FROM `inf_customer` a
                                LEFT JOIN `inf_member` b 
                                ON a.`MemberCode` = b.`MemberCode`
                                LEFT JOIN `set_memberlevel` c
                                ON b.`LevelID` = c.`LevelID`
                                WHERE 1=1 AND a.`status` = 1  {0}
                                 ORDER BY a.`Name` 
                                 LIMIT @StartCount,@EndCount  ";

                string strWhere = "";

                if (!string.IsNullOrEmpty(Name))
                {
                    strWhere += " and a.`Name` like @Name";
                }

                if (LevelID > 0) {
                    strWhere += " and b.`LevelID`=@LevelID ";
                }


                if (ChannelID > 0)
                {
                    strWhere += " and b.`ChannelID`=@ChannelID ";
                }


                if (Status > 0)
                {
                    strWhere += " and a.`Status`=@Status ";
                }
                strSql = string.Format(strSql, strWhere);

                List<Customer_Model> result = db.SetCommand(strSql
                     , db.Parameter("@Name", "%" + Name + "%", DbType.String)
                     , db.Parameter("@LevelID", LevelID, DbType.Int32)
                     , db.Parameter("@ChannelID", ChannelID, DbType.Int32)
                     , db.Parameter("@Status", Status, DbType.Int32)
                     , db.Parameter("@StartCount", StartCount, DbType.Int32)
                     , db.Parameter("@EndCount", EndCount, DbType.Int32)).ExecuteList<Customer_Model>();



                return result;
            }
        }




        public Customer_Model getCustomerDetail(string CustomerCode)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT * FROM `inf_customer` WHERE `CustomerCode`=@CustomerCode ";

                Customer_Model result = db.SetCommand(strSql
                     , db.Parameter("@CustomerCode", CustomerCode, DbType.String)).ExecuteObject<Customer_Model>();

                return result;
            }
        }

        public Member_Model getMemberDetail(string MemberCode) {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT a.*,b.`Name` AS LevelName, c.`ChannelName` FROM `inf_member` a 
LEFT JOIN `set_memberlevel` b
ON a.`LevelID` = b.`LevelID`
LEFT JOIN `set_channel` c
ON a.`ChannelID` = c.`ChannelID` WHERE a.`MemberCode` =@MemberCode";

                Member_Model result = db.SetCommand(strSql
                     , db.Parameter("@MemberCode", MemberCode, DbType.String)).ExecuteObject<Member_Model>();

                return result;
            }
        }


        public List<Balance_Model> getBalanceList(string CustomerCode, string StartDate,string EndDate, int StartCount, int EndCount) {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT * FROM `ope_balance` WHERE `CustomerCode` =@CustomerCode {0}  LIMIT @StartCount,@EndCount";

                string strWhere = "";
                if (!string.IsNullOrEmpty(StartDate)) {
                    strWhere += " and `CreatetTime` >=@StartDate  ";
                }
                if (!string.IsNullOrEmpty(EndDate))
                {
                    strWhere += " and `CreatetTime` <=@EndDate  ";
                }

                strSql = string.Format(strSql, strWhere);

                List<Balance_Model> result = db.SetCommand(strSql
                     , db.Parameter("@CustomerCode", CustomerCode, DbType.String)
                     , db.Parameter("@StartDate", StartDate, DbType.String)
                     , db.Parameter("@EndDate", EndDate, DbType.String)
                     , db.Parameter("@StartCount", StartCount, DbType.Int32)
                     , db.Parameter("@EndCount", EndCount, DbType.Int32)).ExecuteList<Balance_Model>();

                return result;
            }
        }

        public int UpdateCustomer(Customer_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE 
                                  `Inf_Doctor` 
                                SET
                                  `Name` =@Name,
                                  `Gender` =@Gender,
                                  `Birthday` =@Birthday,
                                  `Type` =@Type,
                                  `UpdateTime` =@UpdateTime,
                                  `Updater` =@Updater 
                                WHERE `CustomerCode` =@CustomerCode and  `UserID` =@UserID  ";

                int rows = db.SetCommand(strSql).ExecuteNonQuery();

                if (rows != 1)
                {
                    return 0;
                }

                return 1;
            }
        }


        public List<CustomerProfile_Model> getCustomerProfile(string CustomerCode,  int VerifyStatus, int StartCount, int EndCount) {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT 
                                  a.*,b.`Name` AS CustomerName 
                                FROM
                                  `inf_customerprofile` a 
                                  LEFT JOIN `inf_customer` b 
                                    ON a.`CustomerCode` = b.`CustomerCode` 
                                WHERE  a.`status` = 1 {0}
                                ORDER BY a.`UploadTime` DESC  
                                LIMIT @StartCount,@EndCount  ";

                string strWhere = "";

                if (VerifyStatus > 0)
                {
                    strWhere += " and a.`VerifyStatus`=@VerifyStatus ";
                }

                if (!string.IsNullOrEmpty(CustomerCode)) {
                    strWhere += " and a.`CustomerCode`=@CustomerCode ";
                }
                
                strSql = string.Format(strSql, strWhere);

                List<CustomerProfile_Model> result = db.SetCommand(strSql
                     , db.Parameter("@VerifyStatus", VerifyStatus, DbType.Int32)
                     , db.Parameter("@CustomerCode", CustomerCode, DbType.String)
                     , db.Parameter("@StartCount", StartCount, DbType.Int32)
                     , db.Parameter("@EndCount", EndCount, DbType.Int32)).ExecuteList<CustomerProfile_Model>();



                return result;
            }
        }

        public List<CustomerProfileImg_Model> getCustomerProfileImg(string CustomerCode, int VerifyStatus)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @"SELECT a.* FROM `ima_customerprofile` a
                                    INNER JOIN `inf_customerprofile` b
                                    ON a.`ID` = b.`ID` 
                                    WHERE  1=1 {0}  ";

                string strWhere = "";

                if (VerifyStatus > 0)
                {
                    strWhere += " and b.`VerifyStatus`=@VerifyStatus ";
                }

                if (!string.IsNullOrEmpty(CustomerCode))
                {
                    strWhere += " and b.`CustomerCode`=@CustomerCode ";
                }

                strSql = string.Format(strSql, strWhere);

                List<CustomerProfileImg_Model> result = db.SetCommand(strSql
                     , db.Parameter("@VerifyStatus", VerifyStatus, DbType.Int32)
                     , db.Parameter("@CustomerCode", CustomerCode, DbType.String)).ExecuteList<CustomerProfileImg_Model>();



                return result;
            }
        }

        public int changeVerifyStatus(CustomerProfile_Model model)
        {
            using (DbManager db = new DbManager())
            {
                db.BeginTransaction();
                string strSql = @" UPDATE 
                                  `inf_customerprofile` 
                                SET
                                  `VerifyStatus` =@VerifyStatus,
                                  `UpdateTime` =@UpdateTime,
                                  `Updater` =@Updater 
                                WHERE `ID` =@ID ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@VerifyStatus", model.VerifyStatus, DbType.Int32)
                     , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                     , db.Parameter("@Updater", model.Updater, DbType.Int32)
                     , db.Parameter("@ID", model.ID, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    db.RollbackTransaction();
                    return 0;
                }

                if (model.VerifyStatus !=2) {
                    db.CommitTransaction();
                    return 1;
                }

                string strSqlReward = @" SELECT `UploadReward` FROM `set_getpoint` ";

                decimal Reward = db.SetCommand(strSqlReward).ExecuteScalar<decimal>();

                if (Reward > 0) {
                    string strSqlCustomer = @" SELECT CustomerCode FROM `inf_customerprofile` WHERE ID =@ID ";

                    string customerCode = db.SetCommand(strSqlCustomer
                     , db.Parameter("@ID", model.ID, DbType.Int32)).ExecuteScalar<string>();

                    if (string.IsNullOrEmpty(customerCode)) {
                        db.RollbackTransaction();
                        return 0;
                    }



                    string strSqlBalance = @"SELECT `Balance` FROM `inf_customer` WHERE `CustomerCode`=@CustomerCode ";

                    decimal Balance = db.SetCommand(strSqlBalance
                     , db.Parameter("@CustomerCode", customerCode, DbType.String)).ExecuteScalar<decimal>();

                    string strSqlAddBalance = @"INSERT INTO `ope_balance` 
                                        (`CustomerCode`,`InOutType`,`Type`,`ChangeBalance`,`Balance`,`Status`,`CreatetTime`,`Creator`,`RelatedType`,`RelatedID`) 
                                        VALUES
                                          (@CustomerCode,1,14,@ChangeBalance,@Balance,1,@CreatetTime,@Creator,5,@RelatedID) ;";

                    rows = db.SetCommand(strSqlAddBalance
                     , db.Parameter("@CustomerCode", customerCode, DbType.String)
                     , db.Parameter("@ChangeBalance", Reward, DbType.Decimal)
                     , db.Parameter("@Balance",Balance + Reward, DbType.Decimal)
                     , db.Parameter("@Creator", model.Updater, DbType.Int32)
                     , db.Parameter("@CreatetTime", model.UpdateTime, DbType.DateTime)
                     , db.Parameter("@RelatedID", model.ID, DbType.Int32)).ExecuteNonQuery();

                    if (rows != 1)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }

                    string strSqlUpdateCustomer = @" UPDATE `inf_customer` SET `Balance` = @Balance WHERE `CustomerCode`=@CustomerCode";

                    rows = db.SetCommand(strSqlUpdateCustomer
                    , db.Parameter("@CustomerCode", customerCode, DbType.String)
                    , db.Parameter("@Balance", Balance + Reward, DbType.Decimal)).ExecuteNonQuery();

                    if (rows != 1)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }

                }

                db.CommitTransaction();
                return 1;
            }
        }


        #endregion


        #region Staff
        public List<Staff_Model> getStaffList(string Name, int Role,  int StartCount, int EndCount)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT a.* FROM `inf_staff` a where 1=1 and a.`status` = 1 {0} order by a.`Name` LIMIT @StartCount,@EndCount ";

                string strWhere = "";

                if (!string.IsNullOrEmpty(Name))
                {
                    strWhere += " and a.`Name` like @Name";
                }

                if (Role > 0)
                {
                    strWhere += " and a.`Role`=@Role ";
                }

                strSql = string.Format(strSql, strWhere);

                List<Staff_Model> result = db.SetCommand(strSql
                     , db.Parameter("@Name", "%" + Name + "%", DbType.String)
                     , db.Parameter("@Role", Role, DbType.Int32)
                     , db.Parameter("@StartCount", StartCount, DbType.Int32)
                     , db.Parameter("@EndCount", EndCount, DbType.Int32)).ExecuteList<Staff_Model>();



                return result;
            }
        }


        public Staff_Model getStaffDetail(string StaffCode)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT * FROM `inf_staff` WHERE `StaffCode`=@StaffCode ";

                Staff_Model result = db.SetCommand(strSql
                     , db.Parameter("@StaffCode", StaffCode, DbType.String)).ExecuteObject<Staff_Model>();

                return result;
            }
        }


        public int updateStaff(Staff_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE 
                                  `inf_staff` 
                                SET
                                  `Name` =@Name,
                                  `Gender` =@Gender,
                                  `Mobile` =@Mobile,
                                  `Role` =@Role,
                                  `Status` =@Status,
                                  `UpdateTime` =@UpdateTime,
                                  `Updater` =@Updater 
                                WHERE `StaffCode` =@StaffCode and  `UserID` =@UserID  ";

                int rows = db.SetCommand(strSql
                         , db.Parameter("@Name", model.Name, DbType.String)
                         , db.Parameter("@Gender", model.Gender, DbType.Int32)
                         , db.Parameter("@Mobile", model.Mobile, DbType.String)
                         , db.Parameter("@Role", model.Role, DbType.Int32)
                         , db.Parameter("@Status", model.Status, DbType.Int32)
                         , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                         , db.Parameter("@Updater", model.Updater, DbType.Int32)
                         , db.Parameter("@UserID", model.UserID, DbType.Int32)
                         , db.Parameter("@StaffCode", model.StaffCode, DbType.String)).ExecuteNonQuery();

                if (rows != 1)
                {
                    return 0;
                }

                return 1;
            }
        }


        #endregion

    }
}
