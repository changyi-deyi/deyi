using Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Authorize
{
    public class HTTPBasicAuthorizeAttribute : System.Web.Http.AuthorizeAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (!StringUtils.GetDbBool(System.Configuration.ConfigurationManager.AppSettings["IsAuthorize"]))
            {
                IsAuthorized(actionContext);
            }
            else
            {
                try
                {

                    var US = StringUtils.GetDbInt(actionContext.Request.Headers.GetValues("US").First());
                    var ME = StringUtils.GetDbString(actionContext.Request.Headers.GetValues("ME").First());
                    var TI = StringUtils.GetDbDateTime(actionContext.Request.Headers.GetValues("TI").First());

                    string actionName = actionContext.ActionDescriptor.ActionName;


                    if (actionContext.Request.Headers.Authorization != null)
                    {
                        #region 验证参数是否合法
                        if (string.IsNullOrWhiteSpace(ME))
                        {
                            // 没有Methord
                            actionContext.Request.Headers.Add("errorMessage", "10003");
                            HandleUnauthorizedRequest(actionContext);
                        }
                        




                        #endregion

                        string pars = actionContext.ActionDescriptor.ActionName + actionContext.Request.Content.ReadAsStringAsync().Result + "DYYICANG";
                        string token = actionContext.Request.Headers.Authorization.Scheme;
                        string tokenNew = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(pars.ToUpper(), "MD5");

                        if (token.ToLower().Equals(tokenNew.ToLower()))
                        {
                            IsAuthorized(actionContext);
                        }
                        else
                        {
                            // 验证参数失败
                            actionContext.Request.Headers.Add("errorMessage", "10002");
                            HandleUnauthorizedRequest(actionContext);
                        }
                    }
                    else
                    {
                        // 没有验证参数
                        actionContext.Request.Headers.Add("errorMessage", "10001");
                        HandleUnauthorizedRequest(actionContext);
                    }

                }
                catch (Exception ex)
                {
                    if (ex is System.Web.Http.HttpResponseException)
                    {
                        throw;
                    }
                    else
                    {
                        actionContext.Request.Headers.Add("errorMessage", "10011");
                        HandleUnauthorizedRequest(actionContext);
                    }
                }
            }

        }

        protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var challengeMessage = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            challengeMessage.Headers.Add("errorMessage", actionContext.Request.Headers.GetValues("errorMessage").First());
            throw new System.Web.Http.HttpResponseException(challengeMessage);
        }


    }
}