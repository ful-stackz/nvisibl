﻿using Nvisibl.Cloud.Factories.Interfaces;
using Nvisibl.Cloud.Models;
using Nvisibl.Cloud.Services.Interfaces;
using Nvisibl.DataLibrary.Models;
using Nvisibl.DataLibrary.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nvisibl.Cloud.Services
{
    public class UserManagerService : IUserManagerService
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public UserManagerService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory ?? throw new ArgumentNullException(nameof(unitOfWorkFactory));
        }

        public async Task MakeFriendsAsync(int userId, UserModel friend)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            if (friend is null)
            {
                throw new ArgumentNullException(nameof(friend));
            }

            using var unitOfWork = _unitOfWorkFactory.Create();

            await unitOfWork.GetRepository<IFriendsRepository>().AddAsync(new Friend
            {
                User1Id = userId,
                User2Id = friend.Id,
            });
            _ = await unitOfWork.CompleteAsync();
        }

        public async Task<UserModel> CreateUserAsync(CreateUserModel createUserModel)
        {
            if (createUserModel is null)
            {
                throw new ArgumentNullException(nameof(createUserModel));
            }

            using var unitOfWork = _unitOfWorkFactory.Create();

            var user = new User { Username = createUserModel.Username, };
            await unitOfWork.GetRepository<IUserRepository>().AddAsync(user);
            _ = await unitOfWork.CompleteAsync();

            return Mappers.ToUserModel(user);
        }

        public async Task<UserModel> GetUserByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            using var unitOfWork = _unitOfWorkFactory.Create();

            return Mappers.ToUserModel(await unitOfWork.GetRepository<IUserRepository>().GetAsync(id));
        }

        public async Task<IEnumerable<UserModel>> GetUsersAsync(int rangeOffset = 0, int rangeSize = 10)
        {
            if (rangeOffset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rangeOffset));
            }

            if (rangeSize < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rangeSize));
            }

            using var unitOfWork = _unitOfWorkFactory.Create();

            return (await unitOfWork.GetRepository<IUserRepository>().GetRangeAsync(rangeOffset, rangeSize))
                .Select(Mappers.ToUserModel)
                .ToList();
        }

        public async Task<UserWithFriendsModel> GetUserWithFriendsByIdAsync(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            using var unitOfWork = _unitOfWorkFactory.Create();

            var user = await unitOfWork.GetRepository<IUserRepository>().GetAsync(id);
            if (user is null)
            {
                throw new InvalidOperationException($"User with id ({id}) does not exist.");
            }

            var friends = await unitOfWork.GetRepository<IFriendsRepository>().GetAllFriendsAsync(user);
            return Mappers.ToUserWithFriendsModel(user, friends);
        }
    }
}
