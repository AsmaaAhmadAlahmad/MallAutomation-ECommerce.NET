using AlhamraMall.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlhamraMall.Domains.Models
{
    public class Role
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }

        public List<User> Users { get; set; } = new List<User>();
        public Role()
        {
            Id = new Guid();
        }
    }
}
