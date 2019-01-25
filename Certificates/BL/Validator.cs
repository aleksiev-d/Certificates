using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Certificates.Interfaces;

namespace Certificates.WebAPI.BL
{
    public static class Validator
    {
        public static bool Validate(string certificateId)
        {
            // presumably more checks than just null, eg. longer than 6 chars, etc.
            return (!(certificateId is null));
        }

        public static bool ValidateNoId(Certificate certificate)
        {
            return (!(certificate.Title is null) && !(certificate.OwnerId is null) && !(certificate.CreatedAt > DateTime.Now));
        }

        public static bool ValidateWithId(Certificate certificate)
        {
            return (!(certificate.Id is null) && !(certificate.Title is null) && !(certificate.OwnerId is null));
        }
    }
}
