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
using FileSearch.Model;
using System.Text.RegularExpressions;

namespace FileSearch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private volatile View.InfoBlock UiInfo;
        private System.Timers.Timer InfoPoll = new System.Timers.Timer() { Interval = 100};
        public static volatile ObservableCollection<FoundObject> RootItems = new ObservableCollection<FoundObject>();
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
        }

        private void UIUpdate(object sender, System.Timers.ElapsedEventArgs e)
        {
            string CookedInfo = string.Format("Времени с запуска: {0} \nТекущая директория поиска: {1}\nФайлов всего: {2} / Найдено: {3}", UiInfo.GetTimePassed(), UiInfo.CurrentDirectory, UiInfo.FilesTotal, UiInfo.FilesFound);
            InfoOutput.Dispatcher.BeginInvoke((Action)(() => InfoOutput.Text = CookedInfo));
            if (Worker == null) return;
            string stmp;
            if (Worker.IsPaused && Worker.IsBusy) stmp = "Продолжить";
            else stmp = "Старт";
            if(Worker.IsBusy && !Worker.IsPaused) stmp = "Пауза";
            StartPauseButton.Dispatcher.BeginInvoke((Action)(() => StartPauseButton.Content = stmp));
        }

        private async void MainView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        { 
            //save contents
            
            Properties.Settings.Default.FileMask = FileMaskInput.Text;
            Properties.Settings.Default.Save();

            Worker.CancelWork();
            await Task.Run(()=>
            {
                while (Worker.IsBusy) ; //gracefull stop without throwing exception
            });
        }

        private void ShowFolderBrowserButton_Click(object sender, RoutedEventArgs e)
        {
            var FolderPicker = new WinForms.FolderBrowserDialog() {ShowNewFolderButton =false };
            if (FolderPicker.ShowDialog() == WinForms.DialogResult.OK) Properties.Settings.Default.WorkFolder = FolderPicker.SelectedPath;
            Properties.Settings.Default.Save();
            WorkingFolderInfoView.Text = Properties.Settings.Default.WorkFolder;
        }

        private void StartPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if(Worker!=null)
            {
                if (Worker.IsBusy && Worker.IsPaused)
                {
                    Worker.ResumeWork();
                    return;
                }
                if (Worker.IsBusy && !Worker.IsPaused)
                {
                    Worker.PauseWork();
                    return;
                }

            }
            Properties.Settings.Default.FileMask = FileMaskInput.Text;
            Properties.Settings.Default.Save();
            UiInfo = new View.InfoBlock();
            InfoPoll.Start();
            UIUpdate(null,null);
            CancelButton.IsEnabled = true;
            RootItems.Clear();
            Worker = new SearchWorker(Properties.Settings.Default.WorkFolder, new Regex(Properties.Settings.Default.FileMask,RegexOptions.IgnoreCase), RootItems);
            Worker.Searcher.LogPass += UpdateUIData;
            Worker.WorkerCompleted += UIOnWorkerCompleted;
            Worker.DoWork();
        }

        private void UIOnWorkerCompleted(object sender, EventArgs e)
        {
            InfoPoll.Stop();
            UIUpdate(null,null);
           CancelButton.Dispatcher.BeginInvoke((Action)(()=> CancelButton.IsEnabled = false));
        }

        private void UpdateUIData(object sender, FoundObjectFactoryEventArgs e)
        {
            UiInfo.CurrentDirectory = e.CurrentFolder;
            UiInfo.FilesFound = e.FilesFound;
            UiInfo.FilesTotal = e.FilesTotal;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Worker.CancelWork();
        }
    }
}
