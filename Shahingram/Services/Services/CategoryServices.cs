using Common;
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
    public class CategoryServices : ICategoryServices, IScopedDependency
    {
        protected readonly ICreationRepository<Category> creationcategoryrepository;
        protected readonly IRepository<Category> categoryrepository;
        public CategoryServices(ICreationRepository<Category> creationcategoryrepository, IRepository<Category> categoryrepository)
        {
            this.creationcategoryrepository = creationcategoryrepository;
            this.categoryrepository = categoryrepository;
        }
        public Task CraetionConfigAsync(Category entity, CancellationToken cancellationToken)
        {
            var category = categoryrepository.TableNoTracking.Where(x => x.Title == entity.Title).SingleAsync();
            if (category == null)
            {
                return creationcategoryrepository.CraetionDateAsync(entity, cancellationToken);
            }
            else
                throw new BadRequestException("عنوان تکراری است");
        }
    }
}
