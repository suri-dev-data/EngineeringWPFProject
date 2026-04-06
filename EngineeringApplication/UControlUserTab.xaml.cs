using System;
using System.Collections.Generic;
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

namespace EngineeringApplication
{
    /// <summary>
    /// Interação lógica para UControlUserTab.xam
    /// </summary>
    public partial class UControlUserTab : UserControl
    {
        private bool _isModern = true;
        public UControlUserTab()
        {
            InitializeComponent();
        }

        // Swaps global ResourceDictionaries to enable live theme switching
        public void ChangeStyle(string styleFile)
        {
            var dictParaRemover = Application.Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null && d.Source.OriginalString.EndsWith("Style.xaml"));

            if (dictParaRemover != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(dictParaRemover);
            }

            var uriPath = $"/EngineeringApplication;component/{styleFile}";

            var novoTema = new ResourceDictionary
            {
                Source = new Uri(uriPath, UriKind.RelativeOrAbsolute)
            };

            Application.Current.Resources.MergedDictionaries.Add(novoTema);
        }


        // UI trigger to toggle between "Classic/Win98" and "Modern" aesthetics
        private void ChangeStyleButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isModern)
            {
                ChangeStyle("ResourceWin98Style.xaml");
                _isModern = false;
            }
            else
            {
                ChangeStyle("ResourceModernStyle.xaml");
                _isModern = true;
            }
        }
    }
}
