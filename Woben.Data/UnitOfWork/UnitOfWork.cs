using Woben.Data.Repositories;
using Woben.Domain.Repositories;
using Woben.Domain.Model;
using Woben.Domain.UnitOfWork;
using System.Data.Entity;

namespace Woben.Data.UnitOfWork
{
    /// <summary>
    /// Implementation for the UnitOfWork in the current app
    /// </summary>
    public class UnitOfWork : WobenDbContext, IUnitOfWork
    {
        /// <summary>
        /// ctor
        /// </summary>
        public UnitOfWork()
        {
            ArticleRepository = new ArticleRepository(this.Articles);
            CategoryRepository = new Repository<Category>(this.Categories);
            TagRepository = new Repository<Tag>(this.Tags);
            UserProfileRepository = new Repository<UserProfile>(this.Users as DbSet<UserProfile>);
        }

        /// <summary>
        /// Reporitories
        /// </summary>
        public IRepository<Article> ArticleRepository {get; private set;}        
        public IRepository<Category> CategoryRepository { get; private set; }
        public IRepository<Tag> TagRepository { get; private set; }
        public IRepository<UserProfile> UserProfileRepository { get; private set; }

        /// <summary>
        /// Save Changes
        /// </summary>
        public void Commit()
        {
            this.Commit();
        }
    }
}
