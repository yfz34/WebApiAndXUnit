using Microsoft.EntityFrameworkCore;
using WebApiAndXUnit.Api.Data;
using WebApiAndXUnit.Api.Entities;
using WebApiAndXUnit.Api.IRepositories;

namespace WebApiAndXUnit.Api.Repository;

public class ItemsRepository : IItemsRepository
{
    private readonly AppDbContext _context;

    public ItemsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Item>> GetAllAsync(bool asNoTracking = false)
    {
        var queryable = _context.Items.AsQueryable();
        if (asNoTracking)
        {
            return await queryable.AsNoTracking().ToListAsync();
        }

        return await _context.Items.ToListAsync();
    }

    public async Task<Item?> GetByIdAsync(int id, bool asNoTracking = false)
    {
        var queryable = _context.Items.AsQueryable();
        if (asNoTracking)
        {
            return await queryable.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        return await queryable.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task CreateAsync(Item newItem)
    {
        _context.Items.Add(newItem);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Item updateItem)
    {
        _context.Items.Update(updateItem);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(int id)
    {
        // var item = new Item() { Id = id };
        // _context.Items.Attach(item);
        // _context.Items.Remove(item);
        // await _context.SaveChangesAsync();

        var existingItem = await GetByIdAsync(id);
        if (existingItem == null)
        {
            throw new InvalidOperationException();
        }

        _context.Items.Remove(existingItem);
        await _context.SaveChangesAsync();
    }
}