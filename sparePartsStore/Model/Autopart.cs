using System;
using System.Collections.Generic;

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
    public Autopart CopyFromCountryDPO(AutopartDPO autopartDPO)
    {
        Autopart autopart = new Autopart();

        int knotId = 0;
        int carModelId = 0;
        int manufactureId = 0;

        // получаем объект из БД для замены NameAutopart на int AutopartId
        using (SparePartsStoreContext context = new SparePartsStoreContext())
        {
            List<Knot> knots = context.Knots.ToList();
            List<CarModel> carModels = context.CarModels.ToList();
            List<Manufacture> manufactures = context.Manufactures.ToList();

            Knot knot = knots.FirstOrDefault(k => k.KnotId == autopartDPO.AutopartId);
            if (knot != null)
            {
                knotId = knot.KnotId; // присваиваем полученные ID
            }

            CarModel carModel = carModels.FirstOrDefault(c => c.CarModelId == autopartDPO.CarModelId);
            if (carModel != null)
            {
                carModelId = carModel.CarModelId;
            }

            Manufacture manufacture = manufactures.FirstOrDefault(m => m.ManufactureId == autopartDPO.ManufactureId);
            if(manufacture != null)
            {
                manufactureId = manufacture.ManufactureId;
            }

            if(knotId != 0 && carModelId != 0 && manufactureId != 0)
            {
                autopart.AutopartId = autopartDPO.AutopartId;
                autopart.NumberAutopart = autopartDPO.NumberAutopart;
                autopart.NameAutopart = autopartDPO.NameAutopart;
                autopart.KnotId = knotId;
                autopart.CarModelId = carModelId;
                autopart.ManufactureId = manufactureId;
                autopart.PriceSale = autopartDPO.PriceSale;
                autopart.AvailableityStock = autopartDPO.AvailableityStock;
                autopart.AccountId = autopartDPO.AccountId;
            }

            return autopart;
        }

    }

}
