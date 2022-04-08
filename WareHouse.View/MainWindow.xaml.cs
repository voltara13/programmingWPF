using System;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using Microsoft.Win32;

using WareHouse.ViewModel;

namespace WareHouse.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel(GetBalance());
        }
        /// <summary>
        /// Метод получения начального баланса
        /// </summary>
        /// <returns></returns>
        private double GetBalance()
        {
            var balance = 0.0;
            var balanceDialog = new NewWareHouse();

            if (balanceDialog.ShowDialog() == true)
            {
                balance = Convert.ToDouble(balanceDialog.TextBoxBalance.Text);
            }

            return balance;
        }
        /// <summary>
        /// Евент нажатия на кнопку отрктия позиции покупки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenProcurementButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel mainWindowViewModel)
            {
                var procurementWindow = new ProcurementWindow();

                if (procurementWindow.ShowDialog() == true)
                {
                    mainWindowViewModel.AddProcurement(procurementWindow.ProcurementViewModel);
                }
            }
        }
        /// <summary>
        /// Евент нажатия на кнопку закрытия позиции покупки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseProcurementButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel mainWindowViewModel)
            {
                mainWindowViewModel.CloseProcurement();
            }
        }
        /// <summary>
        /// Евент нажатия на кнопку отмены позиции покупки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelProcurementButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel mainWindowViewModel)
            {
                mainWindowViewModel.CancelProcurement();
            }
        }
        /// <summary>
        /// Евент нажатия на кнопку открытия позиции продажи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenSaleButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel mainWindowViewModel)
            {
                var saleWindow = new SaleWindow(mainWindowViewModel.Inventory.ToList());

                if (saleWindow.ShowDialog() == true)
                {
                    mainWindowViewModel.AddSale(saleWindow.SaleViewModel);
                }
            }
        }
        /// <summary>
        /// Евент нажатия на кнопку закрытия позиции продажи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseSaleButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel mainWindowViewModel)
            {
                mainWindowViewModel.CloseSale();
            }
        }
        /// <summary>
        /// Евент нажатия на кнопку отмены позиции продажи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelSaleButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel mainWindowViewModel)
            {
                mainWindowViewModel.CancelSale();
            }
        }
        /// <summary>
        /// Евент нажатия на кнопку сохранения профиля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            /*Диалоговое окно сохранения файла*/
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "bin files (*.bin)|*.bin"
            };
            if (saveFileDialog.ShowDialog(this) != true) return;
            try
            {
                if (DataContext is MainWindowViewModel mainWindowViewModel)
                {
                    /*Сериализация*/
                    using (var fs = saveFileDialog.OpenFile())
                    {
                        var formatter = new BinaryFormatter();
                        formatter.Serialize(fs, mainWindowViewModel);
                    }
                    MessageBox.Show(
                        "Состояние успешно сохранено.",
                        "Сохранение состояния",
                        MessageBoxButton.OK, 
                        MessageBoxImage.Information);
                }
            }
            catch (System.Runtime.Serialization.SerializationException)
            {
                MessageBox.Show(
                    "Произошла ошибка во время сохранения",
                    "Сохранение состояния",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Евент нажатия на кнопку загрузки профиля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            /*Диалоговое окно загрузки файла*/
            var openFileDialog = new OpenFileDialog()
            {
                Filter = "bin files (*.bin)|*.bin"
            };
            if (openFileDialog.ShowDialog(this) != true) return;
            try
            {
                /*Десериализация*/
                using (var fs = openFileDialog.OpenFile())
                {
                    var formatter = new BinaryFormatter();
                    var mainWindowViewModelNew = (MainWindowViewModel)formatter.Deserialize(fs);
                    DataContext = mainWindowViewModelNew;
                }
                MessageBox.Show(
                    "Состояние успешно загружено.",
                    "Загрузка состояния",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (System.Runtime.Serialization.SerializationException)
            {
                MessageBox.Show(
                    "Произошла ошибка во время загрузки",
                    "Загрузка состояния",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
