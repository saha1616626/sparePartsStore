using System;
using System.Collections.Generic;

namespace sparePartsStore.Model;

public partial class Manufacture
{
    public int ManufactureId { get; set; }

    public string NameManufacture { get; set; } = null!;

    public int CountryId { get; set; }

    public virtual ICollection<Autopart> Autoparts { get; set; } = new List<Autopart>();

    public virtual Country Country { get; set; } = null!;
}
