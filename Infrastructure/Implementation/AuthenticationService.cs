using Infrastructure.Application.Implementation;
using Infrastructure.Entity;
using Infrastructure.Helper;
using Infrastructure.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Implementation
{
    public class AuthenticationService
    {
        private readonly Repository<Person> _personRepository;
        private readonly Repository<UserLogin> _userLoginRepository;

        private readonly EncryptionService _encryptionService;
        private readonly AppSettings _appSettings;

        public AuthenticationService(UnitOfWork unitOfWork, EncryptionService encryptionService, IOptions<AppSettings> appSettings)
        {
            _userLoginRepository = unitOfWork.Repository<UserLogin>();
            _personRepository = unitOfWork.Repository<Person>();

            _encryptionService = encryptionService;
            _appSettings = appSettings.Value;
        }

        public string RegisterUser(UserRegistrationEditModel model)
        {
            var message = "";
            try
            {
                var existingUser = _userLoginRepository.Get(x => x.UserName == model.Email);

                if (existingUser == null)
                {
                    var person = CreatePersonModel(model);
                    _personRepository.Insert(person);

                    var loginModel = CreateUserLoginModel(model);
                    _userLoginRepository.Insert(loginModel);
                }

                else
                {
                    message = "User with same email already exists ! Please user another email";
                }

            }

            catch (Exception ex)
            {
                message = ex.Message;
            }

            finally
            {
                message = "User Created Successfully";
            }

            return message;
        }

        private Person CreatePersonModel(UserRegistrationEditModel model)
        {
            return new Person
            {
                FirstName = model.FirstName.Trim(),
                LastName = model.LastName.Trim(),
                Email = model.Email.ToLower().Trim(),
            };
        }

        private UserLogin CreateUserLoginModel(UserRegistrationEditModel model)
        {
            Tuple<string, string> passwordTuple = _encryptionService.CreatePasswordHash(model.Password);

            return new UserLogin
            {
                UserName = model.Email.ToLower().Trim(),
                Salt = passwordTuple.Item1,
                Password = passwordTuple.Item2
            };
        }


        public UserSessionModel AuthenticateUser(UserAuthenticationEditModel model)
        {
            var userSessionModel = new UserSessionModel();
            var userEmail = model.Email.Trim().ToLower();

            var userLogin = (from u in _userLoginRepository.List
                             join p in _personRepository.List on u.UserName equals p.Email
                             where u.UserName == userEmail
                             select new
                             {
                                 u.Id,
                                 u.UserName,
                                 p.FirstName,
                                 p.LastName
                             }).FirstOrDefault();

            if (userLogin != null)
            {
                string salt = GetSaltForUser(model.Email.ToLower());
                string hashedPassword = _encryptionService.GetUserHashedPassword(salt, model.Password);
                UserLogin verifiedPerson = _userLoginRepository.Get(x => x.UserName == model.Email.ToLower() && x.Password == hashedPassword);

                if (verifiedPerson != null)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();

                    var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]{
                            new Claim(ClaimTypes.Name, userLogin.UserName.ToString()),
                            new Claim(ClaimTypes.PrimarySid, userLogin.Id.ToString())
                        }),
                        Expires = DateTime.UtcNow.AddDays(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    userSessionModel.Token = tokenHandler.WriteToken(token);


                    userSessionModel = CreateSessionModel(userSessionModel,userLogin.UserName, userLogin.FirstName, userLogin.LastName);
                    
                    userSessionModel.Message = "Welcome " + userSessionModel.FullName;
                }
                else
                {
                    userSessionModel.IsAuthenticated = false;
                    userSessionModel.Message = "Invalid username and password combination. Please try again !";
                }

            }

            else
            {
                userSessionModel.IsAuthenticated = false;
                userSessionModel.Message = "User doesn't exists for the given username";
            }

            return userSessionModel;
        }

        private string GetSaltForUser(string userName)
        {
            UserLogin verifiedUserSalt = _userLoginRepository.Get(x => x.UserName.ToLower() == userName.ToLower());
            return verifiedUserSalt.Salt;
        }

        private UserSessionModel CreateSessionModel(UserSessionModel userSessionModel,string email, string firstName, string lastName)
        {

            userSessionModel.Email = email;
            userSessionModel.FirstName = firstName;
            userSessionModel.LastName = lastName;
            userSessionModel.FullName = firstName + " " + lastName;
            userSessionModel.UserId = userSessionModel.UserId;
           
            return userSessionModel;
            
        }
    }
}
