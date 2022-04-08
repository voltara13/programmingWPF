using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using WareHouse.ViewModel;

namespace WareHouse.View
{
    /// <summary>
    /// Логика взаимодействия для SaleWindow.xaml
    /// </summary>
    public partial class SaleWindow : Window
    {
        private int _noOfErrorsOnScreen = 0;
        private List<InventoryViewModel> _inventoryList;

        public SaleWindow(List<InventoryViewModel> inventoryList)
        {
            InitializeComponent();

            _inventoryList = inventoryList;

            ComboBoxName.ItemsSource = _inventoryList.Select(x => x.Name);

            DataContext = new SaleViewModel();
        }
        /// <summary>
        /// Геттер модели-представления позиции продажи
        /// </summary>
        public SaleViewModel SaleViewModel
        {
            get
            {
                if (DataContext is SaleViewModel saleViewModel)
                {
                    saleViewModel.BarCode = _inventoryList.First(x => x.Name == saleViewModel.Name).BarCode;
                    return saleViewModel;
                }

                throw new ArgumentNullException();
            }
        }
        // <summary>
        /// Метод проверки на ошибки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValidationError(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _noOfErrorsOnScreen++;
            else
                _noOfErrorsOnScreen--;
        }
        /// <summary>
        /// Евент проверки возможности на кнопку "Применить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OK_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _noOfErrorsOnScreen == 0;
            e.Handled = true;
        }
        /// <summary>
        /// Евент нажатия на кнопку "Ок"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OK_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DialogResult = true;
            e.Handled = true;
        }
    }
}
