using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.WeChat.Entity
{
    [Serializable]
    public class MessageTemplate
    {
        public string touser { get; set; }
        public string template_id { get; set; }
        public string url { get; set; }
        public string topcolor { get; set; }
        public TemplateDetail data { get; set; }
    }

    [Serializable]
    public class TemplateDetail
    {
        public TemplateDetailParameter first { get; set; }
        public TemplateDetailParameter remark { get; set; }
        public TemplateDetailParameter keyword1 { get; set; }
        public TemplateDetailParameter keyword2 { get; set; }
        public TemplateDetailParameter keyword3 { get; set; }
        public TemplateDetailParameter keyword4 { get; set; }
    }

    [Serializable]
    public class TemplateDetailParameter
    {
        public TemplateDetailParameter() { value = string.Empty; color = "#000000"; }
        public string value { get; set; }        
        public string color { get; set; }
    }
}
