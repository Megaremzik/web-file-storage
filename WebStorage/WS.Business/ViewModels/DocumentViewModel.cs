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
        public string Icon { get; set; }
        public int IntSize { get; set; }
        public DocumentViewModel(DocumentView doc, string size = "", int intSize = 0, bool isShared = false, string icon = "fa fa-file fa-fw")
        {
            Document = doc;
            Size = size;
            IsShared = isShared;
            Icon = icon;
            IntSize = intSize;
        }
    }
}
