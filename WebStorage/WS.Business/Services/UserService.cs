using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using WS.Business.ViewModels;
using WS.Data;
using WS.Data.Repositories;

namespace WS.Business.Services
{
    public class UserService
    {
        private UserRepository repo;
        IMapper mapper;
        public UserService(IMapper map, UserRepository r)
        {
            mapper = map;
            repo = r;
        }
        public string GetUserIdByDocumentId(int documentId)
        {
            return repo.GetUserIdByDocumentId(documentId);
        }
        public UserView GetUserByName(string userName)
        {
            User user = repo.GetUserByName(userName);
            return mapper.Map<User, UserView>(user);
        }
    }
}
