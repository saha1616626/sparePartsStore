using System;
using System.Collections.Generic;

namespace sparePartsStore.Model;

public partial class CarBrand
{
    public int CarBrandId { get; set; }

    public string NameCarBrand { get; set; } = null!;

    public virtual ICollection<CarModel> CarModels { get; set; } = new List<CarModel>();
}
