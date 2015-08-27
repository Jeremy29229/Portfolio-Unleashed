using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortfolioUnleashed.Models.ViewModels
{
    public class VMErrorInformation
    {
        public string OuterException { get; set; }
        public string InnerException { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorSource { get; set; }
        public string StackTrace { get; set; }

        public VMErrorInformation(string outerMessage, string innerMessage, string code, string source, string stack)
        {
            OuterException = outerMessage;
            InnerException = innerMessage;
            ErrorCode = code;
            ErrorSource = source;
            StackTrace = stack;
        }

        public VMErrorInformation()
        {

        }
    }
}