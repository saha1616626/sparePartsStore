using Microsoft.IdentityModel.Tokens;
using sparePartsStore.Helper;
using sparePartsStore.Model;
using sparePartsStore.Model.DPO;
using sparePartsStore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace sparePartsStore.View.ViewAdministrator.ViewWorking
{
    /// <summary>
    /// Interaction logic for PageWorkListMark.xaml
    /// </summary>
    public partial class PageWorkListModel : Page
    {
        private readonly ListCarModelViewModel _listCarModelViewModel; // здесь храним экз. класса ListCarModelViewModel

        private Storyboard _focusAnimation; // анимация подсветки

        public PageWorkListModel()
        {
            InitializeComponent();

            // получаем экз ListCarBrandViewModel
            _listCarModelViewModel = (ListCarModelViewModel)this.Resources["ListCarModelViewModel"];

            // анимация при неккорректном вводе данных в поля. подключаем ресурс
            _focusAnimation = (Storyboard)FindResource("FocusAnimation"); // анимация исчезает через n интервал времени
        }

        // переменная, которая хранит переданные данные из MainHeadViewModel для редактирования
        private CarModelDPO getCarModelDPOs = new CarModelDPO();

        // получаем данные из MainHeadViewModel для вывода на экран и последущего редактирования
        public void DataReception(CarModelDPO carModelDPO)
        {
            getCarModelDPOs = carModelDPO; // сохраняем данные
            // передаём данные в поля                                          
            this.CbCarModel.ItemsSource = _listCarModelViewModel.GetCarModelOnComboBox(); // получаем список для ComBox
            CbCarModel.Text = carModelDPO.CarBrandName;
            nameModelCar.Text = carModelDPO.NameCarModel.Trim();
            
        }

        // вывод данных в ComBox при добавлении
        public void DataReceptionAdd()
        {
            // передаём данные в поля                                          
            CbCarModel.ItemsSource = _listCarModelViewModel.GetCarModelOnComboBox().ToList(); // получаем список для ComBox
        }

        // событие передачи данных из текстового поля
        public event EventHandler<MyEventArgsObject> EventArgsCarModel;
        protected virtual void TransmitData(CarModelDPO value)
        {
            EventArgsCarModel?.Invoke(this, new MyEventArgsObject { Value = value });
        }

        // метод передачи данных из TextBox в MainHeadViewModel
        public void Transmit() // передаются данные для редакирования или добавления
        {
            CarModelDPO carModelDPO = new CarModelDPO();
            carModelDPO.CarModelId = getCarModelDPOs.CarModelId;
            carModelDPO.NameCarModel = nameModelCar.Text.Trim();
            carModelDPO.CarBrandName = this.CbCarModel.Text.Trim();
            TransmitData(carModelDPO); // передали название марки авто
        }

        // сохранение данных после редактирования или добавления данных в таблице
        private void SaveData(object sender, EventArgs e)
        {
            if (nameModelCar.Text.Trim().IsNullOrEmpty())
            {
                errorInput.Text = "Заполните все поля!";
                StartAnimation(nameModelCar); // запуск анимации
                BeginFadeAnimation(errorInput);
            }
            if (CbCarModel.Text.Trim().IsNullOrEmpty())
            {
                errorInput.Text = "Заполните все поля!";
                StartAnimation(CbCarModel);
                BeginFadeAnimation(errorInput);
            }

            else // если есть текст
            {
                _listCarModelViewModel.NameModelInput = nameModelCar;

                bool Checking = true; // по умолчанию не повторяется

                if (RenameButtonBrand.Content != "Редактировать") // если режим редактирования, то проверка на уникальность не выполняется
                {
                    Checking = _listCarModelViewModel.CheckingForMatchDB();
                }

                // проверяем нет ли совпадения в БД
                if (Checking)
                {
                    // вызываем событие для сохранения данных в классе MainHeadViewModel
                    WorkingWithData.SaveDataCreateOrEditCarModels();
                }
                else // если если все поля заполнены
                {
                    if (nameModelCar.Text.Trim() == "")
                    {
                        errorInput.Text = "Заполните все поля!";
                        StartAnimation(nameModelCar); // запуск анимации
                        BeginFadeAnimation(errorInput);
                    }
                    else
                    {
                        errorInput.Text = "Данная марка уже есть в баазе!";
                        BeginFadeAnimation(errorInput);
                    }
                }
            }
        }

        // кнопка выхода на главную страницу
        private void ClosePageAddOrDeleteCarModel(object sender, RoutedEventArgs e)
        {
            // вызываем события для закрытия страницы
            WorkingWithData.ClosePage();
        }

        // анимация уведомления
        private void BeginFadeAnimation(TextBlock textBox)
        {
            textBox.IsEnabled = true;
            textBox.Opacity = 1.0;

            Storyboard storyboard = new Storyboard();
            DoubleAnimation fadeAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(2)
            };
            Storyboard.SetTargetProperty(fadeAnimation, new PropertyPath(TextBlock.OpacityProperty));
            storyboard.Children.Add(fadeAnimation);
            storyboard.Completed += (s, e) => textBox.IsEnabled = false;
            storyboard.Begin(textBox);
        }

        // запуск анимации
        private void StartAnimation(TextBox textBox)
        {
            _focusAnimation.Begin(textBox);
        }
        // запуск анимации с перегрузкой
        private void StartAnimation(ComboBox textBox)
        {
            _focusAnimation.Begin(textBox);
        }

    }
}
