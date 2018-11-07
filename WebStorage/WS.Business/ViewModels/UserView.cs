using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS.Business.ViewModels
{
    public class UserView
    {
        public string Id;
        public string UserName;
        public string Email { get; set; }
        public virtual ICollection<UserDocumentView> UserDocuments { get; set; }
        public virtual ICollection<DocumentView> Documents { get; set; }
    }
}
