using API.Data;
using API.Interfaces;
using AutoMapper;

namespace API.Implement
{
    public class UnitOfWork : IUnitOfWord
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public UnitOfWork(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public IUserRepository UserRepository => new UserRepository(_dataContext, _mapper);

        public IMessageRepository MessageRepository => new MessageRepository(_dataContext, _mapper);

        public ILikesRepository LikesRepository => new LikeRepository(_dataContext);

        public async Task<bool> Complete()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return _dataContext.ChangeTracker.HasChanges();
        }
    }
}
