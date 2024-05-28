using sparePartsStore.Model.DPO;
using sparePartsStore.ViewModel;
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

    // замена string NameUnit на int UnitId
    public Knot CopyFromKnotDPO(KnotDPO knotDPO)
    {
        Knot knot = new Knot();
        ListUnitViewModel listUnitViewModel = new ListUnitViewModel();
        int knotId = 0;

        // получаем объект из БД KnotId для замены  NameUnit на UnitId
        using (SparePartsStoreContext context = new SparePartsStoreContext())
        {
            List<Unit> units = context.Units.ToList();
            Unit unitSearch = units.FirstOrDefault(k => k.UnitId == knotDPO.UnitId);

            if(unitSearch != null)
            {
                if(unitSearch.UnitId == knotDPO.UnitId)
                {
                    knotId = unitSearch.UnitId; // присваиваем полученные ID
                }
            }
        }
        if(knotId != 0)
        {
            knot.KnotId = knotDPO.KnotId;
            knot.NameKnot = knotDPO.NameKnot;
            knot.UnitId = knotId;
        }

        return knot;
    }

}
