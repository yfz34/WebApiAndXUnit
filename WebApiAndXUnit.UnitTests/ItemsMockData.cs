using System;
using System.Collections.Generic;
using WebApiAndXUnit.Api.Dtos;
using WebApiAndXUnit.Api.Entities;

namespace WebApiAndXUnit.UnitTests;

public class ItemsMockData
{
    private readonly Random rand = new();

    public List<Item> GetItems()
    {
        return new List<Item>{
            new Item
            {
                Name = "橡皮擦",
                Description = "這是一個橡皮擦",
                Price = 101
            },
            new Item
            {
                Name = "鉛筆",
                Description = "這是一支鉛筆",
                Price = 102
            },
            new Item
            {
                Name = "原子筆",
                Description = "這是一支原子筆",
                Price = 103
            }
         };
    }

    public Item CreateRandomItem()
    {
        return new()
        {
            Name = "尺",
            Description = "這是一把尺",
            Price = rand.Next(1000),
            CreatedTime = DateTimeOffset.UtcNow,
            UpdatedTime = DateTimeOffset.UtcNow
        };
    }
}