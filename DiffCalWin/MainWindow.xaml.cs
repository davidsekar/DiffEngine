using DifferenceEngine;
using Microsoft.Win32;
using System;
using System.Collections;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DiffCalWin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            TextSourceFilePath.Text = @"C:\Users\david.gnanasekaran\Documents\source.txt";
            TextTargetFilePath.Text = @"C:\Users\david.gnanasekaran\Documents\destination.txt";
#endif
        }

        private static bool ValidFile(string fileName)
        {
            return fileName != string.Empty && File.Exists(fileName);
        }

        private void TextDiff(string sFile, string dFile)
        {
            this.Cursor = Cursors.Wait;

            DiffList_TextFile sLF = null;
            DiffList_TextFile dLF = null;
            try
            {
                sLF = new DiffList_TextFile(sFile);
                dLF = new DiffList_TextFile(dFile);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                MessageBox.Show(ex.Message, "File Error");
                return;
            }

            try
            {
                double time = 0;
                DiffEngine de = new DiffEngine();
                time = de.ProcessDiff(sLF, dLF, DiffEngineLevel.SlowPerfect);

                ArrayList rep = de.DiffReport();
                Results dlg = new Results(sLF, dLF, rep, time);
                try
                {
                    this.Hide();
                    dlg.ShowDialog();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Unexpected Error!", MessageBoxButton.OKCancel, MessageBoxImage.Error,
                        MessageBoxResult.None);
                    dlg.Close();
                }
                finally
                {
                    this.Show();
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                string tmp = string.Format("{0}{1}{1}***STACK***{1}{2}",
                    ex.Message,
                    Environment.NewLine,
                    ex.StackTrace);
                MessageBox.Show(tmp, "Compare Error");
                return;
            }
            this.Cursor = Cursors.Arrow;
        }


        private void BinaryDiff(string sFile, string dFile)
        {
            this.Cursor = Cursors.Wait;

            DiffListBinaryFile sLF = null;
            DiffListBinaryFile dLF = null;
            try
            {
                sLF = new DiffListBinaryFile(sFile);
                dLF = new DiffListBinaryFile(dFile);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                MessageBox.Show(ex.Message, "File Error");
                return;
            }

            try
            {
                double time = 0;
                DiffEngine de = new DiffEngine();
                time = de.ProcessDiff(sLF, dLF, DiffEngineLevel.SlowPerfect);

                ArrayList rep = de.DiffReport();

                //BinaryResults dlg = new BinaryResults(rep, time);
                //dlg.ShowDialog();
                //dlg.Dispose();

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                string tmp = string.Format("{0}{1}{1}***STACK***{1}{2}",
                    ex.Message,
                    Environment.NewLine,
                    ex.StackTrace);
                MessageBox.Show(tmp, "Compare Error");
                return;
            }
            this.Cursor = Cursors.Arrow;
        }

        private void CompareButton_Click(object sender, RoutedEventArgs e)
        {
            var sFile = TextSourceFilePath.Text.Trim();
            var dFile = TextTargetFilePath.Text.Trim();

            if (!ValidFile(sFile))
            {
                MessageBox.Show("Source file name is invalid.", "Invalid File");
                TextSourceFilePath.Focus();
                return;
            }

            if (!ValidFile(dFile))
            {
                MessageBox.Show("Destination file name is invalid.", "Invalid File");
                TextTargetFilePath.Focus();
                return;
            }

            if (OptionBinayCompare.IsChecked.HasValue && OptionBinayCompare.IsChecked.Value)
                BinaryDiff(sFile, dFile);
            else
                TextDiff(sFile, dFile);
        }

        private void ExitApplication_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextFilePath_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            var files = (string[]) e.Data.GetData(DataFormats.FileDrop);
            ((TextBox)sender).Text = files == null ? "" : files[0];
        }

        private void TextFilePath_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        private void FileSelectionButton_Click(object sender, RoutedEventArgs e)
        {
            var clickedButtonName = ((Button) sender).Name;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() != true) return;

            switch (clickedButtonName)
            {
                case "SourceSelectionButton":
                    TextSourceFilePath.Text = openFileDialog.FileName;
                    break;
                case "TargetSelectionButton":
                    TextTargetFilePath.Text = openFileDialog.FileName;
                    break;
            }
        }
    }
}
