using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
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
    public partial class UControlGenericDataTable : UserControl
    {
        private Point _startPoint;

        // Strongly-typed access to the DataContext for clean MVVM interaction
        private PerformanceViewModel VM => (PerformanceViewModel)this.DataContext;

        public UControlGenericDataTable()
        {
            InitializeComponent();

            // Wire up low-level mouse events to implement custom Drag-and-Drop behavior
            MainDataGrid.PreviewMouseLeftButtonDown += DataGrid_MouseLeftButtonDown;
            MainDataGrid.MouseMove += DataGrid_MouseMove;
        }

        // Captures initial click coordinates to prevent accidental drag triggers
        private void DataGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(null);
        }

        // Logic to detect valid drag movement based on system-defined minimum distance
        private void DataGrid_MouseMove(object sender, MouseEventArgs e)
        {
            Vector diff = _startPoint - e.GetPosition(null);

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                 Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                var row = FindVisualParent<DataGridRow>(e.OriginalSource as DependencyObject);
                if (row?.DataContext is LayerModel item)
                {
                    DragDrop.DoDragDrop(row, item, DragDropEffects.Move);
                }
            }
        }

        // Handles data drop, ensuring source and target are distinct before reordering
        private void DataGrid_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(LayerModel)) is LayerModel source &&
                (e.OriginalSource as FrameworkElement)?.DataContext is LayerModel target)
            {
                if (!ReferenceEquals(source, target))
                    VM.ReorderLayers(source, target);
            }
        }

        // Traverses the Visual Tree upwards to find a specific parent container type
        private static T? FindVisualParent<T>(DependencyObject? child) where T : DependencyObject
        {
            while (child != null && !(child is T))
                child = VisualTreeHelper.GetParent(child);
            return child as T;
        }

        // Routes UI button clicks (Copy/Delete) to the corresponding ViewModel logic
        private void BtnAction_Click(object sender, RoutedEventArgs e)
        {
            if (!((sender as Button)?.DataContext is LayerModel item)) return;

            if ((sender as Button)?.Name == "BtnCopy") VM.CopyLayer(item);
            else VM.DeleteLayer(item);
        }


        private void BtnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            // Initialize the OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Filter for specific engineering files (e.g., CSV, Excel, TXT)
            openFileDialog.Filter = "Data files (*.csv;*.txt)|*.csv;*.txt|All files (*.*)|*.*";
            openFileDialog.Title = "Select Engineering Data File";

            // Show the dialog and check if the user clicked "Open"
            if (openFileDialog.ShowDialog() == true)
            {
                // Update the Label content with the local file path
                lblFilePath.Text = openFileDialog.FileName;

                // Optional: Change label color to indicate success
                lblFilePath.Foreground = System.Windows.Media.Brushes.Black;
                lblFilePath.FontStyle = FontStyles.Normal;
            }
        }

    }

    /// <summary>
    /// Represents the physical properties of a single pipe or cable layer.
    /// Acts as the DTO (Data Transfer Object) for the engineering specifications.
    /// </summary>
    public class LayerModel : INotifyPropertyChanged
    {
        private double _thick;
        private double _id;
        private double _od;

        public string LayerType { get; set; } = string.Empty;
        public string Material { get; set; } = string.Empty;
        public int Wires { get; set; }
        public double Angle { get; set; }
        public string Profile { get; set; } = string.Empty;
        public double Thick
        {
            get => _thick;
            set
            {
                _thick = value;
                OnPropertyChanged();
                UpdateOD();
            }
        }

        public double ID
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
                UpdateOD();
            }
        }

        public double OD
        {
            get => _od;
            set
            {
                _od = value;
                OnPropertyChanged();
            }
        }

        private void UpdateOD()
        {
            OD = ID + (2 * Thick);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    /// <summary>
    /// Coordinates UI logic and state management for layer performance analysis.
    /// Implements core CRUD and reordering logic for the Layers collection.
    /// </summary>
    public class PerformanceViewModel
    {
        public ObservableCollection<LayerModel> LayersCollection { get; set; } = new();

        public void AddLayer(LayerModel layer) => LayersCollection.Add(layer);

        public void DeleteLayer(LayerModel layer) => LayersCollection.Remove(layer);

        public void CopyLayer(LayerModel layer)
        {
            if (layer == null) return;
            var clone = new LayerModel
            {
                LayerType = layer.LayerType + " (Copy)",
                Material = layer.Material,
                Wires = layer.Wires,
                Angle = layer.Angle,
                Profile = layer.Profile,
                Thick = layer.Thick,
                ID = layer.ID,
                OD = layer.OD
            };
            LayersCollection.Add(clone);
        }

        public void ReorderLayers(LayerModel source, LayerModel target)
        {
            int oldIndex =  LayersCollection.IndexOf(source);
            int newIndex =  LayersCollection.IndexOf(target);
            if (oldIndex >= 0 && newIndex >= 0)  LayersCollection.Move(oldIndex, newIndex);
        }

    }
}
