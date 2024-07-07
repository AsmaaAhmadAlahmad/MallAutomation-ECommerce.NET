namespace AlhamraMall.Domains.Models
{
    public class Customer
    {
        public  Guid CustomerId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public  required string PostalCode { get; set; }
        public  required string ShippingAdress { get; set; } // عنوان التوصيل
        public ICollection<Order> Orders { get; set; }
        public Customer()
        {
            Orders = new List<Order>();
            CustomerId = Guid.NewGuid();
        }




    }
}