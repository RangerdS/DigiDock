using DigiDock.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiDock.Business.Token
{
    public interface ITokenService
    {
        Task<string> GetToken(User user);
    }
}
