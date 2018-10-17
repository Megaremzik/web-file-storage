using System;
using System.Collections.Generic;

namespace WS.Data
{
    public class Document
    {
        public int Id { get; set; }
        public int User_id { get; set; }
        public string Name { get; set; }
        public DateTime Date_change { get; set; }
        public int Size { get; set; }
        public int ParentId { get; set; }
        public bool IsFile { get; set; }

        public virtual DocumentLink DocumentLink { get; set; }
        public virtual ICollection<UserDocument> UserDocuments { get; set; }
        //User

        public Document()
        {
            UserDocuments = new List<UserDocument>();
        }
    }
}
