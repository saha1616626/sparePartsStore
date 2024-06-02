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
        private string Search {  get; set; }

        // фильруем данные при вводе в текствое поле
        public void HandlerTextBoxChanged(string nameAutoParts)
        {
            Search = nameAutoParts; // передали измененный текст
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
            foreach(AutopartDPO item in ListAutopartDPO)
            {
                autopartDPOs.Add(item);
            }

            if(SelectedCarBrand != null)
            {
                autopartDPOs = autopartDPOs.Where(a => a.CarBrandId == SelectedCarBrand.CarBrandId).ToList();
            }
            if(SelectedCarModel != null)
            {
                autopartDPOs = autopartDPOs.Where(a => a.CarModelId == SelectedCarModel.CarModelId).ToList();
            }
            if(SelectedUnit != null)
            {
                autopartDPOs = autopartDPOs.Where(a => a.UnitId == SelectedUnit.UnitId).ToList();
            }
            if(SelectedKnot != null)
            {
                autopartDPOs = autopartDPOs.Where(a => a.KnotId == SelectedKnot.KnotId).ToList();
            }
            if(Search != null)
            {
                autopartDPOs = autopartDPOs.Where(a => a.NameAutopart == conta).ToList();
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
                //EditCarModel();
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


        #endregion

                #endregion


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
