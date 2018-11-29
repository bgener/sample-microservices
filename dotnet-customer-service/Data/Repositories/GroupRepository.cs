using System;
using System.Linq;
using Data.Entities;

namespace Data.Repositories
{
    public interface IGroupRepository
    {
        IQueryable<string> Get();
        void Insert(Group userGroup);
        bool Exists(int groupId);
    }

    public class GroupRepository : IGroupRepository
    {
        private readonly IDatabaseContext _databaseContext;


        public GroupRepository(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public bool Exists(int groupId)
        {
            var hasGroup = _databaseContext.Groups
               .Any(x => x.Id == groupId);
            return hasGroup;
        }

        public IQueryable<string> Get()
        {
            var groups = _databaseContext.Groups
                .Select(x => x.Name);
            return groups;
        }


        public void Insert(Group userGroup)
        {
            _databaseContext.Groups.Insert(userGroup);
        }
    }
}
