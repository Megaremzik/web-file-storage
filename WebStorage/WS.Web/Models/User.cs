using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WS.Web.Models
{
    public class User
    {
        public int Id { get; set; }
        public string First_name { get; set; }
        public string Second_name { get; set; }
        public string Email { get; set; }
        public string Passwd_hash { get; set; }
    }
}
