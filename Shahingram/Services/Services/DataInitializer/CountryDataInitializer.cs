using Data.Contract;
using Entities.Models;
using Services.DataInitializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DataInitializer
{
    public class CountryDataInitializer : IDataInitializer
    {
        private readonly IRepository<Country> repository;

        public CountryDataInitializer(IRepository<Country> repository)
        {
            this.repository = repository;
        }

        public void InitializeData()
        {
            if (!repository.TableNoTracking.Any(p => p.Title == "ایران"))
            {
                repository.Add(new Country
                {
                    Title = "ایران"
                });
            }
            if (!repository.TableNoTracking.Any(p => p.Title == "انگلیس"))
            {
                repository.Add(new Country
                {
                    Title = "انگلیس"
                });
            }
            if (!repository.TableNoTracking.Any(p => p.Title == "آمریکا"))
            {
                repository.Add(new Country
                {
                    Title = "آمریکا"
                });
            }
        }
    }
}
