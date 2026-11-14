using Saay.Data.Entities;

namespace Saay.Repository.Interfaces
{
    public interface IGroupRepository
    {
        public Task<int?> AddGroupAsync(Group group);
    }
}
