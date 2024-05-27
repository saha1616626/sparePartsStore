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
    public class ListCarModelViewModel : INotifyPropertyChanged
    {
        public ListCarModelViewModel() // конструктор
        {
            // чтение данных из БД
            ListCarModelRead = GetListCarModel();

            // вывод данных в таблицу
            ListCarModelDPO = LoadCarModelBD();
        }

        // коллекция считанная из БД 
        public ObservableCollection<CarModelDPO> ListCarModelReadDPO { get; set; } = new ObservableCollection<CarModelDPO>();

        // список отображения моделей авто на экране
        private ObservableCollection<CarModelDPO> _listCarModelDPO { get; set; }
        public ObservableCollection<CarModelDPO> ListCarModelDPO
        {
            get { return _listCarModelDPO; }
            set { _listCarModelDPO = value; OnPropertyChanged(nameof(ListCarModelDPO)); }
        }

        // метод получения списка моделей авто для вывода на экран. Преобразование ListCarModelRead в ListCarModelReadDPO
        private ObservableCollection<CarModelDPO> LoadCarModelBD()
        {
            // создаём преобразованный список
            ObservableCollection<CarModelDPO> carModels = new ObservableCollection<CarModelDPO>();
            
            // экз для преобразования данных
            CarModelDPO carModelDPO = new CarModelDPO();   

            // проходимся по списку ListCarModelRead для заполнения списка ListCarModelDPO
            foreach(var model in ListCarModelRead)
            {
                carModels.Add(carModelDPO.CopyFromCarModel(model)); // добавляем данные 
            }

            return carModels;
        }

        // коллекция считанная из БД
        public ObservableCollection<CarModel> ListCarModelRead { get; set; } = new ObservableCollection<CarModel>();

        // метод для получения коллекции моделей авто
        private ObservableCollection<CarModel> GetListCarModel()
        {
            try
            {
                ObservableCollection<CarModel> carModels = new ObservableCollection<CarModel>(); // список полученных данных из БД
                using (SparePartsStoreContext context = new SparePartsStoreContext())
                {
                    List<CarModel> carModel = context.CarModels.ToList(); // получаем список моделей авто
                    // копируем данные в список из БД
                    foreach(var model in carModels)
                    {
                        CarModel cModel = new CarModel();
                        cModel.CarBrandId = model.CarBrandId;
                        cModel.NameCarModel = model.NameCarModel;
                        cModel.CarBrandId = model.CarBrandId;
                        // добавляем в список
                        carModels.Add(cModel);
                    }
                }

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
