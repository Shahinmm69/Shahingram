using Data.Contract;
using Entities.Models;
using Shahingram.Models;
using WebFramework.Api;

namespace Shahingram.Controllers
{
        public class PostController : CrudController<PostDto, PostSelectDto, Post>
        {
            public PostController(IRepository<Post> repository)
                : base(repository)
            {
            }
    }
}
