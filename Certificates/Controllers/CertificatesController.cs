using System.Collections.Generic;
using System.Linq;
using Certificates.Interfaces;
using Certificates.WebAPI.BL;
using Microsoft.AspNetCore.Mvc;

namespace Certificates.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Route("api/certificates")]
    [ApiController]
    public class CertificatesController : BaseController
    {
        // GET api/values
        //[HttpGet]
        //public ActionResult<IEnumerable<string>> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        /*
         /certificates[/:certificateId]

          create (POST),
          update (PATCH), 
          read (GET) and 
          delete (DELETE) certiﬁcates. */

        // GET api/certificates/5
        [HttpGet("{certificateId}")]
        public ActionResult<Certificate> Get(string certificateId)
        {
            var objectValid = Validator.Validate(certificateId);

            if (objectValid)
            {
                var certificate = Certificates.Find(s => s.Id == certificateId);

                if (!(certificate is null))
                    return Ok(certificate);
                else
                    return NotFound(certificateId);
            }

            // validation unsuccessful or object does not exist
            return BadRequest("Certificate Id cannot be null!");
        }


        // PUT api/values
        [HttpPut("{certificateId}")]
        public ActionResult<Certificate> Put(string certificateId, [FromBody] Certificate certificate)
        {
            var objectValid = Validator.ValidateNoId(certificate);

            if (objectValid)
            {
                var addedOk = TryAddCertificate(certificate, certificateId);

                if (addedOk)
                    return CreatedAtRoute("api/certificates/certificateId", certificate);
            }

            // addition was unsuccessful
            return BadRequest(certificate);
        }
        

        // POST api/values
        [HttpPost("{certificateId}")]
        public ActionResult<Certificate> Post(string certificateId, [FromBody] Certificate certificate)
        {
            var objectValid = Validator.Validate(certificateId);

            if (objectValid)
            {
                var addedOk = TryAddCertificate(certificate, certificateId);

                if (addedOk)
                    return CreatedAtRoute("api/certificates/certificateId", certificate);
            }

            // addition was unsuccessful
            return BadRequest(certificate);
        }

        // POST api/values
        [HttpPost]
        public ActionResult<Certificate> PostIdInBody([FromBody] Certificate certificate)
        {
            if (certificate.Id is null)
                return BadRequest("Certificate Id cannot be null!");

            var objectValid = Validator.ValidateWithId(certificate);

            if (objectValid)
            {
                var addedOk = TryAddCertificate(certificate, certificate.Id);

                if (addedOk)
                    return CreatedAtRoute("api/certificates/certificateId", certificate);
            }

            // validation or addition was unsuccessful
            return BadRequest(certificate);
        }

        // PATCH api/values/5
        [HttpPatch("{certificateId}")]
        public ActionResult<Certificate> Patch(int certificateId, [FromBody] Certificate certificate)
        {
            if (certificate.Id is null)
                return BadRequest("Certificate Id cannot be null!");

            var objectValid = Validator.ValidateWithId(certificate);

            if (objectValid)
            {
                var updatedOk = TryUpdateCertificate(certificate, certificate.Id);

                if (updatedOk)
                    return CreatedAtRoute("api/certificates/certificateId", certificate);
            }

            // addition was unsuccessful
            return BadRequest(certificate);
        }


        // DELETE api/values/5
        [HttpDelete("{certificateId}")]
        public ActionResult<string> Delete(string certificateId)
        {
            if (certificateId is null)
                return BadRequest("Certificate Id cannot be null!");

            var objectValid = Validator.Validate(certificateId);

            if (objectValid)
            {
                var deletedOk = TryDeleteCertificate(certificateId);

                if (deletedOk)
                    return Ok(certificateId);
            }

            return BadRequest(certificateId);
        }

        //----------- /certificates/:certificateId/transfers endpoint allowing to create(POST) and accept(aka update or PATCH) a transfer.
        // POST /certificates/:certificateId/transfer
        [HttpPost("{certificateId}/transfers")]
        public ActionResult<string> PostTransfer(string certificateId, [FromBody] Transfer transfer)
        {
            // check if user exists
            var user = Users.Find(u => u.Email.Equals(transfer.Email));
            if (user is null)
                return BadRequest("User with email: " + transfer.Email.ToString() + " does not exist!");

            // check if certificate exists
            var certificate = Certificates.Find(c => c.Id == certificateId);
            if (certificate is null)
                return BadRequest("Certificate with id: " + certificateId + "does not exist!");

            // transfer to new user with said email
            certificate.InitiateTransferTo(user);
            return Ok("Initiated transfer of certificate " + certificateId + "!");

            /*{
                to: 'email',     
                status: 'string', } */

        }

        // POST /certificates/:certificateId/transfer
        [HttpPost("{certificateId}/accept-transfers/{toEmail}")]
        public ActionResult<string> AcceptTransfer(string certificateId, string toEmail)
        {
            // check if user exists
            var user = Users.Find(u => u.Email.ToString() == toEmail);
            if (user is null)
                return BadRequest("User with email: " + toEmail + " does not exist!");

            // check if certificate exists
            var certificate = Certificates.Find(c => c.Id == certificateId);
            if (certificate is null)
                return BadRequest("Certificate with id: " + certificateId + "does not exist!");

            // transfer to new user with said email
            var transferAccepted = certificate.AcceptTransferTo(user);
            if (transferAccepted)
            {
                return Ok("Accepted transfer of certificate " + certificateId + "!");
            }

            return BadRequest("Could not accept transfer of certificate with Id: " + certificateId +  " to user with email: " + toEmail);
        }

        // PATCH /certificates/:certificateId/transfer
        [HttpPatch("{certificateId}/transfers")]
        public ActionResult<string> PatchTransfer(string certificateId, [FromBody] Transfer transfer)
        {
            // check if user exists
            var user = Users.Find(u => u.Email.Equals(transfer.Email));
            if (user is null)
                return BadRequest("User with email: " + transfer.Email + " does not exist!");

            // check if certificate exists
            var certificate = Certificates.Find(c => c.Id == certificateId);
            if (certificate is null)
                return BadRequest("Certificate with id: " + certificateId + "does not exist!");

            // transfer to new user with said email
            certificate.InitiateTransferTo(user);
            return Ok("Initiated transfer of certificate " + certificateId + "!");

            /*{
                to: 'email',     
                status: 'string', } */
        }

        private bool TryAddCertificate(Certificate certificate, string certificateId)
        {
            if (!Certificates.Contains(certificate) && Certificates.All(s => s.Id != certificateId))
            {
                Certificates.Add(certificate);
                return true;
            }

            return false;
        }


        private bool TryUpdateCertificate(Certificate certificate, string certificateId)
        {
            if (!(certificateId is null) && Certificates.Any(s=>s.Id == certificateId))
            {
                Certificates.Add(certificate);
                return true;
            }

            return false;
        }


        private bool TryDeleteCertificate(string certificateId)
        {
            if (!(certificateId is null) && Certificates.Any(s => s.Id == certificateId))
            {
                Certificates.RemoveAll(s=>s.Id == certificateId);
                return true;
            }

            return false;
        }
    }
}
