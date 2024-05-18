using System;
using System.Collections.Generic;

namespace sparePartsStore.Model;

public partial class Unit
{
    public int UnitId { get; set; }

    public string NameUnit { get; set; } = null!;

    public virtual ICollection<Knot> Knots { get; set; } = new List<Knot>();
}
