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

            // Изначально делаем проверку, чтобы сразу корректно отобразить поля:
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

        /// <summary>
        /// Включает/выключает видимость блока для «Самовывоза» / «По адресу».
        /// </summary>
        private void UpdateDeliveryFields()
        {
            if (DeliveryMethodComboBox.SelectedIndex == 0) // Самовывоз
            {
                PickupAddressText.Visibility = Visibility.Visible;
                DeliveryAddressBlock.Visibility = Visibility.Collapsed;
                AddressErrorText.Visibility = Visibility.Collapsed;
            }
            else // По адресу
            {
                PickupAddressText.Visibility = Visibility.Collapsed;
                DeliveryAddressBlock.Visibility = Visibility.Visible;
                // чтобы, если ранее была ошибка адреса, при переключении сразу скрыть её
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

        /// <summary>
        /// Ограничение ввода телефона: только цифры и знак '+'
        /// </summary>
        private void PhoneTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Разрешаем только цифры и плюс
            e.Handled = !Regex.IsMatch(e.Text, @"^[0-9\+]+$");
        }

        /// <summary>
        /// Обработчик клика «Оформить заказ»: проверяем все поля
        /// </summary>
        private void SubmitOrderButton_Click(object sender, RoutedEventArgs e)
        {
            // Сначала скрываем все предыдущие ошибки
            NameErrorText.Visibility = Visibility.Collapsed;
            PhoneErrorText.Visibility = Visibility.Collapsed;
            AddressErrorText.Visibility = Visibility.Collapsed;

            bool hasError = false;

            // 1. Проверка имени
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                NameErrorText.Text = "* Пожалуйста, введите имя";
                NameErrorText.Visibility = Visibility.Visible;
                hasError = true;
            }

            // 2. Проверка телефона (должен быть непустой и валидный)
            string phone = PhoneTextBox.Text.Trim();
            // Простой паттерн: от 10 до 15 цифр (можно настроить под ваши требования)
            if (string.IsNullOrEmpty(phone))
            {
                PhoneErrorText.Text = "* Пожалуйста, введите телефон";
                PhoneErrorText.Visibility = Visibility.Visible;
                hasError = true;
            }
            else
            {
                // Проверяем, что в строке от 10 до 15 цифр (игнорируем символ '+')
                string digitsOnly = Regex.Replace(phone, @"\D", "");
                if (digitsOnly.Length < 10 || digitsOnly.Length > 15)
                {
                    PhoneErrorText.Text = "* Неверный формат номера";
                    PhoneErrorText.Visibility = Visibility.Visible;
                    hasError = true;
                }
            }

            // 3. Если выбрано «По адресу», проверяем, что адрес непустой
            if (DeliveryMethodComboBox.SelectedIndex == 1) // «По адресу»
            {
                if (string.IsNullOrWhiteSpace(DeliveryAddressTextBox.Text))
                {
                    AddressErrorText.Text = "* Пожалуйста, введите адрес";
                    AddressErrorText.Visibility = Visibility.Visible;
                    hasError = true;
                }
            }

            // Если есть хоть одна ошибка, не закрываем окно и не продолжаем оформление
            if (hasError)
                return;

            // Здесь можно выполнить сохранение заказа, отправку на сервер и т. д.
            // Например, MessageBox.Show("Заказ успешно оформлен!", "OK", ...);

            MessageBox.Show("Спасибо! Ваш заказ принят.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}