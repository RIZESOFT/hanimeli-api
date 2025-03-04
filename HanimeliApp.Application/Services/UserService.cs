﻿using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using HanimeliApp.Application.Exceptions;
using HanimeliApp.Application.Services.Abstract;
using HanimeliApp.Application.Utilities;
using HanimeliApp.Domain.Dtos.Address;
using HanimeliApp.Domain.Dtos.User;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Enums;
using HanimeliApp.Domain.Models;
using HanimeliApp.Domain.Models.Address;
using HanimeliApp.Domain.Models.User;
using HanimeliApp.Domain.UnitOfWorks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace HanimeliApp.Application.Services
{
    public class UserService : ServiceBase<User, UserModel, CreateUserRequest, UpdateUserRequest>
    {
        private readonly IConfiguration _configuration;

        public UserService(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper) : base(unitOfWork,
            mapper)
        {
            _configuration = configuration;
        }


        public async Task<List<UserModel>> GetListWithFilter(UserFilterModel? filterModel, int pageNumber, int pageSize)
        {
            Expression<Func<User, bool>> filter = x => true;
            if (filterModel != null)
            {
                if (filterModel.IsB2b.HasValue)
                {
                    filter = filter.AndAlso(x => x.Role == Roles.B2B);
                }
            }

            var paging = new EntityPaging();
            paging.PageNumber = pageNumber;
            paging.ItemCount = pageSize;
            var entities = await UnitOfWork.Repository<User>().GetListAsync(filter,
                x => x.OrderBy(y => y.Id),
                paging: paging);
            var models = Mapper.Map<List<UserModel>>(entities);
            return models;
        }

        public override async Task<UserModel> Create(CreateUserRequest request)
        {
            var userRepository = UnitOfWork.Repository<User>();
            var user = await userRepository.GetAsync(x => x.Email == request.Email || x.Phone == request.Phone);

            if (user != null)
                throw ValidationExceptions.UserAlreadyExists;

            var hasher = new PasswordHasher<object>();
            var dummyUser = new object();
            var hashedPassword = hasher.HashPassword(dummyUser, request.Password);

            user = Mapper.Map<User>(request);
            user.Password = hashedPassword;

            await userRepository.InsertAsync(user);
            await UnitOfWork.SaveChangesAsync();
            var userModel = Mapper.Map<UserModel>(user);
            return userModel;
        }

        public override async Task<UserModel> Update(int userId, UpdateUserRequest request)
        {
            var userRepository = UnitOfWork.Repository<User>();
            var user = await userRepository.GetAsync(x => x.Id == userId);

            if (user == null)
                throw ValidationExceptions.InvalidUser;

            Mapper.Map(request, user);

            userRepository.Update(user);
            await UnitOfWork.SaveChangesAsync();
            var userModel = Mapper.Map<UserModel>(user);
            return userModel;
        }

        public async Task<UserLoginResultModel> Login(UserLoginRequest request)
        {
            var user = await UnitOfWork.Repository<User>()
                .GetAsync(x => (x.Email == request.Username || x.Phone == request.Username));

            if (user == null)
                throw AuthenticationExceptions.UserInvalidException;

            var hasher = new PasswordHasher<object>();
            var dummyUser = new object();
            var verificationResult = hasher.VerifyHashedPassword(dummyUser, user.Password, request.Password);

            switch (verificationResult)
            {
                case PasswordVerificationResult.Success:
                case PasswordVerificationResult.SuccessRehashNeeded:
                    break;

                case PasswordVerificationResult.Failed:
                    throw AuthenticationExceptions.UserInvalidException;
            }

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.PhoneNumber, user.Phone),
                new(JwtRegisteredClaimNames.GivenName, user.Name),
                new(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(ClaimTypes.Role, user.Role),
            };

            var token = GenerateToken(claims);

            var result = new UserLoginResultModel(user.Id, user.Name, user.LastName, user.Email, user.Role, token)
            {
                PhoneNumber = user.Phone
            };
            return result;
        }

        public async Task<OperationUserLoginResultModel> OperationLogin(UserLoginRequest request)
        {
            var resultModel = await Login(request);
            var user = await UnitOfWork.Repository<User>().GetAsync(x =>
                x.Id == resultModel.Id && (x.Role == Roles.Cook || x.Role == Roles.Courier));

            if (user == null)
                throw AuthenticationExceptions.UserInvalidException;

            var operationResultModel = new OperationUserLoginResultModel(resultModel);
            if (user!.Role == Roles.Cook)
            {
                var cook = await UnitOfWork.Repository<Cook>().GetAsync(x => x.Id == user.CookId);
                operationResultModel.Iban = cook.Iban;
                operationResultModel.Nickname = cook.Name;
                operationResultModel.Address = cook.Address;
            }

            return operationResultModel;
        }

        private string GenerateToken(List<Claim> claims)
        {
            var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));
            var signingCredentials = new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256);

            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            var tokenExpiration = _configuration.GetValue<int>("JWT:TokenExpiryTimeInDay");
            var audience = _configuration["JWT:ValidAudience"];
            var issuer = _configuration["JWT:ValidIssuer"];

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                Audience = audience,
                Expires = DateTime.UtcNow.AddDays(tokenExpiration),
                SigningCredentials = signingCredentials,
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<List<UserModel>> GetB2BUserList(int pageNumber, int pageSize)
        {
            var paging = new EntityPaging();
            paging.PageNumber = pageNumber;
            paging.ItemCount = pageSize;
            var entities = await UnitOfWork.Repository<User>()
                .GetListAsync(x => x.Role == Roles.B2B, x => x.OrderBy(y => y.Id), paging: paging);
            var models = Mapper.Map<List<UserModel>>(entities);
            return models;
        }

        public async Task<UserModel> GetB2BUserSettings(int userId)
        {
            var userRepository = UnitOfWork.Repository<User>();
            var user = await userRepository.GetAsync(x => x.Id == userId && x.Role == Roles.B2B);

            if (user == null)
                throw ValidationExceptions.InvalidUser;

            var userModel = Mapper.Map<User, UserModel>(user);
            return userModel;
        }

        public async Task<UserModel> UpdateB2BUserSettings(UpdateB2BUserSettingsRequest request)
        {
            var userRepository = UnitOfWork.Repository<User>();

            var user = await userRepository.GetAsync(x => x.Id == request.UserId && x.Role == Roles.B2B);

            if (user == null)
                throw ValidationExceptions.InvalidUser;


            Mapper.Map(request, user);
            userRepository.Update(user);
            await UnitOfWork.SaveChangesAsync();
            var userModel = Mapper.Map<UserModel>(user);
            return userModel;
        }
    }
}