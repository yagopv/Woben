using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

using Woben.Domain.Model;
using Woben.Domain.Repositories;

namespace Woben.Data.Repositories
{
    public class ArticleRepository : Repository<Article>, IArticleRepository
    {
        public ArticleRepository(DbSet<Article> dbset) : base(dbset) { }

        public new IQueryable<Article> All()
        {
            return this.DbSet.Include("Category");
        }

    }
}
