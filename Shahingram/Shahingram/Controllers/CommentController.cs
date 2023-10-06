using AutoMapper.QueryableExtensions;
using Data.Contract;
using Data.Repositories;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shahingram.Models;
using WebFramework.Api;

namespace Shahingram.Controllers
{
    public class CommentController : CrudController<CommentDto, CommentSelectDto, Comment>
    {
        private readonly IRepository<Comment> CommentRepository;

        public CommentController(IRepository<Comment> repository)
            : base(repository)
        {
            CommentRepository = repository;
        }
        public override Task<ActionResult<List<CommentSelectDto>>> Get(CancellationToken cancellationToken)
        {
            return base.Get(cancellationToken);
        }
        public override Task<ApiResult<CommentSelectDto>> Get(int id, CancellationToken cancellationToken)
        {
            return base.Get(id, cancellationToken);
        }
        public override Task<ApiResult<CommentSelectDto>> Create(CommentDto dto, CancellationToken cancellationToken)
        {
            return base.Create(dto, cancellationToken);
        }
        public override Task<ApiResult<CommentSelectDto>> Update(int id, CommentDto dto, CancellationToken cancellationToken)
        {
            return base.Update(id, dto, cancellationToken);
        }
        public override Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            return base.Delete(id, cancellationToken);
        }

        [HttpGet("{id:guid}")]
        public virtual async Task<ApiResult<CommentSelectDto>> GetReplies(int id, CancellationToken cancellationToken)
        {
            return default;
        }
    }
}
