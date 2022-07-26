using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IStatusRepository
    {
        IEnumerable<UserStatus> GetStatus();
        void AddStatus(StatusDto statusDto);
       void Update(UserStatus userStatus);
        void Delete(int id);
    }
}