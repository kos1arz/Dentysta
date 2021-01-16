using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace VisitRegistrationApplication.Models
{
    public static class Hash
    {
        public static string HashPassword(string value)
        {
            return Convert.ToBase64String(
                System.Security.Cryptography.SHA256.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(value))
                );
        }
    }
}