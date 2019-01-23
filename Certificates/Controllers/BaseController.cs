using System.Collections.Generic;
using Certificates.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Certificates.WebAPI.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected List<Certificate> Certificates { get; set; }
        protected List<User> Users { get; set; }
        
    }
}
