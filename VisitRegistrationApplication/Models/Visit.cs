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

        public int Date { get; set; }

        public int UserId { get; set; }

        public int EmployeeId { get; set; }

        public int EmployeeName { get; set; }

    }
}