using Data.Entities;

namespace Data.Repositories
{
    public interface IUserProjectionRepository
    {
        long Insert(UserProjection projection);
    }


    public class UserProjectionRepository : IUserProjectionRepository
    {
        private readonly IDatabaseContext _databaseContext;


        public UserProjectionRepository(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }


        public long Insert(UserProjection projection)
        {
            _databaseContext.ActiveUsers.Insert(projection);
            return projection.UserId;
        }
    }

}
