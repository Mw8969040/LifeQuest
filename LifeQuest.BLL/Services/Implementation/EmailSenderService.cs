using Castle.Core.Smtp;
using EllipticCurve;
using LifeQuest.BLL.Services.Interfaces;
using LifeQuest.DAL.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MimeKit; // Add the MimeKit namespace for MimeMessage يمثل الرسالة ملها والمحتوى الخاص بها
using System.Net;
using System.Net.Mail;

namespace LIfeQuest.BLL.Services.Implementation
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IConfiguration _configuration;
        public EmailSenderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #region Confirmation Email Types SMTP & MailKit
        //    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        //    {
        //        var fmail = "mohamedtalatt5@gmail.com";
        //        var fpassword = "midotalaat162";

        //        var theMsg = new MailMessage();
        //        theMsg.From = new MailAddress(fmail);
        //        theMsg.Subject = subject;
        //        theMsg.To.Add(email);
        //        theMsg.Body =$"<html><body>{htmlMessage}</body></html>";
        //        theMsg.IsBodyHtml = true;

        //        var smtp = new SmtpClient("smtp.gmail.com")
        //        {
        //            EnableSsl = true,
        //            Credentials = new NetworkCredential(fmail, fpassword),
        //            Port = 587
        //        };

        //     await smtp.SendMailAsync(theMsg);
        //    }


        //public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        //    {
        //        var message = new MimeMessage();

        //        message.From.Add(new MailboxAddress("LifeQuest", "your@email.com"));
        //        message.To.Add(new MailboxAddress("", email));
        //        message.Subject = subject;

        //        message.Body = new TextPart("html")
        //        {
        //            Text = htmlMessage
        //        };

        //        using var smtp = new MailKit.Net.Smtp.SmtpClient();

        //        await smtp.ConnectAsync("smtp.gmail.com", 587, false);
        //        await smtp.AuthenticateAsync("email", "app-password");
        //        await smtp.SendAsync(message);
        //        await smtp.DisconnectAsync(true);
        //    }
        #endregion
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();//Mutlible Mail Internet Extension   تمثل محتوي الرسالةوالعنوانها والمرسل والمستلم
                                                 //sender        //Email Address of the sender and the name that will appear in the email
            message.From.Add(new MailboxAddress("LifeQuest", _configuration["EmailSettings:Email"]));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = subject;
            message.Body = new TextPart("html")
            {
                Text = htmlMessage
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
                                   //gmail server     //port number for STARTTLS (TLS)
            await smtp.ConnectAsync("smtp.gmail.com", 587, false/*متستخدمش ال SSL بعد ال Connection مباشرة */); //connection with server

            //used to login to the server stmp server to send the email 
            await smtp.AuthenticateAsync(_configuration["EmailSettings:Email"], //بجيب اللايميل والباسورد من ال appsettings.json
                _configuration["EmailSettings:AppPassword"/*بتتعمل من الجيميل نفسه بتاع ال Company او ال Developer اللي بيعمل ال الموقع*/]);
            await smtp.SendAsync(message); //بعد مجبت السيرفر وخليته يوثق فيها بعمل السطر دا علشان اقدر ابعت الرسالة عن طريقه
            await smtp.DisconnectAsync(true);//علشان ال اتصال ما يفضلش مفتوح بعد ما ابعت الرسالة بعمل السطر دا علشان افصل الاتصال مع السيرفر
        }




    }
}
