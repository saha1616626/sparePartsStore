using sparePartsStore.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Packaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace sparePartsStore.Model.DPO
{
    public class CarModelDPO : INotifyPropertyChanged // модели авто
    {
        public int CarModelId { get; set; }
        private string _nameCarModel {  get; set; }
        public string NameCarModel
        {
            get { return _nameCarModel; }
            set { _nameCarModel = value; OnPropertyChanged(nameof(NameCarModel)); }
        }
        private string _carBrandName { get; set; } // название марки авто
        public string CarBrandName  
        {
            get { return _carBrandName; }
            set { _carBrandName = value; OnPropertyChanged(nameof(CarBrandName)); }
        }

        // замена int CarBrandId на string CarBrandName
        public CarModelDPO CopyFromCarModel(CarModel carModel)
        {
            CarModelDPO carModelDPO = new CarModelDPO();
            ListCarBrandViewModel listCarBrandViewModel = new ListCarBrandViewModel();
            string nameCarRand = string.Empty;
            foreach(var name in listCarBrandViewModel.ListCarBrand)
            {
                if(name.CarBrandId == carModel.CarBrandId)
                {
                    nameCarRand = name.NameCarBrand;
                    break;
                }
            }

            if(nameCarRand != string.Empty)
            {
                carModelDPO.CarModelId = carModel.CarModelId;
                carModelDPO.NameCarModel = carModel.NameCarModel;
                carModelDPO.CarBrandName = nameCarRand;
            }

            return carModelDPO;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
