namespace Fame.Service.ChangeTracking
{
    public interface IUnitOfWork
    {
        void Save();
        void BeginTransaction();
        void CommitTransaction();
        void RollBackTransaction();
    }
}