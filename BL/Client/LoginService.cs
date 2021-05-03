using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Dto;
using DAL.Models;

namespace BL
{    

    public class LoginService
    {
        private AllDbContext dbContext;
        private readonly UserService userService;
        private readonly MapperService mapper;

        public LoginService(AllDbContext dbContext, UserService userService, MapperService mapper)
        {
            this.dbContext = dbContext;
            this.userService = userService;
            this.mapper = mapper;
        }

        public string ErrorMessage { get; private set; }
        

        public async Task<bool> Login(string login, string pass)
        {
            await dbContext.Profiles.Include(x => x.Client).LoadAsync();

            var user = await dbContext.
                Profiles.
                FirstOrDefaultAsync(x => string.Compare(x.Login, login) == 0 && string.Compare(x.Password, pass) == 0);

            if (user != null)
            {
                var dto = mapper.MapTo<Client, ClientDto>(user.Client);
                userService.SetupUser(dto);
                return true;
            }
            ErrorMessage = "Неправильные логин или пароль";

            return false;
        }

    }
}
