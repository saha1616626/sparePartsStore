using sparePartsStore.Model;
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
    public class ListCountryViewModel : INotifyPropertyChanged
    {
        public ListCountryViewModel()
        {
            //чтение данных из БД
            ListCountryRead = GetListCountryBD();

            //вывод данных в таблицу
            ListCountry = LoadCountry();
        }

        // выбранные данные таблицы
        private Country _selectedCountry { get; set; }
        public Country SelectedCountry
        {
            get { return _selectedCountry; }
            set
            {
                _selectedCountry = value;
                OnPropertyChanged(nameof(SelectedCountry));
                OnPropertyChanged(nameof(IsWorkButtonEnable));
            }
        }

        // отображение кнопки редактировать или удалить при выборе данных в таблице
        private bool _isWorkButtonEnable;
        public bool IsWorkButtonEnable
        {
            get { return _selectedCountry != null; } // если нажто поле в таблице
            set
            {
                _isWorkButtonEnable = value;
                OnPropertyChanged(nameof(IsWorkButtonEnable));
            }
        }

        // коллекция считанная из БД 
        public ObservableCollection<Country> ListCountryRead { get; set; } = new ObservableCollection<Country>();

        // коллекция отображения стран на экране
        private ObservableCollection<Country> _listCountry { get; set; }
        public ObservableCollection<Country> ListCountry
        {
            get { return _listCountry; }
            set { _listCountry = value; OnPropertyChanged(nameof(ListCountry)); }
        }

        // метод получения списка стран для вывода на экран. Преобразование ListCountryRead в LoadCountry
        private ObservableCollection<Country> LoadCountry()
        {
            if (ListCountryRead != null)
            {
                // копируем список ListCountryRead в список ListCountry
                ListCountry = ListCountryRead;
            }

            return ListCountry;
        }

        // метод для получения коллекции стран из БД
        private ObservableCollection<Country> GetListCountryBD()
        {
            try
            {
                ObservableCollection<Country> listCountry = new ObservableCollection<Country>(); // список полученных данных из БД
                using (SparePartsStoreContext context = new SparePartsStoreContext())
                {
                    List<Country> countries = context.Countries.ToList(); // получаем список агрегатов
                    if (countries != null)
                    {
                        // копируем данные в список из БД
                        foreach (var c in countries)
                        {
                            Country countryUP = new Country();
                            countryUP.CountryId = c.CountryId;
                            countryUP.NameCountry = c.NameCountry;
                            // добавляем в список
                            listCountry.Add(countryUP);
                        }
                    }
                }

                return listCountry;
                throw new Exception("Ошибка работы БД!");
            }
            catch (Exception ex)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show(ex.ToString(), "Внимание!", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                return null;
            }
        }

        // передаем полученные выбранные данные из таблицы
        public Country TransmitionCountry()
        {
            Country country = new Country();
            country = (Country)SelectedCountry;
            return country;
        }

        // переменная, которая хранит текущие данные в поле у PageWorkCountry
        public TextBox NameCountryInput { get; set; }

        // проверяем, есть ли совпадение данных перед добавлением
        public bool CheckingForMatchDB()
        {
            bool noCoincidence = true; // по умолчанию нет совпадения

            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<Country> country = context.Countries.ToList(); // получаем список агрегатов
                noCoincidence = !country.Any(num => num.NameCountry.ToLower() == NameCountryInput.Text.ToLower());
            }

            return noCoincidence;
        }

        // если пользователь редактирует данные проверяем, что он не изменяет данные уже существующие в таблице
        public bool CheckingForMatchEditDB(Country country)
        {
            bool noCoincidence = false; // по умолчанию совпадение не найдено

            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<Country> countries = context.Countries.ToList(); // получаем список стран
                foreach (Country c in countries)
                {
                    if (c.CountryId == country.CountryId) // если находим данные в списке БД, которые в текущий момент редактируем, то пропускаем
                    {
                        continue;
                    }

                    if (c.NameCountry.ToLower().Trim() == NameCountryInput.Text.ToLower().Trim()) // если нашли совпадение в таблице
                    {
                        noCoincidence = true;
                    }
                }
            }

            return noCoincidence;
        }

        // свойство поля для ввода названия страны
        private TextBox _outNameCountry;
        public TextBox OutNameСountries
        {
            get { return _outNameCountry; }
            set
            {
                _outNameCountry = value;
                OnPropertyChanged(nameof(OutNameСountries));
            }
        }

        // обновляем отображение таблицы после удаления данных
        public void UpdateTabel()
        {
            //чтение данных из БД
            ListCountryRead = GetListCountryBD();

            //вывод данных в таблицу
            ListCountry = LoadCountry();
        }

        // список для фильтров таблицы
        public ObservableCollection<Country> ListSearch { get; set; } = new ObservableCollection<Country>();
        // поиск агрегата авто
        public void HandlerTextBoxChanged(string nameCountry)
        {
            if (nameCountry.Trim() != "")
            {
                ListSearch = GetListCountryBD(); // получаем актуальные данные из БД
                // создаём список с поиском по введенным данным в таблице
                var resCountry = ListSearch.Where(num => num.NameCountry.ToLower().Contains(nameCountry.ToLower())).ToList();

                // выведет всё, если ничего не найдено
                //if (resCountry.Count > 0)
                //{
                //ListCountry.Clear(); // очищаем список отображения данных в таблице
                //                     // вносим актуальные данные основного списка
                //foreach (var unit in resCountry)
                //{
                //    Country cn = new Country();
                //    cn = unit;
                //    ListCountry.Add(cn); // добавляем новые данные в список (с учетом фильтра)
                //}
                //}

                ListCountry.Clear(); // очищаем список отображения данных в таблице
                                  // вносим актуальные данные основного списка
                foreach (var c in resCountry)
                {
                    Country un = new Country();
                    un = c;
                    ListCountry.Add(un); // добавляем новые данные в список (с учетом фильтра)
                }
            }

            if (nameCountry.Trim() == "")
            {
                ListCountry.Clear(); // очищаем список отображения данных в таблице
                ListSearch.Clear(); // очищаем доп список
                ListSearch = GetListCountryBD(); // получаем актуальные данные из БД
                foreach (var u in ListSearch)
                {
                    Country countries = new Country();
                    countries = u;
                    ListCountry.Add(countries);  // добавляем новые данные в список (без фильтра)
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
