using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Options;
using InmobiliariaAlbornoz.Models;
using System.Threading.Tasks;
using System;

namespace InmobiliariaAlbornoz.Services
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value ?? throw new ArgumentNullException(nameof(emailSettings), "La configuración del servicio de correo no puede ser nula.");

            // Validar valores individuales
            if (string.IsNullOrEmpty(_emailSettings.SenderEmail))
                throw new ArgumentNullException(nameof(_emailSettings.SenderEmail), "El correo del remitente no puede ser nulo o vacío.");
            if (string.IsNullOrEmpty(_emailSettings.SenderPassword))
                throw new ArgumentNullException(nameof(_emailSettings.SenderPassword), "La contraseña del remitente no puede ser nula o vacía.");
            if (string.IsNullOrEmpty(_emailSettings.SmtpServer))
                throw new ArgumentNullException(nameof(_emailSettings.SmtpServer), "El servidor SMTP no puede ser nulo o vacío.");
            if (_emailSettings.SmtpPort <= 0)
                throw new ArgumentOutOfRangeException(nameof(_emailSettings.SmtpPort), "El puerto SMTP debe ser un número válido.");
        }


        public async Task SendPasswordResetEmailAsync(string recipientEmail, string resetLink)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Soporte", _emailSettings.SenderEmail));
            message.To.Add(new MailboxAddress("Propietario", recipientEmail));
            message.Subject = "Recuperar contraseña";
            
            // Cuerpo del mensaje en HTML para un botón con el enlace de restablecimiento de contraseña
            /*var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
            <html>
                <body>
                    <p>Para generar una nueva contraseña, haga clic en el siguiente botón:</p>
                    <a href='{resetLink}' style='display: inline-block; padding: 10px 20px; margin: 10px 0; background-color: #007bff; color: white; text-decoration: none; border-radius: 5px;'>Regenerar contraseña</a>
                    <p>Si no solicitó este cambio, ignore este correo.</p>
                </body>
            </html>"
            };*/

            // Cuerpo del mensaje en HTML para un botón con el token de restablecimiento de contraseña
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
            <html>
                <body>
                    <p>Para generar una nueva contraseña, utilice el siguiente token en su aplicación:</p>
                    <p>{resetLink}</p>
                    <p>Si no solicitó este cambio, ignore este correo.</p>
                </body>
            </html>"
            };

            message.Body = bodyBuilder.ToMessageBody();

            // Enviar el correo
            using var client = new SmtpClient();
            await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_emailSettings.User, _emailSettings.SenderPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}

