using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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
    public partial class UControlLoadingStepsBar : UserControl
    {
        public UControlLoadingStepsBar()
        {
            InitializeComponent();
        }
        public void AnimateProgress(double targetValue)
        {
            // Criamos a animação: do valor atual até o alvo
            DoubleAnimation animation = new DoubleAnimation
            {
                From = MyProgressBar.Value,
                To = targetValue,
                Duration = TimeSpan.FromSeconds(1.5), // Tempo da transição
                DecelerationRatio = 0.9 // Suaviza a chegada no final (efeito ease-out)
            };

            // Aplicamos a animação na propriedade Value da ProgressBar
            MyProgressBar.BeginAnimation(ProgressBar.ValueProperty, animation);
        }

    }

}
