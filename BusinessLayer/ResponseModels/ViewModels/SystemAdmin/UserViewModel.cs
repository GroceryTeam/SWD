using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAcessLayer.Models.User;

namespace BusinessLayer.ResponseModels.ViewModels.SystemAdmin
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }

    }
}
