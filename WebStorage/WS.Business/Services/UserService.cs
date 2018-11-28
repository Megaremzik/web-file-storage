using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WS.Business.ViewModels;
using WS.Data;
using WS.Data.Repositories;

namespace WS.Business.Services
{
    public class UserService
    {
        private UserRepository repo;
        IMapper mapper;
        UserManager<User> _manage;
        public UserService(IMapper map, UserRepository r, UserManager<User> manage)
        {

            _manage = manage;
            mapper = map;
            repo = r;
        }
        public string GetUserIdByDocumentId(int documentId)
        {
            return repo.GetUserIdByDocumentId(documentId);
        }
        public string GetUserId(ClaimsPrincipal user)
        {
            return _manage.GetUserId(user);
        }
        public UserView GetUserByName(string userName)
        {
            User user = repo.GetUserByName(userName);
            return mapper.Map<User, UserView>(user);
        }
        public bool IsUserTheOwnerOfTheDocument(string userName, int documentId)
        {
            string userId = GetUserIdByDocumentId(documentId);
            string userId2 = GetUserByName(userName).Id;
            return userId == userId2;
        }
    }
}
