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

            // запуск страницы администратора
            PageMainHead pageMainHead = new PageMainHead();

            NavigationManager.StartFrame = mainFrame;
            mainFrame.Navigate(pageMainHead);
        }
    }
}