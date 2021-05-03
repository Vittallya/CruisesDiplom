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
    public class UserService
    {
        private readonly AllDbContext dbContext;
        private readonly MapperService mapper;

        public UserService(AllDbContext dbContext, MapperService mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public ClientDto CurrentUser { get; private set; }

        public bool IsAutorized { get; private set; }

        public event Action<ClientDto> Autorized;
        public event Action Exited;

        public void Logout()
        {
            CurrentUser = null;
            IsAutorized = false;
            Exited?.Invoke();
        }

        public void SetupUser(ClientDto user)
        {
            CurrentUser = user;
            IsAutorized = true;
            Autorized?.Invoke(user);
        }
        
        public async Task<ProfileDto> GetProfile()
        {
            if (!IsAutorized)
                return null;

            await dbContext.Profiles.LoadAsync();

            var profile = await dbContext.Profiles.FindAsync(CurrentUser.Id);

            if(profile != null)
            {
                return mapper.MapTo<Profile, ProfileDto>(profile);

            }

            return null;
        }
    }
}
