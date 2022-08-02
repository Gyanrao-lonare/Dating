
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

    public class FriendRequestController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly DataContext _context;
        public FriendRequestController(IUserRepository userRepository, IFriendRequestRepository friendRequestRepository, DataContext context)
        {
            _context = context;
            _friendRequestRepository = friendRequestRepository;
            _userRepository = userRepository;
        }

        [HttpPost("{username}")]

        public async Task<ActionResult> SendRequest(string username)
        {

            // List<FriendRequest> allRequests   = new List<FriendRequest>();
            var RequesterId = User.GetUserId();
            var LikedUser = await _userRepository.GetUserByUsernameAsync(username);
            var SourceUser = await _friendRequestRepository.GetUserWithFriendRequest(RequesterId);
            if (LikedUser == null) return NotFound();
            if (SourceUser.UserName == username) return BadRequest("you can not Request Your Self");
            bool contactExists = _context.Freinds.Any(freind => (freind.RequesterId.Equals(RequesterId) && freind.Receiverid.Equals(LikedUser.Id))||(freind.RequesterId.Equals(LikedUser.Id) && freind.Receiverid.Equals(RequesterId)));
    
            if (contactExists) return BadRequest("Already Requested");
            var request = new FriendRequest
            {
                Receiverid = LikedUser.Id,
                RequesterId = RequesterId,
                Status = false,
                DateRequested = DateTime.Now
            };
            _friendRequestRepository.SendRequest(request);
            // SourceUser.RecevedRequests.Add(request);
            if (await _friendRequestRepository.SaveAllAsync()) return Ok("Your Request sent succefully");

            return BadRequest("failed to Like User");

        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<FriendRequestDto>>> GetUserFriendRequest([FromQuery] RequestParms requestParms)
        {
            requestParms.UserId = User.GetUserId();
            var users = await _friendRequestRepository.GetUserFriendRequest(requestParms);
            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok(users);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFrendRequest(int id)
        {
            var Receiverid = User.GetUserId();
            var contactExists = _context.Freinds.FirstOrDefault(freind => (freind.RequesterId.Equals(id) && freind.Receiverid.Equals(Receiverid)) || (freind.RequesterId.Equals(Receiverid) && freind.Receiverid.Equals(id)));
            var friendRequest = await _friendRequestRepository.GetUserFriendRequest(id);
            if (contactExists == null) return NotFound();

            _friendRequestRepository.DeleteRequest(contactExists);

            if (await _friendRequestRepository.SaveAllAsync()) return Ok("Request Removed");
            return BadRequest("Request not deleted");

        }
        [HttpPut("confirm/{id}")]

        public async Task<ActionResult> updateStatus(int id)
        {
            var Receiverid = User.GetUserId();
            var friendRequest = await _friendRequestRepository.GetUserFriendRequest(id);
            var contactExists =  _context.Freinds.FirstOrDefault(freind => freind.RequesterId.Equals(id) && freind.Receiverid.Equals(Receiverid));
            if (contactExists == null) return NotFound();

            contactExists.Status = true;
            if (contactExists.Status)
            {
                _friendRequestRepository.Update(contactExists);
            }
            if (await _friendRequestRepository.SaveAllAsync()) return Ok("You are now freinds");

            return BadRequest("Failed to confirm");

        }

        

    }
}