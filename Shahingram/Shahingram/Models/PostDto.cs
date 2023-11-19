using AutoMapper;
using Common;
using Data.Contract;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Services.Contract;
using System.Threading;
using WebFramework.Api;

namespace Shahingram.Models
{
    public class PostDto : BaseDto<PostDto, Post>
    {
        public string Text { get; set; }
    }

    public class PostSelectDto : BaseSelectDto<PostSelectDto, Post>
    {
        public string Text { get; set; }
        public int UserId{ get; set; }
        public string CrationDate { get; set; }
        public string ModificationDate { get; set; }
        public string IsDeleted { get; set; }
        public string DeletionDate { get; set; }
        public List<string> PhotoAddress { get; set; }
        public List<string> VideoAddress { get; set; }
        public string UserCraetionName { get; set; }
        public string UserModificationName { get; set; }
        public string UserDeletionName { get; set; }
    }
}
