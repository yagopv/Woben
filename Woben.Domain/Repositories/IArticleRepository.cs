using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Woben.Domain.Model;

namespace Woben.Domain.Repositories
{
    public interface IArticleRepository : IRepository<Article>
    {
        IQueryable<Article> All();
    }
}
