using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text;

namespace Certificates.Interfaces
{
    public class Transfer
    {
        public MailAddress Email { get; set; }

        public string Status { get; set; }

        /*{
         to: 'email',     
         status: 'string', } */
    }
}
