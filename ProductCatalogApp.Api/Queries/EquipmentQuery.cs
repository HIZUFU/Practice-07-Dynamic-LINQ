using System.ComponentModel.DataAnnotations;
using System.Linq.Dynamic.Core;
using ProductCatalogApp.Api.Models;
namespace ProductCatalogApp.Api.Queries;

public class EquipmentQuery
{
    [Range(1, int.MaxValue)]
    public int? ProductionSiteId { get; set; }
    [StringLength(100)]
    public string? Search { get; set; }
    [Required, RegularExpression("^(name|model|inventory)$")]
    public string Field { get; set; } = "name";
    [Required, RegularExpression("^(contains|starts|equals)$")]
    public string Match { get; set; } = "contains";
    [Required, RegularExpression("^(name|model|inventory|site|id)$")]
    public string SortBy { get; set; } = "name";
    [Required, RegularExpression("^(asc|desc)$")]
    public string Direction { get; set; } = "asc";
}

public static class EquipmentQueryExtensions
{
    private static readonly ParsingConfig Parsing = new() { RestrictOrderByToPropertyOrField = true };
    private static readonly IReadOnlyDictionary<string, string> Fields = new Dictionary<string, string>
    {
        ["name"] = nameof(Equipment.Name), ["model"] = nameof(Equipment.Model),
        ["inventory"] = nameof(Equipment.InventoryNumber),
        ["site"] = "ProductionSite.Name", ["id"] = nameof(Equipment.Id)
    };

    public static IQueryable<Equipment> Apply(this IQueryable<Equipment> query, EquipmentQuery options)
    {
        if (options.ProductionSiteId.HasValue)
            query = query.Where(Parsing, "ProductionSiteId == @0", options.ProductionSiteId.Value);
        if (!string.IsNullOrWhiteSpace(options.Search))
        {
            var field = Fields[options.Field];
            var predicate = options.Match switch
            {
                "contains" => field + ".Contains(@0)",
                "starts" => field + ".StartsWith(@0)",
                "equals" => field + " == @0",
                _ => throw new ArgumentException("Неизвестный вид поиска")
            };
            query = query.Where(Parsing, predicate, options.Search.Trim());
        }
        // Only property names from the dictionary reach the expression parser.
        return query.OrderBy(Parsing, Fields[options.SortBy] + " " + options.Direction + ", Id");
    }
}
