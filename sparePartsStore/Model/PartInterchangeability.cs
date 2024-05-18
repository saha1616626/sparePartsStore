using System;
using System.Collections.Generic;

namespace sparePartsStore.Model;

public partial class PartInterchangeability
{
    public int PartInterchangeabilityId { get; set; }

    public int AutoPartId { get; set; }

    public int InterchangeableDetailId { get; set; }

    public virtual Autopart AutoPart { get; set; } = null!;

    public virtual Autopart InterchangeableDetail { get; set; } = null!;
}
