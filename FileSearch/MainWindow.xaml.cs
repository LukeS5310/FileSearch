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
using System.Collections.ObjectModel;

namespace FileSearch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private volatile UI.InfoBlock UiInfo = new UI.InfoBlock();
        private System.Timers.Timer InfoPoll = new System.Timers.Timer() { Interval = 100 };
        public static volatile ObservableCollection<Model.FoundObject> RootItems = new ObservableCollection<Model.FoundObject>();
        private Model.SearchWorker Worker;
         public MainWindow()
        {
            InitializeComponent();
            ResultTree.DataContext = this;
        }

        private void MainView_Loaded(object sender, RoutedEventArgs e)
        {
           
            //load contents
            WorkingFolderInfoView.Text = Properties.Settings.Default.WorkFolder;
            FileMaskInput.Text = Properties.Settings.Default.FileMask;
            //init poll
            InfoPoll.Elapsed += UIUpdate;
            InfoPoll.Start();
         

        }

        private void UIUpdate(object sender, System.Timers.ElapsedEventArgs e)
        {
            string CookedInfo = string.Format("Времени с запуска: {0} \nТекущая директория поиска: {1}\nПросмотрено: {2} / Найдено: {3}", "00:00", UiInfo.CurrentDirectory, UiInfo.FilesTotal, UiInfo.FilesFound);
            InfoOutput.Dispatcher.BeginInvoke((Action)(() => InfoOutput.Text = CookedInfo));
            
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

        private void StartPauseButton_Click(object sender, RoutedEventArgs e)
        {
            RootItems.Clear();
            //RootItems = new Model.FoundObjectFactory("C:\\").PerformSearch("*.exe");
            Worker = new Model.SearchWorker("D:\\", "*.jpg", RootItems);
            Worker.DoWork();
            
           
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
