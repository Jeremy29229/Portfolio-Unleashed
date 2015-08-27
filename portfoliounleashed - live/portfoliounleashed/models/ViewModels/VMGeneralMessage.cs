using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortfolioUnleashed.Models.ViewModels
{
    public class VMGeneralMessage
    {
        public string MessageTitle { get; set; }
        public string MessageHeader { get; set; }
        public string[] Messages { get; set; }
        public string OptionalURL { get; set; }
        public string OptionalURLDisplayText { get; set; }

        public VMGeneralMessage(string messageTitle, string messageHeader, string[] messages, string optionalUrl = null, string optionalUrlDisplayText = null)
        {
            MessageTitle = messageTitle;
            MessageHeader = messageHeader;
            Messages = messages;
            OptionalURL = optionalUrl;
            OptionalURLDisplayText = optionalUrlDisplayText;
        }
    }
}