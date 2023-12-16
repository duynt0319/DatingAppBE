using API.DTOs;
using API.Entities;

namespace API.Repository.IRepository
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsyn();
        Task<IEnumerable<AppUser>> GetUsersAsyn();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUserNameAsync(string username);
        Task<IEnumerable<MemberDto>> GetMembersAsync();
        Task<MemberDto> GetMembersAsync(string username);
    }
}
