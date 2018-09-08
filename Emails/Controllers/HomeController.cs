using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Utils;
using Emails.Models;




namespace Emails.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {

            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("TEST PROJECT", "olowofelayinka@gmail.com"));
            mimeMessage.To.Add(new MailboxAddress("lanre", "olowofelayinka@gmail.com"));
            mimeMessage.Subject = "test mail in asp.net core";
            

            var builder = new BodyBuilder();

            // Set the plain-text version of the message text
            builder.TextBody = @"Hey Alice,

            What are you up to this weekend? Monica is throwing one of her parties on
            Saturday and I was hoping you could make it.

            Will you be my +1?
           
            -- Joey
            ";


            // In order to reference selfie.jpg from the html text, we'll need to add it
            // to builder.LinkedResources and then use its Content-Id value in the img src.
            var image = builder.LinkedResources.Add(@"C:\Users\fela\Desktop\My World\Web Design\Latest\Harvard University CS50\PIXS\Capture.PNG");
            image.ContentId = MimeUtils.GenerateMessageId();

            // Set the html version of the message text
            builder.HtmlBody = string.Format(@"<p>Hey Alice,<br>
            <p>What are you up to this weekend? Monica is throwing one of her parties on
            Saturday and I was hoping you could make it.<br>
            <p>Will you be my +1?<br>
            <p>-- Joey<br>
            <center><img src=""cid:{0}""></center>", image.ContentId);



            // We may also want to attach a calendar event for Monica's party...
            builder.Attachments.Add(@"C:\Users\fela\Desktop\My World\Web Design\Latest\Harvard University CS50\PIXS\Capture.PNG");

            

            // Now we just need to set the message body and we're done
            mimeMessage.Body = builder.ToMessageBody();



            using (var client = new SmtpClient())
            {

                client.Connect("smtp.gmail.com", 587, false);  
                client.Authenticate("olowofelayinka@gmail.com", "Yinkaseuntosin21");

                client.Send(mimeMessage);
                
                client.Disconnect(true);

            }




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

        public IActionResult Error()
        {
            return View();
        }
    }
}
