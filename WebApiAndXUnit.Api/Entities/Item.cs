using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiAndXUnit.Api.Entities;

public class Item : BaseEntity
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    [Column(TypeName = "decimal(9, 2)")]
    public decimal Price { get; set; }
}