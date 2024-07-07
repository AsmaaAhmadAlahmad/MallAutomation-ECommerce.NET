using AlhamraMall.Domains.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlhamraMall.Domains.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public required string UserName { get; set; }
        public required string Email { get; set; }
        public bool IsDeleted { get; set; } = false;
        //public string Role { get; set; } 
        public required string Password { get; set; }

        public List<Role> Roles { get; set; }

        public User()
        {
            Id = Guid.NewGuid();
            Roles = new List<Role>();
        }
    }
}
