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
    public class ListCarModelViewModel : INotifyPropertyChanged
    {
        public ListCarModelViewModel() // конструктор
        {
            //чтение данных из БД
            ListCarModelRead = GetListCarModel();

            //вывод данных в таблицу
            ListCarModelDPO = LoadCarModelBD();
        }

        // выбранные данныиз таблицы
        private CarModelDPO _selectedCarModel {  get; set; }
        public CarModelDPO SelectedCarModel
        {
            get { return _selectedCarModel; }
            set 
            { 
                _selectedCarModel = value; 
                OnPropertyChanged(nameof(SelectedCarModel)); 
                OnPropertyChanged(nameof(IsWorkButtonEnable)); 
            }
        }

        // отображение кнопки редактировать или удалить при выборе данных в таблице
        private bool _isWorkButtonEnable;
        public bool IsWorkButtonEnable
        {
            get { return _selectedCarModel != null; } // если нажто поле в таблице
            set { _isWorkButtonEnable = value; OnPropertyChanged(nameof(IsWorkButtonEnable)); }
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

            if (ListCarModelRead != null)
            {
                // проходимся по списку ListCarModelRead для заполнения списка ListCarModelDPO
                foreach (var model in ListCarModelRead.ToList())
                {
                    carModels.Add(carModelDPO.CopyFromCarModel(model)); // добавляем данные 
                }
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
                    if(carModel != null)
                    {
                        // копируем данные в список из БД
                        foreach (var model in carModel)
                        {
                            CarModel cModel = new CarModel();
                            cModel.CarBrandId = model.CarBrandId;
                            cModel.NameCarModel = model.NameCarModel;
                            cModel.CarModelId = model.CarModelId;
                            // добавляем в список
                            carModels.Add(cModel);
                        }
                    }
                }
                return carModels;
                throw new Exception("Ошибка работы БД!");
            }
            catch (Exception ex)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show(ex.ToString(), "Внимание!", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                return null;
            }
        }

        // передаём получаенные выбранные данные из таблицы
        public CarModelDPO TransmitionBrand()
        {
            CarModelDPO carModelDPO = new CarModelDPO();
            carModelDPO = (CarModelDPO)SelectedCarModel;
            return carModelDPO;
        }

        // получаем данные для ComBox
        public List<CarBrand> GetCarModelOnComboBox()
        {
            // список, котроый будет в себе хранить значения comBox
            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<CarBrand> carBrands = context.CarBrands.ToList(); // получили список моделей авто из БД
                List<CarBrand> brands = new List<CarBrand>(); // список для ComBox
                // записываем массив carModelList в carModelDPOs
                foreach (CarBrand carBrand in carBrands)
                {
                    // добавляем данные в список
                    brands.Add(carBrand);
                }
                // возвращаем массив обратно
                return brands;
            }
        }

        // переменная, которая хранит текущие данные в поле у PageWorkListModel
        public TextBox NameModelInput {  get; set; }

        // проверяем, есть ли совпадения данных перед добавления
        public bool CheckingForMatchDB()
        {
            bool noCoincidence = true; // по умолчанию нет совпадения

            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<CarModel> carModel = context.CarModels.ToList(); // получаем список моделей авто
                noCoincidence = !carModel.Any(num => num.NameCarModel.ToLower().Contains(NameModelInput.Text.ToLower()));

            }

            return noCoincidence;
        }

        // свойство поля для ввода названия модели авто
        private TextBox _outNameModel;
        public TextBox OutNameModel
        {
            get { return _outNameModel; }
            set
            {
                _outNameModel = value;
                OnPropertyChanged(nameof(OutNameModel));
            }
        }

        // передача выбранного объекта в Popup
        public CarModelDPO TransmitModel()
        {
            CarModelDPO carModelDPO = new CarModelDPO();
            carModelDPO = (CarModelDPO)SelectedCarModel;
            return carModelDPO;
        }

        // обновляем отображение таблицы после удаления данных
        public void UpdateTabel()
        {
            //чтение данных из БД
            ListCarModelRead = GetListCarModel();

            //вывод данных в таблицу
            ListCarModelDPO = LoadCarModelBD();
        }

        // список для фильтров таблицы
        public ObservableCollection<CarModel> ListSearch { get; set; } = new ObservableCollection<CarModel>();

        // поиск марок авто
        public void HandlerTextBoxChanged(string nameModel)
        {
            if(nameModel.Trim() != "")
            {
                ListSearch = GetListCarModel(); // получаем актуальные данные из БД
                // создаём список с поиском по введенным данным в таблице
                var resCarModel = ListSearch.Where(num => num.NameCarModel.ToLower().Contains(nameModel.ToLower())).ToList();
                if(resCarModel.Count > 0)
                {
                    ListCarModelDPO.Clear();// очищаем основной список
                    // вносим актуальные данные основного списка
                    foreach(var carModel in resCarModel)
                    {
                        CarModelDPO carModelDPO = new CarModelDPO();
                        carModelDPO = carModelDPO.CopyFromCarModel(carModel);
                        ListCarModelDPO.Add(carModelDPO); // добавляем данные
                    }
                }
            }

            if(nameModel.Trim() == "")
            {
                ListCarModelDPO.Clear();// очищаем основной список
                ListSearch.Clear(); // очищаем доп список
                ListSearch = GetListCarModel(); // получаем актуальные данные из БД
                foreach(var carModel in ListSearch)
                {
                    CarModelDPO carModelDPO = new CarModelDPO();
                    carModelDPO = carModelDPO.CopyFromCarModel(carModel);
                    ListCarModelDPO.Add(carModelDPO);
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
