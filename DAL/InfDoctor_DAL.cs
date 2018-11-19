using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Table_Model;
using Model.View_Model;
using Model.Operate_Model;
using BLToolkit.Data;

namespace DAL
{
    public class InfDoctor_DAL
    {
        #region 构造类实例
        public static InfDoctor_DAL Instance
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
            internal static readonly InfDoctor_DAL instance = new InfDoctor_DAL();
        }

        #endregion
        public List<DoctorList_Model> GetDoctorList(Doctor_Model model)
        {
            using (DbManager db = new DbManager())
            {
                List<DoctorList_Model> list = null;
                //从服务页面跳转
                if (model.service)
                {
                    string strSqlDoctor = @" SELECT A.`Name`,
                                                    A.`DoctorCode`,
	                                                A.`Gender`,
	                                                A.`ImageURL`,
	                                                A.`ServiceTimes`,
	                                                A.`Points`,
	                                                B.`TitleName`,
	                                                C.`DepartmentName`,
	                                                D.`HospitalName`,
                                                IF ((SELECT COUNT(1) FROM `Ope_FollowDoctor` 
                                                     WHERE `DoctorCode` = A.`DoctorCode`
		                                               AND `CustomerCode` = @CustomerCode) = 0, 0, 1) AS followFlg
                                                FROM
	                                                `Inf_Doctor` A,
	                                                `Set_Title` B,
	                                                `Set_Department` C,
	                                                `Set_Hospital` D,
                                                    `Inf_DoctorService` E
                                                WHERE 
                                                    A.`TitleID` = B.`TitleID`
                                                AND A.`DepartmentID` = C.`DepartmentID`
                                                AND A.`HospitalID` = D.`HospitalID`
                                                AND A.`DoctorCode` = E.`DoctorCode`
                                                AND A.`Status` = 1
                                                AND B.`Status` = 1
                                                AND C.`Status` = 1
                                                AND D.`Status` = 1 
                                                AND E.`Status` = 1
                                                AND E.`ServiceCode` = @ServiceCode";
                    if (model.DepartmentID != 0)
                    {
                        strSqlDoctor += " AND A.`DepartmentID` = @DepartmentID ";
                    }
                    strSqlDoctor += " ORDER BY A.`Weights` DESC";
                    list = db.SetCommand(strSqlDoctor
                        , db.Parameter("@CustomerCode", model.CustomerCode, DbType.String)
                        , db.Parameter("@DepartmentID", model.DepartmentID, DbType.Int32)
                        , db.Parameter("@ServiceCode", model.ServiceCode, DbType.String)).ExecuteList<DoctorList_Model>();
                    
                }
                //非服务页面跳转
                else
                {
                    string strSqlDoctor = @" SELECT A.`Name`,
                                                    A.`DoctorCode`,
	                                                A.`Gender`,
	                                                A.`ImageURL`,
	                                                A.`ServiceTimes`,
	                                                A.`Points`,
	                                                B.`TitleName`,
	                                                C.`DepartmentName`,
	                                                D.`HospitalName`,
                                                IF ((SELECT COUNT(1) FROM `Ope_FollowDoctor` 
                                                     WHERE `DoctorCode` = A.`DoctorCode`
		                                               AND `CustomerCode` = @CustomerCode) = 0, 0, 1) AS `followFlg`
                                                FROM
	                                                `Inf_Doctor` A,
	                                                `Set_Title` B,
	                                                `Set_Department` C,
	                                                `Set_Hospital` D
                                                WHERE 
                                                    A.`TitleID` = B.`TitleID`
                                                AND A.`DepartmentID` = C.`DepartmentID`
                                                AND A.`HospitalID` = D.`HospitalID`
                                                AND A.`Status` = 1
                                                AND B.`Status` = 1
                                                AND C.`Status` = 1
                                                AND D.`Status` = 1 ";
                    if (model.DepartmentID != 0)
                    {
                        strSqlDoctor += " AND A.`DepartmentID` = @DepartmentID ";
                    }

                    if (model.followFlg == 1)
                    {
                        strSqlDoctor += " AND (SELECT COUNT(1) FROM `Ope_FollowDoctor` WHERE `DoctorCode` = A.`DoctorCode` AND `CustomerCode` = @CustomerCode) > 0 ";
                    }
                    strSqlDoctor += " ORDER BY A.`Weights` DESC";
                    list = db.SetCommand(strSqlDoctor
                        , db.Parameter("@CustomerCode", model.CustomerCode, DbType.String)
                        , db.Parameter("@DepartmentID", model.DepartmentID, DbType.Int32)).ExecuteList<DoctorList_Model>();
                    
                }

                if (list != null && list.Count > 0)
                {
                    string strSqlTag = @" SELECT  A.`TagName`
                                            FROM  `Set_Tag` A, `Ope_DoctorTag` B
                                           WHERE  A.`TagID` = B.`TagID`
                                             AND  B.`DoctorCode` = @DoctorCode";
                    foreach(DoctorList_Model item in list)
                    {
                        List<string> tags = db.SetCommand(strSqlTag
                        , db.Parameter("@DoctorCode", item.DoctorCode, DbType.String)).ExecuteScalarList<string>();

                        item.tag = tags;
                    }
                }

                return list;
            }
        }

        public DoctorList_Model GetDoctorDetail(DoctorDetailIn_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSqlDoctor = @" SELECT A.`Name`,
                                                A.`DoctorCode`,
	                                            A.`Gender`,
	                                            A.`ImageURL`,
	                                            A.`ServiceTimes`,
	                                            A.`Points`,
	                                            B.`TitleName`,
	                                            C.`DepartmentName`,
	                                            D.`HospitalName`,
                                            IF ((SELECT COUNT(1) FROM `Ope_FollowDoctor` 
                                                    WHERE `DoctorCode` = A.`DoctorCode`
		                                            AND `CustomerCode` = @CustomerCode) = 0, 0, 1) AS followFlg
                                            FROM
	                                            `Inf_Doctor` A,
	                                            `Set_Title` B,
	                                            `Set_Department` C,
	                                            `Set_Hospital` D
                                            WHERE 
                                                A.`TitleID` = B.`TitleID`
                                            AND A.`DepartmentID` = C.`DepartmentID`
                                            AND A.`HospitalID` = D.`HospitalID`
                                            AND A.`Status` = 1
                                            AND B.`Status` = 1
                                            AND C.`Status` = 1
                                            AND D.`Status` = 1 
                                            AND A.`DoctorCode` = @DoctorCode";

                DoctorList_Model res = db.SetCommand(strSqlDoctor
                    , db.Parameter("@CustomerCode", model.CustomerCode, DbType.String)
                    , db.Parameter("@DoctorCode", model.DoctorCode, DbType.String)).ExecuteObject<DoctorList_Model>();


                if (res != null )
                {
                    string strSqlTag = @" SELECT  A.`TagName`
                                            FROM  `Set_Tag` A, `Ope_DoctorTag` B
                                           WHERE  A.`TagID` = B.`TagID`
                                             AND  B.`DoctorCode` = @DoctorCode";
                     List<string> tags = db.SetCommand(strSqlTag
                        , db.Parameter("@DoctorCode", res.DoctorCode, DbType.String)).ExecuteScalarList<string>();

                    res.tag = tags;
                }

                return res;
            }
        }

        public List<DoctorService_Model> GetDoctorService(string DoctorCode)
        {
            using (DbManager db = new DbManager())
            {
                string strSqlDoctor = @" SELECT
	                                        A.`ServiceCode`,
	                                        A.`Name`,
	                                        A.`Summary`,
	                                        A.`Introduct`,
	                                        A.`PriceType`,
	                                        A.`OriginPrice`,
	                                        A.`PromPrice`,
	                                        A.`ExchangePrice`,
	                                        A.`ListImageURL`,
	                                        B.`DoctorCode`,
	                                        B.`IsBargain`,
	                                        B.`Price`,
	                                        C.`Sort`,
                                            CASE 
                                                WHEN B.`IsBargain` = 2
                                                THEN B.`Price`
                                                ELSE -1
                                            END AS `trueprice`
                                        FROM
	                                        `Inf_Service` A,
	                                        `Inf_DoctorService` B,
	                                        `Inf_ServiceSort` C
                                        WHERE
	                                        A.`ServiceCode` = B.`ServiceCode`
                                        AND A.`ServiceCode` = C.`ServiceCode`
                                        AND A.`Status` = 1
                                        AND A.`IsVisible` = 1
                                        AND B.`Status` = 1
                                        AND B.`DoctorCode` = @DoctorCode
                                        ORDER BY C.`Sort`";
                List<DoctorService_Model> result = db.SetCommand(strSqlDoctor
                    , db.Parameter("@DoctorCode", DoctorCode, DbType.String)).ExecuteList<DoctorService_Model>();
                
                return result;
            }
        }

    }
}
