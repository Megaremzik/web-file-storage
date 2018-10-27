using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS.Business.ViewModels
{
    class UserView
    {
        public string Id;
        public string Email { get; set; }
        public virtual ICollection<UserDocumentView> UserDocuments { get; set; }
        public virtual ICollection<DocumentView> Documents { get; set; }
    }
}
