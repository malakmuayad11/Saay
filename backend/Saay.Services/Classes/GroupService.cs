using Saay.Data.Entities;
using Saay.Infrastructure.DTOs.GroupDTOs;
using Saay.Repository.Interfaces;
using Saay.Services.Interfaces;
using Saay.Services.Interfacess;

namespace Saay.Services.Classes
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;
        private readonly IGroupInvitationTokenService _groupInvitationTokenService;

        public GroupService(IGroupRepository groupRepository, IUserRepository userRepository,
            IUserGroupRepository userGroupRepository, IEmailService emailService,
            ITokenService tokenService, IGroupInvitationTokenService groupInvitationTokenService)
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
            _userGroupRepository = userGroupRepository;
            _emailService = emailService;
            _tokenService = tokenService;
            _groupInvitationTokenService = groupInvitationTokenService;
        }
        public async Task<int?> CreateGroupAsync(AddGroupDto addGroupDto)
        {
            await _userRepository.BeginTransactionAsync();

            try
            {
                Group newGroup = new Group
                {
                    Name = addGroupDto.GroupName,
                    Description = addGroupDto.Description
                };

                int? groupId = await _groupRepository.AddGroupAsync(newGroup);

                if (groupId == null)
                {
                    await _userRepository.RollbackTransactionAsync();
                    return null;
                }

                UserGroup userGroup = new UserGroup
                {
                    GroupId = groupId.Value,
                    UserId = addGroupDto.UserId,
                    Role = (byte)IUserGroupService.enRole.Admin
                };

                if(!await _userGroupRepository.AddUserGroupAsync(userGroup) == null)
                {
                    await _userRepository.RollbackTransactionAsync();
                    return null;
                }

                await _userRepository.CommitTransactionAsync();

                return groupId;
            }
            catch
            {
                await _userRepository.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<List<GroupDto>> GetUserGroupsAsync(int userId)
        {
            if (!await _userRepository.DoesUserExist(userId)) return null;

            return await _groupRepository.GetUserGroupsAsync(userId);
        }

        public async Task<bool?> InviteUserToGroup(int groupId, string email)
        {
            if (!await _userRepository.DoesEmailExist(email))
                return null; // User not found

            string userName = await _userRepository.GetNameAsync(email);

            if (userName == null) return null; // User not found

            int userId = await _userRepository.GetUserIdByEmailAsync(email);

            if(userId == null) return null; // User not found

            string groupName = await _groupRepository.GetGroupName(groupId);

            if(groupName == null) return null; // Group not found

            string token = _tokenService.GenerateEmailConfirmationToken();

            if(await _groupInvitationTokenService.AddGroupInvitationTokenAsync(groupId, userId, token) == null)
                return false; // Failed to create token

            string confirmationUrl =
                $"https://saay.vercel.app/invitation-confirm?token={Uri.EscapeDataString(token)}";

            return await _emailService.SendGroupInvitationAsync(email, "Join a New Group on Saay!", userName, groupName, confirmationUrl);
        }
    }
}
