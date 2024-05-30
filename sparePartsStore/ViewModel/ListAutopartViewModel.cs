using sparePartsStore.Model;
using sparePartsStore.Model.DPO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace sparePartsStore.ViewModel
{
    public class ListAutopartViewModel : INotifyPropertyChanged
    {
        AuthorizationViewModel _authorizationViewModel = new AuthorizationViewModel(); // работа с авторизацией
        public ListAutopartViewModel()
        {
            //чтение данных из БД
            ListAutopartRead = GetListAutopart();

            //вывод данных в таблицу
            ListAutopartDPO = LoadAutopartBD();
        }

        // выбранные данные из таблицы
        private AutopartDPO _selectedAutopart { get; set; }
        public AutopartDPO SelectedAutopart
        {
            get { return _selectedAutopart; }
            set
            {
                _selectedAutopart = value;
                OnPropertyChanged(nameof(SelectedAutopart));
                OnPropertyChanged(nameof(IsWorkButtonEnable));
            }
        }

        // отображение кнопки редактировать или удалить при выборе данных в таблице
        private bool _isWorkButtonEnable;
        public bool IsWorkButtonEnable
        {
            get { return _selectedAutopart != null; } // если нажто поле в таблице
            set { _isWorkButtonEnable = value; OnPropertyChanged(nameof(IsWorkButtonEnable)); }
        }

        // список отображения запчастей на экране
        private ObservableCollection<AutopartDPO> _listAutopartDPOO { get; set; }
        public ObservableCollection<AutopartDPO> ListAutopartDPO
        {
            get { return _listAutopartDPOO; }
            set { _listAutopartDPOO = value; OnPropertyChanged(nameof(ListAutopartDPO)); }
        }

        // метод получения списка запчастей для вывода на экран
        private ObservableCollection<AutopartDPO> LoadAutopartBD()
        {
            // создаём преобразованный список
            ObservableCollection<AutopartDPO> autoparts = new ObservableCollection<AutopartDPO>();

            // экз для преобразования данных
            AutopartDPO autopartDPO = new AutopartDPO();

            if (ListAutopartRead != null)
            {
                // проходимся по списку ListKnotRead для заполнения списка ListKnotDPO
                foreach (var autopart in ListAutopartRead.ToList())
                {
                    autoparts.Add(autopartDPO.CopyFromAutopart(autopart)); // добавляем данные 
                }
            }

            return autoparts;
        }

        // коллекция считанная из БД
        public ObservableCollection<Autopart> ListAutopartRead { get; set; } = new ObservableCollection<Autopart>();

        // метод для получения коллекции запчастей
        private ObservableCollection<Autopart> GetListAutopart()
        {
            try
            {
                ObservableCollection<Autopart> autoparts = new ObservableCollection<Autopart>(); // список полученных данных из БД
                using (SparePartsStoreContext context = new SparePartsStoreContext())
                {
                    List<Autopart> autopartBD = context.Autoparts.ToList(); // получаем список узлов авто
                    if (autopartBD != null)
                    {
                        // для каждого пользователя можно отображать только свои запчасти
                        string role = _authorizationViewModel.CheckingUserRole();
                        if (role == "Администратор"  || role == "Магазин") // получают весь список
                        {
                            // копируем данные в список из БД
                            foreach (var autopartItem in autopartBD)
                            {
                                Autopart autopart = new Autopart();
                                autopart.AutopartId = autopartItem.AutopartId;
                                autopart.NumberAutopart = autopartItem.NumberAutopart;
                                autopart.NameAutopart = autopartItem.NameAutopart;
                                autopart.KnotId = autopartItem.KnotId;
                                autopart.CarModelId = autopartItem.CarModelId;
                                autopart.ManufactureId = autopartItem.ManufactureId;
                                autopart.PriceSale = autopartItem.PriceSale;
                                autopart.AvailableityStock = autopartItem.AvailableityStock;
                                autopart.AccountId = autopartItem.AccountId;
                                autopart.ModerationStatus = autopartItem.ModerationStatus;

                                // добавляем в список
                                autoparts.Add(autopart);
                            }
                        }
                        else // частично (поставщики)
                        {
                            int idUser = _authorizationViewModel.weGetIdUser();
                            if(idUser != 0)
                            {
                                // копируем данные в список из БД
                                foreach (var autopartItem in autopartBD)
                                {
                                    if (idUser == autopartItem.AccountId)
                                    {
                                        Autopart autopart = new Autopart();
                                        autopart.AutopartId = autopartItem.AutopartId;
                                        autopart.NumberAutopart = autopartItem.NumberAutopart;
                                        autopart.NameAutopart = autopartItem.NameAutopart;
                                        autopart.KnotId = autopartItem.KnotId;
                                        autopart.CarModelId = autopartItem.CarModelId;
                                        autopart.ManufactureId = autopartItem.ManufactureId;
                                        autopart.PriceSale = autopartItem.PriceSale;
                                        autopart.AvailableityStock = autopartItem.AvailableityStock;
                                        autopart.AccountId = autopartItem.AccountId;
                                        autopart.ModerationStatus = autopartItem.ModerationStatus;


                                        // добавляем в список
                                        autoparts.Add(autopart);
                                    }

                                }
                            }
                        }
                        
                    }
                }
                return autoparts;
                throw new Exception("Ошибка работы БД!");
            }
            catch (Exception ex)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show(ex.ToString(), "Внимание!", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                return null;
            }
        }

        // передаём получаенные выбранные данные из таблицы
        public AutopartDPO TransmitionAutopart()
        {
            AutopartDPO autopartDPO = new AutopartDPO();
            autopartDPO = (AutopartDPO)SelectedAutopart;
            return autopartDPO;
        }

        // получаем данные для ComBox CarBrand
        public List<CarBrand> GetCarBrandOnComboBox()
        {
            // список, котроый будет в себе хранить значения comBox
            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<CarBrand> carBrands = context.CarBrands.ToList(); // получили список марок из БД
                List<CarBrand> carBrandList = new List<CarBrand>(); // список для ComBox
                // записываем массив carBrandList
                foreach (CarBrand carBrandtem in carBrands)
                {
                    // добавляем данные в список
                    carBrandList.Add(carBrandtem);
                }
                // возвращаем массив обратно
                return carBrandList;
            }
        }

        // получаем данные для ComBox CarModel
        public List<CarModel> GetCarModelOnComboBox()
        {
            // список, котроый будет в себе хранить значения comBox
            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<CarModel> carModels = context.CarModels.ToList(); // получили список моделей из БД
                List<CarModel> carModelList = new List<CarModel>(); // список для ComBox
                // записываем массив carModelList
                foreach (CarModel сarModeltem in carModels)
                {
                    // добавляем данные в список
                    carModelList.Add(сarModeltem);
                }
                // возвращаем массив обратно
                return carModelList;
            }
        }

        // получаем данные для ComBox Unit
        public List<Unit> GetUnitOnComboBox()
        {
            // список, котроый будет в себе хранить значения comBox
            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<Unit> units = context.Units.ToList(); // получили список агрегатов из БД
                List<Unit> unitList = new List<Unit>(); // список для ComBox
                // записываем массив unitList
                foreach (Unit unitItem in units)
                {
                    // добавляем данные в список
                    unitList.Add(unitItem);
                }
                // возвращаем массив обратно
                return unitList;
            }
        }

        // получаем данные для ComBox Knot
        public List<Knot> GetKnotOnComboBox()
        {
            // список, котроый будет в себе хранить значения comBox
            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<Knot> knots = context.Knots.ToList(); // получили список узлов из БД
                List<Knot> knotList = new List<Knot>(); // список для ComBox
                // записываем массив knotList
                foreach (Knot knotItem in knots)
                {
                    // добавляем данные в список
                    knotList.Add(knotItem);
                }
                // возвращаем массив обратно
                return knotList;
            }
        }

        // получаем данные для ComBox Country
        public ObservableCollection<Country> GetCountryOnComboBox()
        {
            // список, котроый будет в себе хранить значения comBox
            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<Country> сountries = context.Countries.ToList(); // получили список стран из БД
                ObservableCollection<Country> countryList = new ObservableCollection<Country>(); // список для ComBox
                // записываем массив countryList
                foreach (Country countryItem in сountries)
                {
                    // добавляем данные в список
                    countryList.Add(countryItem);
                }
                // возвращаем массив обратно
                return countryList;
            }
        }

        // получаем данные для ComBox Manufacture
        public ObservableCollection<Manufacture> GetManufactureOnComboBox()
        {
            // список, котроый будет в себе хранить значения comBox
            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<Manufacture> manufacturies = context.Manufactures.ToList(); // получили список производителей из БД
                ObservableCollection<Manufacture> manufactureList = new ObservableCollection<Manufacture>(); // список для ComBox
                // записываем массив manufactureList
                foreach (Manufacture manufactureItem in manufacturies)
                {
                    // добавляем данные в список
                    manufactureList.Add(manufactureItem);
                }
                // возвращаем массив обратно
                return manufactureList;
            }
        }

        // получаем данные для ComBox Manufacture
        public ObservableCollection<string> GetModerationStatusOnComboBox()
        {
            // список, котроый будет в себе хранить значения comBox
            ObservableCollection<string> ModerationStatus = new ObservableCollection<string>();
            ModerationStatus.Add("В обработке");
            ModerationStatus.Add("Отображается");
            ModerationStatus.Add("Не отображается");
            ModerationStatus.Add("Отклонён");

            return ModerationStatus;
        }

        // переменная, которая хранит текущие данные в поле у PageWorkAutopart
        public TextBox NameAutopartInput { get; set; }

        // проверяем, есть ли совпадение данных перед добавлением
        public bool CheckingForMatchDB(AutopartDPO autopartDPO)
        {
            bool noCoincidence = true; // по умолчанию нет совпадения

            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<Autopart> autoparts = context.Autoparts.ToList();
                List<Autopart> newAutopart = new List<Autopart>();

                Autopart autopart = new Autopart();
                autopart = autopart.CopyFromAutopartDPO(autopartDPO);

                if(autoparts != null)
                {
                    // список запчастей по выбранному пользователю и другим характеристикам
                    newAutopart = autoparts.Where(a => a.CarModelId == autopart.CarModelId && a.Knot == autopart.Knot && a.ManufactureId == autopart.ManufactureId && a.AccountId == autopart.AccountId).ToList();
                }

                // проверка наличия запчасти у определенного пользователя в приложении
                if(newAutopart != null)
                {
                    noCoincidence = !newAutopart.Any(num => num.NameAutopart.ToLower() == NameAutopartInput.Text.ToLower());
                }
            }
            return noCoincidence;
        }

        // если пользователь редактирует данные проверяем, что он не изменяет данные уже существующие в таблице
        public bool CheckingForMatchEditDB(AutopartDPO autopartDPO)
        {

            bool noCoincidence = false; // по умолчанию совпадение не найдено

            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<Autopart> autoparts = context.Autoparts.ToList();
                List<Autopart> newAutopart = new List<Autopart>();

                Autopart autopart = new Autopart();
                autopart = autopart.CopyFromAutopartDPO(autopartDPO);

                if (autoparts != null)
                {
                    // список запчастей по выбранному пользователю и другим характеристикам
                    newAutopart = autoparts.Where(a => a.CarModelId == autopart.CarModelId && a.Knot == autopart.Knot && a.ManufactureId == autopart.ManufactureId && a.AccountId == autopart.AccountId).ToList();
                }

                
                if (newAutopart != null)
                {
                    foreach(var a in newAutopart)
                    {
                        if(a.AutopartId == autopartDPO.AutopartId)
                        {
                            continue;
                        }

                        if(a.NameAutopart.ToLower().Trim() == NameAutopartInput.Text.ToLower().Trim()) // если нашли совпадение в таблице
                        {
                            noCoincidence = true;
                        }
                    }
                }
            }
            return noCoincidence;
        }

        // свойство поля для ввода названия запчасти
        private TextBox _outNameAutopart;
        public TextBox OutNameAutopart
        {
            get { return _outNameAutopart; }
            set
            {
                _outNameAutopart = value;
                OnPropertyChanged(nameof(OutNameAutopart));
            }
        }

        // обновляем отображение таблицы после удаления данных
        public void UpdateTabel()
        {
            //чтение данных из БД
            ListAutopartRead = GetListAutopart();

            //вывод данных в таблицу
            ListAutopartDPO = LoadAutopartBD();
        }

        // список для фильтров таблицы
        public ObservableCollection<Autopart> ListSearch { get; set; } = new ObservableCollection<Autopart>();

        // поиск запчастей
        public void HandlerTextBoxChanged(string nameAutopart)
        {
            if (nameAutopart.Trim() != "")
            {
                ListSearch = GetListAutopart(); // получаем актуальные данные из БД
                // создаём список с поиском по введенным данным в таблице
                var resAutopart = ListSearch.Where(num => num.NameAutopart.ToLower().Contains(nameAutopart.ToLower())).ToList();

                // выведет всё, если ничего не найдено
                //if (resAutopart.Count > 0)
                //{
                //    ListAutopartDPO.Clear();// очищаем основной список
                //    // вносим актуальные данные основного списка
                //    foreach (var autopartItem in resAutopart)
                //    {
                //        AutopartDPO autopartDPO = new AutopartDPO();
                //        autopartDPO = autopartDPO.CopyFromAutopart(autopartItem);
                //        ListAutopartDPO.Add(autopartDPO); // добавляем данные
                //    }
                //}

                ListAutopartDPO.Clear();// очищаем основной список
                                           // вносим актуальные данные основного списка
                foreach (var autopartItem in resAutopart)
                {
                    AutopartDPO autopartDPO = new AutopartDPO();
                    autopartDPO = autopartDPO.CopyFromAutopart(autopartItem);
                    ListAutopartDPO.Add(autopartDPO); // добавляем данные
                }
            }

            if (nameAutopart.Trim() == "")
            {
                ListAutopartDPO.Clear();// очищаем основной список
                ListSearch.Clear(); // очищаем доп список
                ListSearch = GetListAutopart(); // получаем актуальные данные из БД
                foreach (var Item in ListSearch)
                {
                    AutopartDPO autopartDPO = new AutopartDPO();
                    autopartDPO = autopartDPO.CopyFromAutopart(Item);
                    ListAutopartDPO.Add(autopartDPO);
                }
            }
        }

        // comboBox
        private CarBrand _selectedCarBrand;
        public CarBrand SelectedCarBrand
        {
            get { return _selectedCarBrand; }
            set
            {
                _selectedCarBrand = value;
                OnPropertyChanged(nameof(SelectedCarBrand));
            }
        }

        private CarModel _selectedCarModel;
        public CarModel SelectedCarModel
        {
            get { return _selectedCarModel; }
            set
            {
                _selectedCarModel = value;
                OnPropertyChanged(nameof(SelectedCarModel));
            }
        }

        private Unit _selectedUnit;
        public Unit SelectedUnit
        {
            get { return _selectedUnit; }
            set
            {
                _selectedUnit = value;
                OnPropertyChanged(nameof(SelectedUnit));
            }
        }

        private Knot _selectedKnot;
        public Knot SelectedKnot
        {
            get { return _selectedKnot; }
            set
            {
                _selectedKnot = value;
                OnPropertyChanged(nameof(SelectedKnot));
            }
        }

        #region PropertyComboBoxCountry

        // заголовок страны
        private Country _selectedCountryName;
        public Country SelectedCountryName
        {
            get { return _selectedCountryName; }
            set
            {
                _selectedCountryName = value;
                OnPropertyChanged(nameof(SelectedCountryName));
            }
        }


        // выбранный элемент страна
        private Country _selectedCountry;
        public Country SelectedCountry
        {
            get { return _selectedCountry; }
            set
            {
                _selectedCountry = value;
                OnPropertyChanged(nameof(SelectedCountry));

                // при выборе страны, меняем список фабрики
                EditManufactureOfCountry();
            }
        }

        // вывод списка списка "страна"
        private ObservableCollection<Country> _nameCountryComboBoxItems;
        public ObservableCollection<Country> NameCountryComboBoxItems
        {
            get { return _nameCountryComboBoxItems = GetCountryOnComboBox(); }
            set
            {
                _nameCountryComboBoxItems = value;
                OnPropertyChanged(nameof(NameCountryComboBoxItems));
            }
        }

        // метод изменения списка comboBox производителя, при выборе comboBox элемента страны
        private void EditManufactureOfCountry()
        {
            ObservableCollection<Manufacture> manufacture = new ObservableCollection<Manufacture>();

            NameManufactureComboBoxItems = manufacture;
        }

        #endregion

        #region PropertyComboBoxManufacture

        // вывод списка списка "производитель"
        private ObservableCollection<Manufacture> _nameManufactureComboBoxItems;
        public ObservableCollection<Manufacture> NameManufactureComboBoxItems
        {
            get { return _nameManufactureComboBoxItems = GetManufactureOnComboBox(); }
            set
            {
                _nameManufactureComboBoxItems = value;
                OnPropertyChanged(nameof(NameManufactureComboBoxItems));
            }
        }

        // выбранный элемент "производители"
        private Manufacture _selectedManufacture;
        public Manufacture SelectedManufacture
        {
            get { return _selectedManufacture; }
            set
            {
                _selectedManufacture = value;
                OnPropertyChanged(nameof(SelectedManufacture));

                // вызываем метод который автоматически подставляет нужную страну, относительно выбранной фабрики
                EditCountry();
            }
        }

        // метод подстановки сраны относительно фабрики
        private void EditCountry()
        {
            // подбираем страну для заголовка при выборе нужной фабрики
            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<Country> countries = context.Countries.ToList();
                // список стран
                ObservableCollection<Country> countriesUP = new ObservableCollection<Country>(countries);
                //отображаем список в comboBox
                NameCountryComboBoxItems = countriesUP;

                // находим элемент страны, который соответствует заданной фабрики
                Country country = countries.FirstOrDefault(c => c.CountryId == SelectedManufacture.CountryId);
                // выводим найденную страну в заголовок comboBox
                SelectedCountryName = country;
            }
        }

        #endregion

        // выбранный элемент списка "статус отображения товара"
        private string _selectedModerationStatus;
        public string SelectedModerationStatus
        {
            get { return _selectedModerationStatus; }
            set
            {
                _selectedModerationStatus = value;
                OnPropertyChanged(nameof(SelectedModerationStatus));
            }
        }

        // вывод списка "статусов заказов"
        private ObservableCollection<string> _moderationStatusComboBoxItems;
        public ObservableCollection<string> ModerationStatusComboBoxItems
        {
            get { return _moderationStatusComboBoxItems = GetModerationStatusOnComboBox(); }
            set
            {
                _moderationStatusComboBoxItems = value;
                OnPropertyChanged(nameof(ModerationStatusComboBoxItems));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
