using System.ComponentModel.DataAnnotations;

namespace WebApiAndXUnit.Api.Dtos;

public record ItemDto(int Id, string Name, string Description, decimal Price, DateTimeOffset CreatedTime, DateTimeOffset UpdatedTime);

public record CreateItemDto([Required] string Name, string Description, [Range(1, 1000)] decimal Price);

public record UpdateItemDto([Required] string Name, string Description, [Range(0, 1000)] decimal Price);
