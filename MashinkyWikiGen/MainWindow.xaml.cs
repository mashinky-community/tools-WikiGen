using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MashinkyWikiGen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// The main window provides a simple interface with buttons to generate vehicle and building wiki resources.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The central resources manager that handles all data processing and generation.
        /// </summary>
        private Resources resources;
        
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            resources = new Resources();
            DataContext = resources;
        }

        /// <summary>
        /// Handles the Generate Vehicles button click event.
        /// Initiates the generation of all vehicle-related wiki resources.
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">The event arguments</param>
        private void GenerateVehiclesButton_Click(object sender, RoutedEventArgs e)
        {
            resources.GenerateAllVehicles();
        }

        /// <summary>
        /// Handles the Generate Buildings button click event.
        /// Initiates the generation of all building-related wiki resources.
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">The event arguments</param>
        private void GenerateBuildingsButton_Click(object sender, RoutedEventArgs e)
        {
            resources.GenerateAllBuildings();
        }
    }
}
