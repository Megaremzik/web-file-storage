using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS.Data
{
    public class UserDocument
    {
        public string IdUser { get; set; }
        public int IdDocument { get; set; }
        public string Link { get; set; }
        public bool IsEditable { get; set; }

        public virtual Document Document { get; set; }
        public virtual User ApplicationUser { get; set; }
    }
}
