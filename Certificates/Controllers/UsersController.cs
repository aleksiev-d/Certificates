using System.Collections.Generic;
using System.Linq;
using Certificates.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Certificates.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Route("api/users")]
    [ApiController]
    public class UsersController : BaseController
    {
        // GET api/users
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            return Users;
        }

        // GET api/users/5/
        [HttpGet("{userId}")]
        public ActionResult<User> GetUser(string userId)
        {
            var user = Users.FirstOrDefault(u => u.Id == userId);
            if (user is null)
                return BadRequest("User with Id: " + userId + " does not exist!");

            return Ok(user);
        }

        // GET api/users/5/certificates
        [HttpGet("{userId}/certificates")]
        public ActionResult<IEnumerable<Certificate>> GetUserCertificates(string userId)
        {
            var user = Users.FirstOrDefault(u => u.Id == userId);
            if (user is null)
                return BadRequest("User with Id: " + userId + " does not exist!");

            var userCertificates = Certificates.Where(c => c.OwnerId == userId);

            return Ok(userCertificates);
        }

        // POST api/users
        [HttpPost]
        public ActionResult<string> Post([FromBody] IEnumerable<User> users)
        {
            List<string> badRequestIds = new List<string>();
            foreach (var user in users)
            {
                var userExists = Users.Any(u => u.Id == user.Id);
                if (userExists)
                {
                    badRequestIds.Add(user.Id);
                    continue;
                }

                Users.Add(user);
            }

            if (badRequestIds.Count == 0)
                return Ok();

            return BadRequest("Users with the following Ids were not added because the Ids already exist: " + badRequestIds);

        }

        // PUT api/users/5
        [HttpPut("{userId}")]
        public ActionResult<string> Put(string userId, [FromBody] User user)
        {
            var userExists = Users.Any(u => u.Id == userId);
            if (userExists)
                return BadRequest("User with Id: " + userId + " aleady exist!");

            Users.Add(new User()
            {
                Email = user.Email,
                Id = userId,
                Name = user.Name
            });

            return Ok(user);
        }        

        // PATCH api/users/5
        [HttpPatch("{userId}")]
        public ActionResult<string> Patch(string userId, [FromBody] User user)
        {
            if (userId is null)
                return BadRequest("UserId cannot be null!");

            var userExists = Users.Any(u => u.Id == userId);
            if (!userExists)
                return BadRequest("User with Id: " + userId + " does not exist!");

            Users.RemoveAll(u=>u.Id == userId);
            Users.Add(new User()
            {
                Email = user.Email,
                Id = userId,
                Name = user.Name
            });

            return Ok(user);
        }

        // DELETE api/users/5
        [HttpDelete("{userId}")]
        public ActionResult<string> Delete(string userId)
        {
            if (userId is null)
                return BadRequest("UserId cannot be null!");

            var user = Users.FirstOrDefault(u => u.Id == userId);

            if (user is null)
                return BadRequest("User with Id: " + userId + " does not exist!");

            var userRemoved = Users.RemoveAll(u=>u.Id == userId);
            if (userRemoved >= 1)
            {
                return Ok("Removed user with Id: " + userId);
            }

            return BadRequest("User with Id: " + userId + " does not exist!");
        }
    }
}
