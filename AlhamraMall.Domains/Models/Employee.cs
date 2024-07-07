using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlhamraMall.Domains.Models
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Salary { get; set; }
        public int DepartmentId { get; set; }
        public bool IsDeleted { get; set; } = false;

        public Employee()
        {
            Id = new Guid();
        }
    }
}
