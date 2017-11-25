using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.FileProperties;
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
        int _countChildren;
        String _dividerBarName = "", _inputTextName = "", _deleteName = "";

        public MainPage()
        {
            this.InitializeComponent();
        }

        //navigated to load list data
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            
            try
            {
                //test file string
                Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

                // get file
                var file = await storageFolder.GetFileAsync("list.txt");
                var readFile = await Windows.Storage.FileIO.ReadLinesAsync(file);

                int numLines = 0;

                foreach (var line in readFile)
                {
                    String inputText = line.Split('\n')[0];
                    numLines += line.Split('\n').Length;
                    Debug.WriteLine(numLines);

                    switch (numLines)
                    {
                        case 1:
                            _RowNum = 2;
                            _dividerBarName = "listDividerBar_1";
                            _inputTextName = "listTextBox_1";
                            _deleteName = "deleteImage_1";
                            break;
                        case 2:
                            _RowNum = 3;
                            _dividerBarName = "listDividerBar_2";
                            _inputTextName = "listTextBox_2";
                            _deleteName = "deleteImage_2";
                            break;
                        case 3:
                            _RowNum = 4;
                            _dividerBarName = "listDividerBar_3";
                            _inputTextName = "listTextBox_3";
                            _deleteName = "deleteImage_3";
                            break;
                        case 4:
                            _RowNum = 5;
                            _dividerBarName = "listDividerBar_4";
                            _inputTextName = "listTextBox_4";
                            _deleteName = "deleteImage_4";
                            break;
                        case 5:
                            _RowNum = 6;
                            _dividerBarName = "listDividerBar_5";
                            _inputTextName = "listTextBox_5";
                            _deleteName = "deleteImage_5";
                            break;
                        case 6:
                            _RowNum = 7;
                            _dividerBarName = "listDividerBar_6";
                            _inputTextName = "listTextBox_6";
                            _deleteName = "deleteImage_6";
                            break;
                        case 7:
                            _RowNum = 8;
                            _dividerBarName = "listDividerBar_7";
                            _inputTextName = "listTextBox_7";
                            _deleteName = "deleteImage_7";
                            break;
                        default:
                            _RowNum = 9;
                            _dividerBarName = "listDividerBar_8";
                            _inputTextName = "listTextBox_8";
                            _deleteName = "deleteImage_8";
                            break;
                    } //switch

                    Border dividerBar = new Border();
                    dividerBar.Name = _dividerBarName;
                    dividerBar.Background = new SolidColorBrush(Colors.LightGray);
                    dividerBar.SetValue(Grid.RowProperty, _RowNum);
                    dividerBar.SetValue(Grid.ColumnProperty, 1);
                    dividerBar.SetValue(Grid.ColumnSpanProperty, 2);
                    dividerBar.Margin = new Thickness(0, 0, 0, 48);
                    dividerBar.CornerRadius = new CornerRadius(1);
                    listGrid.Children.Add(dividerBar);

                    TextBlock addInputText = new TextBlock();
                    addInputText.Name = _inputTextName;
                    addInputText.Text = inputText;
                    addInputText.Foreground = new SolidColorBrush(Colors.Gray);
                    addInputText.SetValue(Grid.RowProperty, _RowNum);
                    addInputText.SetValue(Grid.ColumnProperty, 1);
                    addInputText.Margin = new Thickness(15, 2, 10, 0);
                    addInputText.VerticalAlignment = VerticalAlignment.Center;
                    listGrid.Children.Add(addInputText);

                    Image deleteList = new Image();
                    deleteList.Name = _deleteName;
                    deleteList.Source = new BitmapImage(new Uri("ms-appx:///Assets/DeleteIcon.png"));
                    deleteList.Height = 35;
                    deleteList.Width = 35;
                    deleteList.SetValue(Grid.RowProperty, _RowNum);
                    deleteList.SetValue(Grid.ColumnProperty, 2);
                    deleteList.VerticalAlignment = VerticalAlignment.Center;
                    deleteList.Margin = new Thickness(5);
                    listGrid.Children.Add(deleteList);
                    deleteList.Tapped += delete_Tapped;
                }
            }
            catch (Exception)
            {
                // Shouldn't get here 
                Debug.WriteLine("File not found");
            }
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            SplitViewMenu.IsPaneOpen = !SplitViewMenu.IsPaneOpen;
        }

        #region navigation between pages
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
        #endregion


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

        private async void popUpAddItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            TextBox getInputText = FindName("listText") as TextBox;
            string objTextBox = getInputText.Text;

            _countChildren = VisualTreeHelper.GetChildrenCount(listGrid);
            
            if (_countChildren <= 27)
            {
                switch (_countChildren)
                {
                    case 6:
                        _RowNum = 2;
                        _dividerBarName = "listDividerBar_1";
                        _inputTextName = "listTextBox_1";
                        _deleteName = "deleteImage_1";
                        break;
                    case 9:
                        _RowNum = 3;
                        _dividerBarName = "listDividerBar_2";
                        _inputTextName = "listTextBox_2";
                        _deleteName = "deleteImage_2";
                        break;
                    case 12:
                        _RowNum = 4;
                        _dividerBarName = "listDividerBar_3";
                        _inputTextName = "listTextBox_3";
                        _deleteName = "deleteImage_3";
                        break;
                    case 15:
                        _RowNum = 5;
                        _dividerBarName = "listDividerBar_4";
                        _inputTextName = "listTextBox_4";
                        _deleteName = "deleteImage_4";
                        break;
                    case 18:
                        _RowNum = 6;
                        _dividerBarName = "listDividerBar_5";
                        _inputTextName = "listTextBox_5";
                        _deleteName = "deleteImage_5";
                        break;
                    case 21:
                        _RowNum = 7;
                        _dividerBarName = "listDividerBar_6";
                        _inputTextName = "listTextBox_6";
                        _deleteName = "deleteImage_6";
                        break;
                    case 24:
                        _RowNum = 8;
                        _dividerBarName = "listDividerBar_7";
                        _inputTextName = "listTextBox_7";
                        _deleteName = "deleteImage_7";
                        break;
                    default:
                        _RowNum = 9;
                        _dividerBarName = "listDividerBar_8";
                        _inputTextName = "listTextBox_8";
                        _deleteName = "deleteImage_8";
                        break;
                } //switch

                Border dividerBar = new Border();
                dividerBar.Name = _dividerBarName;
                dividerBar.Background = new SolidColorBrush(Colors.LightGray);
                dividerBar.SetValue(Grid.RowProperty, _RowNum);
                dividerBar.SetValue(Grid.ColumnProperty, 1);
                dividerBar.SetValue(Grid.ColumnSpanProperty, 2);
                dividerBar.Margin = new Thickness(0, 0, 0, 48);
                dividerBar.CornerRadius = new CornerRadius(1);
                listGrid.Children.Add(dividerBar);

                TextBlock addInputText = new TextBlock();
                addInputText.Name = _inputTextName;
                addInputText.Text = objTextBox;
                addInputText.Foreground = new SolidColorBrush(Colors.Gray);
                addInputText.SetValue(Grid.RowProperty, _RowNum);
                addInputText.SetValue(Grid.ColumnProperty, 1);
                addInputText.Margin = new Thickness(15, 2, 10, 0);
                addInputText.VerticalAlignment = VerticalAlignment.Center;
                listGrid.Children.Add(addInputText);

                Image deleteList = new Image();
                deleteList.Source = new BitmapImage(new Uri("ms-appx:///Assets/DeleteIcon.png"));
                deleteList.Name = _deleteName;
                deleteList.Height = 35;
                deleteList.Width = 35;
                deleteList.SetValue(Grid.RowProperty, _RowNum);
                deleteList.SetValue(Grid.ColumnProperty, 2);
                deleteList.VerticalAlignment = VerticalAlignment.Center;
                deleteList.Margin = new Thickness(5);
                listGrid.Children.Add(deleteList);
                deleteList.Tapped += delete_Tapped;

                listGrid.Children.Remove(FindName("stackPanelList") as StackPanel);

                try
                {
                    Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                    Windows.Storage.StorageFile listSetFile = await storageFolder.GetFileAsync("list.txt");

                    await FileIO.AppendTextAsync(listSetFile, objTextBox + "\n");

                    //test file string
                    string text = await Windows.Storage.FileIO.ReadTextAsync(listSetFile);
                    Debug.WriteLine(text);
                }
                catch (Exception)
                {
                    // Shouldn't get here 
                    Debug.WriteLine("File not found");
                }

            } //if to limit list items to 8
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
        }
    }
}
