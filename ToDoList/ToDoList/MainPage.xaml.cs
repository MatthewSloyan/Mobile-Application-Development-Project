using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ToDoList
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            setupList();
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            SplitViewMenu.IsPaneOpen = !SplitViewMenu.IsPaneOpen;
        }

        //navigation
        private void ShoppingMenu_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ShoppingPage));
        }

        private void WeeklyMenu_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(WeeklyPage));
        }

        private void SettingsMenu_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }

        private void setupList()
        {
            
        }

        private void Ellipse_Tapped(object sender, TappedRoutedEventArgs e)
        {
            StackPanel popUpAddItem = new StackPanel();
            popUpAddItem.Name = "stackPanelList";
            popUpAddItem.Height = 80;
            popUpAddItem.Width = 400;
            popUpAddItem.CornerRadius = new CornerRadius(5);
            popUpAddItem.SetValue(Grid.RowProperty, 6);
            popUpAddItem.SetValue(Grid.ColumnProperty, 1);
            popUpAddItem.SetValue(Grid.ColumnSpanProperty, 2);
            popUpAddItem.Background = new SolidColorBrush(Colors.Gray);
            popUpAddItem.Margin = new Thickness(5);
            popUpAddItem.Orientation = Orientation.Horizontal;
            popUpAddItem.VerticalAlignment = VerticalAlignment.Center;
            popUpAddItem.HorizontalAlignment = HorizontalAlignment.Left;
            listGrid.Children.Add(popUpAddItem);

            TextBox addListText = new TextBox();
            addListText.Name = "listText";
            addListText.Foreground = new SolidColorBrush(Colors.White);
            addListText.Background = new SolidColorBrush(Colors.Gray);
            addListText.PlaceholderText = "Please enter your list item";
            addListText.Width = 310;
            addListText.MaxLength = 40;
            addListText.Header = "Add a list item";
            addListText.Margin = new Thickness(15, 2, 10, 0);
            addListText.BorderThickness = new Thickness(2, 0, 2, 0);
            addListText.HorizontalAlignment = HorizontalAlignment.Left;
            addListText.VerticalAlignment = VerticalAlignment.Center;
            popUpAddItem.Children.Add(addListText);

            Ellipse confirmListItem = new Ellipse();
            confirmListItem.Height = 40;
            confirmListItem.Width = 40;
            confirmListItem.SetValue(Grid.ColumnProperty, 3);
            confirmListItem.HorizontalAlignment = HorizontalAlignment.Right;
            confirmListItem.Fill = new SolidColorBrush(Colors.White);
            confirmListItem.Margin = new Thickness(5);
            popUpAddItem.Children.Add(confirmListItem);
            popUpAddItem.Tapped += popUpAddItem_Tapped;

        }

        private void popUpAddItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            StackPanel hideItemPanel = FindName("stackPanelList") as StackPanel;
            hideItemPanel.Visibility = Visibility.Collapsed;

            TextBox getInputText = FindName("listText") as TextBox;
            string objTextBox = getInputText.Text;

            TextBlock addInputText = new TextBlock();
            addInputText.Text = objTextBox;
            addInputText.Foreground = new SolidColorBrush(Colors.Gray);
            addInputText.SetValue(Grid.RowProperty, 2);
            addInputText.SetValue(Grid.ColumnProperty, 1);
            addInputText.Margin = new Thickness(15, 2, 10, 0);
            addInputText.VerticalAlignment = VerticalAlignment.Center;
            listGrid.Children.Add(addInputText);

            CheckBox check1 = new CheckBox();
            check1.SetValue(Grid.RowProperty, 2);
            check1.SetValue(Grid.ColumnProperty, 2);
            check1.Margin = new Thickness(14, 3, 0, 0);
            check1.Background = new SolidColorBrush(Colors.Gray);
            listGrid.Children.Add(check1);
        }
    }
}
