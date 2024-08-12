using DigiDock.Data.Domain;

namespace DigiDock.Business.Token
{
    public interface ITokenService
    {
        Task<string> GetToken(User user);
    }
}
