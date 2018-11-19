using Common.Entity;
using Model.Manage_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using Common.Util;
using WebManager.Model;
using Newtonsoft.Json;

namespace WebManager.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult UserLogin(string LoginUserName, string Password)
        {
            ObjectResult<int> result = new ObjectResult<int>();
            Password = Common.Safe.CryptMD5.Encrypt(Password);
            User_Model user = UserM_BLL.Instance.getUserByAccountPassword(LoginUserName, Password);

            if (user == null ||  user.UserID == 0 || user.Type == 1 || user.Type == 2)
            {
                CookieUtil.DeleteCookie("WebManage");
                result.Message = "账户名密码错误";
                result.Code = "0";
                return Json(result);

               
            }
            else
            {
                Cookie_Model cookieModel = new Cookie_Model();
                cookieModel.UserID = user.UserID;
                cookieModel.Type = user.Type;

                if (user.Type == 2)
                {
                    Doctor_Model doctor = UserM_BLL.Instance.getDoctorDetail(user.UserID);
                    if (doctor != null)
                    {
                        cookieModel.UserName = doctor.Name;
                        cookieModel.UserCode = doctor.DoctorCode;
                    }
                }
                else if (user.Type == 3) {
                    Staff_Model staff = UserM_BLL.Instance.getStaffDetail(user.UserID);
                    if (staff != null) {
                        cookieModel.UserName = staff.Name;
                        cookieModel.UserCode = staff.StaffCode;
                        cookieModel.Role = staff.Role;
                    }
                }

                CookieUtil.SetCookie("WebManage", JsonConvert.SerializeObject(cookieModel), 0, true);


                result.Message = "登陆成功";
                result.Code = "1";
                result.Data = 1;
                return Json(result);

            }
        }

    }
}