using LiveMap.Persistence;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace LiveMapDashboard.Tests;

public abstract class TestBase : IAsyncLifetime, IDisposable
{
    private readonly MsSqlContainer _container;
    protected LiveMapContext Context { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    protected TestBase()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
        _container = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("bOrdzRnuTR-EaL02578$#a")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        var options = new DbContextOptionsBuilder<LiveMapContext>()
            .UseSqlServer(_container.GetConnectionString(), x => x.UseNetTopologySuite())
            .Options;

        Context = new LiveMapContext(options);
        await Context.Database.EnsureCreatedAsync();
        SeedBaseData();
    }

    protected virtual void SeedBaseData() { }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
        Context?.Dispose();
    }

    public void Dispose()
    {
        Context?.Dispose();
    }
}