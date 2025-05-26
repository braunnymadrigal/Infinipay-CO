using back_end.Domain;
using back_end.Infraestructure;

using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace back_end.Application
{
    public class Login : ILogin
    {
        private readonly IConfiguration iConfiguration;
        private readonly LoginRepository loginRepository;

        public Login(IConfiguration iConfiguration)
        {
            this.iConfiguration = iConfiguration;
            loginRepository = new LoginRepository();
        }

        public string LogUser(LoginUserModel loginUserModel)
        {
            UserModel userModel = AuthenticateUser(loginUserModel);
            return (GenerateJwt(userModel));
        }

        private string GenerateJwt(UserModel userModel)
        {
            var jwtConfigKey = iConfiguration["JwtConfig:Key"];
            if (jwtConfigKey == null)
            {
                throw new Exception("'JwtConfig:Key' is missing.");
            }
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfigKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, userModel.Nickname),
            new Claim(ClaimTypes.Sid, userModel.PersonId),
            new Claim(ClaimTypes.Role, userModel.Role),
            };
            var token = new JwtSecurityToken
            (
                iConfiguration["JwtConfig:Issuer"],
                iConfiguration["JwtConfig:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel AuthenticateUser(LoginUserModel loginUserModel)
        {
            var nicknameOrEmail = (loginUserModel.NicknameOrEmail).ToLower();
            var inputBytes = Encoding.UTF8.GetBytes(loginUserModel.Password);
            var password = SHA512.HashData(inputBytes);
            var userModel = new UserModel();
            if (nicknameOrEmail.Contains("@"))
            {
                userModel = loginRepository.GetUserByEmail(nicknameOrEmail);
            }
            else
            {
                userModel = loginRepository.GetUserByNickname(nicknameOrEmail);
            }
            var isItPossibleToLogin = IsItPossibleToLogin(userModel);
            if (!isItPossibleToLogin)
            {
                throw new Exception("Too many failed attempts.");
            }
            if (userModel.Password == null)
            {
                throw new Exception("An error in the middle of the login process ocurred.");
            }
            var okPassword = password.SequenceEqual(userModel.Password);
            if (!okPassword)
            {
                DoInCaseNotOkPassword(userModel);
            }
            loginRepository.UpdateNumAttemptsInUser(userModel.PersonId, 0);
            return userModel;
        }

        private void DoInCaseNotOkPassword(UserModel userModel)
        {
            int numAttempts = userModel.NumAttempts + 1;
            loginRepository.UpdateNumAttemptsInUser(userModel.PersonId, numAttempts);
            if (numAttempts >= 5)
            {
                loginRepository.UpdateExactBlockDateInUser(userModel.PersonId);
            }
            throw new Exception("Nickname, Email or Password are not correct.");
        }

        private bool IsItPossibleToLogin(UserModel userModel)
        {
            bool isPossible = true;
            if (userModel.NumAttempts >= 5)
            {
                if (userModel.LastBlock == DateTime.MinValue)
                {
                    loginRepository.UpdateExactBlockDateInUser(userModel.PersonId);
                    isPossible = false;
                }
                else
                {
                    DateTime currentDateTime = DateTime.Now;
                    DateTime lastBlockPlusTen = (userModel.LastBlock).AddMinutes(10);
                    int resultOfDateTimeComparison = DateTime.Compare(lastBlockPlusTen, currentDateTime);
                    if (resultOfDateTimeComparison >= 0)
                    {
                        isPossible = false;
                    }
                }
            }
            return isPossible;
        }
    }
}
