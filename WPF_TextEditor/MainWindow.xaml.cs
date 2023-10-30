using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace WPF_TextEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            txtFileName.Text = $"Select Text File ==>";
        }

        private bool isAutoSaveEnabled = false;

        public object ToastNotificationManager { get; private set; }

        private void btnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = FileManage.Path;
            openFileDialog.Filter = "TXT files|*.txt";
            var result = openFileDialog.ShowDialog();

            if (result == true)
            {
                FileManage.Path = openFileDialog.FileName;
                txtFileName.Text = openFileDialog.SafeFileName;
                txtContent.IsEnabled = true;
                txtContent.Text = FileManage.ReadFile();
            }
        }

        private void btnAutoSave_Checked(object sender, RoutedEventArgs e)
        {
            isAutoSaveEnabled = true;
            txtContent.Focus();
            SystemSounds.Asterisk.Play();
            notifier.ShowInformation(Notifications.NotifAutoSaveIsChecked(isAutoSaveEnabled));
            txtContent.Focus();
        }

        private void btnAutoSave_Unchecked(object sender, RoutedEventArgs e)
        {
            isAutoSaveEnabled = false;
            SystemSounds.Asterisk.Play();
            notifier.ShowInformation(Notifications.NotifAutoSaveIsChecked(isAutoSaveEnabled));
            txtContent.Focus();
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            FileManage.WriteFile(txtContent.Text);
            SystemSounds.Exclamation.Play();    
            notifier.ShowSuccess(Notifications.NotifTextSavedSuccessfully(txtFileName.Text));
            txtContent.Focus();
        }

        private void btnCut_Click(object sender, RoutedEventArgs e)
        {
            if (!Validation.isAnyTextSelected(txtContent)) return;

            var textBox = txtContent as TextBox;
            if (textBox.SelectedText != null)
            {
                int selectionStart = textBox.SelectionStart;
                int selectionLength = textBox.SelectionLength;
                string currentText = textBox.Text;

                Clipboard.SetText(currentText.Substring(selectionStart, selectionLength));

                textBox.Text = currentText.Remove(textBox.SelectionStart, textBox.SelectionLength);
                textBox.CaretIndex = selectionStart;
            }
            txtContent.Focus();
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            if (!Validation.isAnyTextSelected(txtContent)) return;

            var textBox = txtContent as TextBox;
            int selectionStart = textBox.SelectionStart;
            int selectionLength = textBox.SelectionLength;

            if (textBox.SelectedText != null) Clipboard.SetText(textBox.Text.Substring(selectionStart, selectionLength));

            textBox.CaretIndex = selectionStart;
            txtContent.Focus();
        }

        private void btnPaste_Click(object sender, RoutedEventArgs e)
        {
            var textBox = txtContent as TextBox;
            int caretIndex = textBox.CaretIndex;
            textBox.Text = textBox.Text.Insert(caretIndex, Clipboard.GetText());
            textBox.CaretIndex = caretIndex += Clipboard.GetText().Length;
            txtContent.Focus();
        }

        private void btnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            txtContent.SelectAll();
            txtContent.Focus();
        }
        private void txtContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isAutoSaveEnabled) FileManage.WriteFile(txtContent.Text);
        }

        Notifier notifier = new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.MainWindow,
                corner: Corner.TopRight,
                offsetX: 10,
                offsetY: 10);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(3),
                maximumNotificationCount: MaximumNotificationCount.FromCount(5));

            cfg.Dispatcher = Application.Current.Dispatcher;
        });
    }
}