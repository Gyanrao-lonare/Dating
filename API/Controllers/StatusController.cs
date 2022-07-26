using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class StatusController : BaseApiController
    {
        private readonly IStatusRepository _statusRepository;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public StatusController(DataContext context, IStatusRepository statusRepository, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            _statusRepository = statusRepository;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _statusRepository.GetStatus();
            return Ok(users);
        }

        [HttpPost("add")]
        public async Task<ActionResult> UpdateUser(StatusDto statusDto)
        {
            _statusRepository.AddStatus(statusDto);
            if (await _context.SaveChangesAsync() > 0) return Ok("Status Added Succefully");

            return BadRequest("Failed to Add");
        }

        [HttpDelete("delete/{id}")]

        public async Task<ActionResult> DeleteStatus(int id)
        {

            _statusRepository.Delete(id);
            if (await _context.SaveChangesAsync() > 0) return Ok("Status Delete Succefully");

            return BadRequest("Failed to Add");

        }
         [HttpPut("{id}")]

        public async Task<ActionResult> updateStatus(int id ,UserStatus userStatus)
        {        
            _statusRepository.Update(userStatus);
            if (await _context.SaveChangesAsync() > 0) return Ok("Status Updated Succefully");

            return BadRequest("Failed to Update");

        }
    }
}