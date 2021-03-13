using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewStudio.Models
{
    public class User
    {
        public int Id { get; set; }
        public string _Username { get; set; }
        public string _Password { get; set; }
        public string _Name { get; set; }
        public int? RoleId { get; set; }
        public string CreditCard { get; set; }
        public string Adress { get; set; }
        public Role Role { get; set; }
        public List<Order> Orders { get; set; }
        public User()
        {
            Orders = new List<Order>();
        }
    }
}
