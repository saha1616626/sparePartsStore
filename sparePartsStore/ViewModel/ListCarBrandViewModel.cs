using sparePartsStore.Helper;
using sparePartsStore.Model;
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
