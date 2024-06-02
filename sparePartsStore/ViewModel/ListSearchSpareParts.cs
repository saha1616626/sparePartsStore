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

namespace sparePartsStore.ViewModel
{
    public class ListSearchSpareParts : INotifyPropertyChanged
    {
        AuthorizationViewModel _authorizationViewModel = new AuthorizationViewModel(); // работа с авторизацией
        public ListSearchSpareParts()
        {
            //чтение данных из БД
            ListAutopartRead = GetListAutopart();

            //вывод данных в таблицу
            ListAutopartDPO = LoadAutopartBD();
        }

        // работа со списком
        #region dataList

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
                        if (role == "Администратор" || role == "Магазин") // получают весь список
                        {
                            // копируем данные в список из БД
                            autoparts = new ObservableCollection<Autopart>(autopartBD.Where(a => a.ModerationStatus == "Отображается"));
                        }
                        else // частично (поставщики)
                        {
                            int idUser = _authorizationViewModel.weGetIdUser();
                            if (idUser != 0)
                            {
                                // копируем данные в список из БД
                                autoparts = new ObservableCollection<Autopart>(autopartBD.Where(a => a.ModerationStatus == "Отображается" && a.AccountId == idUser));
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

        #endregion

        #region Fields

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

        // поле поиска
        private string Search { get; set; }

        // фильруем данные при вводе в текствое поле
        public void HandlerTextBoxChanged(string nameAutoParts)
        {
            Search = nameAutoParts; // передали измененный текст

            SearchFilter();
        }

        #endregion

        // свойства COMBOBOX

        #region Property

        #region Method

        // фильтруем данные поиска
        private void SearchFilter()
        {
            // очищаем список
            ListAutopartDPO.Clear();

            // перезаписываем список
            ListAutopartDPO = LoadAutopartBD();

            // новая коллекция
            List<AutopartDPO> autopartDPOs = new List<AutopartDPO>();
            // записываем основную коллекцию
            foreach (AutopartDPO item in ListAutopartDPO)
            {
                autopartDPOs.Add(item);
            }

            if (SelectedCarBrand != null)
            {
                autopartDPOs = autopartDPOs.Where(a => a.CarBrandId == SelectedCarBrand.CarBrandId).ToList();
            }
            if (SelectedCarModel != null)
            {
                autopartDPOs = autopartDPOs.Where(a => a.CarModelId == SelectedCarModel.CarModelId).ToList();
            }
            if (SelectedUnit != null)
            {
                autopartDPOs = autopartDPOs.Where(a => a.UnitId == SelectedUnit.UnitId).ToList();
            }
            if (SelectedKnot != null)
            {
                autopartDPOs = autopartDPOs.Where(a => a.KnotId == SelectedKnot.KnotId).ToList();
            }
            if (Search != null)
            {
                autopartDPOs = autopartDPOs.Where(a => a.NameAutopart.ToLower().Contains(Search.ToLower())).ToList();
            }

            // очищаем список
            ListAutopartDPO.Clear();
            // записываем основную коллекцию
            foreach (AutopartDPO item in autopartDPOs)
            {
                ListAutopartDPO.Add(item);
            }

        }

        #endregion

        #region CarBrand

        // выбрання марка авто
        private CarBrand _selectedCarBrand;
        public CarBrand SelectedCarBrand
        {
            get { return _selectedCarBrand; }
            set
            {
                _selectedCarBrand = value;
                OnPropertyChanged(nameof(SelectedCarBrand));
                EditCarModel();
            }
        }

        // список марок авто
        private ObservableCollection<CarBrand> _nameCarBrandComboBoxItems;
        public ObservableCollection<CarBrand> NameCarBrandComboBoxItems
        {
            get { return _nameCarBrandComboBoxItems; }
            set
            {
                _nameCarBrandComboBoxItems = value;
                OnPropertyChanged(nameof(NameCarBrandComboBoxItems));
            }
        }

        public ObservableCollection<CarBrand> GetCarBrandOnComboBox()
        {
            // список, котроый будет в себе хранить значения comBox
            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<CarBrand> carBrands = context.CarBrands.ToList(); // получили список марок из БД
                ObservableCollection<CarBrand> carBrandList = new ObservableCollection<CarBrand>(); // список для ComBox
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

        public bool editCarModel = false; // true - если мы изминили список моделей авто в тот момент, когда SelectedCarModel == null

        // изменяем список моделей авто, в зависимости от марки авто
        private void EditCarModel()
        {
            // передаём список моделей авто, которые соответствуют данной марки авто
            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<CarModel> carModels = context.CarModels.ToList();

                // выводим модели, которые соответствуют данной марки авто
                if (SelectedCarModel == null) // если данные у модели авто не выбранны в ComboBox
                {
                    if (SelectedCarBrand != null) // если марка выбрана корректно
                    {
                        // фильтруем данные списка
                        SearchFilter();

                        // изменяем список моделей
                        using (SparePartsStoreContext context = SparePartsStoreContext())
                        {
                            List<CarModel> carModels = context.CarModels.ToList();

                            // список моделей
                            List < CarModel >

                            foreach (var item in ListAutopartDPO)
                            {
                                carModels = carModels.Where(c => c.CarModelId == item.CarModelId).ToList;
                            }
                            if (carModels != null)
                            {
                                ObservableCollection<CarModel> carModel = new ObservableCollection<CarModel>(carModels);
                            }
                            NameCarModelComboBoxItems = carModel;
                            OnPropertyChanged(nameof(NameCarModelComboBoxItems)); // оповещаем список моделей авто об изменении данных
                            editCarModel = true;
                        }


                        //ObservableCollection<CarModel> carModel = new ObservableCollection<CarModel>(carModels.Where(c => c.CarBrandId == SelectedCarBrand.CarBrandId).ToList());
                        //_nameCarModelComboBoxItems = carModel; // присваиваем новые значения списка ComboBox
                        //OnPropertyChanged(nameof(NameCarModelComboBoxItems)); // оповещаем список моделей авто об изменении данных
                        //editCarModel = true;
                    }


                    if (editCarModel == true)
                    {
                        if (SelectedCarBrand != null) // если марка выбрана корректно
                        {
                            ObservableCollection<CarModel> carModel = new ObservableCollection<CarModel>(carModels.Where(c => c.CarBrandId == SelectedCarBrand.CarBrandId).ToList());
                            _nameCarModelComboBoxItems = carModel; // присваиваем новые значения списка ComboBox
                            OnPropertyChanged(nameof(NameCarModelComboBoxItems)); // оповещаем список моделей авто об изменении данных
                        }
                    }
                }
            }

        #endregion

            #region CarModel

            // выбранная модель авто
        private CarModel _selectedCarModel;
        public CarModel SelectedCarModel
        {
            get { return _selectedCarModel; }
            set
            {
                _selectedCarModel = value;
                OnPropertyChanged(nameof(SelectedCarModel));
                //EditCarBrand();
            }
        }

        // список моделей авто
        private ObservableCollection<CarModel> _nameCarModelComboBoxItems;
        public ObservableCollection<CarModel> NameCarModelComboBoxItems
        {
            get { return _nameCarModelComboBoxItems; }
            set
            {
                _nameCarModelComboBoxItems = value;
                OnPropertyChanged(nameof(NameCarModelComboBoxItems));
            }
        }

        public ObservableCollection<CarModel> GetCarModelOnComboBox()
        {
            // список, котроый будет в себе хранить значения comBox
            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<CarModel> carModels = context.CarModels.ToList(); // получили список моделей из БД
                ObservableCollection<CarModel> carModelList = new ObservableCollection<CarModel>(); // список для ComBox
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


        #endregion

        #region Unit

        private Unit _selectedUnit;
        public Unit SelectedUnit
        {
            get { return _selectedUnit; }
            set
            {
                _selectedUnit = value;
                OnPropertyChanged(nameof(SelectedUnit));
                //EditKnot(); // изменяем список узлов
            }
        }

        // список узлов
        private ObservableCollection<Unit> _nameUnitComboBoxItems;
        public ObservableCollection<Unit> NameUnitComboBoxItems
        {
            get { return _nameUnitComboBoxItems; }
            set
            {
                _nameUnitComboBoxItems = value;
                OnPropertyChanged(nameof(NameUnitComboBoxItems));
            }
        }

        // получаем данные для ComBox Unit
        public ObservableCollection<Unit> GetUnitOnComboBox()
        {
            // список, котроый будет в себе хранить значения comBox
            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<Unit> units = context.Units.ToList(); // получили список агрегатов из БД
                ObservableCollection<Unit> unitList = new ObservableCollection<Unit>(); // список для ComBox
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

        #endregion

        #region Knot

        // выбранный узел
        private Knot _selectedKnot;
        public Knot SelectedKnot
        {
            get { return _selectedKnot; }
            set
            {
                _selectedKnot = value;
                OnPropertyChanged(nameof(SelectedKnot));
                //EditUnit(); // изменяем список агрегатов
            }
        }

        // список узлов
        private ObservableCollection<Knot> _nameKnotComboBoxItems;
        public ObservableCollection<Knot> NameKnotComboBoxItems
        {
            get { return _nameKnotComboBoxItems; }
            set
            {
                _nameKnotComboBoxItems = value;
                OnPropertyChanged(nameof(NameKnotComboBoxItems));

            }

        }

        // получаем данные для ComBox Knot
        public ObservableCollection<Knot> GetKnotOnComboBox()
        {
            // список, котроый будет в себе хранить значения comBox
            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<Knot> knots = context.Knots.ToList(); // получили список узлов из БД
                ObservableCollection<Knot> knotList = new ObservableCollection<Knot>(); // список для ComBox
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

        #endregion

        #endregion


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    
}
