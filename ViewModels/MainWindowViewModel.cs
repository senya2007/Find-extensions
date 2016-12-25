using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using FindExtension.Helpers;
using FindExtension.Models;
using FindExtension.Views;
using System.Linq;

namespace FindExtension.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public static  MainWindow MainWindow { get; set; }



    

        public bool IsTable
        {
            get
            {
                return ViewMode == ViewMode.Table;
            }
            set
            {
                ViewMode = value ? ViewMode.Table : ViewMode;
                RaisePropertyChanged(() => DataGridVisibility);

                if (value)
                {
                    MainWindow.dataGrid1.Visibility = Visibility.Visible;
                    MainWindow.pieChart.Visibility = Visibility.Collapsed;
                }
            }
        }

        public bool IsChart
        {
            get
            {
                return ViewMode == ViewMode.Chart;
            }
            set
            {
                ViewMode = value ? ViewMode.Chart : ViewMode;

                RaisePropertyChanged(() => DataGridVisibility);
                if (value)
                {
                    MainWindow.pieChart.Visibility = Visibility.Visible;
                    MainWindow.dataGrid1.Visibility = Visibility.Collapsed;
                }
            }
        }

        public Visibility DataGridVisibility
        {
            get
            {
                if (ViewMode == Models.ViewMode.Table)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            set
            {
            }
        }

        public bool IsCount
        {
            get
            {
                return ViewModeRow == ViewModeRow.IsCount;
            }
            set
            {
                ViewModeRow = value ? ViewModeRow.IsCount : ViewModeRow;
                RaisePropertyChanged(() => DataGridVisibility);

                if (value)
                {
                    MainWindow.dataGrid1.Columns[2].Visibility = Visibility.Collapsed;
                    MainWindow.dataGrid1.Columns[1].Visibility = Visibility.Visible;
                }
            }
        }

        public bool IsSize
        {
            get
            {
                return ViewModeRow == ViewModeRow.IsSize;
            }
            set
            {
                ViewModeRow = value ? ViewModeRow.IsSize : ViewModeRow;
                RaisePropertyChanged(() => DataGridVisibility);

                if (value)
                {
                    MainWindow.dataGrid1.Columns[2].Visibility = Visibility.Visible;
                    MainWindow.dataGrid1.Columns[1].Visibility = Visibility.Collapsed;
                }
            }
        }


     
        public Visibility DataRowVisibility
        {
            get
            {
                if (ViewModeRow == Models.ViewModeRow.IsCount)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            set
            {
            }
        }

        private ViewModeRow _viewModeRow = ViewModeRow.IsSize;
        public ViewModeRow ViewModeRow
        {
            get { return _viewModeRow; }
            set
            {
                if (_viewModeRow != value)
                {
                    _viewModeRow = value;
                    RaisePropertyChanged(() => ViewModeRow);
                    RaisePropertyChanged(() => DataRowVisibility);
                }
            }
        }

        private ViewMode _viewMode = ViewMode.Table;
        public ViewMode ViewMode
        {
            get { return _viewMode; }
            set
            {
                if (_viewMode != value)
                {
                    _viewMode = value;
                    RaisePropertyChanged(() => ViewMode);
                    RaisePropertyChanged(() => DataGridVisibility);
                }
            }
        }

        private int _progress;
        public int Progress
        {
            get { return _progress; }
            private set
            {
                if (_progress != value)
                {
                    _progress = value;
                    RaisePropertyChanged(() => Progress);
                }
            }
        }

        private ObservableCollection<ExtensionModel> _extensionCollection;
        public ObservableCollection<ExtensionModel> ExtensionCollection
        {
            get { return _extensionCollection; }
            set
            {
                if (_extensionCollection != value)
                {
                    _extensionCollection = value;
                    RaisePropertyChanged(() => ExtensionCollection);
                }
            }
        }

        public ICommand SelectRootDirCommand
        {
            get
            {
                return new DelegateCommand(SelectRootDir);
            }
        }

        public MainWindowViewModel()
        {
            ExtensionCollection = new ObservableCollection<ExtensionModel>();
            File.AppendAllText("ext.dat", "");
            var models = File.ReadAllLines("ext.dat").Select(s => new ExtensionModel(s));

            foreach (var model in models)
            {
                ExtensionCollection.Add(model);
            }
        }
     
        private void SelectRootDir()
        {
            var dialog = new FolderBrowserDialog();
            var result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                ExtensionCollection.Clear();

                var worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += (sender, e) => SearchFiles(dialog.SelectedPath, (BackgroundWorker)sender);
                worker.RunWorkerAsync();
            }
        }
        
        private void SearchFiles(string filepath, BackgroundWorker worker)
        {
            decimal totalProgress = 0;

            var dic = new Dictionary<string, ExtensionModel>();
            new FileSearcher(filepath, file =>
            {
                totalProgress += file.ProgressSize;

                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    MainWindow.ProgressBar.Value = (int)(totalProgress * 100);
                }));

                if (file.Path == null) return;

                var ext = Path.GetExtension(file.Path);

                ExtensionModel model;
                if (!dic.TryGetValue(ext, out model))
                {
                    model = new ExtensionModel
                    {
                        Ext = ext,
                        Count = 0
                    };
                    dic[ext] = model;

                    App.Current.Dispatcher.BeginInvoke(new Action(() => this.ExtensionCollection.Add(model)));
                }
                model.AddFile(file);
            }).Start();
        }
    }
}