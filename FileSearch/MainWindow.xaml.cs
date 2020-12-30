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
using WinForms = System.Windows.Forms;

namespace FileSearch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
         public MainWindow()
        {
            InitializeComponent();
        }

        private void MainView_Loaded(object sender, RoutedEventArgs e)
        {
            //load contents
            WorkingFolderInfoView.Text = Properties.Settings.Default.WorkFolder;
            FileMaskInput.Text = Properties.Settings.Default.FileMask;
            
        }

        private void MainView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        { 
            //save contents
            
            Properties.Settings.Default.FileMask = FileMaskInput.Text;
            Properties.Settings.Default.Save();
        }

        private void ShowFolderBrowserButton_Click(object sender, RoutedEventArgs e)
        {
            var FolderPicker = new WinForms.FolderBrowserDialog() {ShowNewFolderButton =false };
            if (FolderPicker.ShowDialog() == WinForms.DialogResult.OK) Properties.Settings.Default.WorkFolder = FolderPicker.SelectedPath;
            Properties.Settings.Default.Save();
        }
}
}
