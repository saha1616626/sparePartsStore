using sparePartsStore.Helper;
using sparePartsStore.View.ViewAdministrator.ViewMainPages;
using sparePartsStore.View.ViewAdministrator.ViewWorking;
using sparePartsStore.ViewModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace sparePartsStore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // тестовый запуск страницы администратора
            //mainFrame.Navigate(new Uri("/View/ViewAdministrator/ViewMainPages/PageMainHead.xaml", UriKind.Relative));

            MainHeadViewModel mainHeadViewModel = new MainHeadViewModel(); // создали экз ViewModel, чтобы можно было потом с ним общаться в PageMainHead
            PageMainHead pageMainHead = new PageMainHead(mainHeadViewModel);

            NavigationManager.StartFrame = mainFrame;
            mainFrame.Navigate(pageMainHead);
        }
    }
}