using Nvisibl.Cloud.Models;
using Nvisibl.DataLibrary.Models;
using Nvisibl.DataLibrary.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nvisibl.Cloud.Services
{
    public class UserManagerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserManagerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task MakeFriendsAsync(int userId, UserModel friend)
        {
            await _unitOfWork.FriendsRepository.AddAsync(new Friend
            {
                User1Id = userId,
                User2Id = friend.Id,
            });
            _ = await _unitOfWork.CompleteAsync();
        }

        public async Task<UserModel> CreateUserAsync(CreateUserModel createUserModel)
        {
            if (createUserModel is null)
            {
                throw new ArgumentNullException(nameof(createUserModel));
            }

            var user = new User { Username = createUserModel.Username, };
            await _unitOfWork.UserRepository.AddAsync(user);
            _ = await _unitOfWork.CompleteAsync();
            return Mappers.ToUserModel(user);
        }

        public async Task<UserModel> GetUserByIdAsync(int id)
        {
            return Mappers.ToUserModel(await _unitOfWork.UserRepository.GetAsync(id));
        }

        public async Task<IEnumerable<UserModel>> GetUsersAsync(int rangeOffset = 0, int rangeSize = 10)
        {
            return (await _unitOfWork.UserRepository.GetRangeAsync(rangeOffset, rangeSize))
                .Select(Mappers.ToUserModel)
                .ToList();
        }

        public async Task<UserWithFriendsModel> GetUserWithFriendsByIdAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(id);
            if (user is null)
            {
                throw new InvalidOperationException($"User with id ({id}) does not exist.");
            }

            var friends = await _unitOfWork.FriendsRepository.GetAllFriendsAsync(user);
            return Mappers.ToUserWithFriendsModel(user, friends);
        }
    }
}
