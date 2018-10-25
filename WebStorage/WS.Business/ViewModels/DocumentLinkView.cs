using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS.Business.ViewModels
{
    public class DocumentLinkView
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public bool IsEditable { get; set; }

        public virtual Document Document { get; set; }
    }
}
