using Microsoft.IdentityModel.Tokens;
using sparePartsStore.Helper;
using sparePartsStore.Model;
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
    /// Interaction logic for PageWorkUnit.xaml
    /// </summary>
    public partial class PageWorkUnit : Page
    {
        private readonly ListUnitViewModel _listUnitViewModel; // здесь храним экз. класса ListUnitViewModel
        
        private Storyboard _focusAnimation; // анимация подсветки

        // переменная, которая хранит переданные данные из MainHeadViewModel для редактирования
        private Unit getUnit = new Unit();

        public PageWorkUnit()
        {
            InitializeComponent();

            // получаем экз ListUnitViewModel
            _listUnitViewModel = (ListUnitViewModel)this.Resources["ListUnitViewModel"];

            // анимация при неккорректном вводе данных в поля. подключаем ресурс
            _focusAnimation = (Storyboard)FindResource("FocusAnimation"); // анимация исчезает через n интервал времени
        }

        // получаем данные из MainHeadViewModel для вывода на экран и последущего редактирования
        public void DataReception(Unit unit)
        {
            getUnit = unit; // сохроняем данные из MainHeadViewModel
            // передаём данные в поля
            nameUnit.Text = unit.NameUnit.Trim();
        }

        // событие передачи данных из текстового поля
        public EventHandler<MyEventArgsObject> EventArgsUnit;
        protected virtual void TransmitData(Unit value)
        {
            EventArgsUnit?.Invoke(this, new MyEventArgsObject { Value = value });
        }

        // метод передачи данных из TextBox в MainHeadViewModel
        public void Transmit() // передаются данные для редакирования или добавления
        {
            Unit unit = new Unit();
            unit.UnitId = getUnit.UnitId;
            unit.NameUnit = nameUnit.Text.Trim();
            TransmitData(unit); // передаем агрегат
        }

        // сохранение данных после редактирования или добавления данных в таблице
        private void SaveData(object sender, EventArgs e)
        {
            if (nameUnit.Text.Trim().IsNullOrEmpty())
            {
                errorInput.Text = "Заполните все поля!";
                StartAnimation(nameUnit); // запуск анимации
                BeginFadeAnimation(errorInput);
            }

            else // если все поля заполнены
            {
                _listUnitViewModel.NameUnitInput = nameUnit;

                bool Checking = true; // по умолчанию не повторяется

                if (RenameButtonBrand.Content != "Редактировать") // если режим редактирования, то проверка на уникальность не выполняется
                {
                    Checking = _listUnitViewModel.CheckingForMatchDB();
                }
                else // если пользователь редактирует данные проверяем, что он не изменяет данные уже существующие в таблице
                {
                   bool CheckingItem = _listUnitViewModel.CheckingForMatchEditDB(getUnit);
                    if (CheckingItem)
                    {
                        errorInput.Text = "Данный агрегат уже есть в базе!";
                        BeginFadeAnimation(errorInput);

                        Checking = false;
                    }
                    else
                    {
                        if (_listUnitViewModel.CheckingForMatchDB())
                        {
                            Checking = true;
                        }
                        else
                        {
                            errorInput.Text = "";
                            Checking = false;
                        }
                        
                    }
                }

                // проверяем нет ли совпадения в БД
                if (Checking)
                {
                    // вызываем событие для сохранения данных в классе MainHeadViewModel
                    WorkingWithData.SaveDataCreateOrEditUnit();
                }
                else // если все поля заполнены
                {
                    if (nameUnit.Text.Trim() == "")
                    {
                        errorInput.Text = "Заполните все поля!";
                        StartAnimation(nameUnit); // запуск анимации
                        BeginFadeAnimation(errorInput);
                    }
                    else
                    {
                        errorInput.Text = "Данный агрегат уже есть в базе!";
                        BeginFadeAnimation(errorInput);
                    }
                }
            }
        }

        // кнопка выхода на главную страницу
        private void ClosePageAddOrDeleteCarModel(object sender, RoutedEventArgs e)
        {
            // вызываем события для закрытия страницы
            WorkingWithData.ClosePageUnit();
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

        // запуск анимации для TextBox
        private void StartAnimation(TextBox textBox)
        {
            _focusAnimation.Begin(textBox);
        }
    }
}
