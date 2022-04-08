using System;
using System.Windows;

namespace WareHouse.View
{
    /// <summary>
    /// Логика взаимодействия для NewWareHouse.xaml
    /// </summary>
    public partial class NewWareHouse : Window
    {
        public NewWareHouse()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Евент нажатия на кнопку "Ок"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var temp = Convert.ToDouble(TextBoxBalance.Text);
                if (temp < 0)
                    throw new FormatException();
                DialogResult = true;
            }
            catch (FormatException)
            {
                MessageBox.Show(
                    "Введите корректный баланс.",
                    "Сохранение состояния",
                    MessageBoxButton.OK,
                    MessageBoxImage.Asterisk);
            }
        }
    }
}
