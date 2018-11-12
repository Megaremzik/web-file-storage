using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WS.Data;
using WS.Interfaces;

namespace WS.Business.Services
{
    public class DocumentLinkServices
    {
        private IRepository<Document> repo;
        private readonly IMapper mapper;

        public DocumentLinkServices(IMapper map, IRepository<Document> r)
        {
            mapper = map;
            repo = r;
        }


    }
} 
