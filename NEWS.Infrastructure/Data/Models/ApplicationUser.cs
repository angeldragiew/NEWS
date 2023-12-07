using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEWS.Infrastructure.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(20, MinimumLength = 3)]
        public string? FirstName { get; set; }

        [StringLength(20, MinimumLength = 3)]
        public string? LastName { get; set; }

        [StringLength(60, MinimumLength = 5)]
        public string? Address { get; set; }

        public ICollection<News> Orders { get; set; } = new List<News>();
    }
}
