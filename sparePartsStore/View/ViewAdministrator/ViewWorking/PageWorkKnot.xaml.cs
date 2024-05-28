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
    /// Interaction logic for PageWorkKnot.xaml
    /// </summary>
    public partial class PageWorkKnot : Page
    {
        private readonly ListKnotViewModel _listKnotViewModel; // объект класса
        private Storyboard _focusAnimation; // анимация подсветки
        public PageWorkKnot()
        {
            InitializeComponent();

            // получаем экз ListKnotViewModel
            _listKnotViewModel = (ListKnotViewModel)this.Resources["ListKnotViewModel"];

            // анимация при неккорректном вводе данных в поля. подключаем ресурс
            _focusAnimation = (Storyboard)FindResource("FocusAnimation"); // анимация исчезает через n интервал времени
        }

        // переменная, которая хранит переданные данные из MainHeadViewModel для редактирования
        private KnotDPO getKnotDPOs = new KnotDPO();

        // получаем данные из MainHeadViewModel для вывода на экран и последущего редактирования
        public void DataReception(KnotDPO knotDPO)
        {
            getKnotDPOs = knotDPO; // сохраняем данные
            // передаём данные в поля                                          
            this.CbUnit.ItemsSource = _listKnotViewModel.GetUnitOnComboBox(); // получаем список для ComBox
            CbUnit.Text = knotDPO.NameUnit;
            nameKnot.Text = knotDPO.NameKnot.Trim();
        }

        // вывод данных в ComBox при добавлении
        public void DataReceptionAdd()
        {
            // передаём данные в поля                                          
            CbUnit.ItemsSource = _listKnotViewModel.GetUnitOnComboBox().ToList(); // получаем список для ComBox
        }

        // событие передачи данных из текстового поля
        public event EventHandler<MyEventArgsObject> EventArgsKnot;
        protected virtual void TransmitData(KnotDPO value)
        {
            EventArgsKnot?.Invoke(this, new MyEventArgsObject { Value = value });
        }

        // метод передачи данных из TextBox в MainHeadViewModel
        public void Transmit() // передаются данные для редакирования или добавления
        {
            KnotDPO knotDPO = new KnotDPO();
            
            Unit unit = new Unit();
            unit = (Unit)CbUnit.SelectedItem; // получаем ID из ComBox
            knotDPO.UnitId = unit.UnitId; // передали ID
            knotDPO.KnotId = getKnotDPOs.KnotId;
            knotDPO.NameKnot = nameKnot.Text.Trim();
            knotDPO.NameUnit = this.CbUnit.Text.Trim();
            TransmitData(knotDPO); // передали название марки авто
        }

        // сохранение данных после редактирования или добавления данных в таблице
        private void SaveData(object sender, EventArgs e)
        {
            if (nameKnot.Text.Trim().IsNullOrEmpty())
            {
                errorInput.Text = "Заполните все поля!";
                StartAnimation(nameKnot); // запуск анимации
                BeginFadeAnimation(errorInput);
            }
            if (CbUnit.Text.Trim().IsNullOrEmpty())
            {
                errorInput.Text = "Заполните все поля!";
                StartAnimation(CbUnit);
                BeginFadeAnimation(errorInput);
            }

            else // если есть текст
            {
                _listKnotViewModel.NameKnotInput = nameKnot;

                bool Checking = true; // по умолчанию не повторяется

                if (RenameButtonBrand.Content != "Редактировать") // если режим редактирования, то проверка на уникальность не выполняется
                {
                    Checking = _listKnotViewModel.CheckingForMatchDB();
                }
                else // если пользователь редактирует данные проверяем, что он не изменяет данные уже существующие в таблице
                {
                    bool CheckingItem = _listKnotViewModel.CheckingForMatchEditDB(getKnotDPOs);
                    if (CheckingItem)
                    {
                        errorInput.Text = "Данный узел уже есть в базе!";
                        BeginFadeAnimation(errorInput);

                        Checking = false;
                    }
                    else
                    {
                        if (_listKnotViewModel.CheckingForMatchDB())
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
                    WorkingWithData.SaveDataCreateOrEditKnot();
                }
                else // если если все поля заполнены
                {
                    if (nameKnot.Text.Trim() == "")
                    {
                        errorInput.Text = "Заполните все поля!";
                        StartAnimation(nameKnot); // запуск анимации
                        BeginFadeAnimation(errorInput);
                    }
                    else
                    {
                        errorInput.Text = "Данный узел уже есть в базе!";
                        BeginFadeAnimation(errorInput);
                    }
                }
            }
        }

        // кнопка выхода на главную страницу
        private void ClosePageAddOrDeleteKnot(object sender, RoutedEventArgs e)
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
