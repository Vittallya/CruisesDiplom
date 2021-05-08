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
    public class InsurancesService
    {
        private readonly AllDbContext dbContext;
        private readonly MapperService mapper;

        public InsurancesService(AllDbContext dbContext, MapperService mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        IEnumerable<Insurance> _insurances;
        IEnumerable<InsuranceDto> _insurancesDto;

        public async Task ReloadAsync(ValuteGetterService valuteGetter)
        {
            await dbContext.Insurances.LoadAsync();
            _insurances = await dbContext.Insurances.AsNoTracking().ToListAsync();
            _insurancesDto = _insurances.Select(x => 
            {
                var dto = mapper.MapTo<Insurance, InsuranceDto>(x);
                dto.CostUSD = valuteGetter.GetUSDValue(x.Cost);
                dto.CostEUR = valuteGetter.GetEuroValue(x.Cost);
                return dto;
            }).ToList();
        }

        private void ReloadDtoCollection()
        {

        }


        public IEnumerable<InsuranceDto> GetInsurances()
        {
            return _insurancesDto;
        }

    }
}
