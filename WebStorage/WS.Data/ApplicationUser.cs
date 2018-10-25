using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WS.Data
{
    // Add profile data for application users by adding properties to the User class
    public class User : IdentityUser
    {
        public virtual ICollection<UserDocument> UserDocuments { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
    }
}
