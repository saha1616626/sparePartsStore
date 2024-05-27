using sparePartsStore.Helper;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using sparePartsStore.ViewModel;
using sparePartsStore.View.ViewAdministrator.ViewMainPages;
using sparePartsStore.Model;
using System.Windows.Media.Animation;
using Microsoft.IdentityModel.Tokens;

namespace sparePartsStore.View.ViewAdministrator.ViewWorking
{
    /// <summary>
    /// Interaction logic for PageWorkListBrand.xaml
    /// </summary>
    public partial class PageWorkListBrand : Page
    {
        private readonly ListCarBrandViewModel _listCarBrandViewModel; // здесь храним экз. класса ListCarBrandViewModel
                                                                       
        private Storyboard _focusAnimation; // анимация подсветки
        public PageWorkListBrand()
        {
            InitializeComponent();

            // анимация при неккорректном вводе данных в поля. подключаем ресурс
            _focusAnimation = (Storyboard)FindResource("FocusAnimation"); // анимация не изчезнит, пока на полях не будет фокуса

            // получаем экз ListCarBrandViewModel
            _listCarBrandViewModel = (ListCarBrandViewModel)this.Resources["ListCarBrandViewModel"];
        }

        // закрываем страницу 
        private void ClosePageAddOrDeleteCarBrands(object sender, EventArgs e)
        {
            // вызываем события для закрытия страницы
            WorkingWithData.ClosePage();
        }

        // сохраняем данные после изменениня или добавления данных в БД
        private void SaveData(object sender, EventArgs e)
        {
            //TextBox nameBrand = _listCarBrandViewModel.OutNameBrand;
            // обрабатываем логику полей перед добавлением или изменением
            // запускаем анимацию красной подсветки при неверном вводе данных
            if (nameBrand.Text.Trim().IsNullOrEmpty())
            {
                errorInput.Text = "Заполните все поля!";
                StartAnimation(nameBrand);
                BeginFadeAnimation(errorInput);
            }
            else // если текст есть 
            {
                _listCarBrandViewModel.NameBrandInput = nameBrand;
                bool Checking = _listCarBrandViewModel.CheckingForMatchDB();
                // проверяем нет ли совпадения в БД
                if (Checking)
                {
                    // вызываем событие для сохранения данных в классе MainHeadViewModel
                    WorkingWithData.SaveDataCreateOrEditCarBrands();
                }
                else // если данные уже есть в таблице
                {
                    errorInput.Text = "Данная марка уже есть в баазе!";
                    BeginFadeAnimation(errorInput);
                }

            }

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


        // событие передачи данных из текстового поля
        public event EventHandler<MyEventArgsObject> MyEventArgsObject;
        protected virtual void TransmitData(CarBrand value)
        {
            MyEventArgsObject?.Invoke(this, new MyEventArgsObject { Value = value});
        }
        // метод передачи данных из TextBox в MainHeadViewModel
        public void Transmit() // передаются данные для редакирования или добавления
        {
            CarBrand carBrand = new CarBrand();
            carBrand.CarBrandId = GetCarBrand.CarBrandId;
            carBrand.NameCarBrand = nameBrand.Text.Trim();
            TransmitData(carBrand); // передали название марки авто
        }

        // передача данных из MainHeadViewModel для вывода на экран и последующего редактирования
        public void DataReception(CarBrand carBrand)
        {
            GetCarBrand = carBrand; // сохраняем объект данных
            nameBrand.Text = carBrand.NameCarBrand.Trim();
        }

        // переменная, которая хранит данные текущего объекта таблицы марки авто
        private CarBrand GetCarBrand = new CarBrand();


        // запуск анимации
        private void StartAnimation(TextBox textBox)
        {
            _focusAnimation.Begin(textBox);
        }



    }
}
