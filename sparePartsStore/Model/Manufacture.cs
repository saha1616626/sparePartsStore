using sparePartsStore.Model.DPO;
using sparePartsStore.ViewModel;
using System;
using System.Collections.Generic;

namespace sparePartsStore.Model;

public partial class Manufacture
{
    public int ManufactureId { get; set; }

    public string NameManufacture { get; set; } = null!;

    public int CountryId { get; set; }

    public virtual ICollection<Autopart> Autoparts { get; set; } = new List<Autopart>();

    public virtual Country Country { get; set; } = null!;

    // замена string NameCountry на int CountryId
    public Manufacture CopyFromCountryDPO(ManufactureDPO manufactureDPO)
    {
        Manufacture manufacture = new Manufacture();
        ListCountryViewModel listCountryViewModel = new ListCountryViewModel();
        int manufactureId = 0;

        // получаем объект из БД CountryId для замены NameCountry на CountryId
        using (SparePartsStoreContext context = new SparePartsStoreContext())
        {
            List<Country> country = context.Countries.ToList();
            Country CountrySearch = country.FirstOrDefault(k => k.CountryId == manufactureDPO.CountryId);

            if (CountrySearch != null)
            {
                if (CountrySearch.CountryId == manufactureDPO.CountryId)
                {
                    manufactureId = CountrySearch.CountryId; // присваиваем полученные ID
                }
            }
        }
        if (manufactureId != 0)
        {
            manufacture.CountryId = manufactureId;
            manufacture.ManufactureId = manufactureDPO.ManufactureId;
            manufacture.NameManufacture = manufactureDPO.NameManufacture;
        }

        return manufacture;
    }
}
