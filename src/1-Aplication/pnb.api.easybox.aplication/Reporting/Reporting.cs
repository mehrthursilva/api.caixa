using pnb.api.easybox.domain.Model;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using MimeKit;
using MailKit.Net.Smtp;

namespace pnb.api.easybox.aplication.Reporting
{
    public static class Reporting
    {
        static readonly HttpClient _client = new HttpClient();
        static readonly StringBuilder assembly = new StringBuilder();
        public static async void ReportToTeams(string message,string hook)
        {
                var text = new TextReport 
                { 
                    text = $"[{DateTime.Now}] - [BUG] API EASY BOX \n MENSAGEM : {message}" 
                };
                var content = JsonSerializer.Serialize(text); 
                var httpResponseMessage = await _client.PostAsync(hook, new StringContent(content));
                var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
        }

        public static void ReportToTwilio(string number,string messages) 
        {         
            string accountSid = "ACf7e935f737766a1364234c21ff919e0a";
            string authToken = "6504656a42ce6e08621f4521dfcf744d";
            TwilioClient.Init(accountSid, authToken);
            var message = MessageResource.Create(
                body: messages,
                from: new Twilio.Types.PhoneNumber("+13608034646"),
                to: new Twilio.Types.PhoneNumber(number)
            );
        }

        public static async void ReportingToEmail(string email,string subject,string content) 
        {
            var message = new Message(new string[] { email }, subject, content, null);
            var mailMessage = CreateEmailMessage(message);
            await SendAsync(mailMessage);
        }

        private static MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("mehrthursilvaforprodesp@gmail.com"));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            var bodyBuilder = new BodyBuilder { HtmlBody = string.Format("<h2 style='color:red;'>{0}</h2>", message.Content) };
            if (message.Attachments != null && message.Attachments.Any())
            {
                byte[] fileBytes;
                foreach (var attachment in message.Attachments)
                {
                    using (var ms = new MemoryStream())
                    {
                        attachment.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }
                    bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }
            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }

        private static async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync("smtp.gmail.com", 465, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync("mehrthursilvaforprodesp@gmail.com", "nkcldttwbtannhib");
                    await client.SendAsync(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception, or both.
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}
