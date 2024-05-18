using System;
using System.Collections.Generic;

namespace sparePartsStore.Model;

public partial class Country
{
    public int CountryId { get; set; }

    public string NameCountry { get; set; } = null!;

    public virtual ICollection<Manufacture> Manufactures { get; set; } = new List<Manufacture>();
}
