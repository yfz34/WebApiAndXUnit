using WebApiAndXUnit.Api.Dtos;
using WebApiAndXUnit.Api.Entities;

public static class Extensions
{
    public static ItemDto AsDto(this Item item)
    {
        return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedTime, item.UpdatedTime);
    }

    public static Item ToItem(this CreateItemDto createItemDto)
    {
        return new Item
        {
            Name = createItemDto.Name,
            Description = createItemDto.Description,
            Price = createItemDto.Price,
            CreatedTime = DateTimeOffset.UtcNow,
            UpdatedTime = DateTimeOffset.UtcNow
        };
    }

    public static Item ToItem(this UpdateItemDto updateItemDto, int id)
    {
        return new Item
        {
            Id = id,
            Name = updateItemDto.Name,
            Description = updateItemDto.Description,
            Price = updateItemDto.Price,
            UpdatedTime = DateTimeOffset.UtcNow
        };
    }
}