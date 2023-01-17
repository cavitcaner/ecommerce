using Microsoft.EntityFrameworkCore;

namespace Order.API.Database
{
    [Owned]
    public class Address
    {
        public string FullAdress { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string City { get; set; }
    }
}
