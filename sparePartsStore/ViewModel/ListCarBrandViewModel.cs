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

        // коллекция считанная из бд
        public ObservableCollection<CarBrand> ListCarBrandRead { get; set; } = new ObservableCollection<CarBrand>();
        // коллекция для отображения списка марок авто
        public ObservableCollection<CarBrand> ListCarBrand { get; set; } = new ObservableCollection<CarBrand>();

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
                using (SparePartsStoreContext context = new SparePartsStoreContext())
                {
                    List<CarBrand> carBrand = context.CarBrands.ToList(); // получаем список марок авто
                    // копируем данные из БД в ListCarBrandRead
                    foreach(var brand in carBrand)
                    {
                        CarBrand cBrand = new CarBrand();
                        cBrand.NameCarBrand = brand.NameCarBrand;
                        // добавляем в список
                        ListCarBrandRead.Add(cBrand);
                    }
                }
                return ListCarBrand;
                throw new Exception("Ошибка работы БД!");
            }
            catch (Exception ex)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show(ex.ToString(), "Внимание!", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                return null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
