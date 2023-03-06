using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _18_E_LEARN.DataAccess.Data.ViewModels.User
{
    public class EditUserVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public string LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public string Role { get; set; }
    }
}
