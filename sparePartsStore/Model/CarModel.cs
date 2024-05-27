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
        ListCarModelViewModel listCarModelViewModel = new ListCarModelViewModel();
        int carModelId = 0;
        foreach(var name in listCarModelViewModel.ListCarModelDPO)
        {
            if (name.CarModelId == carModel.CarModelId)
            {
                carModelId = name.CarModelId;
                break;  
            }
        }

        if(carModelId != 0)
        {
            this.CarModelId = carModel.CarModelId;
            this.NameCarModel = carModel.NameCarModel;
            this.CarBrandId = carModelId;
        }
        return this;
    }
}
