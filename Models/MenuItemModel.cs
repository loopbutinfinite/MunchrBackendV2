using System;
using System.ComponentModel.DataAnnotations;

namespace MunchrBackendV2.Models
{
    public class MenuItemModel
    {
        [Key]
        public int Id { get; set; }

        // The business this menu item belongs to.
        public int BusinessId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }

        // Food-truck menu category, e.g. "Main Dishes", "Sides", "Drinks".
        public string? Category { get; set; }

        // Placeholder for a future blob-storage image URL. Nullable for now.
        public string? ImageUrl { get; set; }
    }
}
