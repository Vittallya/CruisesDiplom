﻿using DAL;
using DAL.Dto;
using DAL.Migrations;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
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
        public async Task ReloadAsync(int tourId, string defImgPath)
        {
            //Todo: вычесть из числа кают те, что занятые
            await allDbContext.Cabins.LoadAsync();
            await allDbContext.Placements.LoadAsync();

            _placements = await allDbContext.Placements.SqlQuery("SELECT * FROM [Placements] " +
                "WHERE [OrderId] IN (SELECT [Id] FROM [Orders] WHERE [TourId] = @tId)", 
                new SqlParameter("@tId", tourId)).AsNoTracking().
                ToListAsync();

            _cabins = await allDbContext.Cabins.AsNoTracking().ToListAsync();



            _placementsDto = _placements.Select(x => 
            {
                var dto = map.MapTo<Placement, PlacementDto>(x);
                return dto;
            }).ToList();

            _cabinsDto = _cabins.Select(x => 
            {
                var dto = map.MapTo<Cabin, CabinDto>(x);
                string name = $"c{(int)dto.CabinType + 1}.jpg";
                dto.ImagePath = Path.Combine(defImgPath, name);
                return dto;

            }).ToList();
        }
        public IEnumerable<PlacementDto> GetAllPlacements()
        {
            return _placementsDto;
        }
        public IEnumerable<CabinDto> GetCabins(int laynerId, int asults, int childs)
        {
            var busyCabins = _placementsDto.Select(x => x.CabinId);

            return _cabinsDto.
                Where(x => 
                //x.LaynerId == laynerId && 
                x.AdultCount == asults && 
                !busyCabins.Contains(x.Id) &&
                x.ChildCount == childs);
        }
    }
}
