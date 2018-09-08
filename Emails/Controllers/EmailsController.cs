using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Emails.Data;
using Emails.Models;
using MimeKit;
using MimeKit.Utils;
using MailKit.Net.Smtp;

namespace Emails.Controllers
{
    public class EmailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmailsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Emails
        public async Task<IActionResult> Index()
        {
            return View(await _context.Email.ToListAsync());
        }

        // GET: Emails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var email = await _context.Email
                .SingleOrDefaultAsync(m => m.EmailID == id);
            if (email == null)
            {
                return NotFound();
            }

            return View(email);
        }

        // GET: Emails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Emails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Email email)
        {
            if (ModelState.IsValid)
            {

                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress("TEST PROJECT", "olowofelayinka@gmail.com"));
                mimeMessage.To.Add(new MailboxAddress(email.ToAddressTitle, email.ToAddress));
                mimeMessage.Subject = email.Message;


                var builder = new BodyBuilder();

                // Set the plain-text version of the message text
            //    builder.TextBody = @"Hey Alice,

            //What are you up to this weekend? Monica is throwing one of her parties on
            //Saturday and I was hoping you could make it.

            //Will you be my +1?
           
            //-- Joey
            //";


                // In order to reference selfie.jpg from the html text, we'll need to add it
                // to builder.LinkedResources and then use its Content-Id value in the img src.
                var image = builder.LinkedResources.Add(email.FromAddressTitle);
                image.ContentId = MimeUtils.GenerateMessageId();

                // Set the html version of the message text
                builder.HtmlBody = string.Format(@"<br>
                                                  <center><img src=""cid:{0}""></center><br>" + @"<p>" + email.Message + @"< /p>" , image.ContentId);



                // We may also want to attach a calendar event for Monica's party...
                builder.Attachments.Add(email.FromAddressTitle);



                // Now we just need to set the message body and we're done
                mimeMessage.Body = builder.ToMessageBody();



                using (var client = new SmtpClient())
                {

                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("write d name of your email", "write d name of your password");

                    client.Send(mimeMessage);

                    client.Disconnect(true);

                }


                return RedirectToAction("Index");
            }
            return View(email);
        }

        // GET: Emails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var email = await _context.Email.SingleOrDefaultAsync(m => m.EmailID == id);
            if (email == null)
            {
                return NotFound();
            }
            return View(email);
        }

        // POST: Emails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmailID,FromAddress,FromAddressTitle,ToAddress,ToAddressTitle,Message,BodyContent")] Email email)
        {
            if (id != email.EmailID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(email);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmailExists(email.EmailID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(email);
        }

        // GET: Emails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var email = await _context.Email
                .SingleOrDefaultAsync(m => m.EmailID == id);
            if (email == null)
            {
                return NotFound();
            }

            return View(email);
        }

        // POST: Emails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var email = await _context.Email.SingleOrDefaultAsync(m => m.EmailID == id);
            _context.Email.Remove(email);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool EmailExists(int id)
        {
            return _context.Email.Any(e => e.EmailID == id);
        }
    }
}
