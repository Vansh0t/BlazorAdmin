using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlazorAdmin.Annotations;

namespace BlazorAdmin.Tests
{
    public class DbContextFixture
    {
        private const string ConnectionString = @"Host=localhost;Database=testdb;Username=postgres;Password=Khara111";

        private static readonly object _lock = new();
        private static bool _databaseInitialized;

        public DbContextFixture()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();
                        var count = context.Parents.Count();
                        
                        if(count == 0)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                var parent = new ParentEntity { Name = "parent" + i, Url = "url" + i };
                                for (int c = 0; c < 5; c++)
                                {
                                    var e = new ChildEntity1
                                    {
                                        Title = "Child1" + c,
                                        Content = "Content" + c
                                    };
                                    parent.ManyChildren.Add(e);
                                }
                                var oneChild = new ChildEntity2();
                                parent.OneToOneChild = oneChild;
                                context.Add(parent);
                            }
                            
                        }
                        count = context.NotIndexed.Count();
                        if(count==0)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                var notIndexed = new NotIndexedEntity { Data = "NotIndexed" + i };
                                context.Add(notIndexed);
                            }
                        }
                        context.SaveChanges();
                    }

                    _databaseInitialized = true;
                }
            }
        }


        public TestContext CreateContext()
            => new TestContext(
                new DbContextOptionsBuilder<TestContext>()
                    .UseNpgsql(ConnectionString)
                    .Options);

        public class TestContext : DbContext
        {
            public virtual DbSet<ParentEntity> Parents { get; set; }
            public virtual DbSet<ChildEntity1> Children1 { get; set; }
            public virtual DbSet<ChildEntity2> Children2 { get; set; }

            public virtual DbSet<NotIndexedEntity> NotIndexed { get; set; }
            public TestContext(DbContextOptions<TestContext> options)
            : base(options)
            {
            }
        }
        [AdminVisible]
        public class ParentEntity
        {
            [AdminVisible]
            public int Id { get; set; }
            [AdminVisible]
            public string Name { get; set; }
            [AdminVisible]
            public string Url { get; set; }
            [AdminVisible]
            public DateTime DateTime { get; set; }
            [AdminVisible]
            public DateTimeOffset DateTimeOffset { get; set; }
            [AdminVisible]
            public virtual List<ChildEntity1> ManyChildren { get; set; } = new();
            [AdminVisible]
            public virtual ChildEntity2 OneToOneChild { get; set; }
        }
        [AdminVisible]
        public class ChildEntity1
        {
            [AdminVisible]
            public int Id { get; set; }
            [AdminVisible]
            public string Title { get; set; }
            [AdminVisible]
            public string Content { get; set; }
            [AdminVisible]
            public int ParentEntityId { get; set; }
            [AdminVisible]
            public virtual ParentEntity ParentEntity { get; set; }
        }
        [AdminVisible]
        public class ChildEntity2
        {
            [AdminVisible]
            public int Id { get; set; }
            [AdminVisible]
            public int OneToOneParentId { get; set; }
            [AdminVisible]
            public ParentEntity OneToOneParent { get; set; }

        }
        public class NotIndexedEntity
        {
            public int Id { get; set; }
            public string Data { get; set; }
        }

    }
}
