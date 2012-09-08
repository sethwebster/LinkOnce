using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LinkOnce.Models
{
    public interface IDatabaseContext
    {
        IDbSet<Link> Links { get; set; }
        int SaveChanges();
    }

    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public IDbSet<Link> Links { get; set; }
    }
}