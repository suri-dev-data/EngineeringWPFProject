using Microsoft.Win32;
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
    public partial class WindowApp : Window
    {


        public WindowApp()
        {


            InitializeComponent();

            UserTab.ChangeStyle("ResourceModernStyle.xaml");


            EngineeringProgress.AnimateProgress(66.5);

            // Dynamic injection of a reusable DataGrid module into the UI container
            var novoModulo3 = new UControlGenericDataTable();
            PainelDeModulos3.Children.Add(novoModulo3);

            // Mocking seed data for Sensor Templates; ideally moved to a DataService in production
            var listaSensores = new List<PDTemplate>
            {
                new PDTemplate { IsSelected = true, Name = "PL 1", Description = "Lok 10 mm" },
                new PDTemplate { IsSelected = false, Name = "PL 2", Description = "Lok 12 mm" },
                new PDTemplate { IsSelected = false, Name = "PL 3", Description = "Lok 14 mm" }
            };
            foreach (var item in listaSensores)
            {
                item.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(PDTemplate.IsSelected))
                    {
                        var changedItem = s as PDTemplate;
                        if (changedItem != null && changedItem.IsSelected)
                        {
                            foreach (var otherItem in listaSensores)
                            {
                                if (otherItem != changedItem)
                                {
                                    otherItem.IsSelected = false;
                                }
                            }
                        }
                    }
                };
            }
            LocalVerticalOptions.DataGridSensores.ItemsSource = listaSensores;

            // Mocking seed data for Design References
            var projDesigns = new List<PDReference>
            {
                new PDReference { IsSelected = true, Ref = "REF-001", Design = "PL 1", Description = "Lok 10 mm - High Tension" },
                new PDReference { IsSelected = false, Ref = "REF-002", Design = "PL 2", Description = "Lok 12 mm - Standard" },
                new PDReference { IsSelected = false, Ref = "REF-003", Design = "PL 3", Description = "Lok 14 mm - Reinforced" },
                new PDReference { IsSelected = false, Ref = "REF-004", Design = "PL 4", Description = "Custom Alloy 8mm" }
            };
            foreach (var item in projDesigns)
            {
                item.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(PDReference.IsSelected))
                    {
                        var changedItem = s as PDReference;
                        if (changedItem != null && changedItem.IsSelected)
                        {
                            foreach (var otherItem in projDesigns)
                            {
                                if (otherItem != changedItem)
                                {
                                    otherItem.IsSelected = false;
                                }
                            }
                        }
                    }
                };
            }
            LocalPRVerticalOptions.DataGridSensores.ItemsSource = projDesigns;

            // ViewModel instantiation and seeding for pipe layer specifications
            var viewModelLayers = new PerformanceViewModel();
            viewModelLayers.LayersCollection.Add(new LayerModel { LayerType = "Internal Carcass", Material = "Carbon Steel", Wires = 1, Angle = 85.0, Profile = "Interlocked", Thick = 4.0, ID = 101.6, OD = 109.6 });
            viewModelLayers.LayersCollection.Add(new LayerModel { LayerType = "Pressure Armor", Material = "Carbon Steel", Wires = 42, Angle = 88.0, Profile = "Z-Shape", Thick = 5.5, ID = 115.0, OD = 126.0 });
            viewModelLayers.LayersCollection.Add(new LayerModel { LayerType = "Anti-Wear Tape", Material = "HDPE", Wires = 0, Angle = 20.0, Profile = "Tape", Thick = 1.5, ID = 126.1, OD = 129.1 });
            viewModelLayers.LayersCollection.Add(new LayerModel { LayerType = "Tensile Armor 1", Material = "Carbon Steel", Wires = 50, Angle = 35.0, Profile = "Rectangular", Thick = 3.0, ID = 129.2, OD = 135.2 });
            viewModelLayers.LayersCollection.Add(new LayerModel { LayerType = "Tensile Armor 2", Material = "Carbon Steel", Wires = 50, Angle = -35.0, Profile = "Rectangular", Thick = 3.0, ID = 135.3, OD = 141.3 });
            viewModelLayers.LayersCollection.Add(new LayerModel { LayerType = "External Sheath", Material = "HDPE", Wires = 1, Angle = 0.0, Profile = "Extruded", Thick = 6.0, ID = 141.4, OD = 153.4 });
            novoModulo3.DataContext = viewModelLayers;

        }

        // Event handler to orchestrate sidebar "exclusive-open" logic (Panel B triggers Panel A close)
        public void ACloseB(object sender, string templateName)
        {
            LocalPRVerticalOptions.CloseDrawer();
        }

        // Event handler to orchestrate sidebar "exclusive-open" logic (Panel A triggers Panel B close)
        public void BCloseA(object sender, string templateName)
        {
            LocalVerticalOptions.CloseDrawer();
        }

        // Instantiates and displays the technical analysis plot as a modal dialog
        public void OpenAnalysis(object sender, RoutedEventArgs e)
        {
            EngineeringProgress.AnimateProgress(100);
            var novaJanela = new WindowEngineeringPlot();
            novaJanela.Owner = Window.GetWindow(this);
            novaJanela.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            novaJanela.ShowDialog();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }

}