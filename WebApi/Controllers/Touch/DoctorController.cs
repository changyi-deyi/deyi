using BLL;
using Common.Caching;
using Common.Entity;
using Common.Util;
using Model.Operate_Model;
using Model.Table_Model;
using Model.View_Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Authorize;

namespace WebApi.Controllers.Touch
{
    public class DoctorController : BaseController
    {
        //抽取科室数据
        [HttpPost]
        [ActionName("GetDepartment")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage GetDepartment(JObject obj)
        {
            ObjectResult<List<SetDepartment_Model>> result = new ObjectResult<List<SetDepartment_Model>>();
            result.Code ="0";
            result.Message = "数据抽取失败";
            result.Data = null;
            
            List<SetDepartment_Model> res = SetDepartment_BLL.Instance.GetDepartment();
            if (res != null && res.Count > 0)
            {
                result.Code = "1";
                result.Data = res;
                result.Message = "数据抽取成功";
            }

            return toJson(result);
        }

        //找医生
        [HttpPost]
        [ActionName("GetDoctorList")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage GetDoctorList(JObject obj)
        {
            ObjectResult<List<DoctorList_Model>> result = new ObjectResult<List<DoctorList_Model>>();
            result.Code = "0";
            result.Message = "数据抽取失败";
            result.Data = null;

            if (obj == null)
            {
                result.Message = "不合法参数";
                return toJson(result);
            }


            string strSafeJson = Common.Util.StringUtils.GetDbString(obj);

            Doctor_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<Doctor_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.CustomerCode))
            {
                result.Message = "不合法参数";
                return toJson(result);
            }

            if (model.service && string.IsNullOrEmpty(model.ServiceCode))
            {
                result.Message = "不合法参数";
                return toJson(result);
            }

            List<DoctorList_Model> list = InfDoctor_BLL.Instance.GetDoctorList(model);

            if (list != null &&list.Count > 0)
            {
                foreach (DoctorList_Model item in list)
                {
                    if (!string.IsNullOrEmpty(item.ImageURL))
                    {
                        item.ImageURL = System.Configuration.ConfigurationManager.AppSettings["Domian"] + item.ImageURL;
                    }
                }
                result.Code = "1";
                result.Data = list;
                result.Message = "数据抽取成功";
            }

            return toJson(result);
        }

        //医生关注，取关
        [HttpPost]
        [ActionName("FollowOrCancel")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage FollowOrCancel(JObject obj)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Message = "操作失败";
            result.Data = false;

            if (obj == null)
            {
                result.Message = "不合法参数";
                return toJson(result);
            }


            string strSafeJson = Common.Util.StringUtils.GetDbString(obj);

            FollowDoctor_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<FollowDoctor_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.CustomerCode)|| string.IsNullOrEmpty(model.DoctorCode))
            {
                result.Message = "不合法参数";
                return toJson(result);
            }

            int res = OpeFollowDoctor_BLL.Instance.FollowOrCancle(model);

            if (res == 1)
            {
                result.Code = "1";
                result.Data = true;
                result.Message = "操作成功";
            }

            return toJson(result);
        }

        //医生详情
        [HttpPost]
        [ActionName("GetDoctorDetail")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage GetDoctorDetail(JObject obj)
        {
            ObjectResult<DoctorDetail_Model> result = new ObjectResult<DoctorDetail_Model>();
            result.Code = "0";
            result.Message = "操作失败";
            result.Data = null;

            DoctorDetail_Model res = new DoctorDetail_Model();

            if (obj == null)
            {
                result.Message = "不合法参数";
                return toJson(result);
            }
            
            string strSafeJson = Common.Util.StringUtils.GetDbString(obj);

            DoctorDetailIn_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<DoctorDetailIn_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.DoctorCode) || string.IsNullOrEmpty(model.CustomerCode))
            {
                result.Message = "不合法参数";
                return toJson(result);
            }
            //获取医生信息
            DoctorList_Model doctorInfo = InfDoctor_BLL.Instance.GetDoctorDetail(model);

            if (doctorInfo == null)
            {
                return toJson(result);
            }
            if (!string.IsNullOrEmpty(doctorInfo.ImageURL))
            {
                doctorInfo.ImageURL = System.Configuration.ConfigurationManager.AppSettings["Domian"] + doctorInfo.ImageURL;
            }
            res.DoctorInfo = doctorInfo;
            //获取医生服务信息
            List<DoctorService_Model> doctorService = InfDoctor_BLL.Instance.GetDoctorService(model.DoctorCode);

            if (doctorService !=null && doctorService.Count > 0)
            {
                foreach (DoctorService_Model item in doctorService)
                {
                    //获取折扣信息
                    InfMemberServiceDiscount_Model MemberService = InfMemberServiceDiscount_BLL.Instance.GetDiscount(item.ServiceCode, model.LevelID);
                    //无折扣信息
                    if (MemberService == null)
                    {
                        item.DiscountFlg = false;
                    }
                    //有折扣信息
                    else
                    {
                        item.DiscountFlg = true;
                        item.Discount = MemberService.Discount;
                        if (item.IsBargain == 2)
                        {
                            item.DiscountPrice = item.trueprice * item.Discount;
                        }
                    }
                }
            }
            res.DoctorService = doctorService;
            //获取医生评价
            List<OpeComment_Model> doctorComment = OpeComment_BLL.Instance.GetServiceOrder(model.DoctorCode);
            res.DoctorComment = doctorComment;

            result.Code = "1";
            result.Message = "操作成功";
            result.Data = res;

            return toJson(result);
        }

    }
}
