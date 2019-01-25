using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Certificates.Interfaces
{
    public class Certificate
    {
        private static readonly MailAddress DontRespondEmail = new MailAddress("donotrespond@email.com");

        public string Id { get; set; }

        public string Title { get; set; }

        public DateTime CreatedAt { get; set; }

        public string OwnerId { get; set; }

        public uint Year { get; set; }

        public string Note { get; set; }
        public Transfer Transfer { get; set; }
        /*
         * certificate
        {     
            id: 'string'     
            title: 'string'     
            createdAt: 'date',     
            ownerId: 'string',     
            year: 'number', 
            note: 'string',     
            transfer: <object representing the transfer state> 
         } 
         */
        public void InitiateTransferTo(User user)
        {
            this.Transfer = new Transfer()
            {
                Email = user.Email,
                Status = "pending"
            };

            // send an email to user
            this.SendTransferEmailRequestTo(user);
        }

        public bool AcceptTransferTo(User user)
        {
            // check if status is pending and the transfer user is the one from the acceptance
            if (this.Transfer.Email.ToString() == user.Email.ToString() && this.Transfer.Status == "pending")
            {
                this.Transfer.Status = "accepted";
                this.OwnerId = user.Id;
                return true;
            }

            return false;
        }

        private void SendTransferEmailRequestTo(User toUser)
        {
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.EnableSsl = true;
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential("democertificates2019@gmail.com", "adminpass_2019");

            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("sender@domain.com", "Customer Service");
            mailMessage.To.Add(new MailAddress("someone@domain.com"));
            mailMessage.Subject = "Please accept the transfer of certificate" + this.Title + " to your name!";
            mailMessage.IsBodyHtml = true;

            var body = new StringBuilder();
            body.AppendFormat("Hello {0},\n", toUser.Name);
            body.AppendLine(@"You have been sent a certificate titled" + this.Title + " by " + this.OwnerId +
                            ", please click the activation link bwlow to complete the transfer of ownership!");
            body.AppendLine("<a href=\"http://localhost:5001/api/certificates/" + this.Id +
                            "/accept-transfers/" + toUser.Email + "\">Accept certificate</a>");

            mailMessage.Body = body.ToString();
            mailMessage.From = DontRespondEmail;
            mailMessage.To.Add(toUser.Email);
            smtp.Send(mailMessage);
        }
    }
}
