using Microsoft.AspNetCore.Mvc;
using WebApiAndXUnit.Api.Dtos;
using WebApiAndXUnit.Api.Entities;
using WebApiAndXUnit.Api.IRepositories;

namespace WebApiAndXUnit.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly ILogger<ItemsController> _logger;
    private readonly IItemsRepository _itemsRepository;

    public ItemsController(ILogger<ItemsController> logger, IItemsRepository itemsRepository)
    {
        _logger = logger;
        _itemsRepository = itemsRepository;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ItemDto>), StatusCodes.Status200OK)]
    public async Task<IEnumerable<ItemDto>> GetItemsAsync(string? name = null)
    {
        var items = (await _itemsRepository.GetAllAsync()).Select(item => item.AsDto());

        if (!string.IsNullOrWhiteSpace(name))
        {
            items = items.Where(item => item.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        _logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrieved {items}");

        return items;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ItemDto>> GetItemAsync(int id)
    {
        var item = await _itemsRepository.GetByIdAsync(id);

        if (item is null)
            return NotFound();

        return item.AsDto();
    }

    [HttpPost]
    [ProducesResponseType(typeof(ItemDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto createItemDto)
    {

        var item = createItemDto.ToItem();

        try
        {
            await _itemsRepository.CreateAsync(item);
            return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id }, item.AsDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500);
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ItemDto), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateItemAsync(int id, UpdateItemDto updateItemDto)
    {
        var existingItem = await _itemsRepository.GetByIdAsync(id);

        if (existingItem is null)
            return NotFound();

        existingItem.Name = updateItemDto.Name;
        existingItem.Description = updateItemDto.Description;
        existingItem.Price = updateItemDto.Price;
        existingItem.UpdatedTime = DateTimeOffset.UtcNow;

        await _itemsRepository.UpdateAsync(existingItem);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ItemDto), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteItemAsync(int id)
    {
        var existingItem = await _itemsRepository.GetByIdAsync(id);

        if (existingItem is null)
            return NotFound();

        await _itemsRepository.RemoveAsync(id);

        return NoContent();
    }
}