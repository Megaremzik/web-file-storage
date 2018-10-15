using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WS.Web.Models
{
    public class File
    {
        public int Id { get; set; }
        public int Owner_id { get; set; }
        public string File_name { get; set; }
        public DateTime Data_change { get; set; }
        public int Size { get; set; }
        public string Path { get; set; }
    }
}
