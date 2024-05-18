using System;
using System.Collections.Generic;

namespace sparePartsStore.Model;

public partial class AccountRole
{
    public int AccountRoleId { get; set; }

    public string AccountNameRole { get; set; } = null!;

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
