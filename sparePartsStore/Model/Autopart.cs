using System;
using System.Collections.Generic;
using sparePartsStore.Model.DPO;

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

    // замена string NameAutopart на int AutopartId
    public Autopart CopyFromAutopartDPO(AutopartDPO autopartDPO)
    {
        Autopart autopart = new Autopart();

        autopart.AutopartId = autopartDPO.AutopartId;
        autopart.NumberAutopart = autopartDPO.NumberAutopart;
        autopart.NameAutopart = autopartDPO.NameAutopart;
        autopart.KnotId = autopartDPO.KnotId;
        autopart.CarModelId = autopartDPO.CarModelId;
        autopart.ManufactureId = autopartDPO.ManufactureId;
        autopart.PriceSale = autopartDPO.PriceSale;
        autopart.AvailableityStock = autopartDPO.AvailableityStock;
        autopart.AccountId = autopartDPO.AccountId;
        autopart.ModerationStatus = autopartDPO.ModerationStatus;

        return autopart;
    }

}
