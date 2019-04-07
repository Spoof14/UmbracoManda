using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using UmbracoMandatoryAssignment.ViewModels;
using System.Net.Mail;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;

namespace UmbracoMandatoryAssignment.Controllers
{
    public class ContactFormSurfaceController : SurfaceController
    {
        // GET: ContactFormSurface
        public ActionResult Index()
        {
			return PartialView("ContactForm", new ContactForm());
        }

		[HttpPost]
		public ActionResult HandleFormSubmit(ContactForm model)
		{
			if (!ModelState.IsValid)
			{
				return CurrentUmbracoPage();
			}

			IContent message = Services.ContentService.CreateContent(model.Subject, CurrentPage.Id, "message");

			message.SetValue("messageName", model.Name);
			message.SetValue("email", model.Email);
			message.SetValue("subject", model.Subject);
			message.SetValue("messageContent", model.Message);
			Services.ContentService.Save(message);

			MailMessage msg = new MailMessage();
			msg.To.Add("username@eaaa.dk");
			msg.Subject = model.Subject;
			msg.From = new MailAddress(model.Email, model.Name);
			msg.Body = model.Message;
			using (SmtpClient smtp = new SmtpClient())
			{
				smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
				smtp.UseDefaultCredentials = false;
				smtp.EnableSsl = true;
				smtp.Host = "smtp.gmail.com"; smtp.Port = 587;
				smtp.Credentials = new System.Net.NetworkCredential("username@gmail.com", "password");// send mailsmtp.Send(message);
			}
			TempData["success"] = true;
			return RedirectToCurrentUmbracoPage();
		}
	}
}