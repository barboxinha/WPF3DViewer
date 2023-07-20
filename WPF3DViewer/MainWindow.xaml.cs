using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace WPF3DViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string SAMPLE_MODEL_PATH = "Assets/Models/sample-model.obj";
        private string _modelPath;
        private bool _loadSampleModel;
        private readonly ModelVisual3D _visual3d;

        public MainWindow()
        {
            InitializeComponent();
            _modelPath = SAMPLE_MODEL_PATH;
            _visual3d = new ModelVisual3D() 
            {
                Content = Display3d(_modelPath)
            };
            // ***** Add to Viewport *****
            viewport3d.Children.Add(_visual3d);
        }

        /// <summary>
        /// Display 3D Model
        /// </summary>
        /// <param name="model">Model file path with '.stl' or '.obj' extension.</param>
        /// <returns>3D Model Content</returns>
        private Model3D Display3d(string model)
        {
            Model3D model3d = null;

            try
            {
                // ***** Adding a gesture *****
                viewport3d.RotateGesture = new MouseGesture(MouseAction.LeftClick);

                // ***** Import 3D model file *****
                ModelImporter import =new ModelImporter();

                // ***** Load the 3D model file *****
                model3d = import.Load(model);
            }
            catch (Exception e)
            {
                // ***** Handle exception in case it cannot find the 3D model file *****
                MessageBox.Show($"Error: {e.Message}\n\n{e.StackTrace}");
            }

            return model3d;
        }

        private void ResetViewportScene()
        {
            _visual3d.Content = Display3d(_modelPath);
            viewport3d.Children.Clear();
            viewport3d.Children.Add(_visual3d);
        }

        #region Control EventHandlers
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            string currentPath = _modelPath;

            if (_loadSampleModel)
            {
                _modelPath = SAMPLE_MODEL_PATH;
            }
            else
            {
                OpenFileDialog openFile = new OpenFileDialog()
                {
                    RestoreDirectory = true,
                    Multiselect = false,
                    Filter = "STL Files|*.stl|OBJ Files|*.obj"
                };

                if (openFile.ShowDialog(this) == true)
                {
                    _modelPath = openFile.FileName;
                } 
            }

            if (currentPath != _modelPath)
            {
                ResetViewportScene();
            }
        }

        private void cbxUseSample_Checked(object sender, RoutedEventArgs e)
        {
            _loadSampleModel = true;
        }

        private void cbxUseSample_Unchecked(object sender, RoutedEventArgs e)
        {
            _loadSampleModel = false;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
