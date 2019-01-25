using System;
using System.Collections.Generic;
using System.Net.Mail;
using Certificates.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Certificates.WebAPI.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        public BaseController()
        {
            Certificates = new List<Certificate>
            {
                new Certificate()
                {
                    CreatedAt = DateTime.Now,
                    Id = "certId1",
                    Note = "Blah blah notes",
                    OwnerId = "admin",
                    Title = "Title1",
                    Transfer = new Transfer(),
                    Year = 2017
                },
                new Certificate()
                {
                    CreatedAt = DateTime.Now,
                    Id = "certId2",
                    Note = "Notes notes",
                    OwnerId = "user1",
                    Title = "Title2",
                    Transfer = new Transfer(),
                    Year = 2018
                },
                new Certificate()
                {
                    CreatedAt = DateTime.Now,
                    Id = "certId3",
                    Note = "Some notes",
                    OwnerId = "user2",
                    Title = "Title2",
                    Transfer = new Transfer(),
                    Year = 2019
                },

            };

            Users = new List<User>
            {
                new User()
                {
                    Email = new MailAddress("admin@mail.com"),
                    Id = "admin",
                    Name = "Yoana Smeshnata"
                },
                new User()
                {
                    Email = new MailAddress("user1@mail.com"),
                    Id = "user1",
                    Name = "User Userov"
                },
                new User()
                {
                    Email = new MailAddress("user2@mail.com"),
                    Id = "user2",
                    Name = "Another Userski"
                }
            };
        }
        protected List<Certificate> Certificates { get; set; }
        protected List<User> Users { get; set; }
        
    }
}
