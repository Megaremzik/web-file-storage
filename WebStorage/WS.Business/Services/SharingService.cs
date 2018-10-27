using System;
using WS.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WS.Business.Services
{
    class SharingService
    {
        User user = new User();
        public SharingService()
        {
            
        }
        public Guid OpenPublicAccesToFile(int? fileid)
        {
            HttpContext context. 
            return Guid.NewGuid();
        }
    }
}
