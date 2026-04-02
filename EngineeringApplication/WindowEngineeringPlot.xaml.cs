using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
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
using System.Windows.Shapes;

namespace EngineeringApplication
{
    public partial class WindowEngineeringPlot : Window
    {
        public PlotModel MyPlotModel { get; private set; }

        public WindowEngineeringPlot()
        {
            InitializeComponent();

            // Initialize the Plot Model with technical titles
            MyPlotModel = new PlotModel
            {
                Title = "Stress-Strain Analysis",
                Subtitle = "Comparative Material Testing",
                TitleFontSize = 18
            };

            // Configure X-Axis: Strain (Dimensionless or mm/mm)
            MyPlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Strain (ε)",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                TickStyle = TickStyle.Outside
            });

            // Configure Y-Axis: Stress (MPa)
            MyPlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Stress (σ) [MPa]",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                TickStyle = TickStyle.Outside
            });

            // --- SERIES 1: Carbon Steel 1020 (Dark Blue) ---
            var steelSeries = new LineSeries
            {
                Title = "Carbon Steel 1020",
                Color = OxyColors.DarkBlue,
                StrokeThickness = 2.5
            };
            GenerateMaterialData(steelSeries, 210000, 350, 0.20);
            MyPlotModel.Series.Add(steelSeries);

            // --- SERIES 2: Aluminum Alloy 6061 (Dark Red) ---
            var aluminumSeries = new LineSeries
            {
                Title = "Aluminum 6061-T6",
                Color = OxyColors.DarkRed,
                StrokeThickness = 2.5
            };
            GenerateMaterialData(aluminumSeries, 70000, 240, 0.12);
            MyPlotModel.Series.Add(aluminumSeries);

            // --- SERIES 3: Titanium Grade 5 (Dark Green) ---
            var titaniumSeries = new LineSeries
            {
                Title = "Titanium Ti-6Al-4V",
                Color = OxyColors.DarkGreen,
                StrokeThickness = 2.5
            };
            GenerateMaterialData(titaniumSeries, 114000, 880, 0.10);
            MyPlotModel.Series.Add(titaniumSeries);

            var l = new OxyPlot.Legends.Legend
            {
                LegendPosition = LegendPosition.RightBottom,
                LegendPlacement = LegendPlacement.Inside,
                LegendBorder = OxyColors.Black,
                LegendBackground = OxyColor.FromAColor(200, OxyColors.White),
                LegendTitle = "Materials"
            };

            MyPlotModel.Legends.Add(l);

            // Set the DataContext to 'this' so the XAML can find MyPlotModel
            this.DataContext = this;
        }

        // Helper method to simulate Stress-Strain curve points
        private void GenerateMaterialData(LineSeries series, double youngModulus, double yieldStrength, double maxStrain)
        {
            // 1. Elastic Region 
            double yieldStrain = yieldStrength / youngModulus;
            for (double x = 0; x <= yieldStrain; x += 0.0001)
            {
                series.Points.Add(new DataPoint(x, x * youngModulus));
            }

            // 2. Plastic Region 
            double currentStrain = yieldStrain;
            double step = (maxStrain - yieldStrain) / 20;

            for (int i = 1; i <= 20; i++)
            {
                currentStrain += step;
                // Simple hardening approximation
                double plasticStress = yieldStrength + (Math.Sqrt(i) * (yieldStrength * 0.2));
                series.Points.Add(new DataPoint(currentStrain, plasticStress));
            }
        }
    }
}
