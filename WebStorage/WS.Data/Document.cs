using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WS.Data
{
    public class Document
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public DateTime Date_change { get; set; }
        public int Size { get; set; }
        public int ParentId { get; set; }
        public bool IsFile { get; set; }
        public string Extention { get; set; }
        public virtual DocumentLink DocumentLink { get; set; }
        public virtual ICollection<UserDocument> UserDocuments { get; set; }
        public virtual User User { get; set; }

        public Document()
        {
            UserDocuments = new List<UserDocument>();
        }
    }
}
