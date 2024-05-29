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
    public class ListManufactureViewModel : INotifyPropertyChanged
    {
        public ListManufactureViewModel()
        {
            //чтение данных из БД
            ListManufactureRead = GetListManufacture();

            //вывод данных в таблицу
            ListManufactureDPO = LoadManufactureBD();
        }

        // выбранные данные из таблицы
        private ManufactureDPO _selectedManufacture { get; set; }
        public ManufactureDPO SelectedManufacture
        {
            get { return _selectedManufacture; }
            set
            {
                _selectedManufacture = value;
                OnPropertyChanged(nameof(SelectedManufacture));
                OnPropertyChanged(nameof(IsWorkButtonEnable));
            }
        }

        // отображение кнопки редактировать или удалить при выборе данных в таблице
        private bool _isWorkButtonEnable;
        public bool IsWorkButtonEnable
        {
            get { return _selectedManufacture != null; } // если нажто поле в таблице
            set { _isWorkButtonEnable = value; OnPropertyChanged(nameof(IsWorkButtonEnable)); }
        }

        // список отображения производителей на экране
        private ObservableCollection<ManufactureDPO> _listManufactureDPO { get; set; }
        public ObservableCollection<ManufactureDPO> ListManufactureDPO
        {
            get { return _listManufactureDPO; }
            set { _listManufactureDPO = value; OnPropertyChanged(nameof(ListManufactureDPO)); }
        }

        // метод получения списка производителей для вывода на экран
        private ObservableCollection<ManufactureDPO> LoadManufactureBD()
        {
            // создаём преобразованный список
            ObservableCollection<ManufactureDPO> manufacturies = new ObservableCollection<ManufactureDPO>();

            // экз для преобразования данных
            ManufactureDPO manufactureDPO = new ManufactureDPO();

            if (ListManufactureRead != null)
            {
                // проходимся по списку ListKnotRead для заполнения списка ListKnotDPO
                foreach (var manufacture in ListManufactureRead.ToList())
                {
                    manufacturies.Add(manufactureDPO.CopyFromCountry(manufacture)); // добавляем данные 
                }
            }

            return manufacturies;
        }

        // коллекция считанная из БД
        public ObservableCollection<Manufacture> ListManufactureRead { get; set; } = new ObservableCollection<Manufacture>();

        // метод для получения коллекции производителей
        private ObservableCollection<Manufacture> GetListManufacture()
        {
            try
            {
                ObservableCollection<Manufacture> manufacturies = new ObservableCollection<Manufacture>(); // список полученных данных из БД
                using (SparePartsStoreContext context = new SparePartsStoreContext())
                {
                    List<Manufacture> manufactureDB = context.Manufactures.ToList(); // получаем список узлов авто
                    if (manufactureDB != null)
                    {
                        // копируем данные в список из БД
                        foreach (var manufactureItem in manufactureDB)
                        {
                            Manufacture manufactureUP = new Manufacture();
                            manufactureUP.ManufactureId = manufactureItem.ManufactureId;
                            manufactureUP.NameManufacture = manufactureItem.NameManufacture;
                            manufactureUP.CountryId = manufactureItem.CountryId;
                            // добавляем в список
                            manufacturies.Add(manufactureUP);
                        }
                    }
                }
                return manufacturies;
                throw new Exception("Ошибка работы БД!");
            }
            catch (Exception ex)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show(ex.ToString(), "Внимание!", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                return null;
            }
        }

        // передаём получаенные выбранные данные из таблицы
        public ManufactureDPO TransmitionKnot()
        {
            ManufactureDPO manufactureDPO = new ManufactureDPO();
            manufactureDPO = (ManufactureDPO)SelectedManufacture;
            return manufactureDPO;
        }

        // получаем данные для ComBox
        public List<Country> GetCountryOnComboBox()
        {
            // список, котроый будет в себе хранить значения comBox
            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<Country> countries = context.Countries.ToList(); // получили список стран из БД
                List<Country> countryList = new List<Country>(); // список для ComBox
                // записываем массив countryList в countryDPOs
                foreach (Country countrytem in countries)
                {
                    // добавляем данные в список
                    countryList.Add(countrytem);
                }
                // возвращаем массив обратно
                return countryList;
            }
        }

        // переменная, которая хранит текущие данные в поле у PageWorkManufacture
        public TextBox NameManufactureInput { get; set; }

        // проверяем, есть ли совпадение данных перед добавлением
        public bool CheckingForMatchDB()
        {
            bool noCoincidence = true; // по умолчанию нет совпадения

            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<Manufacture> country = context.Manufactures.ToList(); // получаем список агрегатов
                noCoincidence = !country.Any(num => num.NameManufacture.ToLower().Contains(NameManufactureInput.Text.ToLower()));
            }

            return noCoincidence;
        }

        // если пользователь редактирует данные проверяем, что он не изменяет данные уже существующие в таблице
        public bool CheckingForMatchEditDB(ManufactureDPO manufactureDPO)
        {
            bool noCoincidence = false; // по умолчанию совпадение не найдено

            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<Manufacture> manufacturies = context.Manufactures.ToList(); // получаем список производителей
                foreach (Manufacture c in manufacturies)
                {
                    if (c.ManufactureId == manufactureDPO.ManufactureId) // если находим данные в списке БД, которые в текущий момент редактируем, то пропускаем
                    {
                        continue;
                    }

                    if (c.NameManufacture.ToLower().Trim() == NameManufactureInput.Text.ToLower().Trim()) // если нашли совпадение в таблице
                    {
                        noCoincidence = true;
                    }
                }
            }

            return noCoincidence;
        }

        // свойство поля для ввода названия производителя
        private TextBox _outNameManufacture;
        public TextBox OutNameManufacture
        {
            get { return _outNameManufacture; }
            set
            {
                _outNameManufacture = value;
                OnPropertyChanged(nameof(OutNameManufacture));
            }
        }

        // обновляем отображение таблицы после удаления данных
        public void UpdateTabel()
        {
            //чтение данных из БД
            ListManufactureRead = GetListManufacture();

            //вывод данных в таблицу
            ListManufactureDPO = LoadManufactureBD();
        }

        // список для фильтров таблицы
        public ObservableCollection<Manufacture> ListSearch { get; set; } = new ObservableCollection<Manufacture>();

        // поиск производителей
        public void HandlerTextBoxChanged(string nameManufacture)
        {
            if (nameManufacture.Trim() != "")
            {
                ListSearch = GetListManufacture(); // получаем актуальные данные из БД
                // создаём список с поиском по введенным данным в таблице
                var resManufacture = ListSearch.Where(num => num.NameManufacture.ToLower().Contains(nameManufacture.ToLower())).ToList();

                // выведет всё, если ничего не найдено
                //if (resManufacture.Count > 0)
                //{
                //    ListManufactureDPO.Clear();// очищаем основной список
                //    // вносим актуальные данные основного списка
                //    foreach (var manufacturetem in resManufacture)
                //    {
                //        ManufactureDPO manufactureDPO = new ManufactureDPO();
                //        manufactureDPO = manufactureDPO.CopyFromCountry(manufacturetem);
                //        ListManufactureDPO.Add(manufactureDPO); // добавляем данные
                //    }
                //}

                ListManufactureDPO.Clear();// очищаем основной список
                                    // вносим актуальные данные основного списка
                foreach (var manufacturetem in resManufacture)
                {
                    ManufactureDPO manufactureDPO = new ManufactureDPO();
                    manufactureDPO = manufactureDPO.CopyFromCountry(manufacturetem);
                    ListManufactureDPO.Add(manufactureDPO); // добавляем данные
                }
            }

            if (nameManufacture.Trim() == "")
            {
                ListManufactureDPO.Clear();// очищаем основной список
                ListSearch.Clear(); // очищаем доп список
                ListSearch = GetListManufacture(); // получаем актуальные данные из БД
                foreach (var knotItem in ListSearch)
                {
                    ManufactureDPO manufactureDPO = new ManufactureDPO();
                    manufactureDPO = manufactureDPO.CopyFromCountry(knotItem);
                    ListManufactureDPO.Add(manufactureDPO);
                }
            }
        }

        // comboBox
        private Country _selectedCountry;
        public Country SelectedCountry
        {
            get { return _selectedCountry; }
            set
            {
                _selectedCountry = value;
                OnPropertyChanged(nameof(SelectedCountry));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
