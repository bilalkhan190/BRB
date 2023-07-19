using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.ObjectModel;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;

namespace BusinessObjects.Helper
{
    public static class ApplicationHelper
    {
        public static string GetMonthName(int monthInNumber)
        {
            string monthName = string.Empty;
            switch (monthInNumber)
            {
                case 1: { 
                        monthName = "January";
                        break;
                    }
                case 2:
                    {
                        monthName = "Feburary";
                        break;
                    }
                case 3:
                    {
                        monthName = "March";
                        break;
                    }
                case 4:
                    {
                        monthName = "April";
                        break;
                    }
                case 5:
                    {
                        monthName = "May";
                        break;
                    }
                case 6:
                    {
                        monthName = "June";
                        break;
                    }
                case 7:
                    {
                        monthName = "July";
                        break;
                    }
                case 8:
                    {
                        monthName = "August";
                        break;
                    }
                case 9:
                    {
                        monthName = "September";
                        break;
                    }
                case 10:
                    {
                        monthName = "October";
                        break;
                    }
                case 11:
                    {
                        monthName = "November";
                        break;
                    }
                case 12:
                    {
                        monthName = "December";
                        break;
                    }

            }
            return monthName;
        }

        public static async Task<string> RenderViewAsync<TModel>(this Microsoft.AspNetCore.Mvc.Controller controller, string viewName, TModel model, bool partial = false)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = controller.ControllerContext.ActionDescriptor.ActionName;
            }

            controller.ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                Microsoft.AspNetCore.Mvc.ViewEngines.IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                Microsoft.AspNetCore.Mvc.ViewEngines.ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext, viewName, !partial);

                if (viewResult.Success == false)
                {
                    return $"A view with the name {viewName} could not be found";
                }

                Microsoft.AspNetCore.Mvc.Rendering.ViewContext viewContext = new Microsoft.AspNetCore.Mvc.Rendering.ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }

        public static void SendResume(string strFileName, string strAddress)
        {
            MailMessage mailMessage = new MailMessage("bestresumebuilderresume@gmail.com", strAddress);
            ((Collection<MailAddress>)mailMessage.ReplyToList).Add(new MailAddress("bestresumebuilderresume@gmail.com"));
            mailMessage.Subject = "Best Resume Builder - Your Resume";
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<strong>CONGRATULATIONS!</strong><br /><br />");
            stringBuilder.Append("Your resume is attached in Word.  As you see, the file is named with your first and last name along with the month and date your resume was submitted to Best Resume Builder.  We recommend keeping this as your file format so you and employers will know that they have the most current copy of your skills and background.<br /><br />");
            stringBuilder.Append("<strong>Be sure to check the following to finalize your resume:</strong>");
            stringBuilder.Append("<li>Any jobs/organizations/experiences that have ENDED have a ‘past tense’ verb at the front of the bullet point.  Some bullet points will need to have information modified based on your personal experience, so be sure they make sense and are specific with details where possible.</li>");
            stringBuilder.Append("<li>If you only have ONE job within a company, you will see that the dates of employment will be duplicated right under each other.  Please be sure to remove the dates of employment in the line with the job title and keep the dates in the line with the company name and location.</li>");
            stringBuilder.Append("<li>Check that the font size is consistent for your headings and titles. Some browsers may create inconsistent font sizes, so make changes where necessary to have your resume be consistent from section to section.</li>");
            stringBuilder.Append("<li>You should try to condense your resume to one page – or the MOST important information fits on the first page.  You may have to eliminate some bullet points or change font size (be careful not to go smaller than 10pt).</li>");
            stringBuilder.Append("<li>Use spell check – there may have been input fields that you entered quickly and may have a misspelled word.</li>");
            stringBuilder.Append("<br />");
            stringBuilder.Append("Good luck in your career search process!  For more information on other resources to help with your resume, interview skills, job offer negotiation, or other topics, please go to our website ");
            stringBuilder.Append("<a href='www.bestresumebuilder.com'>www.bestresumebuilder.com</a> and check out the ‘Additional Resources’ page.  Thank you for using Best Resume Builder.");
            stringBuilder.Append("<br />");
            stringBuilder.Append("<br />");
            mailMessage.Body = stringBuilder.ToString();
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.IsBodyHtml = true;
            ((Collection<Attachment>)mailMessage.Attachments).Add(new Attachment(strFileName, "application/octet-stream"));
            try
            {
                new SmtpClient()
                {
                    DeliveryMethod = ((SmtpDeliveryMethod)0),
                    Host = "relay-hosting.secureserver.net",
                    Port = 25
                }.Send(mailMessage);
            }
            catch (Exception ex)
            {

            }
        }

        public static void SendEmail(string toEmail,string subject , string body)
        {
            MailMessage mailMessage = new MailMessage("bestresumebuilderresume@gmail.com", toEmail);
            ((Collection<MailAddress>)mailMessage.ReplyToList).Add(new MailAddress("bestresumebuilderresume@gmail.com"));
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.IsBodyHtml = true;
            try
            {
                new SmtpClient()
                {
                    DeliveryMethod = ((SmtpDeliveryMethod)0),
                    Host = "relay-hosting.secureserver.net",
                    Port = 25
                }.Send(mailMessage);
            }
            catch (Exception ex)
            {

            }
        }

    }
}
