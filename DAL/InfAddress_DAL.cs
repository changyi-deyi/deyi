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
    public class InfAddress_DAL
    {
        #region 构造类实例
        public static InfAddress_DAL Instance
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
            internal static readonly InfAddress_DAL instance = new InfAddress_DAL();
        }

        #endregion
        public List<InfAddress_Model> GetAddress(string CustomerCode)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT  A.`ID`
                                          ,A.`CustomerCode`
                                          ,A.`ProvinceID`
                                          ,A.`CityID`
                                          ,A.`DistrictID`
                                          ,A.`Address`
                                          ,A.`Person`
                                          ,A.`Phone`
                                          ,A.`IsDefault`
                                          ,B.`REGION_NAME`  AS  `ProvinceName` 
                                          ,C.`REGION_NAME`  AS  `CityName`
                                          ,D.`REGION_NAME`  AS  `DistrictName`
                                    FROM   `Inf_Address` A, `Set_District` B, `Set_District` C, `Set_District` D
                                   WHERE   A.`CustomerCode` = @CustomerCode
                                     AND   A.`ProvinceID` = B.`REGION_CODE`
                                     AND   A.`CityID` = C.`REGION_CODE`
                                     AND   A.`DistrictID` = D.`REGION_CODE`
                                     AND   A.`Status` = 1 ";
                List<InfAddress_Model> result = db.SetCommand(strSql, db.Parameter("@CustomerCode", CustomerCode, DbType.String)).ExecuteList<InfAddress_Model>();
                return result;
            }
        }


        public int SaveAddress(SaveAddress_Model model)
        {
            using (DbManager db = new DbManager())
            {
                db.BeginTransaction();
                //变更默认
                if (model.IsDefault == 1)
                {
                    string strIns = @"  UPDATE  `Inf_Address`
                                           SET  `IsDefault` = 2
                                         WHERE  `CustomerCode` = @CustomerCode ";

                    int rows = db.SetCommand(strIns
                        , db.Parameter("@CustomerCode", model.CustomerCode, DbType.String)).ExecuteNonQuery();
                    if (rows < 0)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }
                }
                //新增地址
                if (model.IsNew == 1)
                {
                    string strIns = @" INSERT `Inf_Address` (`CustomerCode`, `ProvinceID`, `CityID`, `DistrictID`, `Address`, `Person`, `Phone`, `IsDefault`, `Status`, `CreatetTime`, `Creator`)
                                        VALUES (@CustomerCode, @ProvinceID, @CityID, @DistrictID, @Address, @Person, @Phone, @IsDefault, 1, @now, @UserID)";

                    int rows = db.SetCommand(strIns
                        , db.Parameter("@CustomerCode", model.CustomerCode, DbType.String)
                        , db.Parameter("@ProvinceID", model.ProvinceID, DbType.Int32)
                        , db.Parameter("@CityID", model.CityID, DbType.Int32)
                        , db.Parameter("@DistrictID", model.DistrictID, DbType.Int32)
                        , db.Parameter("@Address", model.Address, DbType.String)
                        , db.Parameter("@Person", model.Name, DbType.String)
                        , db.Parameter("@Phone", model.Phone, DbType.String)
                        , db.Parameter("@IsDefault", model.IsDefault, DbType.Int32)
                        , db.Parameter("@now", DateTime.Now, DbType.DateTime)
                        , db.Parameter("@UserID", model.UserID, DbType.Int32)).ExecuteNonQuery();
                    if (rows <= 0)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }
                }
                //变更地址
                else if (model.IsNew == 2)
                {
                    string strUpd = @" UPDATE  `Inf_Address`
                                          SET  `ProvinceID` = @ProvinceID
                                              ,`CityID` = @CityID
                                              ,`DistrictID` = @DistrictID
                                              ,`Address` = @Address
                                              ,`Person` = @Person
                                              ,`Phone` = @Phone
                                              ,`IsDefault` = @IsDefault
                                              ,`UpdateTime` = @now
                                              ,`Updater` = @UserID
                                        WHERE  `ID` = @ID ";
                    int rows = db.SetCommand(strUpd
                        , db.Parameter("@ProvinceID", model.ProvinceID, DbType.Int32)
                        , db.Parameter("@CityID", model.CityID, DbType.Int32)
                        , db.Parameter("@DistrictID", model.DistrictID, DbType.Int32)
                        , db.Parameter("@Address", model.Address, DbType.String)
                        , db.Parameter("@Person", model.Name, DbType.String)
                        , db.Parameter("@Phone", model.Phone, DbType.String)
                        , db.Parameter("@IsDefault", model.IsDefault, DbType.Int32)
                        , db.Parameter("@now", DateTime.Now, DbType.DateTime)
                        , db.Parameter("@UserID", model.UserID, DbType.Int32)
                        , db.Parameter("@ID", model.ID, DbType.Int32)).ExecuteNonQuery();
                    if (rows <= 0)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }
                }
                db.CommitTransaction();
                return 1;
            }
        }

        public int DeleteAddress(SaveAddress_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strUpd = @" UPDATE  `Inf_Address`
                                      SET  `Status` = 2
                                          ,`UpdateTime` = @now
                                          ,`Updater` = @UserID
                                    WHERE  `ID` = @ID ";
                int rows = db.SetCommand(strUpd
                    , db.Parameter("@now", DateTime.Now, DbType.DateTime)
                    , db.Parameter("@UserID", model.UserID, DbType.Int32)
                    , db.Parameter("@ID", model.ID, DbType.Int32)).ExecuteNonQuery();
                if (rows <= 0)
                {
                    return 0;
                }
                return 1;
            }
        }
    }
}
