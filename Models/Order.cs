using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
public enum type
{
    Cash = 1,
    Card
}
public enum status
{
    Open = 1,
    Close
}
namespace NewStudio.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int _Count { get; set; }
        public type type { get; set; }
        public status status { get; set; }
        public int TovarId { get; set; }
        public Tovar Tovar{ get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
