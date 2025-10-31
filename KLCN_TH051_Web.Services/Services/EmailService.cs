using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using KLCN_TH051_Web.Services.Models;

namespace KLCN_TH051_Web.Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendConfirmationEmailAsync(string email, string callbackUrl)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Xác thực tài khoản - KLCN TH051 Website";

            // callbackUrl nên đã encode token trước khi truyền vào
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $@"
        <h2>Xin chào!</h2>
        <p>Cảm ơn bạn đã đăng ký tài khoản tại KLCN TH051 Website.</p>
        <p>Vui lòng <a href='{callbackUrl}' style='color: #007bff; text-decoration: none;'>nhấp vào đây</a> để xác thực email của bạn.</p>
        <p>Nếu bạn không đăng ký, vui lòng bỏ qua email này.</p>
        <p>Trân trọng,<br/>Đội ngũ KLCN TH051</p>";

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
