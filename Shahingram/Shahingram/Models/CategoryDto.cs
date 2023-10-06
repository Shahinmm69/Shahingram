using Entities.Models;
using WebFramework.Api;

namespace Shahingram.Models
{
    public class CategoryDto : BaseDto<CategoryDto, Category>
    {
            public string Title { get; set; }
    }
}
