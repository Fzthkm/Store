using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewStudio.Models
{
    public class UpdateViewModel
    {
        public User User { get; set; }
        public string newPassword { get; set; }
        public string confirmPassword { get; set; }
        public string oldPassword { get; set; }
        public string newAdress { get; set; }
        public string newCard { get; set; }

    }
}
