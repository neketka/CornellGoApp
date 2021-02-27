using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendModel
{
    public class CornellGoDbFactory : IDesignTimeDbContextFactory<CornellGoDb>
    {
        public CornellGoDb CreateDbContext(string[] args)
        {
            var o = new DbContextOptionsBuilder<CornellGoDb>();
            o.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=test;Database=dbo", e => e.UseNetTopologySuite());

            return new CornellGoDb(o.Options);
        }
    }
}
