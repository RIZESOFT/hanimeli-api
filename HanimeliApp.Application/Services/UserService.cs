using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using HanimeliApp.Application.Exceptions;
using HanimeliApp.Application.Services.Abstract;
using HanimeliApp.Domain.Dtos.User;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models.User;
using HanimeliApp.Domain.UnitOfWorks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace HanimeliApp.Application.Services
{
    public class UserService : ServiceBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public UserService(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper) : base(unitOfWork)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<User?> GetUserById(int id)
        {
            var user = await UnitOfWork.Repository<User>().GetAsync(x => x.Id == id);
            return user;
        }
        
        public async Task<UserModel> CreateUser(CreateUserRequest request)
        {
            var userRepository = UnitOfWork.Repository<User>();
            var user = await userRepository.GetAsync(x => x.Email == request.Email || x.Phone == request.Phone);

            if (user != null)
                throw ValidationExceptions.UserAlreadyExists;
            
            var hasher = new PasswordHasher<object>();
            var dummyUser = new object();
            var hashedPassword = hasher.HashPassword(dummyUser, request.Password);

            user = _mapper.Map<User>(request);
            user.Password = hashedPassword;
            
            await userRepository.InsertAsync(user);
            await UnitOfWork.SaveChangesAsync();
            var userModel = _mapper.Map<UserModel>(user);
            return userModel;
        }

        public async Task<UserModel> UpdateUser(int userId, UpdateUserRequest request)
        {
            var userRepository = UnitOfWork.Repository<User>();
            var user = await userRepository.GetAsync(x => x.Id == userId);

            if (user == null)
                throw ValidationExceptions.InvalidUser;
            
            _mapper.Map(request, user);
            
            userRepository.Update(user);
            await UnitOfWork.SaveChangesAsync();
            var userModel = _mapper.Map<UserModel>(user);
            return userModel;
        }

        public async Task<UserLoginResultModel> Login(UserLoginRequest request)
        {
            var user = await UnitOfWork.Repository<User>().GetAsync(x => (x.Email == request.Username || x.Phone == request.Username));

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

            var result = new UserLoginResultModel(user.Name, user.LastName, user.Email, token)
            {
                PhoneNumber = user.Phone
            };
            return result;
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
    }
}
