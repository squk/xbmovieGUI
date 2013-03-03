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
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;

namespace XDKCapture
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Elysium.Controls.Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            busyRing.IsEnabled = false;
            string savedXDK = Properties.Settings.Default.xdkName;
            string savedXBM = Properties.Settings.Default.xbmoviePath;
            int savedVideoFormat = Properties.Settings.Default.videoFormatIndex;

            if (savedXDK != "")
            {
                ipBox.Text = savedXDK;
            }
            if (savedXBM != "")
            {
                xbmoviePathBox.Text = savedXBM;
            }

            if (savedVideoFormat != 1337)
            {
                recordingFormat.SelectedItem = savedVideoFormat;
            }
        }

        private void recordButton_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(xbmoviePathBox.Text))
            {

            }
            else
            {
                Properties.Settings.Default.xdkName = ipBox.Text;
                Properties.Settings.Default.xbmoviePath = xbmoviePathBox.Text;
                Properties.Settings.Default.videoFormatIndex = recordingFormat.SelectedIndex;
                Properties.Settings.Default.Save();
                busyRing.IsEnabled = true;
                
                BackgroundWorker bw = new BackgroundWorker();
                bw.WorkerReportsProgress = false;

                bw.DoWork += new DoWorkEventHandler(
                delegate(object o, DoWorkEventArgs args)
                {
                    string[] A =(string[]) args.Argument;
                    BackgroundWorker b = o as BackgroundWorker;
                    string commandText = "/X:" + A[0] + " /F:" + A[1] + " " + A[2];
                    Process p = new Process();
                    p.StartInfo.FileName = A[3];
                    p.StartInfo.Arguments = commandText;
                    p.Start();
                    p.WaitForExit();
                });
                
                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                delegate(object o, RunWorkerCompletedEventArgs args)
                {
                    busyRing.IsEnabled = false;
                });

                string[] a = { ipBox.Text, recordingFormat.SelectedIndex++.ToString(), savePathBox.Text, xbmoviePathBox.Text };
                bw.RunWorkerAsync(a);
            }
        }

        private void savePathButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "recording.wmv"; 
            dlg.DefaultExt = ".wmv"; 
            dlg.Filter = "XDK Recording File (*.wmv)|*.wmv";

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                savePathBox.Text = dlg.FileName;
            }
        }

        private void xbmoviePathButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "xbmovie.exe";
            dlg.DefaultExt = ".exe";
            dlg.Filter = "xbmovie (xbmovie.exe)|xbmovie.exe";

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                xbmoviePathBox.Text = dlg.FileName;
            }
        }
    }
}
