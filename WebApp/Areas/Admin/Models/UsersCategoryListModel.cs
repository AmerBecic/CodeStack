using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Admin.Models
{
    public class UsersCategoryListModel
    {
        public int CategoryId { get; set; }

        public ICollection<UserModel> Users { get; set; } //All users available

        public ICollection<UserModel> UsersSelected { get; set; }   //All users selected for particular CategoryId
    }
}
