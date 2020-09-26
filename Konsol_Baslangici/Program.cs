using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Konsol_Baslangici
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("C# Mail Gönderme İşlemi");

            MailSend("C# Konu Metni", "Merhaba Yetkili, \n İçerik Kısmı \n Teşekkürler", new List<string>() { /*alıcı mail adresleri */});
            Console.ReadLine();
        }

        public static void MailSend(string subject, string body, List<string> receivers, List<string> attachments = null)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 11000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("mailAddress", "mailPassword");
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("mailAddress", "Display Name");
                mailMessage.SubjectEncoding = Encoding.UTF8;
                mailMessage.Subject = subject; //E-posta Konu Kısmı
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.Body = body; // E-posta'nın Gövde Metni
                foreach (string item in receivers)
                {
                    mailMessage.To.Add(item);
                }
                mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                if (attachments != null)
                {
                    if (attachments.Count > 0)
                    {
                        foreach (string filePath in attachments)
                        {
                            if (File.Exists(filePath))
                            {
                                Attachment attachment = new Attachment(filePath);
                                mailMessage.Attachments.Add(attachment);
                            }
                        }
                    }
                }
                client.Send(mailMessage);
                Console.WriteLine("İşlem Başarılı");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

    }
}
