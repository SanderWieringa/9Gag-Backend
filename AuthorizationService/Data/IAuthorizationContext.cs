﻿using AuthorizationService.Models;

namespace AuthorizationService.Data
{
    public interface IAuthorizationContext
    {
        IEnumerable<User> GetAllUsers();
        void CreateUser(User user);
        Task<int> SaveChangesAsync();
        
    }
}
