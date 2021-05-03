using DAL;
using DAL.Dto;
using DAL.Migrations;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class PlacementService
    {
        private readonly AllDbContext allDbContext;
        private readonly MapperService map;
        private IEnumerable<Placement> _placements;
        private IEnumerable<Cabin> _cabins;

        private IEnumerable<PlacementDto> _placementsDto;
        private IEnumerable<CabinDto> _cabinsDto;

        public PlacementService(AllDbContext allDbContext, MapperService mapperService)
        {
            this.allDbContext = allDbContext;
            this.map = mapperService;
        }

        public async Task ReloadAsync()
        {
            await allDbContext.Cabins.LoadAsync();
            await allDbContext.Placements.LoadAsync();

            _placements = await allDbContext.Placements.AsNoTracking().ToListAsync();
            _cabins = await allDbContext.Cabins.AsNoTracking().ToListAsync();

            _placementsDto = _placements.Select(x => map.MapTo<Placement, PlacementDto>(x)).ToList();
            _cabinsDto = _cabins.Select(x => map.MapTo<Cabin, CabinDto>(x)).ToList();
        }

        public IEnumerable<PlacementDto> GetAllPlacements()
        {
            return _placementsDto;
        }
        public IEnumerable<CabinDto> GetCabins(int laynerId, int asults, int childs)
        {
            return _cabinsDto.
                Where(x => x.LaynerId == laynerId && 
                x.AdultCount >= asults && 
                x.ChildCount >= childs);
        }
    }
}
