using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Net.Mail;


namespace SmKPlugins.Plugins
{
    public class EmailPlugin
    {
        private readonly EmailService _emailService;
        private readonly string _from;

        public EmailPlugin(string smtpServer, int port, string username, string password)
        {
            _emailService = new EmailService(smtpServer, port, username, password);
            _from = username;
        }

        [KernelFunction, Description("Envía un correo electrónico personalizado usando HTML para el cuerpo del mensaje.")]
        public async Task SendEmailAsync(
            [Description("Aquí debe ir la Dirección de correo del destinatario.")] string to,
            [Description("Asunto del email")] string subject,
            [Description("Contenido del mensaje")] string body)
        {

            await Task.Run(() => _emailService.SendEmail(_from, to, subject, body));
        }

    }
}