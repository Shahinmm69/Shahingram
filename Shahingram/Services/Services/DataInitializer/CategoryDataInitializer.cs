using Data.Contract;
using Data.Repositories;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.DataInitializer
{
    public class CategoryDataInitializer : IDataInitializer
    {
        private readonly IRepository<Category> repository;

        public CategoryDataInitializer(IRepository<Category> repository)
        {
            this.repository = repository;
        }

        public void InitializeData()
        {
            if (!repository.TableNoTracking.Any(p => p.Title == "مهندسی"))
            {
                repository.Add(new Category
                {
                    Title = "مهندسی"
                });
            }
            if (!repository.TableNoTracking.Any(p => p.Title == "پزشکی"))
            {
                repository.Add(new Category
                {
                    Title = "پزشکی"
                });
            }
            if (!repository.TableNoTracking.Any(p => p.Title == "آشپزی"))
            {
                repository.Add(new Category
                {
                    Title = "آشپزی"
                });
            }
        }
    }
}
