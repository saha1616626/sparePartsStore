using System;
using System.Collections.Generic;

namespace sparePartsStore.Model;

public partial class Autopart
{
    public int AutopartId { get; set; }

    public int NumberAutopart { get; set; }

    public string NameAutopart { get; set; } = null!;

    public int KnotId { get; set; }

    public int CarModelId { get; set; }

    public int ManufactureId { get; set; }

    public decimal PriceSale { get; set; }

    public int AvailableityStock { get; set; }

    public int AccountId { get; set; }

    public string ModerationStatus { get; set; } = null!;

    public virtual Account Account { get; set; } = null!;

    public virtual CarModel CarModel { get; set; } = null!;

    public virtual Knot Knot { get; set; } = null!;

    public virtual Manufacture Manufacture { get; set; } = null!;
}
