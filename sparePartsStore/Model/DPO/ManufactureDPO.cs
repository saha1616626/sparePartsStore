using sparePartsStore.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace sparePartsStore.Model.DPO
{
    public class ManufactureDPO : INotifyPropertyChanged
    {
        public int ManufactureId { get; set; }

        private string _nameManufacture { get; set; }
        public string NameManufacture
        {
            get { return _nameManufacture; }
            set { _nameManufacture = value; OnPropertyChanged(nameof(NameManufacture)); }
        }

        private int _countryId { get; set; }
        public int CountryId
        {
            get { return _countryId; }
            set { _countryId = value; OnPropertyChanged(nameof(CountryId)); }
        }
        private string _nameCountry { get; set; }
        public string NameCountry
        {
            get { return _nameCountry; }
            set { _nameCountry = value; OnPropertyChanged(nameof(NameCountry)); }
        }

        // замена int CountryId на string NameCountry
        public ManufactureDPO CopyFromCountry(Manufacture manufacture)
        {
            ManufactureDPO manufactureDPO = new ManufactureDPO();
            ListCountryViewModel listCountryViewModel = new ListCountryViewModel();
            string nameCountry = string.Empty;
            foreach (var name in listCountryViewModel.ListCountry)
            {
                if (name.CountryId == manufacture.CountryId)
                {
                    nameCountry = name.NameCountry;
                    break;
                }
            }

            if (nameCountry != string.Empty)
            {
                manufactureDPO.ManufactureId = manufacture.ManufactureId;
                manufactureDPO.NameCountry = nameCountry;
                manufactureDPO.NameManufacture = manufacture.NameManufacture;
            }

            return manufactureDPO;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
