using Woben.Domain.Repositories;
using Woben.Domain.Model;

namespace Woben.Domain.UnitOfWork
{
    /// <summary>
    /// Contract for the UnitOfWork
    /// </summary>
    public interface IUnitOfWork
    {
        IRepository<Article> ArticleRepository { get; }
        IRepository<Category> CategoryRepository { get; }
        IRepository<Tag> TagRepository { get; }
        IRepository<UserProfile> UserProfileRepository { get; }

        void Commit();
    }
}
