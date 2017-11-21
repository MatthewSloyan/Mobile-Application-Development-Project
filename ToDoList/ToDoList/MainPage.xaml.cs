using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
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
        //global variables
        int _RowNum;
        int _ColumnNum;

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
            popUpAddItem.SetValue(Grid.RowProperty, 13);
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

            Image confirmListItem = new Image();
            confirmListItem.Source = new BitmapImage(new Uri("ms-appx:///Assets/AddIconWhite.png"));
            confirmListItem.Height = 40;
            confirmListItem.Width = 40;
            confirmListItem.SetValue(Grid.ColumnProperty, 3);
            confirmListItem.HorizontalAlignment = HorizontalAlignment.Right;
            confirmListItem.Margin = new Thickness(5);
            popUpAddItem.Children.Add(confirmListItem);
            popUpAddItem.Tapped += popUpAddItem_Tapped;

        }

        private void popUpAddItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int countChildren;
            String dividerBarName = "", inputTextName = "", deleteName = "";

            //StackPanel hideItemPanel = FindName("stackPanelList") as StackPanel;
            //hideItemPanel.Visibility = Visibility.Collapsed;
            
            TextBox getInputText = FindName("listText") as TextBox;
            string objTextBox = getInputText.Text;

            countChildren = VisualTreeHelper.GetChildrenCount(listGrid);
            //Debug.WriteLine(countChildren);
            if(countChildren <= 27) {
                switch (countChildren)
                {
                    case 6:
                        _RowNum = 2;
                        dividerBarName = "listDividerBar_1";
                        inputTextName = "listTextBox_1";
                        deleteName = "deleteImage_1";
                        break;
                    case 9:
                        _RowNum = 3;
                        dividerBarName = "listDividerBar_2";
                        inputTextName = "listTextBox_2";
                        deleteName = "deleteImage_2";
                        break;
                    case 12:
                        _RowNum = 4;
                        dividerBarName = "listDividerBar_3";
                        inputTextName = "listTextBox_3";
                        deleteName = "deleteImage_3";
                        break;
                    case 15:
                        _RowNum = 5;
                        dividerBarName = "listDividerBar_4";
                        inputTextName = "listTextBox_4";
                        deleteName = "deleteImage_4";
                        break;
                    case 18:
                        _RowNum = 6;
                        dividerBarName = "listDividerBar_5";
                        inputTextName = "listTextBox_5";
                        deleteName = "deleteImage_5";
                        break;
                    case 21:
                        _RowNum = 7;
                        dividerBarName = "listDividerBar_6";
                        inputTextName = "listTextBox_6";
                        deleteName = "deleteImage_6";
                        break;
                    case 24:
                        _RowNum = 8;
                        dividerBarName = "listDividerBar_7";
                        inputTextName = "listTextBox_7";
                        deleteName = "deleteImage_7";
                        break;
                    default:
                        _RowNum = 9;
                        dividerBarName = "listDividerBar_8";
                        inputTextName = "listTextBox_8";
                        deleteName = "deleteImage_8";
                        break;
                } //switch

                Border dividerBar = new Border();
                dividerBar.Name = dividerBarName;
                dividerBar.Background = new SolidColorBrush(Colors.LightGray);
                dividerBar.SetValue(Grid.RowProperty, _RowNum);
                dividerBar.SetValue(Grid.ColumnProperty, 1);
                dividerBar.SetValue(Grid.ColumnSpanProperty, 2);
                dividerBar.Margin = new Thickness(0, 0, 0, 48);
                dividerBar.CornerRadius = new CornerRadius(1);
                listGrid.Children.Add(dividerBar);

                TextBlock addInputText = new TextBlock();
                addInputText.Name = inputTextName;
                addInputText.Text = objTextBox;
                addInputText.Foreground = new SolidColorBrush(Colors.Gray);
                addInputText.SetValue(Grid.RowProperty, _RowNum);
                addInputText.SetValue(Grid.ColumnProperty, 1);
                addInputText.Margin = new Thickness(15, 2, 10, 0);
                addInputText.VerticalAlignment = VerticalAlignment.Center;
                listGrid.Children.Add(addInputText);
            
                Image deleteList = new Image();
                deleteList.Source = new BitmapImage(new Uri("ms-appx:///Assets/DeleteIcon.png"));
                deleteList.Name = deleteName;
                deleteList.Height = 35;
                deleteList.Width = 35;
                deleteList.SetValue(Grid.RowProperty, _RowNum);
                deleteList.SetValue(Grid.ColumnProperty, 2);
                deleteList.VerticalAlignment = VerticalAlignment.Center;
                deleteList.Margin = new Thickness(5);
                listGrid.Children.Add(deleteList);
                deleteList.Tapped += delete_Tapped;

                listGrid.Children.Remove(FindName("stackPanelList") as StackPanel);
            } //if
        }

        private void delete_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Image currentImage = (Image)sender;
            String currentObj = currentImage.Name;
            string output = currentObj.Substring(currentObj.Length - 1, 1);

            if (currentObj == "deleteImage_" + output)
            {
                //object is returned, so remove
                listGrid.Children.Remove(FindName("listDividerBar_" + output) as Border);
                listGrid.Children.Remove(FindName("listTextBox_" + output) as TextBlock);
                listGrid.Children.Remove(FindName("deleteImage_" + output) as Image);
            }

            //Border currentBorder = (Border)sender;
            //listGrid.Children.Remove(currentBorder as Border);

            //ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            //Array data = new Array();

            //Add values you want to store
            String[] data = new String[8];
            data[0] = "Hello";
            data[1] = "Hi";
            data[2] = "Matthew";

            //Change to string and save to local Storage
            //toString will convert the array to a string with values separated by a comma
            localSettings.Values["someSettingInStorage"] = data.ToString();
            Debug.WriteLine(data);

            ////To retrieve the stored string value as a usable array
            //if (localSettings.Values["someSetting"] != null)
            //    data = (localSettings.Values["someSetting"]).Split();
        }
    }
}
