using sparePartsStore.Model.DPO;
using sparePartsStore.ViewModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace sparePartsStore.Model
{
    public class AutopartDPO : INotifyPropertyChanged
    {
        public int AutopartId { get; set; }

        private int _numberAutopart { get; set; }
        public int NumberAutopart
        {
            get { return _numberAutopart; }
            set { _numberAutopart = value; OnPropertyChanged(nameof(NumberAutopart)); }
        }

        private string _nameAutopart { get; set; }
        public string NameAutopart
        {
            get { return _nameAutopart; }
            set { _nameAutopart = value; OnPropertyChanged(nameof(NameAutopart)); }
        }

        private int _knotId { get; set; }
        public int KnotId
        {
            get { return _knotId; }
            set { _knotId = value; OnPropertyChanged(nameof(KnotId)); }
        }

        private string _nameKnot { get; set; }
        public string NameKnot
        {
            get { return _nameKnot; }
            set { _nameKnot = value; OnPropertyChanged(nameof(NameKnot)); }
        }
        private string _nameUnit { get; set; }
        public string NameUnit
        {
            get { return _nameUnit; }
            set { _nameUnit = value; OnPropertyChanged(nameof(NameUnit)); }
        }

        private int _unitId { get; set; }
        public int UnitId
        {
            get { return _unitId; }
            set { _unitId = value; OnPropertyChanged(nameof(UnitId)); }
        }

        private string _nameCarModel { get; set; }
        public string NameCarModel
        {
            get { return _nameCarModel; }
            set { _nameCarModel = value; OnPropertyChanged(nameof(NameCarModel)); }
        }

        private int _carModelId { get; set; }
        public int CarModelId
        {
            get { return _carModelId; }
            set { _carModelId = value; OnPropertyChanged(nameof(CarModelId)); }
        }

        private string _carBrandName { get; set; } // название марки авто
        public string CarBrandName
        {
            get { return _carBrandName; }
            set { _carBrandName = value; OnPropertyChanged(nameof(CarBrandName)); }
        }

        private int _carBrandId { get; set; } // название марки авто
        public int CarBrandId
        {
            get { return _carBrandId; }
            set { _carBrandId = value; OnPropertyChanged(nameof(CarBrandId)); }
        }

        private int _manufactureId { get; set; }
        public int ManufactureId
        {
            get { return _manufactureId; }
            set { _manufactureId = value; OnPropertyChanged(nameof(ManufactureId)); }
        }
        private string _nameManufacture { get; set; }
        public string NameManufacture
        {
            get { return _nameManufacture; }
            set { _nameManufacture = value; OnPropertyChanged(nameof(NameManufacture)); }
        }

        private decimal _priceSale { get; set; }
        public decimal PriceSale
        {
            get { return _priceSale; }
            set {  _priceSale = value; OnPropertyChanged(nameof(PriceSale)); }
        }

        private int _availableityStock { get; set; }
        public int AvailableityStock
        {
            get { return _availableityStock; }
            set { _availableityStock = value; OnPropertyChanged(nameof(AvailableityStock)); }
        }

        private int _accountId { get; set; }
        public int AccountId
        {
            get { return _accountId; }
            set { _accountId = value; OnPropertyChanged(nameof(AccountId)); }
        }

        private string _moderationStatus { get; set; }
        public string ModerationStatus
        {
            get { return _moderationStatus; }
            set { _moderationStatus = value; OnPropertyChanged(nameof(ModerationStatus)); }
        }

        // замена int AutopartId на string NameAutopart
        public AutopartDPO CopyFromCountry(Autopart autopart)
        {
            AutopartDPO autopartDPO = new AutopartDPO();

            // классы, из которых мы будет получать string данные для замены ID
            ListCarBrandViewModel listCarBrandViewModel = new ListCarBrandViewModel();
            ListCarModelViewModel listCarModelViewModel = new ListCarModelViewModel();
            ListUnitViewModel listUnitViewModel = new ListUnitViewModel();
            ListKnotViewModel listKnotViewModel = new ListKnotViewModel();
            ListCountryViewModel listCountryViewModel = new ListCountryViewModel();
            ListManufactureViewModel listManufactureViewModel = new ListManufactureViewModel();

            string nameCarBrand = string.Empty;
            string nameCarModel = string.Empty;
            string nameUnit = string.Empty;
            string nameKnot = string.Empty;
            string nameCountry = string.Empty;
            string nameManufactureBrand = string.Empty;
            
            // заменяем id - модель и марка авто
            CarModelDPO carModelDPO = listCarModelViewModel.ListCarModelDPO.FirstOrDefault(c => c.CarModelId == autopart.CarModelId);
            if(carModelDPO != null)
            {
                nameCarBrand = carModelDPO.CarBrandName;
                nameCarModel = carModelDPO.NameCarModel;
            }

            // заменяем id - агрегат и узел авто
            KnotDPO knotDPO = listKnotViewModel.ListKnotDPO.FirstOrDefault(c => c.KnotId == autopart.KnotId);
            if(knotDPO != null)
            {
                nameKnot = knotDPO.NameKnot;
                nameUnit = knotDPO.NameUnit;
            }

            // заменяем id - страна и поставщик
            ManufactureDPO manufactureDPO = listManufactureViewModel.ListManufactureDPO.FirstOrDefault(c => c.ManufactureId == autopart.ManufactureId);
            if(manufactureDPO != null)
            {
                nameManufactureBrand = manufactureDPO.NameManufacture;
                nameCountry = manufactureDPO.NameCountry;
            }

            if(nameCarBrand != string.Empty && nameCarModel != string.Empty && nameKnot != string.Empty && nameUnit != string.Empty && nameManufactureBrand != string.Empty && nameCountry != string.Empty)
            {
                autopartDPO.AutopartId = autopart.AutopartId;
                autopartDPO.NumberAutopart = autopart.NumberAutopart;
                autopartDPO.NameAutopart = autopart.NameAutopart;

                autopartDPO.NameCarModel = nameCarModel;
                autopartDPO.NameKnot = nameKnot;
                autopartDPO.NameManufacture = nameManufactureBrand;

                autopartDPO.CarModelId = autopart.CarModelId;
                autopartDPO.KnotId = autopart.KnotId;
                autopart.ManufactureId = autopart.ManufactureId;

                autopartDPO.PriceSale = autopart.PriceSale;
                autopartDPO.AvailableityStock = autopart.AvailableityStock;
                autopartDPO.AccountId = autopart.AccountId;
                autopartDPO.ModerationStatus = autopart.ModerationStatus;
            }

            return autopartDPO;

        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
