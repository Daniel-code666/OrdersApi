
using Microsoft.Extensions.Options;
using Orders.Worker.Messaging;
using Orders.Worker.Orders;
using System.Net;
using System.Net.Mail;

namespace Orders.Worker.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly MailPitOptions _mail_pit_options;

        public EmailSender(IOptions<MailPitOptions> mail_pit_options)
        {
            _mail_pit_options = mail_pit_options.Value;
        }

        public async Task SendOrderCreatedEmailAsync(OrdersCreated order_created)
        {
            var subject = $"Orden creada correctamente - {order_created.OrderNumber}";

            var body = $@"
                Se ha creado una nueva orden correctamente.

                Detalle de la orden:
                    - Número de orden: {order_created.OrderNumber}
                    - Cliente: {order_created.CustomerName}
                    - Correo del cliente: {order_created.CustomerEmail}
                    - Teléfono del cliente: {order_created.CustomerPhone}
                    - Monto total: {order_created.TotalAmount}
                    - Fee total: {order_created.TotalFee}
                    - Estado: {order_created.OrderState}
                    - Activa: {order_created.Active}
                    - Fecha de creación: {order_created.CreatedAt:yyyy-MM-dd HH:mm:ss} UTC
                ";

            using var message = new MailMessage
            {
                From = new MailAddress(_mail_pit_options.From),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };

            message.To.Add(_mail_pit_options.To);

            using var smtp_client = new SmtpClient(_mail_pit_options.Host, _mail_pit_options.Port)
            {
                EnableSsl = _mail_pit_options.EnableSsl,
                UseDefaultCredentials = _mail_pit_options.UseDefaultCredentials
            };

            if (!_mail_pit_options.UseDefaultCredentials &&
                !string.IsNullOrWhiteSpace(_mail_pit_options.Username))
            {
                smtp_client.Credentials = new NetworkCredential(
                    _mail_pit_options.Username,
                    _mail_pit_options.Password);
            }

            await smtp_client.SendMailAsync(message);
        }
    }
}
