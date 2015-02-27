using System;
using System.Text;

namespace Tarento.Common.Utilities.Exceptions
{
    /// <summary>
    /// Class that contains a standardized single control to display to users in the case of an error occurring.MO
    /// </summary>
    /// <remarks></remarks>
    public static class ErrorVisualizer
    {
        /// <summary>
        /// Gets the error HTML to be displayed when a webpart or web control fails
        /// </summary>
        /// <param name="ex">The original exception object</param>
        /// <param name="Qualifier">Pass a unique control name value</param>
        /// <param name="Message">The message you want to additionally add to the HTML</param>
        /// <returns>an html string to embed in your control (add as literal to child controls or write to page in render)</returns>
        /// <remarks></remarks>
        public static string GetErrorHTML(Exception ex, string Qualifier, string Message)
        {
            StringBuilder builder = new StringBuilder();
            string additional_information = string.Empty;
            bool flag = true;
            try
            {
                if (ex is CustomException)
                {
                    CustomException exception = ex as CustomException;
                    if (string.IsNullOrEmpty(Message))
                    {
                        Message = exception.DisplayMessage;
                    }
                    if (exception.AdditionalInformation != string.Empty)
                    {
                        additional_information = exception.AdditionalInformation;
                    }
                }
                else if (ex is ApplicationException)
                {
                    if (string.IsNullOrEmpty(Message))
                    {
                        Message = ex.Message;
                    }
                }
                else if (string.IsNullOrEmpty(Message))
                {
                    Message = Tarento.Common.Utilities.Properties.Resources.ExceptionHandlerTitle;
                }
                if (flag)
                {
                    builder.Append(@"<table border=""0""><tr><td valign=""top"" width=""1%""><img src=""/_layouts/images/errlg.gif"" border=""0""/></td><td style=""padding-top:8px"" valign=""top"">");
                    Exception baseException = ex.GetBaseException();
                    builder.Append(Message);
                    builder.AppendFormat(@"<br /><br /><a id=""_errorMsgLabel{0}"" href=""#"" onclick=""javascript:if(_errorMsgLabel{0}.innerHTML=='{1}'){{_errorMsgDiv{0}.style.display='inline';_errorMsgLabel{0}.innerHTML='{2}';}}else{{_errorMsgDiv{0}.style.display='none';_errorMsgLabel{0}.innerHTML='{1}';}}"";>{1}</a><br />",
                        Qualifier,
                        Tarento.Common.Utilities.Properties.Resources.ExceptionHandlerShowDetails,
                        Tarento.Common.Utilities.Properties.Resources.ExceptionHandlerHideDetails);
                    string message = string.Empty;
                    if (string.IsNullOrEmpty(baseException.ToString()))
                    {
                        message = baseException.Message;
                    }
                    else
                    {
                        message = baseException.ToString();
                    }
                    builder.AppendFormat(@"<div style=""display:none;"" id=""_errorMsgDiv{0}"">{1}", Qualifier, message);
                    if (additional_information != string.Empty)
                    {
                        builder.AppendFormat(@"<br /><br /><a href=""#""></a><br />", Tarento.Common.Utilities.Properties.Resources.ExceptionHandlerAdditionalInformation);
                        builder.AppendFormat("<div>{0}</div><br />", additional_information);
                    }
                    builder.Append("</div><br>");
                    builder.Append("</td></tr></table>");
                }
                else
                {
                    builder.AppendFormat(@"<div style=""padding:3px;"">{0}</div>", Message);
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return builder.ToString();
        }
    }
}
