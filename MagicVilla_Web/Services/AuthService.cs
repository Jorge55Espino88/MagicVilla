using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string villaURL;

        public AuthService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            villaURL = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }
        public Task<T> LoginAsync<T>(LoginRequestDto obj)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = SD.APIType.POST,
                Data = obj,
                URL = villaURL + "/api/UsersAuth/login"
            });
        }

        public Task<T> RegisterAsync<T>(UserDTO obj)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = SD.APIType.POST,
                Data = obj,
                URL = villaURL + "/api/UsersAuth/register"
            });
        }
    }
}
