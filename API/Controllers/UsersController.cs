using API.Data;
using API.DTOs;
using API.Entities;
using API.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUser()
        {
            var user = await _userRepository.GetMembersAsync();
            return Ok(user);

        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _userRepository.GetMembersAsync(username);
        }
    }
}
