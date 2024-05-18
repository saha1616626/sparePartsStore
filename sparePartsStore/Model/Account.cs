using System;
using System.Collections.Generic;

namespace sparePartsStore.Model;

public partial class Account
{
    public int AccountId { get; set; }

    public string AccountLogin { get; set; } = null!;

    public string AccountPassword { get; set; } = null!;

    public int AccountRoleId { get; set; }

    public int NameOrganization { get; set; }

    public string Inn { get; set; } = null!;

    public string? Ogrn { get; set; }

    public string? Ogrnip { get; set; }

    public string? Kpp { get; set; }

    public virtual AccountRole AccountRole { get; set; } = null!;

    public virtual ICollection<Autopart> Autoparts { get; set; } = new List<Autopart>();
}
