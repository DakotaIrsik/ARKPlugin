using Refit;
using ArkFury.Common.Models;
using System.Threading.Tasks;

namespace ArkFury.Common
{
    public interface IIdentityServerAPI
    {

        [Post("/api/Account")]
        Task<RegistrationResponse> Register();
    }
}
