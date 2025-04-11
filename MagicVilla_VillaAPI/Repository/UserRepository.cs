using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVilla_VillaAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private string secretKey;

        public UserRepository(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public bool IsUniqueUser(string username)
        {
            var user = _db.LocalUsers.FirstOrDefault(x => x.UserName == username);
            if (user == null)
                return true; //No encontró ese usario, por lo tanto es unico

            return false; //Significa que ya existe uno
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDto loginRequest)
        {
            var user = _db.LocalUsers.FirstOrDefault(
                u => u.UserName.ToLower() == loginRequest.UserName.ToLower() && 
                u.Password == loginRequest.Password);

            if (user == null) 
                return null;

            //if user was found, generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler(); //Si se encontró un usuario, se crea una instancia de JwtSecurityTokenHandler, que es una clase que se utiliza para crear y validar JWTs.

            var key = Encoding.ASCII.GetBytes(secretKey); //Aquí se convierte una clave secreta (secretKey) en un arreglo de bytes utilizando la codificación ASCII. Esta clave se utilizará para firmar el token JWT, asegurando que no pueda ser alterado.

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.Id.ToString()),
                    new Claim(ClaimTypes.Role,user.Role),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token = tokenHandler.WriteToken(token),
                User = user
            };
            return loginResponseDTO;

        }

        public async Task<LocalUser> Register(RegisterationRequestDTO registerationRequest)
        {
            LocalUser user = new LocalUser()
            {
                UserName = registerationRequest.UserName,
                Password = registerationRequest.Password,
                Name = registerationRequest.Name,
                Role = registerationRequest.Role
            };

            _db.LocalUsers.Add(user);
            await _db.SaveChangesAsync();
            user.Password = "";
            return user;
        }
    }
}
