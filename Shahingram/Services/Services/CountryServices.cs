using Common.Exceptions;
using Data.Contract;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class CountryServices : ICountryServices
    {
        protected readonly ICreationRepository<Country> creationcountryrepository;
        protected readonly IRepository<Country> countryrepository;
        public CountryServices(ICreationRepository<Country> creationcountryrepository, IRepository<Country> countryrepository)
        {
            this.creationcountryrepository = creationcountryrepository;
            this.countryrepository = countryrepository;
        }
        public Task CraetionConfigAsync(Country entity, CancellationToken cancellationToken)
        {
            var country = countryrepository.TableNoTracking.Where(x => x.Title == entity.Title).SingleAsync();
            if (country == null)
            {
                return creationcountryrepository.CraetionDateAsync(entity, cancellationToken);
            }
            else
                throw new BadRequestException("عنوان تکراری است");
        }
    }
}
