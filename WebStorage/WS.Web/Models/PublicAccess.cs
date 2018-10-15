using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WS.Web.Models
{
    public class PublicAccess
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public int File_id { get; set; }
        public bool Write { get; set; }
    }
}
