using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Certificates.Interfaces
{
    public class User
    {
        public string Id { get; set; }

        public MailAddress Email { get; set; }

        public string Name { get; set; }
        /*{
            id: 'string',     
            email: '',     
            name: '', }
         */
    }
}
