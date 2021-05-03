using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Models;
using DAL.Dto;
using System.Data.Entity;
using AutoMapper;

namespace BL
{
    public class ToursService
    {
        private readonly AllDbContext dbContext;
        private readonly MapperService mapper;
        private IEnumerable<Tour> allTours;
        private IEnumerable<TourDto> allToursDto;


        public ToursService(AllDbContext dbContext, MapperService mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task ReloadAsync(Func<string, string> pathGetter)
        {
            await dbContext.Tours.LoadAsync();

            allTours = await dbContext.Tours.Include(x => x.Layner).AsNoTracking().ToListAsync();

            allToursDto = allTours.Select(x => 
            {
                var t = mapper.MapTo<Tour, TourDto>(x);
                t.ImagePath = pathGetter?.Invoke(t.ImageName);
                t.LaynerDto = mapper.MapTo<Layner, LaynerDto>(x.Layner);
                t.LaynerDto.ImagePath = pathGetter(t.LaynerDto.ImageName);

                return t;
            }).ToList();
        }

        public TourDto GetTour(int id)
        {
            return allToursDto.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<TourDto> GetAllTours(string name = null)
        {            

            if(name != null)
            {
                return allToursDto.Where(x => x.Name.Contains(name));
            }

            return allToursDto;
        }

    }
}
