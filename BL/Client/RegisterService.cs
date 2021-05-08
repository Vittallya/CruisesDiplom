using DAL;
using DAL.Dto;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class RegisterService
    {
        private readonly AllDbContext dbContext;
        private readonly MapperService mapperService;
        private readonly UserService userService;
        public bool IsEdit { get; private set; }
        
        public string ErrorMessage { get; private set; }
        public bool IsRegisterRequiered 
        {
            get 
            {
                var temp = isRegisterRequiered;
                isRegisterRequiered = false; 
                return temp; 
            } 
            set => isRegisterRequiered = value; 
        }

        private Client _client;
        private Profile _profile;
        private bool isRegisterRequiered;
        public void Clear()
        {
            _client =null;
            _profile =null;
            isRegisterRequiered = false;
        }
        public RegisterService(AllDbContext dbContext, MapperService mapperService, UserService userService)
        {
            this.dbContext = dbContext;
            this.mapperService = mapperService;
            this.userService = userService;
        }
        public async Task StartEditClient(int clientId)
        {
            await dbContext.Clients.LoadAsync();
            IsEdit = true;
            _client = await dbContext.Clients.FindAsync(clientId);
        }
        public ClientDto GetClient()
        {
            return _client != null ? mapperService.MapTo<Client, ClientDto>(_client) : new ClientDto();
        }
        public void SetupClient(ClientDto dto)
        {
            _client = mapperService.MapTo<ClientDto, Client>(dto);

        }
        public async Task<bool> SetupProfile(ProfileDto profileDto)
        {
            await dbContext.Profiles.LoadAsync();


            if (string.IsNullOrEmpty(profileDto.Login) || string.IsNullOrEmpty(profileDto.Password))   
            {
                ErrorMessage = "Поля 'Логин' и 'Пароль' должны быть заполнены";
                return false;
            }

            if (await dbContext.Profiles.AnyAsync(x => x.Login == profileDto.Login))
            {
                ErrorMessage = "Такой логин уже есть";
                return false;
            }

            if (await dbContext.Profiles.AnyAsync(x => x.Password == profileDto.Password))
            {
                ErrorMessage = "Такой пароль уже есть";
                return false;
            }
            _profile = mapperService.MapTo<ProfileDto, Profile>(profileDto);

            return true;
        }
        public async Task<(bool, int)> RegisterAsync()
        {
            
            if (_profile != null)
            {
                _profile.Client = _client;
                _client.Profile = _profile;
                dbContext.Profiles.Add(_profile);
            }

            dbContext.Clients.Add(_client);

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return (false, 0);
            }

            if (_profile != null)
            {
                userService.SetupUser(mapperService.MapTo<Client, ClientDto>(_client));
            }

            return (true, _client.Id);
        }
    }
}
