using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WebApiAndXUnit.Api.Controllers;
using WebApiAndXUnit.Api.Dtos;
using WebApiAndXUnit.Api.Entities;
using WebApiAndXUnit.Api.IRepositories;
using Xunit;

namespace WebApiAndXUnit.UnitTests;

public class ItemsControllerTests
{

    private readonly Mock<IItemsRepository> repositoryStub = new();
    private readonly Mock<ILogger<ItemsController>> loggerStub = new();
    private readonly Random rand = new();

    [Fact]
    public async Task GetItemAsync_WithUnexistingItem_ReturnsNotFound()
    {
        // Arrange
        repositoryStub.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), false)).ReturnsAsync((Item?)null);

        var controller = new ItemsController(loggerStub.Object, repositoryStub.Object);

        // Act
        var result = await controller.GetItemAsync(It.IsAny<int>());

        // Assert
        // Assert.IsType<NotFoundResult>(result.Result);

        // FluentAssertions
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetItemAsync_WithExistingItem_ReturnsExpectedItem()
    {
        // Arrange
        var expectedItem = CreateRandomItem();
        repositoryStub.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), false)).ReturnsAsync(expectedItem);

        var controller = new ItemsController(loggerStub.Object, repositoryStub.Object);

        // Act
        var result = await controller.GetItemAsync(It.IsAny<int>());

        // Assert

        // Assert.IsType<ItemDto>(result.Value);
        // var dto = (result as ActionResult<ItemDto>).Value;
        // Assert.Equal(expectedItem.Id, dto.Id);
        // Assert.Equal(expectedItem.Name, dto.Name);

        // FluentAssertions
        // result.Value.Should().BeEquivalentTo(
        //     expectedItem.AsDto(),
        //     options => options.ComparingByMembers<Item>());
        result.Value.Should().BeEquivalentTo(expectedItem.AsDto());
    }

    [Fact]
    public async Task GetItemsAsync_WithExistingItems_ReturnsAllItems()
    {
        // Arrange
        var expectedItems = new[] { CreateRandomItem(), CreateRandomItem(), CreateRandomItem() };

        repositoryStub.Setup(repo => repo.GetAllAsync(false)).ReturnsAsync(expectedItems);

        var controller = new ItemsController(loggerStub.Object, repositoryStub.Object);

        // Act
        var actualItems = await controller.GetItemsAsync();

        // Assert
        actualItems.Should().BeEquivalentTo(expectedItems);
    }

    [Fact]
    public async Task GetItemsAsync_WithMatchingItems_ReturnsMatchingItems()
    {
        // Arrange
        var allItems = new[]
        {
            new Item
            {
                Id = 1,
                Name = "橡皮擦",
                Description = "這是一個橡皮擦"
            },
            new Item
            {
                Id = 2,
                Name = "鉛筆",
                Description = "這是一支鉛筆"
            },
            new Item
            {
                Id = 3,
                Name = "原子筆",
                Description = "這是一支原子筆"
            }
        };

        var nameToMatch = "橡皮擦";

        repositoryStub.Setup(repo => repo.GetAllAsync(false)).ReturnsAsync(allItems);

        var controller = new ItemsController(loggerStub.Object, repositoryStub.Object);

        // Act
        IEnumerable<ItemDto> foundItems = await controller.GetItemsAsync(nameToMatch);

        // Assert
        foundItems.Should().OnlyContain(
            item => item.Name == allItems[0].Name || item.Name == allItems[1].Name || item.Name == allItems[2].Name
        );
    }

    [Fact]
    public async Task CreateItemAsync_WithItemToCreate_ReturnsCreatedItem()
    {
        // Arrange
        var itemToCreate = new CreateItemDto(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            rand.Next(1000));

        var controller = new ItemsController(loggerStub.Object, repositoryStub.Object);

        // Act
        var result = await controller.CreateItemAsync(itemToCreate);

        // Assert
        var createdItem = (result.Result as CreatedAtActionResult)?.Value as ItemDto;
        itemToCreate.Should().BeEquivalentTo(
            createdItem,
            options => options.ComparingByMembers<ItemDto>().ExcludingMissingMembers()
        );
        // createdItem.Id.Should().NotBe(0);
        createdItem?.CreatedTime.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task UpdateItemAsync_WithExistingItem_ReturnsNoContent()
    {
        // Arrange
        var existingItem = CreateRandomItem();
        repositoryStub.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), false)).ReturnsAsync(existingItem);

        var itemId = existingItem.Id;
        var itemToUpdate = new UpdateItemDto(Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            existingItem.Price + 3);

        var controller = new ItemsController(loggerStub.Object, repositoryStub.Object);

        // Act
        var result = await controller.UpdateItemAsync(itemId, itemToUpdate);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteItemAsync_WithExistingItem_ReturnsNoContent()
    {
        // Arrange
        var existingItem = CreateRandomItem();
        repositoryStub.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), false)).ReturnsAsync(existingItem);

        var controller = new ItemsController(loggerStub.Object, repositoryStub.Object);

        // Act
        var result = await controller.DeleteItemAsync(existingItem.Id);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    private Item CreateRandomItem()
    {
        return new()
        {
            Id = 1,
            Name = Guid.NewGuid().ToString(),
            Price = rand.Next(1000),
            CreatedTime = DateTimeOffset.UtcNow,
            UpdatedTime = DateTimeOffset.UtcNow
        };
    }
}