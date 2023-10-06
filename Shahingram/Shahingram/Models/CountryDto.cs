using Entities.Models;
using WebFramework.Api;

namespace Shahingram.Models
{
    public class CountryDto : BaseDto<CountryDto, Country>
    {
            public string Title { get; set; }
    }
}
