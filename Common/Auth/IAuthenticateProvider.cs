using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Auth
{
    public interface IAuthenticateProvider
    {
        bool Authenticate(string userName, string password);
    }
}
