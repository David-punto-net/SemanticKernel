using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Net.Mail;
using static SmKPlugins.Plugins.CustomerPlugin;

namespace SmKPlugins.Plugins
{
    public class EmailPlugin
    {
        private readonly EmailService _emailService;

        public EmailPlugin(string smtpServer, int port, string username, string password)
        {
            _emailService = new EmailService(smtpServer, port, username, password);
        }

        //[KernelFunction, Description("Envía un correo electrónico personalizado a un cliente específico.")]
        [KernelFunction, Description("Sends a personalized email to a specific client.")]
        public async Task SendEmailAsync(
            //[Description("Dirección de correo del remitente.")] string from,
            //[Description("Aquí debe ir la Dirección de correo del destinatario o cliente.")] string to,
            //[Description("Asunto del email")] string subject,
            //[Description("Contenido del mensaje")] string body)

            [Description("Sender's email address.")] string from,
            [Description("Recipient's email address.")] string to,
            [Description("Email subject.")] string subject,
            [Description("Email message content.")] string body)

        {
            await Task.Run(() => _emailService.SendEmail(from, to, subject, body));
        }


        //[KernelFunction, Description("Envía un email a todos los clientes de la lista")]
        //public async Task SendEmailToAllAsync(
        //    [Description("Dirección de correo del remitente.")] string from,
        //    [Description("Aquí debe ir la Lista de clientes")] List<Customer> customers,
        //    [Description("Asunto del email")] string subject,
        //    [Description("Contenido del mensaje")] string body)
        //{

        //    foreach (var customer in customers)
        //    {
        //        try
        //        {
        //            await Task.Run(() => _emailService.SendEmail(from, customer.Email, subject, body));
        //            Console.WriteLine($"Email enviado a {customer.Nombre} ({customer.Email})");
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Error enviando a {customer.Email}: {ex.Message}");
        //        }
        //    }

        //}
    }
}