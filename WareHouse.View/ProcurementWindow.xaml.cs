using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using WareHouse.ViewModel;

namespace WareHouse.View
{
    /// <summary>
    /// Логика взаимодействия для ProcurementWindow.xaml
    /// </summary>
    public partial class ProcurementWindow : Window
    {
        private int _noOfErrorsOnScreen = 0;

        public ProcurementWindow()
        {
            InitializeComponent();

            DataContext = new ProcurementViewModel();
        }
        /// <summary>
        /// Геттер модели-представления позиции покупки
        /// </summary>
        public ProcurementViewModel ProcurementViewModel
        {
            get
            {
                if (DataContext is ProcurementViewModel procurementViewModel)
                {
                    return procurementViewModel;
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
