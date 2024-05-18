using System;
using System.Collections.Generic;

namespace sparePartsStore.Model;

public partial class Knot
{
    public int KnotId { get; set; }

    public string NameKnot { get; set; } = null!;

    public int UnitId { get; set; }

    public virtual ICollection<Autopart> Autoparts { get; set; } = new List<Autopart>();

    public virtual Unit Unit { get; set; } = null!;
}
