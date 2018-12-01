using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WS.Business.ViewModels;

namespace WS.Business.ViewModels
{
    public class DocumentViewModel
    {
        public DocumentView Document { get; set; }
        public string Size { get; set; }
        public bool IsShared { get; set; }
        public DocumentViewModel(DocumentView doc, string size="", bool isShared = false)
        {
            Document = doc;
            Size = size;
            IsShared = isShared;
        }
    }
}
