using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

    public class PDTemplate : INotifyPropertyChanged
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        public required string Name { get; set; }
        public required string Description { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public partial class UControlVerticalOptions : UserControl
    {
        public UControlVerticalOptions()
        {
            InitializeComponent();
        }

        private bool _isHorizontalDrawerOpen = false;
        private const double ClosedWidth = 32;  
        private const double OpenWidth = 500;   


        public event EventHandler<string>? CloseOtherVerticalOptions;

        // Handles the toggle button and broadcasts the close signal to other components
        private void BtnToggle_Click(object sender, RoutedEventArgs e)
        {
            ToggleDrawer();
            CloseOtherVerticalOptions?.Invoke(this, "Close");
        }

        // UX enhancement: allows toggling by clicking the header bar area
        private void DrawerHeader_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ToggleDrawer();
        }

        // Public method to force collapse the drawer from external logic
        public void CloseDrawer()
        {
            if (_isHorizontalDrawerOpen)
            {
                ToggleDrawer();
            }
        }

        // Manages the expansion/collapse transitions using property-based animations
        public void ToggleDrawer()
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
