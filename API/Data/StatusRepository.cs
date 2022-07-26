using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class StatusRepository : IStatusRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public StatusRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public void AddStatus(StatusDto statusDto)
        {
        var status = _mapper.Map<UserStatus>(statusDto);
            _context.Status.Add(status);
        }

        public void Delete(int id)
        {
            var status = getStatus(id);
            _context.Status.Remove(status);
        }

        public IEnumerable<UserStatus> GetStatus()
        {
            return _context.Status;
        }

        public void Update(UserStatus userStatus)
        {
            // _context.Status.Update(status);
            // _context.SaveChanges();

             _context.Entry(userStatus).State = EntityState.Modified;
            //  var status = getStatus(id);
            
        }

         private UserStatus getStatus(int id)
        {
            var status = _context.Status.Find(id);
            if (status == null) throw new KeyNotFoundException("User not found");
            return status;
        }
    }
}