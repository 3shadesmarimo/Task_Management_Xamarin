using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Models
{
    public class UserModel
    {
        public UserModel(string uId, string name, string password)
        {
            this.uId = uId;
            this.name = name;
            this.password = password;
        }

        public string uId { get; set; }
        public string name { get; set; }

        public string password { get; set; }

    }
}
