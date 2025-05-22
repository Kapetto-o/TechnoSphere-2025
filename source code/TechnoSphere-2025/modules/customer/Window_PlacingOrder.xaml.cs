using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TechnoSphere_2025.modules.customer
{
    /// <summary>
    /// Логика взаимодействия для Window_PlacingOrder.xaml
    /// </summary>
    public partial class Window_PlacingOrder : Window
    {
        private bool _isInitialized = false;

        public Window_PlacingOrder()
        {
            InitializeComponent();
            _isInitialized = true;

            UpdateDeliveryFields();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void WindowControl_Moving_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void DeliveryMethodComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isInitialized) return;

            UpdateDeliveryFields();
        }

        private void UpdateDeliveryFields()
        {
            if (DeliveryMethodComboBox.SelectedIndex == 0)
            {
                PickupAddressText.Visibility = Visibility.Visible;
                DeliveryAddressBlock.Visibility = Visibility.Collapsed;
                AddressErrorText.Visibility = Visibility.Collapsed;
            }
            else
            {
                PickupAddressText.Visibility = Visibility.Collapsed;
                DeliveryAddressBlock.Visibility = Visibility.Visible;
                AddressErrorText.Visibility = Visibility.Collapsed;
            }
        }

        private void InstallmentCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            InstallmentInfoText.Visibility = Visibility.Visible;
        }

        private void InstallmentCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            InstallmentInfoText.Visibility = Visibility.Collapsed;
        }

        private void PhoneTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[0-9\+]+$");
        }

        private void SubmitOrderButton_Click(object sender, RoutedEventArgs e)
        {
            NameErrorText.Visibility = Visibility.Collapsed;
            PhoneErrorText.Visibility = Visibility.Collapsed;
            AddressErrorText.Visibility = Visibility.Collapsed;

            bool hasError = false;

            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                NameErrorText.Text = "* Пожалуйста, введите имя";
                NameErrorText.Visibility = Visibility.Visible;
                hasError = true;
            }

            string phone = PhoneTextBox.Text.Trim();
            if (string.IsNullOrEmpty(phone))
            {
                PhoneErrorText.Text = "* Пожалуйста, введите телефон";
                PhoneErrorText.Visibility = Visibility.Visible;
                hasError = true;
            }
            else
            {
                string digitsOnly = Regex.Replace(phone, @"\D", "");
                if (digitsOnly.Length < 10 || digitsOnly.Length > 15)
                {
                    PhoneErrorText.Text = "* Неверный формат номера";
                    PhoneErrorText.Visibility = Visibility.Visible;
                    hasError = true;
                }
            }

            if (DeliveryMethodComboBox.SelectedIndex == 1)
            {
                if (string.IsNullOrWhiteSpace(DeliveryAddressTextBox.Text))
                {
                    AddressErrorText.Text = "* Пожалуйста, введите адрес";
                    AddressErrorText.Visibility = Visibility.Visible;
                    hasError = true;
                }
            }

            if (hasError)
                return;

            MessageBox.Show("Спасибо! Ваш заказ принят.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}