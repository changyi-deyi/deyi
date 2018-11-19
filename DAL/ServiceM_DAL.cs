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
    public class ServiceM_DAL
    {
        #region 构造类实例
        public static ServiceM_DAL Instance
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
            internal static readonly ServiceM_DAL instance = new ServiceM_DAL();
        }

        #endregion

        public List<Service_Model> getServiceList(int Status, int StartCount, int EndCount)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT a.*,b.`Sort` FROM `Inf_Service` a 
                                INNER JOIN `inf_servicesort` b 
                                ON a.`ServiceCode` = b.`ServiceCode` 
                                WHERE 1=1  {0}  ORDER BY b.`Sort` LIMIT @StartCount,@EndCount ";

                string strWhere = "";

                if (Status > 0)
                {
                    strWhere += " and a.Status=@Status ";
                }

                strSql = string.Format(strSql, strWhere);

                List<Service_Model> result = db.SetCommand(strSql
                     , db.Parameter("@Status", Status, DbType.Int32)
                     , db.Parameter("@StartCount", StartCount, DbType.Int32)
                     , db.Parameter("@EndCount", EndCount, DbType.Int32)).ExecuteList<Service_Model>();



                return result;
            }
        }


        public Service_Model getServiceDetail(string ServiceCode)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT * FROM `Inf_Service` WHERE `ServiceCode` =@ServiceCode  ";


                Service_Model result = db.SetCommand(strSql
                     , db.Parameter("@ServiceCode", ServiceCode, DbType.String)).ExecuteObject<Service_Model>();



                return result;
            }
        }

        public int addService(Service_Model model)
        {
            using (DbManager db = new DbManager())
            {
                db.BeginTransaction();
                var outParmeter = db.OutputParameter("@Result", DbType.String);
                var sv = db.SetCommand(CommandType.StoredProcedure, "GetSerialNo"
                                  , db.Parameter("@TN", "inf_service", DbType.String)
                                  , outParmeter).ExecuteScalar<string>();
                string ServiceCode = outParmeter.Value.ToString();

                if (string.IsNullOrEmpty(ServiceCode))
                {
                    db.RollbackTransaction();
                    return 0;
                }

                string strSql = @" INSERT INTO `inf_service` 
                            (`ServiceCode`,`Name`,`Introduct`,`OriginPrice`,`PromPrice`,`ExchangePrice`,`Status`,`CreatetTime`,`Creator`,`Summary`,`ListImageURL`,`IsVisible`) 
                            VALUES
                            (@ServiceCode,@Name,@Introduct,@OriginPrice,@PromPrice,@ExchangePrice,1,@CreatetTime,@Creator,@Summary,@ListImageURL,@IsVisible)   ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@ServiceCode", ServiceCode, DbType.String)
                     , db.Parameter("@Name", model.Name, DbType.String)
                     , db.Parameter("@Introduct", model.Introduct, DbType.String)
                     , db.Parameter("@OriginPrice", model.OriginPrice, DbType.Decimal)
                     , db.Parameter("@PromPrice", model.PromPrice, DbType.Decimal)
                     , db.Parameter("@ExchangePrice", model.ExchangePrice, DbType.Decimal)
                     , db.Parameter("@CreatetTime", model.CreatetTime, DbType.DateTime)
                     , db.Parameter("@Creator", model.Creator, DbType.Int32)
                     , db.Parameter("@Summary", model.Summary, DbType.String)
                     , db.Parameter("@ListImageURL", model.ListImageURL, DbType.String)
                     , db.Parameter("@IsVisible", model.IsVisible, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    db.RollbackTransaction();
                    return 0;
                }

                string strSqlSortCount = " SELECT COUNT(0) FROM inf_servicesort  ";

                int count = db.SetCommand(strSqlSortCount).ExecuteScalar<int>();

                string strSqlSort = @" INSERT INTO `inf_servicesort` 
                                (`ServiceCode`, `Sort`) 
                                VALUES
                                (@ServiceCode, @Sort) ";
                rows = db.SetCommand(strSqlSort
                   , db.Parameter("@ServiceCode", ServiceCode, DbType.String)
                   , db.Parameter("@Sort", count, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    db.RollbackTransaction();
                    return 0;
                }


                db.CommitTransaction();
                return 1;
            }
        }

        public int updateService(Service_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE 
                              `inf_service` 
                            SET
                              `Name` =@Name,
                              `Introduct` =@Introduct,
                              `OriginPrice` =@OriginPrice,
                              `PromPrice` =@PromPrice,
                              `ExchangePrice` =@ExchangePrice,
                              `UpdateTime` =@UpdateTime,
                              `Updater` =@Updater,
                              `Summary` =@Summary,
                              `ListImageURL` =@ListImageURL,
                              `IsVisible` =@IsVisible
                            WHERE `ServiceCode` =@ServiceCode    ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@Name", model.Name, DbType.String)
                     , db.Parameter("@Introduct", model.Introduct, DbType.String)
                     , db.Parameter("@OriginPrice", model.OriginPrice, DbType.Decimal)
                     , db.Parameter("@PromPrice", model.PromPrice, DbType.Decimal)
                     , db.Parameter("@ExchangePrice", model.ExchangePrice, DbType.Decimal)
                     , db.Parameter("@Updater", model.Updater, DbType.Int32)
                     , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                     , db.Parameter("@Summary", model.Summary, DbType.String)
                     , db.Parameter("@ListImageURL", model.ListImageURL, DbType.String)
                     , db.Parameter("@IsVisible", model.IsVisible, DbType.Int32)
                     , db.Parameter("@ServiceCode", model.ServiceCode, DbType.String)).ExecuteNonQuery();

                if (rows != 1)
                {
                    return 0;
                }

                return 1;
            }
        }

        public int deleteService(Service_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE 
                                  `Inf_Service` 
                                SET
                                  `Status` =@Status,
                                  `UpdateTime` =@UpdateTime,
                                  `Updater` =@Updater 
                                WHERE `ServiceCode` =@ServiceCode   ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@Status", model.Status, DbType.Int32)
                     , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                     , db.Parameter("@Updater", model.Updater, DbType.Int32)
                     , db.Parameter("@ServiceCode", model.ServiceCode, DbType.String)).ExecuteNonQuery();

                if (rows != 1)
                {
                    return 0;
                }

                return 1;
            }
        }

        #region servicedoctor
        public List<ServiceDoctor_Model> getServiceDoctorList(string ServiceCode)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT 
                                  a.`Name` as DoctorName,a.`DoctorCode`,b.`IsBargain`,b.`Price`,b.`ServiceCode`
                                FROM
                                `inf_doctor` a 
                                LEFT JOIN `inf_doctorservice` b 
                                ON a.`DoctorCode` = b.`DoctorCode` AND b.`ServiceCode` = @ServiceCode
                                WHERE a.`Status` = 1 ";

                List<ServiceDoctor_Model> result = db.SetCommand(strSql
                     , db.Parameter("@ServiceCode", ServiceCode, DbType.String)).ExecuteList<ServiceDoctor_Model>();



                return result;
            }
        }

        public int addServiceDoctor(string ServiceCode, List<ServiceDoctor_Model> DoctorList, int UserID)
        {
            using (DbManager db = new DbManager())
            {
                db.BeginTransaction();
                DateTime dt = DateTime.Now.ToLocalTime();
                string strSql = @" DELETE FROM  inf_doctorservice WHERE `ServiceCode`=@ServiceCode ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@ServiceCode", ServiceCode, DbType.String)).ExecuteNonQuery();


                string strSqlAdd = @" INSERT INTO `inf_doctorservice` 
                        (`DoctorCode`,`ServiceCode`,`IsBargain`,`Price`,`Status`,`CreatetTime`,`Creator`) 
                        VALUES
                        (@DoctorCode,@ServiceCode,@IsBargain,@Price,1,@CreatetTime,@Creator) ; ";

                foreach (ServiceDoctor_Model item in DoctorList)
                {
                    rows = db.SetCommand(strSqlAdd
                     , db.Parameter("@DoctorCode", item.DoctorCode, DbType.String)
                     , db.Parameter("@ServiceCode", ServiceCode, DbType.String)
                     , db.Parameter("@IsBargain", item.IsBargain, DbType.Int32)
                     , db.Parameter("@Price", item.Price, DbType.Decimal)
                     , db.Parameter("@CreatetTime", item.CreatetTime, DbType.DateTime)
                     , db.Parameter("@Creator", item.Creator, DbType.Int32)).ExecuteNonQuery();

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

        #region serviceLevel
        public List<MemberService_Model> getMemberServiceList(string ServiceCode)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" 
                        SELECT a.`Name` AS LevelName ,IFNULL(b.`ID`,0) AS ID,a.`LevelID`,b.`Discount`  FROM `set_memberlevel` a 
                        LEFT JOIN `inf_memberservicediscount` b
                        ON a.`LevelID` = b.`LevelID` AND b.`ServiceCode` =@ServiceCode
                         WHERE a.`Status` = 1";

                List<MemberService_Model> result = db.SetCommand(strSql
                     , db.Parameter("@ServiceCode", ServiceCode, DbType.String)).ExecuteList<MemberService_Model>();



                return result;
            }
        }

        public int addMemberService(MemberService_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" INSERT INTO `inf_memberservicediscount` 
                            (`LevelID`,`ServiceCode`,`Discount`,`Status`,`CreatetTime`,`Creator`) 
                            VALUES
                            (@LevelID,@ServiceCode,@Discount,1,@CreatetTime,@Creator)  ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@LevelID", model.LevelID, DbType.Int32)
                     , db.Parameter("@ServiceCode", model.ServiceCode, DbType.String)
                     , db.Parameter("@Discount", model.Discount, DbType.Decimal)
                     , db.Parameter("@CreatetTime", model.CreatetTime, DbType.DateTime)
                     , db.Parameter("@Creator", model.Creator, DbType.Int32)).ExecuteNonQuery();


                if (rows != 1)
                {
                    db.RollbackTransaction();
                    return 0;
                }

                return 1;
            }
        }

        public int UpdatedMemberService(MemberService_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE 
                              `inf_memberservicediscount` 
                            SET
                              `Discount` =@Discount,
                              `UpdateTime` =@UpdateTime,
                              `Updater` =@Updater 
                            WHERE `ID` =@ID AND `LevelID` =@LevelID and  `ServiceCode` =@ServiceCode  ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@LevelID", model.LevelID, DbType.Int32)
                     , db.Parameter("@ServiceCode", model.ServiceCode, DbType.String)
                     , db.Parameter("@Discount", model.Discount, DbType.Decimal)
                     , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                     , db.Parameter("@Updater", model.Updater, DbType.Int32)
                     , db.Parameter("@ID", model.ID, DbType.Int32)).ExecuteNonQuery();


                if (rows != 1)
                {
                    db.RollbackTransaction();
                    return 0;
                }

                return 1;
            }
        }



        #endregion


        #region img
        public List<ServiceImg_Model> getServiceImg(string ServiceCode)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT * FROM `ima_service` WHERE `ServiceCode`=@ServiceCode and Status = 1 ";

                List<ServiceImg_Model> result = db.SetCommand(strSql
                     , db.Parameter("@ServiceCode", ServiceCode, DbType.String)).ExecuteList<ServiceImg_Model>();

                return result;
            }
        }


        public int addServiceImg(ServiceImg_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @"INSERT INTO `ima_service` 
                            (`ServiceCode`,`FileName`,`Status`,`CreatetTime`,`Creator`) 
                            VALUES
                            (@ServiceCode,@FileName,1,@CreatetTime,@Creator) 
                              ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@ServiceCode", model.ServiceCode, DbType.String)
                     , db.Parameter("@FileName", model.FileName, DbType.String)
                     , db.Parameter("@CreatetTime", model.CreatetTime, DbType.DateTime)
                     , db.Parameter("@Creator", model.Creator, DbType.Int32)).ExecuteNonQuery();


                if (rows != 1)
                {
                    return 0;
                }

                return 1;
            }
        }

        public int DeleteServiceImg(ServiceImg_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE `ima_service`  
                                SET `Status`=@Status
                                WHERE `ID` =@ID  ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@Status", model.Status, DbType.Int32)
                     , db.Parameter("@ID", model.ID, DbType.Int32)).ExecuteNonQuery();


                if (rows != 1)
                {
                    db.RollbackTransaction();
                    return 0;
                }

                return 1;
            }
        }


        #endregion


        public int UpdateSort(string ServiceCode, int Sort)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE 
                                  `inf_servicesort` 
                                SET
                                  `Sort` =@Sort
                                WHERE `ServiceCode` =@ServiceCode   ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@Sort", Sort, DbType.String)
                     , db.Parameter("@ServiceCode", ServiceCode, DbType.String)).ExecuteNonQuery();

                if (rows != 1)
                {
                    return 0;
                }

                return 1;
            }
        }

    }
}
