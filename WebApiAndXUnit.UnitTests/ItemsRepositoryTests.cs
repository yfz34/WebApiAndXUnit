using System;
using System.Linq;
using System.Threading.Tasks;
using WebApiAndXUnit.Api.Data;
using WebApiAndXUnit.Api.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Collections.Generic;
using WebApiAndXUnit.Api.Entities;
using WebApiAndXUnit.Api.Dtos;

namespace WebApiAndXUnit.UnitTests;

public class ItemsRepositoryTests : IDisposable
{
    protected readonly AppDbContext _context;
    private readonly ItemsMockData _itemsMockData;

    public ItemsRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

        _context = new AppDbContext(options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        _itemsMockData = new ItemsMockData();
    }

    private ItemsRepository CreateRepository()
    {
        PopulateData();
        return new ItemsRepository(_context);
    }

    private void PopulateData()
    {
        var items = _itemsMockData.GetItems();
        _context.Items.AddRange(items);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetAllAsync_Success_ReturnsAllItems()
    {
        // Arrange
        var repositoryStub = CreateRepository();

        // Act
        var items = await repositoryStub.GetAllAsync();

        // Assert
        Assert.Equal(3, items.Count);
    }

    [Fact]
    public async Task GetByIdAsyncc_Success_ReturnsItem()
    {
        // Arrange
        var repositoryStub = CreateRepository();
        var items = _itemsMockData.GetItems();

        // Act
        var item = await repositoryStub.GetByIdAsync(1);

        // Assert
        Assert.NotNull(item);
        Assert.Equal(items[0].Name, item?.Name);
        Assert.Equal(items[0].Description, item?.Description);
        Assert.Equal(items[0].Price, item?.Price);
    }

    [Fact]
    public async Task CreateAsync_Success_Test()
    {
        // Arrange
        var repositoryStub = CreateRepository();
        var newItem = _itemsMockData.CreateRandomItem();

        // Act
        await repositoryStub.CreateAsync(newItem);

        // Assert
        int expectedRecordCount = (_itemsMockData.GetItems().Count() + 1);
        _context.Items.Count().Should().Be(expectedRecordCount);
    }

    [Fact]
    public async Task UpdateAsync_Success_Test()
    {
        // Arrange
        var repositoryStub = CreateRepository();

        // Act
        var itemToUpdate = await repositoryStub.GetByIdAsync(1, false);

        if (itemToUpdate is null)
            return;

        itemToUpdate.Name = Guid.NewGuid().ToString();
        itemToUpdate.Description = Guid.NewGuid().ToString();
        itemToUpdate.Price = 1;

        await repositoryStub.UpdateAsync(itemToUpdate);

        var result = await repositoryStub.GetByIdAsync(1, false);

        // Assert
        result.Should().BeEquivalentTo(itemToUpdate);
    }

    [Fact]
    public async Task RemoveAsync_Success_Test()
    {
        // Arrange
        var repositoryStub = CreateRepository();

        // Act
        await repositoryStub.RemoveAsync(1);

        // Assert
        var items = await repositoryStub.GetAllAsync();
        Assert.Equal(2, items.Count);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}