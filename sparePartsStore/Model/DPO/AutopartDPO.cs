using sparePartsStore.ViewModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace sparePartsStore.Model.DPO
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
            set { _priceSale = value; OnPropertyChanged(nameof(PriceSale)); }
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


        private string _nameOrganization { get; set; }
        public string NameOrganization
        {
            get { return _nameOrganization; }
            set { _nameOrganization = value; OnPropertyChanged(nameof(NameOrganization)); }
        }

        private string _moderationStatus { get; set; }
        public string ModerationStatus
        {
            get { return _moderationStatus; }
            set { _moderationStatus = value; OnPropertyChanged(nameof(ModerationStatus)); }
        }

        // замена int AutopartId на string NameAutopart
        public AutopartDPO CopyFromAutopart(Autopart autopart)
        {
            AutopartDPO autopartDPO = new AutopartDPO();

            autopartDPO.NameAutopart = autopart.NameAutopart;
            autopartDPO.AutopartId = autopart.AutopartId;
            autopartDPO.NumberAutopart = autopart.NumberAutopart;

            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<CarBrand> carBrands = context.CarBrands.ToList();
                List<CarModel> carModels = context.CarModels.ToList();
                List<Unit> units = context.Units.ToList();
                List<Knot> knots = context.Knots.ToList();
                List<Country> countries = context.Countries.ToList();
                List<Manufacture> manufactures = context.Manufactures.ToList();
                List<Account> accounts = context.Accounts.ToList();

                // заменяем марку и модель
                CarModel carModel = carModels.FirstOrDefault(c => c.CarModelId == autopart.CarModelId);
                if(carModel != null)
                {
                    autopartDPO.CarModelId = carModel.CarModelId;
                    autopartDPO.NameCarModel = carModel.NameCarModel;

                    CarBrand carBrand = carBrands.FirstOrDefault(c => c.CarBrandId == carModel.CarBrandId);
                    if( carBrand != null)
                    {
                        autopartDPO.CarBrandId = carBrand.CarBrandId;
                        autopartDPO.CarBrandName = carBrand.NameCarBrand;
                    }
                }

                // заменяем агрегат и узел
                Knot knot = knots.FirstOrDefault(k => k.KnotId == autopart.KnotId);
                if(knot != null)
                {
                    autopartDPO.KnotId = knot.KnotId;
                    autopartDPO.NameKnot = knot.NameKnot;

                    Unit unit = units.FirstOrDefault(u => u.UnitId == knot.UnitId);
                    if(unit != null)
                    {
                        autopartDPO.UnitId = unit.UnitId;
                        autopartDPO.NameUnit = unit.NameUnit;
                    }
                }

                // заменяем страну и производителя
                Manufacture manufacture = manufactures.FirstOrDefault(m => m.ManufactureId == autopart.ManufactureId);
                if(manufacture != null)
                {
                    autopartDPO.ManufactureId = manufacture.ManufactureId;
                    autopartDPO.NameManufacture = manufacture.NameManufacture;

                    Country country = countries.FirstOrDefault(c => c.CountryId == manufacture.CountryId);
                    if(country != null)
                    {
                        autopartDPO.CountryId = country.CountryId;
                        autopartDPO.NameCountry = country.NameCountry;
                    }
                }

                // заменяем акаунт
                Account account = accounts.FirstOrDefault(a => a.AccountId == autopart.AccountId);
                if(account != null)
                {
                    autopartDPO.AccountId = account.AccountId;
                    autopartDPO.NameOrganization = account.NameOrganization;
                }

            }

            autopartDPO.PriceSale = autopart.PriceSale;
            autopartDPO.AvailableityStock = autopart.AvailableityStock;
            autopartDPO.ModerationStatus = autopart.ModerationStatus;
            
            return autopartDPO;

        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
