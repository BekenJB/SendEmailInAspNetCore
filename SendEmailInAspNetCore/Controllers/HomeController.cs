using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using SendEmailInAspNetCore.Helpers;
using SendEmailInAspNetCore.Models;
using System;
using System.Diagnostics;

namespace SendEmailInAspNetCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
        [HttpPost]
        public IActionResult Contact(ContactViewModel contactViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string emailBody = string.Empty;
                    bool useEmailTemplate = true;
                    //instantiate a new MimeMessage
                    var message = new MimeMessage();

                    //Setting the To e-mail address
                    message.To.Add(new MailboxAddress("E-mail Recipient Name", "recipient@domain.com"));
                    //Setting the From e-mail address
                    message.From.Add(new MailboxAddress("E-mail From Name", "fromemail@domain.com"));
                    //E-mail subject 
                    message.Subject = contactViewModel.Subject;
                    //E-mail message body
                    if (useEmailTemplate)
                    {
                        emailBody = ViewsToStringOutputHelper.RenderRazorViewToString(this, "Welcome", null);
                    }
                    else
                    {
                        emailBody = contactViewModel.Message + " Message was sent by: " + contactViewModel.Name + " E-mail: " + contactViewModel.Email;

                    }
                    message.Body = new TextPart(TextFormat.Html)
                    {
                        Text = emailBody
                    };

                    //Configure the e-mail
                    using (var emailClient = new SmtpClient())
                    {
                        emailClient.Connect("smtp.gmail.com", 587, false);
                        emailClient.Authenticate("gmailaccount@gmail.com", "password");
                        emailClient.Send(message);
                        emailClient.Disconnect(true);
                    }
                }
                catch (Exception ex)
                {

                    ModelState.Clear();
                    ViewBag.Exception = $" Oops! Message could not be sent. Error:  {ex.Message}";
                }

            }
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Welcome()
        {
            return View();
        }

    }
}
