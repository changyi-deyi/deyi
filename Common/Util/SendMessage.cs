using Model.Manage_Model;
using Submail.AppConfig;
using Submail.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Util
{
    public class SendMessage
    {
        public static bool SendAuthCode(string mobile, string code)
        {
            IAppConfig appConfig = new MessageConfig("27389", "fe98920ac52fabc0185971b9ceeb4b62");
            MessageXSend messageXSend = new MessageXSend(appConfig);
            messageXSend.AddTo(mobile);
            messageXSend.SetProject("vfOd6");
            messageXSend.AddVar("code", code);
            string returnMessage = string.Empty;
            return messageXSend.XSend(out returnMessage);
        }

        public static bool SendCustomer(string mobile, SendCustomer_Model model)
        {
            IAppConfig appConfig = new MessageConfig("27389", "fe98920ac52fabc0185971b9ceeb4b62");
            MessageXSend messageXSend = new MessageXSend(appConfig);
            messageXSend.AddTo(mobile);
            messageXSend.SetProject("9UHBu3");
            messageXSend.AddVar("customer", model.customer);
            messageXSend.AddVar("hospital", model.hospital);
            messageXSend.AddVar("department", model.department);
            messageXSend.AddVar("doctor", model.doctor);
            messageXSend.AddVar("datetime", model.datetime);
            messageXSend.AddVar("address", model.address);
            messageXSend.AddVar("code", model.code);
            string returnMessage = string.Empty;
            return messageXSend.XSend(out returnMessage);
        }

        public static bool SendDoctor(string mobile, SendDoctor_Model model)
        {
            IAppConfig appConfig = new MessageConfig("27389", "fe98920ac52fabc0185971b9ceeb4b62");
            MessageXSend messageXSend = new MessageXSend(appConfig);
            messageXSend.AddTo(mobile);
            messageXSend.SetProject("Xz0HK1");
            messageXSend.AddVar("doctor", model.doctor);
            messageXSend.AddVar("customer", model.customer);
            messageXSend.AddVar("datetime", model.datetime);
            messageXSend.AddVar("address", model.address);
            messageXSend.AddVar("code", model.code);
            string returnMessage = string.Empty;
            return messageXSend.XSend(out returnMessage);
        }

    }
}
