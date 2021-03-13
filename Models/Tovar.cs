using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
public enum category
{
    mouse = 1,
    kreslo,
    keyboard, 
    notebook,
    games,
    console
}

namespace NewStudio.Models
{
    public class Tovar
    {
        public int Id { get; set; }
        public int Cost { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public category category { get; set; }
        public string Description { get; set; }

        public List<Order> Orders { get; set; }
        public Tovar()
        {
            Orders = new List<Order>();
        }
    }
}
