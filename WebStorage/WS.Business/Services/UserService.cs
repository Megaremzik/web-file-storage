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
        private UserManager<User> _userManager;
        public UserService(IMapper map, UserRepository r, UserManager<User> userManager)
        {
            _userManager = userManager;
            mapper = map;
            repo = r;
        }
        public string GetUserIdByDocumentId(int documentId)
        {
            return repo.GetUserIdByDocumentId(documentId);
        }
        public UserView GetUserByUserClaims(ClaimsPrincipal userClaims)
        {
            User user = repo.GetUserByName(userClaims.Identity.Name);
            return mapper.Map<User, UserView>(user);
        }
        public bool IsUserTheOwnerOfTheDocument(ClaimsPrincipal user, int documentId)
        {
            string userId = GetUserIdByDocumentId(documentId);
            string userId2 = _userManager.GetUserId(user);
            return userId == userId2;
        }
        public string GetUserId(ClaimsPrincipal user)
        {
            return _manage.GetUserId(user);
        }
    }
}
