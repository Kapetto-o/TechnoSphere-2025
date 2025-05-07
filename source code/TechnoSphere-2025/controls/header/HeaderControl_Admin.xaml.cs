using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TechnoSphere_2025.controls.header
{
    public partial class HeaderControl_Admin : UserControl
    {
        private bool _isCross = false;
        private const double BurgerIconSize = 25;
        private const double CrossIconSize = 20;

        public HeaderControl_Admin()
        {
            InitializeComponent();
        }

        private void BurgerCatalogButton_Click(object sender, RoutedEventArgs e)
        {
            _isCross = !_isCross;

            var key = _isCross ? "CrossIcon" : "BurgerIcon";
            var icon = (ImageSource)FindResource(key);
            BurgerCatalogImage.Source = icon;

            if (_isCross)
            {
                BurgerCatalogImage.Width = CrossIconSize;
                BurgerCatalogImage.Height = CrossIconSize;
            }
            else
            {
                BurgerCatalogImage.Width = BurgerIconSize;
                BurgerCatalogImage.Height = BurgerIconSize;
            }
        }
    }
}