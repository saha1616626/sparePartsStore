using Microsoft.IdentityModel.Tokens;
using sparePartsStore.Helper;
using sparePartsStore.Model;
using sparePartsStore.Model.DPO;
using sparePartsStore.View.ViewAdministrator.ViewMainPages;
using sparePartsStore.View.ViewAdministrator.ViewWorking;
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
    // класс для работы с маркамо авто
    public class ListCarBrandViewModel : INotifyPropertyChanged
    {
        public ListCarBrandViewModel() // конструктор
        {
            // чтение данных из БД
            ListCarBrandRead = GetListCarBrand();
            // вывод данных в таблицу
            ListCarBrand = LoadCarBrandBD();
        }

        // выбранные данные в таблице
        private CarBrand _selectedCarBrand;
        public CarBrand SelectedCarBrand
        {
            get { return _selectedCarBrand; }
            set
            {
                _selectedCarBrand = value;
                OnPropertyChanged(nameof(SelectedCarBrand));
                OnPropertyChanged(nameof(IsEditButtonEnabled)); // Уведомляем об изменении доступности кнопки
            }
        }
        // отображение кнопки редактирования данных
        private bool _isEditButtonEnabled;
        public bool IsEditButtonEnabled
        {
            get { return _selectedCarBrand != null; } // возвращает true, если выбран объект в таблице
            set
            {
                _isEditButtonEnabled = value;
                OnPropertyChanged(nameof(IsEditButtonEnabled));
            }
        }


        // коллекция считанная из бд
        public ObservableCollection<CarBrand> ListCarBrandRead { get; set; } = new ObservableCollection<CarBrand>();
        // коллекция для отображения списка марок авто
        private ObservableCollection<CarBrand> _listCarBrand {  get; set; }
        public ObservableCollection<CarBrand> ListCarBrand
        {
            get { return _listCarBrand; }
            set { _listCarBrand = value; OnPropertyChanged(nameof(ListCarBrand)); }
        }

        // метод для вывод из БД списка марок авто для отображения в таблице
        private ObservableCollection<CarBrand> LoadCarBrandBD()
        {
            ListCarBrand = ListCarBrandRead; // получаем данные для вывода
            return ListCarBrand;
        }

        // метод для получения коллекции марок авто
        private ObservableCollection<CarBrand> GetListCarBrand()
        {
            try
            {
                ObservableCollection<CarBrand> carBrands = new ObservableCollection<CarBrand>(); // список данных из БД
                using (SparePartsStoreContext context = new SparePartsStoreContext())
                {
                    List<CarBrand> carBrand = context.CarBrands.ToList(); // получаем список марок авто
                    // копируем данные из БД в ListCarBrandRead
                    foreach(var brand in carBrand)
                    {
                        CarBrand cBrand = new CarBrand();
                        cBrand.CarBrandId = brand.CarBrandId;
                        cBrand.NameCarBrand = brand.NameCarBrand;
                        // добавляем в список
                        carBrands.Add(cBrand);
                    }
                }
                return carBrands;
                throw new Exception("Ошибка работы БД!");
            }
            catch (Exception ex)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show(ex.ToString(), "Внимание!", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                return null;
            }
        }

        // редактирования данных таблицы
        private RelayCommand _btn_EditCarBrand {  get; set; }
        public RelayCommand Btn_EditCarBrand
        {
            get
            {
                return _btn_EditCarBrand ??
                    (_btn_EditCarBrand = new RelayCommand(obj =>
                    {

                    }, (obj) => true));
            }
        }

        // свойство данных поля текста
        private TextBox _outNameBrand;
        public TextBox OutNameBrand
        {
            get { return _outNameBrand; }
            set
            {
                _outNameBrand = value;
                OnPropertyChanged(nameof(OutNameBrand));
            }
        }

        // передача выбранного объекта в таблице
        public CarBrand TransmitBrand()
        {
            CarBrand carBrand = new CarBrand();
            carBrand = (CarBrand)SelectedCarBrand;
            return carBrand;
        }

        // обновляем отображение таблицы после удаления данных
        public void UpdateTabel()
        {
            // чтение данных из БД
            ListCarBrandRead = GetListCarBrand();
            // вывод данных в таблицу
            ListCarBrand = LoadCarBrandBD();
        }

        // свойство обновления данных таблицы при вводе данных в поле
        private string _nameBrand {  get; set; }
        private string NameBrand
        {
            get { return _nameBrand; }
            set { _nameBrand = value; OnPropertyChanged(nameof(NameBrand)); } 
        }

        // список для фильтров таблицы
        public ObservableCollection<CarBrand> ListSearch { get; set; } = new ObservableCollection<CarBrand>();

        // поиск марок авто
        public void HandlerTextBoxChanged(string nameBrand)
        {
            if(nameBrand.Trim() != "")
            {
                ListSearch = GetListCarBrand(); // получаем актуальные данные из БД
                // создаём список с поиском по введенным данным в таблице
                var resCarBrand = ListSearch.Where(num => num.NameCarBrand.ToLower().Contains(nameBrand.ToLower())).ToList();
                ListCarBrand.Clear(); // очищаем основной список
                // вносим актуальные данные основного списка
                foreach (CarBrand carBrand in resCarBrand)
                {
                    ListCarBrand.Add(carBrand); // добавляем данные
                }
            }
            if (nameBrand.Trim() == "")
            {
                ListCarBrand.Clear(); // очищаем основной список
                ListSearch.Clear(); // очищаем доп список
                ListSearch = GetListCarBrand();// получаем актуальные данные из БД
                foreach (var zakazchik in ListSearch)
                {
                    ListCarBrand.Add(zakazchik);
                }
            }
        }

        // переменная, которая хранит текущие данные в поле у PageWorkListBrand
        public TextBox NameBrandInput {  get; set; }

        // проверяем, есть ли совпадения в БД перед добавление в БД
        public bool CheckingForMatchDB()
        {
            bool noCoincidence = true; // по умолчанию нет совпадения

            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<CarBrand> carBrand = context.CarBrands.ToList(); // получаем список марок авто
                noCoincidence = !carBrand.Any(num => num.NameCarBrand.ToLower().Contains(NameBrandInput.Text.ToLower()));

            }

            return noCoincidence;
        }

        // если пользователь редактирует данные проверяем, что он не изменяет данные уже существующие в таблице
        public bool CheckingForMatchEditDB(CarBrand brand)
        {
            bool noCoincidence = false; // по умолчанию совпадение не найдено

            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<CarBrand> brands = context.CarBrands.ToList(); // получаем список агрегатов
                foreach (CarBrand b in brands)
                {
                    if (b.CarBrandId == brand.CarBrandId) // если находим данные в списке БД, которые в текущий момент редактируем, то пропускаем
                    {
                        continue;
                    }

                    if (b.NameCarBrand.ToLower().Trim() == NameBrandInput.Text.ToLower().Trim()) // если нашли совпадение в таблице
                    {
                        noCoincidence = true;
                    }
                }
            }

            return noCoincidence;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
