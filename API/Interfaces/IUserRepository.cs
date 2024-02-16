

using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsyn();
        Task<IEnumerable<AppUser>> GetUsersAsyn();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUserNameAsync(string username);
        Task<PageList<MemberDto>> GetMembersAsync(UserParams userParams);
        Task<MemberDto> GetMembersAsync(string username);
    }
}
