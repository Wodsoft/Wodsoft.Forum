using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wodsoft.Forum.Sample.Entity;

namespace Wodsoft.Forum.Sample
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }

        public DataContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        public DbSet<Board> Board { get; set; }

        public DbSet<Member> Member { get; set; }

        public DbSet<Entity.Forum> Forum { get; set; }

        public DbSet<Thread> Thread { get; set; }

        public DbSet<Post> Post { get; set; }
    }
}
