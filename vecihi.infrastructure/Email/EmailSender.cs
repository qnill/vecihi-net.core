using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading.Tasks;
using vecihi.helper;
using vecihi.helper.Const;

namespace vecihi.infrastructure
{
    public interface IEmailSender
    {
        Task<ApiResult> Send(string to, string subject, string message, string cc = null, string bcc = null);
        Task<ApiResult> Send(string[] to, string subject, string message, string[] cc = null, string[] bcc = null);
    }

    public class EmailSender: IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        private async Task<ApiResult> Send(MimeMessage mimeMessage, string subject, string message)
        {
            try
            {
                mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.Sender));
                mimeMessage.Subject = subject;
                mimeMessage.Body = new TextPart("html")
                {
                    Text = message
                };

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => _emailSettings.UseSsl;

                    await client.ConnectAsync(_emailSettings.MailServer, _emailSettings.MailPort, _emailSettings.UseSsl);
                    await client.AuthenticateAsync(_emailSettings.Sender, _emailSettings.Password);

                    await client.SendAsync(mimeMessage);
                    await client.DisconnectAsync(true);
                }

                return new ApiResult { Message = ApiResultMessages.Ok };
            }
            catch (Exception ex)
            {
                return new ApiResult { Data = ex.Message, Message = ApiResultMessages.EME0001 };
            }
        }

        public async Task<ApiResult> Send(string to, string subject, string message, string cc = null, string bcc = null)
        {
            try
            {
                var mimeMessage = new MimeMessage();

                mimeMessage.To.Add(new MailboxAddress(to));

                if (!string.IsNullOrWhiteSpace(cc))
                    mimeMessage.Cc.Add(new MailboxAddress(cc));

                if (!string.IsNullOrWhiteSpace(bcc))
                    mimeMessage.Bcc.Add(new MailboxAddress(bcc));

                return await Send(mimeMessage, subject, message);
            }
            catch (Exception ex)
            {
                return new ApiResult { Data = ex.Message, Message = ApiResultMessages.EME0001 };
            }
        }

        public async Task<ApiResult> Send(string[] to, string subject, string message, string[] cc = null, string[] bcc = null)
        {
            try
            {
                var mimeMessage = new MimeMessage();

                foreach (var toAddress in to)
                    mimeMessage.To.Add(new MailboxAddress(toAddress));

                if (cc != null)
                {
                    foreach (var ccAddress in cc)
                        mimeMessage.Cc.Add(new MailboxAddress(ccAddress));
                }

                if (bcc != null)
                {
                    foreach (var bccAddress in bcc)
                        mimeMessage.Bcc.Add(new MailboxAddress(bccAddress));
                }

                return await Send(mimeMessage, subject, message);
            }
            catch (Exception ex)
            {
                return new ApiResult { Data = ex.Message, Message = ApiResultMessages.EME0001 };
            }
        }
    }
}