using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ToDoList
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WeeklyPage : Page
    {
        public WeeklyPage()
        {
            this.InitializeComponent();
        }

        //global variables
        int _RowNum;
        int _countChildren;
        String _listStackPanel = "", _inputTextName = "", _finalName, _colour;
        Ellipse currentColour;
       
        //List to add list items to allow removal and saving to file
        List<String> listData = new List<String>();
        List<String> colourData = new List<String>();

        #region navigation between pages
        //navigation
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            SplitViewMenu.IsPaneOpen = !SplitViewMenu.IsPaneOpen;
        }

        //navigation
        private void PersonalMenu_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void ShoppingMenu_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ShoppingPage));
        }

        private void SettingsMenu_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }
        #endregion

        #region navigated to method
        //navigated to, which load list data from file
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            //try to access the file
            try
            {
                // read in list item text file
                Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

                var colourFile = await storageFolder.GetFileAsync("colourList.txt");
                var readColourFile = await Windows.Storage.FileIO.ReadLinesAsync(colourFile);

                var file = await storageFolder.GetFileAsync("weeklyList.txt");
                var readFile = await Windows.Storage.FileIO.ReadLinesAsync(file);

                int numLines = 0;
                int colourNumLines = 0;
                listData.Clear(); //clears the list to re-add
                colourData.Clear();

                //Debug.WriteLine("Yeah");

                foreach (var line in readColourFile)
                {
                    String inputColour = line.Split('\n')[0]; //splits the line when new line encountered
                    colourNumLines += line.Split('\n').Length; //gets number of lines to determine where to place list 
                    
                    //sets values for list items using number of lines in the file
                    _RowNum = colourNumLines + 1;

                    _listStackPanel = "listSp_" + (colourNumLines - 1);

                    createColourSpFromFile(inputColour);
                }
                
                foreach (var line in readFile)
                {
                    String inputText = line.Split('\n')[0]; //splits the line when new line encountered
                    numLines += line.Split('\n').Length; //gets number of lines to determine where to place list 

                    //sets values for list items using number of lines in the file
                    _RowNum = numLines + 1;
                    _inputTextName = "listTextBox_" + (numLines - 1);
                    _finalName = "listTextBox_" + (numLines - 1);

                    //calls create list item method
                    createListItem(inputText);
                }
            }
            catch (Exception)
            {
                // Shouldn't get here 
                Debug.WriteLine("File not found");
            }
        }

        
        #region navigated from method
        //when navigated from it saves list to file
        protected override async void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            try
            {
                // Create file; replace if exists.
                Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile listFile = await storageFolder.CreateFileAsync("weeklyList.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);
                Windows.Storage.StorageFile cFile = await storageFolder.CreateFileAsync("colourList.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);

                for (int i = 0; i < listData.Count; i++)
                {
                    await FileIO.AppendTextAsync(listFile, listData[i] + "\n");
                    await FileIO.AppendTextAsync(cFile, colourData[i] + "\n");
                }
            }
            catch (Exception)
            {
                // Shouldn't get here 
                Debug.WriteLine("File not found");
            }
        }
        #endregion

        //when add list item image is selected, show text box
        private void Ellipse_Tapped(object sender, TappedRoutedEventArgs e)
        {
            StackPanel popUpAddItem = new StackPanel();
            popUpAddItem.Name = "stackPanelList";
            popUpAddItem.Height = 75;
            popUpAddItem.Width = 400;
            popUpAddItem.CornerRadius = new CornerRadius(5);
            popUpAddItem.SetValue(Grid.RowProperty, 13);
            popUpAddItem.SetValue(Grid.ColumnProperty, 1);
            popUpAddItem.Background = new SolidColorBrush(Colors.Gray);
            popUpAddItem.Margin = new Thickness(5);
            popUpAddItem.Orientation = Orientation.Horizontal;
            popUpAddItem.VerticalAlignment = VerticalAlignment.Center;
            popUpAddItem.HorizontalAlignment = HorizontalAlignment.Left;
            listGrid.Children.Add(popUpAddItem);

            //add text box to allow input
            TextBox addListText = new TextBox();
            addListText.Name = "listText";
            addListText.Foreground = new SolidColorBrush(Colors.White);
            addListText.Background = new SolidColorBrush(Colors.Gray);
            addListText.PlaceholderText = "Please enter your list item";
            addListText.Width = 310;
            addListText.MaxLength = 50;
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
            confirmListItem.HorizontalAlignment = HorizontalAlignment.Right;
            confirmListItem.Margin = new Thickness(5);
            popUpAddItem.Children.Add(confirmListItem);

            popUpAddItem.Tapped += popUpAddItem_Tapped;

            StackPanel colourSp = new StackPanel();
            colourSp.Name = "spColour";
            colourSp.Height = 40;
            colourSp.Width = 400;
            colourSp.CornerRadius = new CornerRadius(5);
            colourSp.SetValue(Grid.RowProperty, 9);
            colourSp.SetValue(Grid.ColumnProperty, 1);
            colourSp.Margin = new Thickness(5, 0, 5, 0);
            colourSp.Orientation = Orientation.Horizontal;
            colourSp.VerticalAlignment = VerticalAlignment.Center;
            colourSp.HorizontalAlignment = HorizontalAlignment.Left;
            listGrid.Children.Add(colourSp);

            //adds a text block with input text
            TextBlock addColourText = new TextBlock();
            addColourText.Text = "Please select a colour: ";
            addColourText.Foreground = new SolidColorBrush(Colors.Gray);
            addColourText.SetValue(Grid.RowProperty, _RowNum);
            addColourText.SetValue(Grid.ColumnProperty, 1);
            addColourText.Margin = new Thickness(25, 0, 0, 0);
            addColourText.VerticalAlignment = VerticalAlignment.Center;
            colourSp.Children.Add(addColourText);
            
            for (int i = 0; i < 4; i++)
            {
                //add the coloured pegs to the board
                Ellipse selectColour = new Ellipse();
                selectColour.Height = 20;
                selectColour.Width = 20;
                selectColour.Margin = new Thickness(15, 0, 0, 0);

                switch (i)
                {
                    case 0:
                        Color LightBlue = Color.FromArgb(255, 54, 192, 255);
                        selectColour.Fill = new SolidColorBrush(LightBlue);
                        selectColour.Name = "Blue";
                        break;
                    case 1:
                        selectColour.Fill = new SolidColorBrush(Colors.LightSeaGreen);
                        selectColour.Name = "Green";
                        break;
                    case 2:
                        selectColour.Fill = new SolidColorBrush(Colors.DarkSlateBlue);
                        selectColour.Name = "DarkBlue";
                        break;
                    default:
                        selectColour.Fill = new SolidColorBrush(Colors.Goldenrod);
                        selectColour.Name = "Gold";
                        break;
                }

                colourSp.Children.Add(selectColour);
                selectColour.Tapped += selectColour_Tapped;
            }
            
        }

        private void selectColour_Tapped(object sender, TappedRoutedEventArgs e)
        {
            currentColour = (Ellipse)sender;
            _colour = currentColour.Name;
            colourData.Add(_colour); 

            listGrid.Children.Remove(FindName("spColour") as StackPanel);
        }

        //add list item when add icon is tapped
        private void popUpAddItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_colour == "Blue" || _colour == "Green" || _colour == "DarkBlue" || _colour == "Gold")
            { 
                //gets textbox object to get text from
                TextBox getInputText = FindName("listText") as TextBox;
                string objTextBox = getInputText.Text;

                //counts the number of children to determine where to place list
                _countChildren = VisualTreeHelper.GetChildrenCount(listGrid);

                if (_countChildren <= 23)
                {
                    switch (_countChildren)
                    {
                        case 9:
                            //sets name for each list item to allow removal
                            _RowNum = 2;
                            _listStackPanel = "listSp_0";
                            _inputTextName = "listTextBox_0";
                            break;
                        case 11:
                            _RowNum = 3;
                            _listStackPanel = "listSp_1";
                            _inputTextName = "listTextBox_1";
                            break;
                        case 13:
                            _RowNum = 4;
                            _listStackPanel = "listSp_2";
                            _inputTextName = "listTextBox_2";
                            break;
                        case 15:
                            _RowNum = 5;
                            _listStackPanel = "listSp_3";
                            _inputTextName = "listTextBox_3";
                            break;
                        case 17:
                            _RowNum = 6;
                            _listStackPanel = "listSp_4";
                            _inputTextName = "listTextBox_4";
                            break;
                        case 19:
                            _RowNum = 7;
                            _listStackPanel = "listSp_5";
                            _inputTextName = "listTextBox_5";
                            break;
                        case 21:
                            _RowNum = 8;
                            _listStackPanel = "listSp_6";
                            _inputTextName = "listTextBox_6";
                            break;
                        default:
                            _RowNum = 9;
                            _listStackPanel = "listSp_7";
                            _inputTextName = "listTextBox_7";
                            break;
                    } //switch

                    _finalName = _inputTextName;

                    //calls create list item method
                    createColourSp();
                    createListItem(objTextBox);

                    //remove the add list text box from view
                    listGrid.Children.Remove(FindName("stackPanelList") as StackPanel);

                } //if to limit list items to 9
            } //outer if for validation
        }

        //creates the coloured border when user enters input
        private void createColourSp()
        {
            Border listB = new Border();
            listB.Name = _listStackPanel;
            listB.Height = 35;
            listB.CornerRadius = new CornerRadius(5);
            listB.SetValue(Grid.RowProperty, _RowNum);
            listB.SetValue(Grid.ColumnProperty, 1);
            listB.Margin = new Thickness(0, 10, 0, 0);
            listB.VerticalAlignment = VerticalAlignment.Center;
            listB.Background = currentColour.Fill;

            listGrid.Children.Add(listB);
        }

        //creates the coloured border when loading from file
        private void createColourSpFromFile(string inputColour)
        {
            Border listB = new Border();
            listB.Name = _listStackPanel;
            listB.Height = 35;
            listB.CornerRadius = new CornerRadius(5);
            listB.SetValue(Grid.RowProperty, _RowNum);
            listB.SetValue(Grid.ColumnProperty, 1);
            listB.Margin = new Thickness(0, 10, 0, 0);
            listB.VerticalAlignment = VerticalAlignment.Center;

            switch (inputColour)
            {
                case "Blue":
                    Color LightBlue = Color.FromArgb(255, 54, 192, 255);
                    listB.Background = new SolidColorBrush(LightBlue);
                    break;
                case "Green":
                    listB.Background = new SolidColorBrush(Colors.LightSeaGreen);
                    break;
                case "DarkBlue":
                    listB.Background = new SolidColorBrush(Colors.DarkSlateBlue);
                    break;
                default:
                    listB.Background = new SolidColorBrush(Colors.Goldenrod);
                    break;
            }

            colourData.Add(inputColour);
            listGrid.Children.Add(listB);
        }
        #endregion

        //create list item and place in determined row
        private void createListItem(string inputText)
        {
            if (!(inputText == "")) //input validation to check if list is not empty
            {
                //adds a text block with input text
                TextBlock addInputText = new TextBlock();
                addInputText.Name = _inputTextName;
                addInputText.Text = inputText;
                addInputText.Foreground = new SolidColorBrush(Colors.White);
                addInputText.SetValue(Grid.RowProperty, _RowNum);
                addInputText.SetValue(Grid.ColumnProperty, 1);
                addInputText.Margin = new Thickness(15, 9, 0, 0);
                addInputText.VerticalAlignment = VerticalAlignment.Center;
                listGrid.Children.Add(addInputText);

                listData.Add(inputText); //add text to list to save to file
            } //if
        }

        //when delete icon is tapped it removes list item
        private void delete_Tapped(object sender, TappedRoutedEventArgs e)
        {
            String soundString;

            try
            {
                string output = _finalName.Substring(_finalName.Length - 1, 1); //cuts off last character to get list position to add it to name
                int pos = Convert.ToInt32(output); //convert to integer

                listData.Clear();
                colourData.Clear();

                for (int i = 0; i <= pos; i++)
                {
                    //removes individual list element using name
                    listGrid.Children.Remove(FindName("listSp_" + i) as Border);
                    listGrid.Children.Remove(FindName("listTextBox_" + i) as TextBlock);
                }
            }
            catch (Exception)
            {
                // Shouldn't get here 
                Debug.WriteLine("Exception");
            }

            ApplicationDataContainer localSetting = ApplicationData.Current.LocalSettings;
            soundString = Convert.ToString(localSetting.Values["soundChoice"]); //convert to string

            if (soundString == "ON")
            {
                //plays delete sound when clicked
                MediaPlayer mediaPlayer = new MediaPlayer();
                mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/deleteSound.mp3"));
                mediaPlayer.Play();
            }
        } //delete_Tapped
    } //main page
}
