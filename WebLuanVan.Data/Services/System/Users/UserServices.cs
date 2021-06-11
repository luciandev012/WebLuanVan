using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebLuanVan.Data.Entity;
using WebLuanVan.Data.Services.Common;
using WebLuanVan.Data.ViewModels.Common;
using WebLuanVan.Data.ViewModels.ModelBinding;
using WebLuanVan.Data.ViewModels.Request.Users;

namespace WebLuanVan.Data.Services.System.Users
{
    public class UserServices : IUserService
    {
        private readonly IMongoCollection<Account> _userCollection;
        private readonly IConfiguration _config;
        private readonly IMongoCollection<Role> _roleCollecton;
        public UserServices(IMongoDatabase database, IConfiguration config)
        {
            _userCollection = database.GetCollection<Account>("account");
            _config = config;
            _roleCollecton = database.GetCollection<Role>("role");
        }
        public async Task<string> Authenticate(LoginRequest request)
        {
            string hashPassword = HashPasswordMD5.CreateMD5(request.Password);
            var account = await _userCollection.Find(x => x.Username == request.UserName && x.Password == hashPassword).FirstOrDefaultAsync();
            if(account == null)
            {
                return null;
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, account.FirstName + " " + account.LastName),
                new Claim(ClaimTypes.Role, account.RoleId.ToString()),
                //new Claim(ClaimTypes.)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<PagedResult<User>> GetUsersPaging(GetUserPagingRequest request)
        {
            var result = await _userCollection.Find(new BsonDocument()).ToListAsync();
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                result = await _userCollection.Find(x => x.FirstName.Contains(request.Keyword) || x.LastName.Contains(request.Keyword))
                    .Skip((request.PageIndex - 1) * request.PageSize).Limit(request.PageSize).ToListAsync();
   
            }
            List<User> listUser = new List<User>();
            foreach(var item in result) //Binding user from collection account from database
            {
                User user = new User();
                user.Username = item.Username;
                user.LastName = item.LastName;
                user.FirstName = item.FirstName;
                user.Password = item.Password;
                listUser.Add(user);
            }
            int totalRow = listUser.Count;
            var pagedResult = new PagedResult<User>()
            {
                Items = listUser,
                TotalRecord = totalRow
            };
            return pagedResult;
        }

        public async Task<bool> Register(RegisterRequest request)
        {
            var account = new Account()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = HashPasswordMD5.CreateMD5(request.Password),
                RoleId = await GetRoleId("User"),
                Username = request.UserName
            };
            await _userCollection.InsertOneAsync(account);
            return true;
        }
        private async Task<ObjectId> GetRoleId(string roleName)
        {
            var role = await _roleCollecton.Find(x => x.Name == roleName).FirstOrDefaultAsync();

            return role.Id;
        }
    }
}
