using System.Windows;
using System.Windows.Input;
using SmartMealWPF.Domain.Entities;
using SmartMealWPF.Helpers;
using SmartMealWPF.Presentation.ViewModels;

namespace SmartMealWPF.Presentation.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MinButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2) return;
            DragMove();
        }
        
        private void OnlyText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !ValidationInputHelper.Validate(e.Text);
        }
        
        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string))!;
                if (!ValidationInputHelper.Validate(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
    }
}