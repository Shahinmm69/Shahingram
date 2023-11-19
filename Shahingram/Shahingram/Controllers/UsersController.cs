using Common.Exceptions;
using Common.Utilities;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyApi.Models;
using Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebFramework.Api;
using WebFramework.Filters;
using Common;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Data.Contract;
using Services.Contract;
using Entities.Models;
using Shahingram.Models;

namespace MyApi.Controllers.v1
{
    //[ApiVersion("1")]
    public class UsersController : BaseController
    {
        private readonly IUserRepository userRepository;
        private readonly ILogger<UsersController> logger;
        private readonly IJwtService jwtService;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly SignInManager<User> signInManager;
        private readonly IUserServices userServices;
        private readonly IDeletionRepository<User> deletionUserRepository;

        public UsersController(IUserRepository userRepository, ILogger<UsersController> logger, IJwtService jwtService,
            UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager, IUserServices userServices
            , IDeletionRepository<User> deletionUserRepository)
        {
            this.userRepository = userRepository;
            this.logger = logger;
            this.jwtService = jwtService;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.userServices = userServices;
            this.deletionUserRepository = deletionUserRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public virtual async Task<ActionResult<List<User>>> Get(CancellationToken cancellationToken)
        {
            //var userName = HttpContext.User.Identity.GetUserName();
            //userName = HttpContext.User.Identity.Name;
            //var userId = HttpContext.User.Identity.GetUserId();
            //var userIdInt = HttpContext.User.Identity.GetUserId<int>();
            //var phone = HttpContext.User.Identity.FindFirstValue(ClaimTypes.MobilePhone);
            //var role = HttpContext.User.Identity.FindFirstValue(ClaimTypes.Role);

            var users = await userRepository.TableNoTracking.ToListAsync(cancellationToken);
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public virtual async Task<ApiResult<UserSelectDto>> Get(int id, CancellationToken cancellationToken)
        {
            //_ = await userManager.FindByIdAsync(id.ToString());
            //_ = await roleManager.FindByNameAsync("Admin");

            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            if (user == null)
                return NotFound();

            await userManager.UpdateSecurityStampAsync(user);

            var userSelectDto = new UserSelectDto
            {
                Biography = user.Biography,
                BirthCountryTitle = await userServices.GetCountryTitleByIdAsync(user.BirthCountryId, cancellationToken),
                CategoryTitle = await userServices.GetCategoryTitleByIdAsync(user.CategoryId, cancellationToken),
                CrationDate = user.CrationDate.ToString(),
                DeletionDate = user.DeletionDate.ToString(),
                FirstName = user.FirstName,
                FollowersCount = await userServices.GetFollowersCountAsync(id, cancellationToken),
                FollowingsCount = await userServices.GetFollowersCountAsync(id, cancellationToken),
                IsDeleted = user.IsDeleted.ToString(),
                LastName = user.LastName,
                LifeCountryTitle = await userServices.GetCountryTitleByIdAsync(user.LifeCountryId, cancellationToken),
                MiddleName = user.MiddleName,
                ModificationDate = user.ModificationDate.ToString(),
                PostsCount = await userServices.GetPostsCountAsync(id, cancellationToken),
                UserName = user.UserName,
                PhotoAddress = await userServices.GetPhotoAsync(id, cancellationToken),
                PostsId = await userServices.GetPostsIdAsync(id, cancellationToken),
                UserCraetionName = await userServices.GetUserNameByIdAsync(user.UserCraetionId,cancellationToken),
                UserModificationName = await userServices.GetUserNameByIdAsync(user.UserModificationId, cancellationToken),
                UserDeletionName = await userServices.GetUserNameByIdAsync(user.UserDeletionId, cancellationToken)
            };
            return userSelectDto;
        }

        /// <summary>
        /// This method generate JWT Token
        /// </summary>
        /// <param name="tokenRequest">The information of token request</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [AllowAnonymous]
        public virtual async Task<ActionResult> Token(TokenRequest tokenRequest, CancellationToken cancellationToken)
        {
            if (!tokenRequest.grant_type.Equals("password", StringComparison.OrdinalIgnoreCase))
                throw new Exception("OAuth flow is not password.");

            //var user = await userRepository.GetByUserAndPass(username, password, cancellationToken);
            var user = await userManager.FindByNameAsync(tokenRequest.username);
            if (user == null)
                throw new BadRequestException("نام کاربری یا رمز عبور اشتباه است");

            var isPasswordValid = await userManager.CheckPasswordAsync(user, tokenRequest.password);
            if (!isPasswordValid)
                throw new BadRequestException("نام کاربری یا رمز عبور اشتباه است");

            var jwt = await jwtService.GenerateAsync(user);
            return new JsonResult(jwt);
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual async Task<ApiResult<User>> Create(UserDto userDto, CancellationToken cancellationToken)
        {
            logger.LogError("متد Create فراخوانی شد");
            //HttpContext.RiseError(new Exception("متد Create فراخوانی شد"));

            var user = new User
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                MiddleName = userDto.MiddleName,
                Biography = userDto.Biography,
                BirthCountryId = userDto.BirthCountryId,
                LifeCountryId = userDto.LifeCountryId,
                CategoryId = userDto.CategoryId,
                PhoneNumber = userDto.Mobile,
                UserCraetionId = signInManager.Context.Request.HttpContext.User.Identity.GetUserId().ToInt()
        };
            var newUser = await userServices.CraetionConfigAsync(user, cancellationToken);
            var result = await userManager.CreateAsync(newUser, userDto.Password);
            var result1 = await roleManager.CreateAsync(new Role
            {
                Name = "Public",
                Description = "public role"
            });
            var result2 = await userManager.AddToRoleAsync(newUser, "Public");

            return newUser;
        }

        [HttpPut]
        public virtual async Task<ApiResult> Update(UserDto userDto, CancellationToken cancellationToken)
        {
            int id = signInManager.Context.Request.HttpContext.User.Identity.GetUserId().ToInt();
            var updateUser = await userRepository.GetByIdAsync(cancellationToken, id);

            updateUser.UserName = userDto.UserName;
            updateUser.PhoneNumber = userDto.Mobile;
            updateUser.FirstName = userDto.FirstName;
            updateUser.LastName = userDto.LastName;
            updateUser.MiddleName = userDto.MiddleName;
            updateUser.Biography = userDto.Biography;
            updateUser.BirthCountryId = userDto.BirthCountryId;
            updateUser.LifeCountryId = userDto.LifeCountryId;
            updateUser.Email = userDto.Email;
            updateUser.CategoryId = userDto.CategoryId;
            updateUser.UserModificationId = id;
            await userRepository.UpdatModificationDateAsync(updateUser, cancellationToken);

            return Ok();
        }

        [HttpDelete]
        public virtual async Task<ApiResult> Delete(CancellationToken cancellationToken)
        {
            int id = signInManager.Context.Request.HttpContext.User.Identity.GetUserId().ToInt();
            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            user.UserDeletionId = id;
            await deletionUserRepository.DeletionDateAsync(user, cancellationToken);

            return Ok();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public virtual async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            user.UserDeletionId = signInManager.Context.Request.HttpContext.User.Identity.GetUserId().ToInt();
            await deletionUserRepository.DeletionDateAsync(user, cancellationToken);

            return Ok();
        }

        /// <summary>
        /// This method Add user's photo
        /// </summary>
        /// <param name="file"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        [HttpPost("[action]")]
        public virtual async Task<ApiResult> Photo(IFormFile file, CancellationToken cancellationToken)
        {
            int id = signInManager.Context.Request.HttpContext.User.Identity.GetUserId().ToInt();
            if (file.ContentType.ToString() is "image/jpg" or "image/png" or "image/jpeg" or "image/bnp")
                {
                    var filePath = Path.GetTempFileName();

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await file.CopyToAsync(stream);
                    }
                    await userServices.NewPhotoHandlerAsync(filePath, id, cancellationToken);
                }
                else
                {
                    throw new BadRequestException("فرمت فایل ارسالی اشتباه است");
                }

            return Ok();
        }

        /// <summary>
        /// This method delete user's photo
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("[action]")]
        public virtual async Task<ApiResult> Photo(CancellationToken cancellationToken)
        {
            int id = signInManager.Context.Request.HttpContext.User.Identity.GetUserId().ToInt();
            await userServices.DeletePhotoHandlerAsync(id, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// This method delete every user's photo by unique id (only Admin)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("[action]/{id:int}")]
        [Authorize(Roles = "Admin")]
        public virtual async Task<ApiResult> Photo(int id, CancellationToken cancellationToken)
        {
            await userServices.DeletePhotoHandlerAsync(id, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// This method retrieves list of users that have direct with current user
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public virtual async Task<List<User>?> GetUsersHaveDirects(CancellationToken cancellationToken)
        {
            int id = signInManager.Context.Request.HttpContext.User.Identity.GetUserId().ToInt();
            var users = await userServices.GetUsersHaveDirectsAsync(id, cancellationToken);
            return users;
        }

        /// <summary>
        /// This method retrieves list of users that have direct with every user by unique id (only Admin)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("[action]/{id:int}")]
        [Authorize(Roles = "Admin")]
        public virtual async Task<List<User>> UsersHaveDirects(int id, CancellationToken cancellationToken)
        {
            var users = await userServices.GetUsersHaveDirectsAsync(id, cancellationToken);
            return users;
        }

        /// <summary>
        /// This method retrieves all of directs with other user by unique id for current user
        /// </summary>
        /// <param name="anoutherid"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public virtual async Task<List<int>> DirectsIdWithAnother(int anoutherid, CancellationToken cancellationToken)
        {
            int id = signInManager.Context.Request.HttpContext.User.Identity.GetUserId().ToInt();
            var ids = await userServices.GetDirectsWithAnotherAsync(id, anoutherid, cancellationToken);
            return ids;
        }

        /// <summary>
        /// This method retrieves all of directs with other user by unique id for all of users (only Admin)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="anoutherid"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("[action]/{id:int}")]
        [Authorize(Roles = "Admin")]
        public virtual async Task<List<int>> DirectsIdWithAnother(int id, int anoutherid, CancellationToken cancellationToken)
        {
            var ids = await userServices.GetDirectsWithAnotherAsync(id, anoutherid, cancellationToken);
            return ids;
        }

        /// <summary>
        /// This method retrieves user with username
        /// </summary>
        /// <param name="username"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public virtual async Task<ApiResult<User>> SearchWithUsername(string username, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByNameAsync(username);
            return user;
        }

        /// <summary>
        /// This method create new admin (only Admin)
        /// </summary>
        /// <param name="userDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public virtual async Task<ApiResult<User>> NewAdmin(UserDto userDto, CancellationToken cancellationToken)
        {
            var user = new User
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                MiddleName = userDto.MiddleName,
                Biography = userDto.Biography,
                BirthCountryId = userDto.BirthCountryId,
                LifeCountryId = userDto.LifeCountryId,
                CategoryId = userDto.CategoryId,
                PhoneNumber = userDto.Mobile,
                UserCraetionId = signInManager.Context.Request.HttpContext.User.Identity.GetUserId().ToInt()
            };
            var newUser = await userServices.CraetionConfigAsync(user, cancellationToken);
            _ = await userManager.CreateAsync(newUser, userDto.Password);
            _ = await roleManager.CreateAsync(new Role
            {
                Name = "Admin",
                Description = "admin role"
            });
            _ = await userManager.AddToRoleAsync(newUser, "Admin");

            return user;
        }
    }
}
