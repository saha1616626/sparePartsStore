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
    public class ListAnaloguesViewModel : INotifyPropertyChanged
    {
        AuthorizationViewModel _authorizationViewModel = new AuthorizationViewModel(); // работа с авторизацией

        public ListAnaloguesViewModel()
        {
            //чтение данных из БД
            ListAutopartRead = GetListAutopart();

            //вывод данных в таблицу доступных запчастей
            ListAutopartDPO = LoadAutopartBD();
        }

        // передача данных асинхронным путем
        #region Analogues

        // свойства текстового поля детали, которой мы подбираем аналоги
        private string _analog;
        public string Analog
        {
            get { return _analog; } set { _analog = value;  OnPropertyChanged(nameof(Analog)); }
        }

        // хранит выбранные данные для подбора аналогов
        private AutopartDPO _autopartDPO;

        // асинхронная передача данных для подбора аналогов
        public async Task InitializeAsync(AutopartDPO autopartDPO)
        {
            _autopartDPO = autopartDPO;
            // теперь вы можете использовать _autopartDPO в методах ViewModel
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            // отображаем запчастькоторой подбираем аналог
            Analog = _autopartDPO.NameAutopart + " " + _autopartDPO.CarBrandName + " " + _autopartDPO.NameCarModel + " " + _autopartDPO.NameManufacture;

            // выводим список аналогов
            LaunchAnalogAutoPart();

            //чтение данных из БД
            ListAutopartRead = GetListAutopart();

            //вывод данных в таблицу доступных запчастей
            ListAutopartDPO = LoadAutopartBD();
        }

        #endregion

        // работа со списком
        #region dataList

        // список отображения запчастей на экране возможные аналоги
        private ObservableCollection<AutopartDPO> _listAutopartDPO { get; set; }
        public ObservableCollection<AutopartDPO> ListAutopartDPO
        {
            get { return _listAutopartDPO; }
            set { _listAutopartDPO = value; OnPropertyChanged(nameof(ListAutopartDPO)); }
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

        // список аналогов
        private ObservableCollection<AutopartDPO> _listAnalogDPO { get; set; }
        public ObservableCollection<AutopartDPO> ListAnalogDPO
        {
            get { return _listAnalogDPO; }
            set { _listAnalogDPO = value; OnPropertyChanged(nameof(ListAnalogDPO)); }
        }

        // поиск аналогов текущей запчасти
        private void LaunchAnalogAutoPart()
        {
            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<PartInterchangeability> partInterchangeabilityIds = context.PartInterchangeabilities.ToList(); // список аналогов
                List<Autopart> autoparts = context.Autoparts.ToList();

                // получаем текущий список аналого
                List<PartInterchangeability> partInterchangeabilities = partInterchangeabilityIds.Where(p => p.AutoPartId == _autopartDPO.AutopartId).ToList();

                if(partInterchangeabilities != null)
                {
                    foreach (var part in partInterchangeabilities)
                    {
                        // получаем запчасть аналог
                        Autopart autopart = autoparts.FirstOrDefault(p => p.AutopartId == part.InterchangeableDetailId); // получаем запчасть аналог
                        AutopartDPO autopartDPO = new AutopartDPO();
                        autopartDPO = autopartDPO.CopyFromAutopart(autopart); // преобразуем данные
                        if(autopartDPO != null)
                        {
                            ListAnalogDPO.Add(autopartDPO); // заполняем список анлогов для текущей запчасти
                        }
                    }
                }
            }
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
                        
                            if(_autopartDPO != null)
                            {
                                
                                // копируем данные в список из БД (с фильтрацией по модели авто и узлу)
                                autoparts = new ObservableCollection<Autopart>(autopartBD.Where(a => a.ModerationStatus == "Отображается" && _autopartDPO.CarModelId == a.CarModelId && _autopartDPO.KnotId == a.KnotId && _autopartDPO.AutopartId != a.AutopartId));
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

        // выбранные данные из таблицы доступные запчасти
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

        // выбор данных в таблице аналог текущей таблицы
        private AutopartDPO _selectedAnalog { get; set; }
        public AutopartDPO SelectedAnalog
        {
            get { return _selectedAnalog; }
            set
            {
                _selectedAnalog = value;
                OnPropertyChanged(nameof(SelectedAnalog));
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



        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
