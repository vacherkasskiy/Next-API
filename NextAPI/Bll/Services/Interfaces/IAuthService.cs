﻿using System.Security.Claims;
using NextAPI.Dal.Entities;

namespace NextAPI.Bll.Services.Interfaces;

public interface IAuthService
{
    Task<ClaimsPrincipal> Register(User user);
    Task<ClaimsPrincipal> Login(string email, string password);
}