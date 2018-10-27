using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WS.Data
{
    public class DocumentLink
    {
        [Key]
        [ForeignKey("Document")]
        public int Id { get; set; }
        public Guid Link { get; set; }
        public bool IsEditable { get; set; }

        public virtual Document Document { get; set; }
    }
}
