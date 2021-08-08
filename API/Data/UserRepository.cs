using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext Context;
        private readonly IMapper Mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            this.Mapper = mapper;
            this.Context = context;
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await this.Context.Users
            .Include(p => p.Photos)
            .ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await this.Context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await this.Context.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await this.Context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            this.Context.Entry(user).State = EntityState.Modified;
        }

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            return await this.Context.Users
                .ProjectTo<MemberDto>(this.Mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await this.Context.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDto>(this.Mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }
    }
}