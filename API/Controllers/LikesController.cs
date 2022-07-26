
using API.DTOs;
using API.Entities;
using API.Extenstions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(LogUserActivity))]

    public class LikesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikesRepository _likesRepository;

        public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)
        {
            _likesRepository = likesRepository;
            _userRepository = userRepository;
        }

        [HttpPost("{username}")]

        public async Task<ActionResult> AddLike(string username)
        {
            var SourceUserId = User.GetUserId();
            var LikedUser = await _userRepository.GetUserByUsernameAsync(username);
            var SourceUser = await _likesRepository.GetUserWithLikes(SourceUserId);
            if (LikedUser == null) return NotFound();
            if (SourceUser.UserName == username) return BadRequest("you can not like your self");

            var userLike = await _likesRepository.GetUserLike(SourceUserId, LikedUser.Id);
            if (userLike != null) return BadRequest("you already Like this user");

            userLike = new UserLike
            {
                SourceUserId = SourceUserId,
                LikedUserId = LikedUser.Id
            };

            SourceUser.LikedUsers.Add(userLike);
            if (await _userRepository.SaveAllAsync()) return Ok("You like this Profile");

            return BadRequest("failed to Like User");

        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery]LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();
            var users = await _likesRepository.GetUserLikes(likesParams);
            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok(users);
        }
    }
}