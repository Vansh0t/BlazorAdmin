using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using BlazorAdmin.Utils;
using System.Linq;
using System.Reflection;
using System;
using Xunit;
using Xunit.Abstractions;
using BlazorAdmin.Pages;
namespace BlazorAdmin.Tests;

public class DatabaseTest: IClassFixture<DbContextFixture>
{
    private readonly ITestOutputHelper output;
    public DbContextFixture Fixture { get; }
    public DatabaseTest(DbContextFixture fixture, ITestOutputHelper output)
    {
        Fixture = fixture;
        this.output = output;
    }

    public class Set
    {
        public string name;
        public int count;
        public Set(string name, int count)
        {
            this.name = name;
            this.count = count;
        }
    }

    [Fact]
    public async Task CountSetsAsync_AdminVisible_Test()
    {
        
        using(var ctx = Fixture.CreateContext())
        {
            var sets = await Database.CountSetsAsync(ctx);

            Assert.Equal(3, sets.Count);
        }
        
    }
    [Fact]
    public async Task CountSetsAsync_Test()
    {

        using (var ctx = Fixture.CreateContext())
        {
            var sets = await Database.CountSetsAsync(ctx, false);

            Assert.Equal(4, sets.Count);
        }

    }
    [Fact]
    public void GetDbQueryable_Test()
    {

        using (var ctx = Fixture.CreateContext())
        {
            var result = Database.GetSetQueryable(ctx, "Parents");
            Assert.NotNull(result);
        }

    }
    [Fact]
    public async Task GetModelIndexData_Test()
    {
        ModelIndex pageParent = new ModelIndex();
        ModelIndex pageChild1 = new ModelIndex();
        ModelIndex pageChild2 = new ModelIndex();
        ModelIndex notIndexed = new ModelIndex();
        using (var ctx = Fixture.CreateContext())
        {
            await pageParent.InitDbDataAsync("Parents", ctx);
            Assert.Equal(7, pageParent.modelProps.Count);
            Assert.Equal(3, pageParent.rows.Count);
            Assert.Equal(typeof(DbContextFixture.ParentEntity), pageParent.modelType);

            await pageChild1.InitDbDataAsync("Children1", ctx);
            Assert.Equal(5, pageChild1.modelProps.Count);
            Assert.Equal(15, pageChild1.rows.Count);
            Assert.Equal(typeof(DbContextFixture.ChildEntity1), pageChild1.modelType);

            await pageChild2.InitDbDataAsync("Children2", ctx);
            Assert.Equal(3, pageChild2.modelProps.Count);
            Assert.Equal(3, pageChild2.rows.Count);
            Assert.Equal(typeof(DbContextFixture.ChildEntity2), pageChild2.modelType);

            await notIndexed.InitDbDataAsync("NotIndexed", ctx);
            Assert.NotEqual("", notIndexed.Error); //NotIndexed does not have [AdminVisible] on model class. It should display error.

        }
    }
    [Fact]
    public async Task GetEntityById_Test()
    {

        using (var ctx = Fixture.CreateContext())
        {
            var result = await Database.GetEntityByIdAsync(ctx, typeof(DbContextFixture.ParentEntity), 1);
            Assert.NotNull(result);
        }

    }
    [Fact]
    public async Task GetNavigationSets_Test()
    {

        using (var ctx = Fixture.CreateContext())
        {
            var result = await Database.GetNavigationSetsAsync(ctx, typeof(DbContextFixture.ParentEntity));
            var manyChildrenPropKey = result.Select(_ => _.Key).FirstOrDefault(_ => _.Name == "ManyChildren");
            var oneChildPropKey = result.Select(_ => _.Key).FirstOrDefault(_ => _.Name == "OneToOneChild");
            Assert.NotEmpty(result);
            Assert.NotNull(manyChildrenPropKey);
            Assert.IsType<DbContextFixture.ChildEntity1>(result[manyChildrenPropKey][0]);
            Assert.NotNull(oneChildPropKey);
            Assert.IsType<DbContextFixture.ChildEntity2>(result[oneChildPropKey][0]);
        }

    }
    //specific DateTime test for npgsql
    [Fact]
    public async Task DateTimeNpgsql_Test()
    {
        using (var ctx = Fixture.CreateContext())
        {
            var entity = await ctx.Parents.FirstAsync();
            //yyyy-MM-ddThh:mm
            string dtStr = "2022-08-22T21:13";
            var dt = DateTime.SpecifyKind(DateTime.Parse(dtStr), DateTimeKind.Utc);
            var dto = DateTimeOffset.Parse(dtStr).ToOffset(TimeSpan.Zero);
            entity.DateTime = dt;
            entity.DateTimeOffset = dto;
            await ctx.SaveChangesAsync();
        }
    }
}