using System.Threading.Tasks;
using OpenSchool.src.Models;

namespace OpenSchool.src.Services
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);
        Task<AuthModel> GetTokenAsync(TokenRequestModel model);
    }
}
