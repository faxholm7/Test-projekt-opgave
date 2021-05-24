using Google.Apis.Auth.OAuth2;
using MailKit.Security;
using MimeKit;
using MitForbrug.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Web;

namespace MitForbrug.Services
{
    public class EmailService
    {
        private IConfigurationManager _configManger;

        public EmailService(IConfigurationManager configManger)
        {
            _configManger = configManger;
        }

        public string Sendmail(string email, string link)
        {
            try
            {
                var credential = GetCredentialWithoutToken();
                var result = credential.RequestAccessTokenAsync(CancellationToken.None).Result;               

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTlsWhenAvailable);

                    var oauth2 = new SaslMechanismOAuth2(credential.User, credential.Token.AccessToken);                    
                    client.Authenticate(oauth2);

                    if(!string.IsNullOrEmpty(email))
                        client.Send(GetMessage(link, credential.User, email));
                    else
                        return "mailrecipientsdonotexist";
                }
            }
            catch(Exception)
            {
                throw;
            }
            return string.Empty;
        }

        internal ServiceAccountCredential GetCredentialWithoutToken()
        {
            var certificate = new X509Certificate2(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase), @"Certificates\mitforbrug-oauthservice-e89667966264.p12").Replace("file:\\", ""), "notasecret", X509KeyStorageFlags.MachineKeySet
             | X509KeyStorageFlags.PersistKeySet
             | X509KeyStorageFlags.Exportable);

            var gSuiteAccout = _configManger.GetAppSetting("GSuiteServiceAccount");
            var defualtUser = _configManger.GetAppSetting("DefaultSmtpUser");

            return new ServiceAccountCredential(new ServiceAccountCredential
                    .Initializer(gSuiteAccout)
            {
                Scopes = new[] { "https://mail.google.com/" },
                User = defualtUser
            }.FromCertificate(certificate));
        }

        #region "Helpers"

        internal MimeMessage GetMessage(string link, string emailSender, string emailReciver)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(MailboxAddress.Parse(emailSender));
                message.To.Add(MailboxAddress.Parse(emailReciver));
                message.Subject = GetSubject();
                message.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
                {
                    Text = GetBody(link).Replace("\\n", Environment.NewLine)
                };
                return message;
            }
            catch (Exception)
            {

                throw;
            }

        }
        internal string GetSubject()
        {
            try
            {
                return _configManger.GetAppSetting("ReportEmailSubject");
            }
            catch (ConfigurationErrorsException) { return "Nyt Password til MitForbrug"; }

        }
        internal string GetBody(string link)
        {
            try
            {
                return _configManger.GetAppSetting("ReportEmailBody") + link + _configManger.GetAppSetting("ReportEmailEnd");
            }
            catch (ConfigurationErrorsException) { return "Kære modtager, \\n" + "Klik på linket for at blive vidrestillet til at lave et nyt password. \\n" + link; }
        }

        #endregion
    }
}