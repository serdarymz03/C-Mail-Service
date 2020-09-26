using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormProject
{
    public partial class FrmMailService : Form
    {
        List<AttachmentFile> attachments;
        List<Receiver> receivers;
        public FrmMailService()
        {
            InitializeComponent();
            attachments = new List<AttachmentFile>();
            receivers = new List<Receiver>()
            {
                // Alıcılar..
            };
            DtgReceivers.DataSource = receivers;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = openFileDialog1.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                string filePath = openFileDialog1.FileName;
                AttachmentFile attachment = new AttachmentFile(Path.GetFileName(filePath), filePath);
                attachments.Add(attachment);
                DtgAttachments.DataSource = attachments;
            }
        }

        public void MailSend(string subject, string body, List<string> receivers, List<string> attachments = null)
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
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() => MailSendMethod());
        }

        void MailSendMethod()
        {
            /*foreach (Receiver item in receivers)
            {
                MailSend(item.Name + " Kişisi İçin Konu Kısmı", "İçerik", receivers.Select(i => i.MailAddress).ToList(), attachments.Select(i => i.Filepath).ToList());
            }*/

            for (int i = 0; i < receivers.Count; i++)
            {
                MailSend(receivers[i].Name + " Kişisi İçin Konu Kısmı", "İçerik", receivers.Select(j => j.MailAddress).ToList(), attachments.Select(k => k.Filepath).ToList());
                DtgReceivers.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
            }
        }
    }

    class AttachmentFile
    {
        string name, filePath;

        public string Name { get => name; set => name = value; }
        public string Filepath { get => filePath; set => filePath = value; }

        public AttachmentFile(string name, string filePath)
        {
            this.name = name;
            this.filePath = filePath;
        }
    }

    class Receiver
    {
        string name, mailAddress;

        public string Name { get => name; set => name = value; }
        public string MailAddress { get => mailAddress; set => mailAddress = value; }

        public Receiver(string name, string mailAddress)
        {
            this.name = name;
            this.mailAddress = mailAddress;
        }
    }
}
