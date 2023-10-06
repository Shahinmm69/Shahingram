using Data.Contract;
using Data.Repositories;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Shahingram.Models;
using System;
using WebFramework.Api;

namespace MyApi.Controllers.v1
{
    public class CategoryController : CrudController<CategoryDto, CategoryDto, Category>
    {
        public CategoryController(IRepository<Category> repository)
            : base(repository)
        {
        }
    }
}
