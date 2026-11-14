using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using Saay.Services.Interfaces;

namespace Saay.Services.Classes
{
    public class GmailService : IEmailService
    {
        private readonly ILogger<GmailService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _SMTPServer;
        private readonly int _portNumber;
        private readonly string _authenticationPassword;
        private readonly string _emailFrom;

        public GmailService(IConfiguration configuration, ILogger<GmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _SMTPServer = "smtp.gmail.com";
            _portNumber = 587;
            _authenticationPassword = _configuration["AuthenticationPassword"];
            _emailFrom = _configuration["EmailFrom"];
        }

        private MimeMessage _GenerateConfirmationMessage(
            string EmailFrom,
            string EmailTo,
            string Receiver,
            string Sender,
            string confirmationUrl)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress(Sender, EmailFrom));
            message.To.Add(new MailboxAddress(Receiver, EmailTo));
            message.Subject = "Confirm Your Saay Account";

            message.Body = new TextPart("html")
            {
                Text = $"""
        <div style="font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 24px; color: #333; line-height: 1.6;">

            <h2 style="color: #1a3a9a;">Welcome to Saay, {Receiver}!</h2>

            <p>Thank you for creating a Saay account. We're excited to have you with us!</p>

            <p>To get started, please verify your email address by clicking the button below:</p>

            <p style="margin: 30px 0;">
                <a href="{confirmationUrl}"
                   style="background-color: #1a3a9a; color: #ffffff; padding: 12px 24px;
                          text-decoration: none; border-radius: 6px; font-weight: bold;
                          display: inline-block;">
                    Verify My Email
                </a>
            </p>

            <p>This confirmation link will expire in <strong>24 hours</strong>.</p>

            <p>If the button doesn't work, copy and paste the following link into your browser:</p>

            <p style="word-break: break-all;">
                <a href="{confirmationUrl}" style="color: #1a3a9a;">
                    {confirmationUrl}
                </a>
            </p>

            <p>If you didn't create a Saay account, you can safely ignore this email.</p>

            <p style="margin-top: 30px;">
                Best regards,<br />
                <strong>The Saay Team</strong>
            </p>

            <p style="color: #777; font-size: 13px; text-align: center; margin-top: 30px;">
                Build better habits, together.
            </p>

        </div>
        """
            };

            return message;
        }

        public async Task<bool> SendConfirmationEmailAsync(string to, string subject, string recipientName, string confirmationURL)
        {
            MimeMessage Message = _GenerateConfirmationMessage(_emailFrom, to, recipientName, "Saay سعي", confirmationURL);

            try
            {
                using (var Client = new SmtpClient())
                {
                    await Client.ConnectAsync(_SMTPServer, Convert.ToInt32(_portNumber), MailKit.Security.SecureSocketOptions.StartTls);
                    await Client.AuthenticateAsync(_emailFrom, _authenticationPassword);
                    _logger.LogInformation(await Client.SendAsync(Message));
                    await Client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending email: {ex.Message}");
                return false;
            }
            _logger.LogInformation($"Email sent successfully to {to}");
            return true;
        }

        private MimeMessage _GeneratePasswordConfirmationMessage(string EmailFrom, string EmailTo, string Receiver, string Sender, string code)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress(Sender, EmailFrom));
            message.To.Add(new MailboxAddress(Receiver, EmailTo));
            message.Subject = "Reset Your Password";
            message.Body = new TextPart("html")
            {
                Text = $"""
                <h2>Reset Your Password</h2>

                <p>You have requested to reset your password. Please enter the following code:</p>

                <p>
                {code}
                </p>
                <p>Please note that this code will expire in 10 minutes.</p>
               """
            };
            return message;
        }

        public async Task<bool> SendConfirmationPasswordAsync(string to, string subject, string recipientName, string code)
        {
            MimeMessage Message = _GeneratePasswordConfirmationMessage(_emailFrom, to, recipientName, "Saay سعي", code);

            try
            {
                using (var Client = new SmtpClient())
                {
                    await Client.ConnectAsync(_SMTPServer, Convert.ToInt32(_portNumber), MailKit.Security.SecureSocketOptions.StartTls);
                    await Client.AuthenticateAsync(_emailFrom, _authenticationPassword);
                    _logger.LogInformation(await Client.SendAsync(Message));
                    await Client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending email: {ex.Message}");
                return false;
            }
            _logger.LogInformation($"Email sent successfully to {to}");
            return true;
        }


        private MimeMessage _GenerateGroupInvitationMessage(
            string emailFrom,
            string emailTo,
            string receiver,
            string sender,
            string groupName,
            string invitationUrl)
        {
            MimeMessage message = new MimeMessage();

            message.From.Add(new MailboxAddress(sender, emailFrom));
            message.To.Add(new MailboxAddress(receiver, emailTo));

            message.Subject = $"You're Invited to Join {groupName} on Saay!";

            message.Body = new TextPart("html")
            {
                Text = $"""
        <!DOCTYPE html>
        <html>
        <body style="font-family: Arial, sans-serif; color: #333; line-height: 1.6;">

            <h2>Hello, {receiver}!</h2>

            <p>
                You've been invited to join <strong>{groupName}</strong>
                on <strong>Saay</strong>!
            </p>

            <p style="margin: 30px 0;">
                <a href="{invitationUrl}"
                   style="background-color: #1a3a9a;
                          color: #ffffff;
                          padding: 12px 24px;
                          text-decoration: none;
                          border-radius: 8px;
                          font-weight: bold;
                          display: inline-block;">
                    Accept Invitation
                </a>
            </p>

            <p>
                If the button doesn't work, copy and paste the following
                link into your browser:
            </p>

            <p>
                <a href="{invitationUrl}">{invitationUrl}</a>
            </p>

            <p>
                If you weren't expecting this invitation or don't wish
                to join the group, you can safely ignore this email.
            </p>

            <hr style="border: 0; border-top: 1px solid #eee; margin-top: 30px;">

            <p style="color: #777; font-size: 13px;">
                Best regards,<br>
                The Saay Team
            </p>

            <p style="color: #999; font-size: 12px;">
                Build better habits, together.
            </p>

        </body>
        </html>
        """
            };

            return message;
        }

        public async Task<bool> SendGroupInvitationAsync(
            string to,
            string subject,
            string recipientName,
            string groupName,
            string invitationUrl)
        {
            MimeMessage message = _GenerateGroupInvitationMessage(
                _emailFrom,
                to,
                recipientName,
                "Saay سعي",
                groupName,
                invitationUrl);

            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(
                        _SMTPServer,
                        Convert.ToInt32(_portNumber),
                        MailKit.Security.SecureSocketOptions.StartTls);

                    await client.AuthenticateAsync(
                        _emailFrom,
                        _authenticationPassword);

                    _logger.LogInformation(await client.SendAsync(message));

                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending group invitation email to {Email}", to);
                return false;
            }

            _logger.LogInformation("Group invitation email sent successfully to {Email}", to);

            return true;
        }

    }
}
