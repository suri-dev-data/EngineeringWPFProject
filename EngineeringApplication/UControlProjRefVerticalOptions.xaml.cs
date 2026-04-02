using System;
using System.Collections.Generic;
using System.Text;
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

namespace EngineeringApplication
{
    public class PDReference
    {
        public required bool IsSelected { get; set; }
        public required string Ref { get; set; }
        public required string Design { get; set; }
        public required string Description { get; set; }
    }

    public partial class UControlProjRefVerticalOptions : UserControl
    {
        public UControlProjRefVerticalOptions()
        {
            InitializeComponent();

        }

        private bool _isHorizontalDrawerOpen = false;
        private const double ClosedWidth = 32;  
        private const double OpenWidth = 500;  

        public event EventHandler<string>? CloseOtherVerticalOptions;

        // Handles the toggle button click and notifies sibling components to collapse
        private void BtnToggle_Click(object sender, RoutedEventArgs e)
        {
            ToggleDrawer();
            CloseOtherVerticalOptions?.Invoke(this, "Close");
        }

        // Improves UX by allowing the entire header area to act as a toggle trigger
        private void DrawerHeader_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ToggleDrawer();
        }

        // Public API to programmatically collapse the drawer from external controllers
        public void CloseDrawer()
        {
            if (_isHorizontalDrawerOpen)
            {
                ToggleDrawer();
            }
        }

        // Core animation logic using WPF DoubleAnimation for smooth transitions
        private void ToggleDrawer()
        {
            DoubleAnimation widthAnimation = new DoubleAnimation();
            DoubleAnimation opacityAnimation = new DoubleAnimation();

            if (!_isHorizontalDrawerOpen)
            {
                widthAnimation.From = ClosedWidth;
                widthAnimation.To = OpenWidth;
                opacityAnimation.To = 1; 
                BtnToggle.Content = "◀"; 
            }
            else
            {
                widthAnimation.From = OpenWidth;
                widthAnimation.To = ClosedWidth;
                opacityAnimation.To = 0; 
                BtnToggle.Content = "▶"; 
            }

            widthAnimation.Duration = TimeSpan.FromSeconds(0.3);
            opacityAnimation.Duration = TimeSpan.FromSeconds(0.2);

            HorizontalDrawer.BeginAnimation(Border.WidthProperty, widthAnimation);
            DrawerContentHolder.BeginAnimation(Border.OpacityProperty, opacityAnimation);

            _isHorizontalDrawerOpen = !_isHorizontalDrawerOpen;
        }

    }
}
