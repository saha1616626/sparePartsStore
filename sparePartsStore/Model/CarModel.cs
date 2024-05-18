using System;
using System.Collections.Generic;

namespace sparePartsStore.Model;

public partial class CarModel
{
    public int CarModelId { get; set; }

    public string NameCarModel { get; set; } = null!;

    public int CarBrandId { get; set; }

    public virtual ICollection<Autopart> Autoparts { get; set; } = new List<Autopart>();

    public virtual CarBrand CarBrand { get; set; } = null!;
}
