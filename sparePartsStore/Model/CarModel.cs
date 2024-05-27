using sparePartsStore.Model.DPO;
using sparePartsStore.ViewModel;
using System;
using System.Collections.Generic;

namespace sparePartsStore.Model;

public partial class CarModel
{
    public int CarModelId { get; set; }

    public string NameCarModel { get; set; } = null!;

    public int CarBrandId { get; set; }

    public virtual ICollection<Autopart> Autoparts { get; set; } = new List<Autopart>();

    public virtual CarBrand CarBrand { get; set; } = null!;

    // замена string CarBrandName на int CarBrandId
    public CarModel CopyFromCarModelDPO(CarModelDPO carModel)
    {
        ListCarBrandViewModel listCarBrandViewModel = new ListCarBrandViewModel();
        int carBrandId = 0;
        foreach(var name in listCarBrandViewModel.ListCarBrand)
        {
            if (name.NameCarBrand == carModel.CarBrandName)
            {
                carBrandId = name.CarBrandId;
                break;  
            }
        }

        if(carBrandId != 0)
        {
            this.CarModelId = carModel.CarModelId;
            this.NameCarModel = carModel.NameCarModel;
            this.CarBrandId = carBrandId;
        }
        return this;
    }
}
