using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsLibrary.Models;

namespace GeolocationAds.Factories
{
    public interface IUserFactory
    {
        User CreateUser(string email, string fullName, string phone, ToolsLibrary.Models.Login login);
    }
}
