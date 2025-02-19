using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsLibrary.Models;

namespace GeolocationAds.Factories
{
    public class UserFactory : IUserFactory
    {
        public User CreateUser(string email, string fullName, string phone, ToolsLibrary.Models.Login login)
        {
            return new User
            {
                CreateBy = 0,
                CreateDate = DateTime.Now,
                Email = email,
                FullName = fullName,
                Phone = phone,
                Login = login
            };
        }
    }
}
