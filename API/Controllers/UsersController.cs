using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extenstions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(LogUserActivity))]

    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        public IMapper _Mapper { get; }
        private readonly IPhotoService _photoService;
        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            _photoService = photoService;
            _Mapper = mapper;
            _userRepository = userRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUser([FromQuery] UserParms userParms)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUserName());
            userParms.CurrentUserName = user.UserName;
            if (string.IsNullOrEmpty(userParms.Gender))
                userParms.Gender = user.Gender == "male" ? "female" : "male";

            var users = await _userRepository.GetMembersAsync(userParms);
            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok(users);
        }
        [HttpGet("Photos")]
        public IActionResult GetPhotos()
        {
            var photos = _userRepository.GetPhotosAsync();
            return Ok(photos);
        }
        [HttpGet("{username}")]
        public async Task<ActionResult<AppUser>> GetUser(string username)
        {
            var user = await _userRepository.GetMemberAsync(username);
            return Ok(_Mapper.Map<MemberDto>(user));
        }

        [HttpPut]

        public async Task<ActionResult> UpdateUser(memberUpdateDto memberUpdateDto)
        {

            // var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUserName());

            _Mapper.Map(memberUpdateDto, user);

            _userRepository.update(user);
            if (await _userRepository.SaveAllAsync()) return Ok("User Updated Succefully");

            return BadRequest("Failed to Update");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<photosDto>> AddPhoto(IFormFile file)
        {

            var user = await _userRepository.GetUserByUsernameAsync(User.GetUserName());
            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);
            var photo = new photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };
            if (user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }
            user.Photos.Add(photo);

            if (await _userRepository.SaveAllAsync())
                return _Mapper.Map<photosDto>(photo);
            return BadRequest("Problem Adding Photo");
        }

        [HttpPut("add-main-photo/{photoId}")]

        public async Task<ActionResult> SetMainPhoto(int photoId)
        {

            var user = await _userRepository.GetUserByUsernameAsync(User.GetUserName());
            var photo = user.Photos.FirstOrDefault(x => x.ID == photoId);
            if (photo.IsMain) return BadRequest("Photo already is Already your Main Photo");
            if(photo.IsAproved == false) return BadRequest("Photo is not aproved"); 

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;
            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to set Main Photo");
        }

        [HttpDelete("delete-photo/{photoId}")]

        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUserName());
            var delPhoto = user.Photos.FirstOrDefault(x => x.ID == photoId);
            if (delPhoto == null) return NotFound();
            if (delPhoto.IsMain) return BadRequest("You can not delete your main photo");

            if (delPhoto.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(delPhoto.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);

            }
            user.Photos.Remove(delPhoto);
            if (await _userRepository.SaveAllAsync()) return Ok();
            return BadRequest("failed to delete photo");
        }
        [HttpPut("photo-aprove/{id}")]

        public async Task<ActionResult> updatePhoto(int id)
        {
            var photo = await _userRepository.GetphotoAsync(id);
            // var appPhoto =  photos.FirstOrDefault(x=>x.ID == id);
            if (photo == null) return NotFound();
            photo.IsAproved = true;
            _userRepository.UpdatePhotosAsync(photo);
            if (await _userRepository.SaveAllAsync()) return Ok("Photo aproved succefully");


            return BadRequest("Failed to Update");

        }
    }
}