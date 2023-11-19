using AutoMapper.QueryableExtensions;
using Common.Exceptions;
using Data.Contract;
using Entities.Common;
using Entities.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebFramework.Filters;

namespace WebFramework.Api
{
    //[ApiVersion("1")]
    [AllowAnonymous]
    public class CrudControllerType2<TDto, TSelectDto, TEntity> : BaseController
        where TDto : BaseDto<TDto, TEntity>, new()
        where TSelectDto : BaseSelectDto<TSelectDto, TEntity>, new()
        where TEntity : Modification, IDeletion, new()
    {
        private readonly IRepository<TEntity> _repository;
        private readonly IDeletionRepository<TEntity> deletionRepository;
        private readonly IModificationRepository<TEntity> modificationRepository;
        private readonly SignInManager<User> signInManager;

        public CrudControllerType2(IRepository<TEntity> repository, IDeletionRepository<TEntity> deletionRepository
            , IModificationRepository<TEntity> modificationRepository, SignInManager<User> signInManager)
        {
            _repository = repository;
            this.deletionRepository = deletionRepository;
            this.modificationRepository = modificationRepository;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public virtual async Task<ActionResult<List<TSelectDto>>> Get(CancellationToken cancellationToken)
        {
            var list = await _repository.TableNoTracking.ProjectTo<TSelectDto>()
                .ToListAsync(cancellationToken);

            return Ok(list);
        }

        [HttpGet("{id:guid}")]
        public virtual async Task<ApiResult<TSelectDto>> Get(int id, CancellationToken cancellationToken)
        {
            var dto = await _repository.TableNoTracking.ProjectTo<TSelectDto>()
                .SingleOrDefaultAsync(p => p.Id.Equals(id), cancellationToken);

            if (dto == null)
                return NotFound();

            return dto;
        }

        [HttpPost]
        public virtual async Task<ApiResult<TSelectDto>> Create(TDto dto, CancellationToken cancellationToken)
        {
            var model = dto.ToEntity();

            await modificationRepository.CraetionDateAsync(model, cancellationToken);

            var resultDto = await _repository.TableNoTracking.ProjectTo<TSelectDto>().SingleOrDefaultAsync(p => p.Id.Equals(model.Id), cancellationToken);

            return resultDto;
        }

        [HttpPut]
        public virtual async Task<ApiResult<TSelectDto>> Update(int id, TDto dto, CancellationToken cancellationToken)
        {
            var userCreationId = Convert.ToInt32(signInManager.Context.Request.HttpContext.User.Identity.GetUserId());
            var model = await _repository.GetByIdAsync(cancellationToken, id);

            if (model.UserCreationId == userCreationId)
            {
                model = dto.ToEntity(model);

                await modificationRepository.UpdatModificationDateAsync(model, cancellationToken);

                var resultDto = await _repository.TableNoTracking.ProjectTo<TSelectDto>().SingleOrDefaultAsync(p => p.Id.Equals(model.Id), cancellationToken);

                return resultDto;
            }
            throw new BadRequestException("عدم دسترسی");
        }

        [HttpDelete("{id:guid}")]
        public virtual async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            var userCreationId = Convert.ToInt32(signInManager.Context.Request.HttpContext.User.Identity.GetUserId());
            var model = await _repository.GetByIdAsync(cancellationToken, id);

            if (model.UserCreationId == userCreationId)
            {
                await deletionRepository.DeletionDateAsync(model, cancellationToken);

                return Ok();
            }
            throw new BadRequestException("عدم دسترسی");
        }
    }

    //public class CrudController<TDto, TSelectDto, TEntity> : CrudController<TDto, TSelectDto, TEntity, int>
    //    where TDto : BaseDto<TDto, TEntity>, new()
    //    where TSelectDto : BaseSelectDto<TSelectDto, TEntity>, new()
    //    where TEntity : Creation, IDeletion, new()
    //{
    //    public CrudController(IRepository<TEntity> repository)
    //        : base(repository)
    //    {
    //    }
    //}


    //public class CrudController<TDto, TEntity> : CrudController<TDto, TDto, TEntity, int>
    //    where TDto : BaseSelectDto<TDto, TEntity, int>, new()
    //    where TEntity : BaseEntity<int>, new()
    //{
    //    public CrudController(IRepository<TEntity> repository)
    //        : base(repository)
    //    {
    //    }
    //}
}
