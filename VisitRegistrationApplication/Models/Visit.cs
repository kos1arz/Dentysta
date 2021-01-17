using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VisitRegistrationApplication.Models
{
    public class Visit
    {
        public int Id { get; set; }

        public string Date { get; set; }

        public string UserId { get; set; }

        public string EmployeeId { get; set; }

    }
}