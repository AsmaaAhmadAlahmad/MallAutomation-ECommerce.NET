using AlhamraMall.Domains.Entities;
using AlhamraMall.Domains.Models;
using Microsoft.EntityFrameworkCore;

namespace AlhamraMall.Data
{
    public class AlhamraMallDbContext : DbContext
    {
        public AlhamraMallDbContext()
        {
            
        }
        public AlhamraMallDbContext(DbContextOptions<AlhamraMallDbContext> options)
            :base(options)
        {

        }
        public DbSet<Customer> Customers { get; set; }

        // الجدول التالي سأقوم بحذفه و الاستعاضة عنه بجعل حقل رقم القسم في جدول من النوع 
        // في جدول المحال التجارية والموظفين
        // في الفرونت ايند ويتم اضافته كرقم الى الباكايند  Enum 
        // public DbSet<Department> Departments { get; set; } 
        public DbSet<CommercialStore> CommercialStores { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer();
        }


       
    }
}