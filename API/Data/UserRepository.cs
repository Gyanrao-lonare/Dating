using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        public DataContext _Context { get; }
        public IMapper _mapper { get; }
        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _Context = context;
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _Context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _Context.Users
            .Include(p => p.Photos.Where(d=> d.IsAproved))
            .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _Context.Users
            .Include(p => p.Photos.Where(d=> d.IsAproved))
            .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _Context.SaveChangesAsync() > 0;
        }

        public void update(AppUser user)
        {
            _Context.Entry(user).State = EntityState.Modified;
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParms userParms)
        {
            var query = _Context.Users.AsQueryable();
            query = query.Where(u => u.UserName != userParms.CurrentUserName);
            query = query.Where(u => u.Gender == userParms.Gender);

            var minDob = DateTime.Today.AddYears(-userParms.MaxAge - 1);
            var maxDob = DateTime.Today.AddYears(-userParms.MinAge);

            query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);
            query = userParms.OrderBy switch
            {
                "created" => query.OrderByDescending(u => u.CreatedDate),
                _ => query.OrderByDescending(u => u.LastActive)
            };

            return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(_mapper
            .ConfigurationProvider).AsNoTracking(), userParms.pageNumber, userParms.PageSize);
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _Context.Users
            .Where(x => x.UserName == username)
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
        }

        public void UpdatePhotosAsync(photo photo)
        {
             _Context.Entry(photo).State = EntityState.Modified;
            // throw new NotImplementedException();
        }

        public IEnumerable<photo> GetPhotosAsync()
        {
            return  _Context.Photos;
            // throw new NotImplementedException();


        }

        public async Task<photo> GetphotoAsync(int id)
        {
            return await _Context.Photos.FindAsync(id);
            // throw new NotImplementedException();
        }
    }
}