using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using OpenSchool.src.Helpers;
using OpenSchool.src.Models;

namespace OpenSchool.src.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWT _jwt;

        public AuthService(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
        }

        // Registration 
        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {

            // Check if user already Registered
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
            {
                return new AuthModel { Message = "Email is already Registered !" };
            }
            if (await _userManager.FindByNameAsync(model.Username) is not null)
            {
                return new AuthModel { Message = "Username is already Registered !" };
            }

            // if user not registered => register user 
            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Username
            };

            // Create User 
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description}";
                }
                return new AuthModel { Message = errors };
            }

            // Assign user to role
            await _userManager.AddToRoleAsync(user, "User");

            // Generate Token for user 
            var jwtSecurityToken = await CreateJwtToken(user);

            return new AuthModel
            {
                Username = user.UserName,
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "user" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            };

        }

        // GetToken
        public async Task<AuthModel> GetTokenAsync(TokenRequestModel model)
        {
            var authModel = new AuthModel();

            if (model.Email == null)
            {
                authModel.Message = "Email or username are required!";
                authModel.IncompleteForm = true;
                return authModel;
            }
            if (model.Password == null)
            {
                authModel.Message = "Password is required!";
                authModel.IncompleteForm = true;
                return authModel;
            }

            var userByEmail = await _userManager.FindByEmailAsync(model.Email);
            var userByName = await _userManager.FindByNameAsync(model.Email);
            var usedField = userByEmail is not null? "Email": "Username";
            var user = userByEmail ?? userByName;
            var password = await _userManager.CheckPasswordAsync(user, model.Password);

            if (user is null || !password)
            {
                authModel.Message = $"{usedField} or password are incorrect!";
                return authModel;
            }

            var jwtSecurityToken = await CreateJwtToken(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Username = user.UserName;
            authModel.Email = user.Email;

            var rolesList = await _userManager.GetRolesAsync(user);
            authModel.Roles = rolesList.ToList();


            return authModel;
        }

        //Generate JwtToken
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user); // Add user Claim
            var roles = await _userManager.GetRolesAsync(user); // get roles => optional may you need it in front-end
            var roleClaims = new List<Claim>(); // to add Calims of roles 

            // loop in [ var roles ] that user assinged to and assign it to roleClaims 
            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),

				// we can add only[CustomValue] this in  var claims = new[]
        new Claim("uid", user.Id)
    }
            .Union(userClaims)
            .Union(roleClaims);  // Union Calims with userClaims & RoleClaims

            // SymmetricKey Using key add in appsetting.json
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            // signingCredentials using SymmetricKey  & use algorithm HmacSha256 [ HS256 ]
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            // Value that used to generate JWT token 
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,   // in appsetting.json
                audience: _jwt.Audience, // in appsetting.json
                claims: claims,         // List of Claims 
                expires: DateTime.Now.AddDays(_jwt.DurationInDays), // Expire 
                signingCredentials: signingCredentials);

            return jwtSecurityToken; // return jwtSecurityToken from type of JwtSecurityToken
        }


    }
}
