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
using WebLuanVan.Data.ViewModels.Common;
using WebLuanVan.Data.ViewModels.ModelBinding;
using WebLuanVan.Data.ViewModels.Request.Users;

namespace WebLuanVan.Data.Services.System.Users
{
    public class UserServices : IUserService
    {
        private readonly IMongoCollection<Account> _userCollection;
        private readonly IConfiguration _config;
        private readonly IMongoCollection<Role> _roleCollection;
        public UserServices(IMongoDatabase database, IConfiguration config)
        {
            _userCollection = database.GetCollection<Account>("account");
            _config = config;
            _roleCollection = database.GetCollection<Role>("role");
            
        }
        public async Task CreateIndexAsync()
        {
            var indexKeysDefinition = Builders<Account>.IndexKeys.Ascending(account => account.Username);
            await _userCollection.Indexes.CreateOneAsync(new CreateIndexModel<Account>(indexKeysDefinition));
        }
        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
        {
            string hashPassword = Common.Common.CreateMD5(request.Password);
            var account = await _userCollection.Find(x => x.Username == request.UserName && x.Password == hashPassword).FirstOrDefaultAsync();
            if(account == null)
            {
                return new ApiErrorResult<string>("Username or password is incorrect!");
            }
            if (!account.Status)
            {
                return new ApiErrorResult<string>("Account is not active!");
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, account.FirstName + " " + account.LastName),
                new Claim(ClaimTypes.Role, GetRoleName(account.RoleId))
                //new Claim(ClaimTypes.)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);
            return new ApiSuccessResult<string>(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public async Task<bool> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return false;
            }
            ObjectId objId = ObjectId.Parse(id);
            var result = await _userCollection.DeleteOneAsync(x => x.Id == objId);
            if(result.DeletedCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<ApiResult<User>> GetUserById(string id)
        {
            ObjectId objId = ObjectId.Parse(id);
            var account = await _userCollection.Find(x => x.Id == objId).FirstOrDefaultAsync();
            if (account == null)
            {
                return new ApiErrorResult<User>("Cannot find user!");
            }
            User user = new User()
            {
                Username = account.Username,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Status = account.Status,
                Id = account.Id.ToString()
                //Password = account.
            };
            return new ApiSuccessResult<User>(user);
        }

        public async Task<ApiResult<PagedResult<User>>> GetUsersPaging(GetUserPagingRequest request)
        {
            //var result = await _userCollection.Find(new BsonDocument()).ToListAsync();
            if (string.IsNullOrEmpty(request.Keyword))
            {
                request.Keyword = "";
            }
            int totalRow = (int)await _userCollection.Find(x => x.FirstName.ToLower().Contains(request.Keyword) || x.LastName.ToLower().Contains(request.Keyword)).CountDocumentsAsync();
            var result = await _userCollection.Find(x => x.FirstName.ToLower().Contains(request.Keyword) || x.LastName.ToLower().Contains(request.Keyword))
                    .Skip((request.PageIndex - 1) * request.PageSize).Limit(request.PageSize).ToListAsync();

            
            List<User> listUser = new List<User>();
            foreach(var item in result) //Binding user from collection account from database
            {
                User user = new User();
                user.Username = item.Username;
                user.LastName = item.LastName;
                user.FirstName = item.FirstName;
                user.Password = item.Password;
                user.Id = item.Id.ToString();
                user.Status = item.Status;
                listUser.Add(user);
            }
            
            var pagedResult = new PagedResult<User>()
            {
                Items = listUser,
                TotalRecord = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
            return new ApiSuccessResult<PagedResult<User>>(pagedResult);
        }

        public async Task<ApiResult<bool>> Register(RegisterRequest request)
        {
            await CreateIndexAsync();
            var checkExist = await _userCollection.CountDocumentsAsync(x => x.Username == request.UserName);
            if(checkExist > 0)
            {
                return new ApiErrorResult<bool>("Username is Exist!"); 
            }
            var account = new Account()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = Common.Common.CreateMD5(request.Password),
                RoleId = GetRoleId("User"),
                Username = request.UserName,
                Status = false
            };
            await _userCollection.InsertOneAsync(account);
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> Update(string id, UserUpdateRequest request)
        {
            ObjectId objId = ObjectId.Parse(id);
            var user = await _userCollection.Find(x => x.Id == objId).FirstOrDefaultAsync();
            if(user == null)
            {
                return new ApiErrorResult<bool>("Cannot find user!");
            }
            var filter = Builders<Account>.Filter.Eq("_id", objId);
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            var result = await _userCollection.ReplaceOneAsync(filter, user);
            if(result.MatchedCount > 0)
            {
                return new ApiSuccessResult<bool>();
            }
            else
            {
                return new ApiErrorResult<bool>("Cannot update user!");
            }
            
        }

        public ObjectId GetRoleId(string roleName)
        {
            var role = _roleCollection.Find(x => x.Name == roleName).FirstOrDefault();

            return role.Id;
        }
        private string GetRoleName(ObjectId roleId)
        {
            var name = _roleCollection.Find(x => x.Id == roleId).FirstOrDefault();
            return name.Name;
        }
        public async Task<bool> ChangeStatus(string id)
        {
            ObjectId objId = ObjectId.Parse(id);
            var user = await _userCollection.Find(x => x.Id == objId).FirstOrDefaultAsync();
            if (user == null)
            {
                return false;
            }
            bool status = user.Status ? false : true;
            var filter = Builders<Account>.Filter.Eq("_id", objId);
            var update = Builders<Account>.Update.Set("status", status);
            var result = await _userCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
    }
}
