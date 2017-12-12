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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ToDoList
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShoppingPage : Page
    {
        public ShoppingPage()
        {
            this.InitializeComponent();
        }
        
        //global variables
        int _RowNum = 1;
        int _countChildren;
        double _totalCostEntered = 0;
        String _inputText; //used to validate if input is entered
        String _inputCost;
        String _dividerBarName = "", _inputTextName = "", _inputListCost = "", _deleteName = "";

        //List to add list items to allow removal and saving to file
        List<String> listData = new List<String>();
        List<String> listCost = new List<String>();

        //when burger menu icon is tapped menu opens up
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            SplitViewMenu.IsPaneOpen = !SplitViewMenu.IsPaneOpen;
        }

        #region navigation between pages
        //navigation
        private void PersonalMenu_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void SettingsMenu_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }

        private void WeeklyMenu_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(WeeklyPage));
        }
        #endregion

        #region navigated to method
        //navigated to, which load list data from file
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            //// Create file; replace if exists.
            //Windows.Storage.StorageFolder storageFolder1 = Windows.Storage.ApplicationData.Current.LocalFolder;
            //Windows.Storage.StorageFile listFile = await storageFolder1.CreateFileAsync("shoppingList.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);

            //try to access the file
            try
            {
                //test file string
                Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

                // read in list item text file
                var file = await storageFolder.GetFileAsync("shoppingList.txt");
                var readFile = await Windows.Storage.FileIO.ReadLinesAsync(file);

                var costFile = await storageFolder.GetFileAsync("shoppingListCost.txt");
                var readCostFile = await Windows.Storage.FileIO.ReadLinesAsync(costFile);

                int numLines = 0;
                int costNumLines = 0;
                listData.Clear(); //clears the list to re-add
                listCost.Clear();

                foreach (var line in readFile)
                {
                    _inputText = "";

                    _inputText = line.Split('\n')[0]; //splits the line when new line encountered
                    numLines += line.Split('\n').Length; //gets number of lines to determine where to place list 
                    Debug.WriteLine(numLines);

                    //sets values for list items using number of lines in the file
                    _RowNum = numLines + 1;
                    _dividerBarName = "listDividerBar_" + (numLines - 1);
                    _inputTextName = "listTextBox_" + (numLines - 1);
                    _deleteName = "deleteImage_" + (numLines - 1);

                    //calls create list item method
                    createListItem(_inputText);
                }

                foreach (var line in readCostFile)
                {
                    _inputCost = "";

                    _inputCost = line.Split('\n')[0]; //splits the line when new line encountered
                    costNumLines += line.Split('\n').Length; //gets number of lines to determine where to place list 
                    Debug.WriteLine(numLines);

                    //sets values for list items using number of lines in the file
                    _RowNum = costNumLines + 1;
                    _inputListCost = "listCost_" + (costNumLines - 1);

                    createListCostItem(_inputCost);
                }
            }
            catch (Exception)
            {
                // Shouldn't get here 
                Debug.WriteLine("File not found");
            }
        }
        #endregion

        #region navigated from method
        //when navigated from it saves list to file
        protected override async void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            try
            {
                // Create file; replace if exists.
                Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile listFile = await storageFolder.CreateFileAsync("shoppingList.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);
                Windows.Storage.StorageFile listCostFile = await storageFolder.CreateFileAsync("shoppingListCost.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);

                for (int i = 0; i < listData.Count; i++)
                {
                    await FileIO.AppendTextAsync(listFile, listData[i] + "\n");
                    await FileIO.AppendTextAsync(listCostFile, listCost[i] + "\n");
                }

                //test file string
                string text = await Windows.Storage.FileIO.ReadTextAsync(listFile);
                string cost = await Windows.Storage.FileIO.ReadTextAsync(listCostFile);
                Debug.WriteLine(text);
                Debug.WriteLine(cost);
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
            popUpAddItem.Height = 80;
            popUpAddItem.Width = 400;
            popUpAddItem.CornerRadius = new CornerRadius(5);
            popUpAddItem.SetValue(Grid.RowProperty, 13);
            popUpAddItem.SetValue(Grid.ColumnProperty, 1);
            popUpAddItem.SetValue(Grid.ColumnSpanProperty, 3);
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
            addListText.PlaceholderText = "Shopping list item";
            addListText.Width = 190;
            addListText.MaxLength = 20;
            addListText.Header = "Add a list item";
            addListText.Margin = new Thickness(15, 2, 10, 0);
            addListText.BorderThickness = new Thickness(2, 0, 2, 0);
            addListText.HorizontalAlignment = HorizontalAlignment.Left;
            addListText.VerticalAlignment = VerticalAlignment.Center;
            popUpAddItem.Children.Add(addListText);

            TextBox addShoppingCost = new TextBox();
            addShoppingCost.Name = "listCost";
            addShoppingCost.Foreground = new SolidColorBrush(Colors.White);
            addShoppingCost.Background = new SolidColorBrush(Colors.Gray);
            addShoppingCost.PlaceholderText = "E.g 12.99";
            addShoppingCost.Width = 100;
            addShoppingCost.MaxLength = 10;
            addShoppingCost.Header = "Add the cost";
            addShoppingCost.Margin = new Thickness(10, 2, 10, 0);
            addShoppingCost.BorderThickness = new Thickness(2, 0, 2, 0);
            addShoppingCost.VerticalAlignment = VerticalAlignment.Center;
            popUpAddItem.Children.Add(addShoppingCost);

            //scope of cost input
            InputScope currencyScope = new InputScope();
            InputScopeName scopeName = new InputScopeName();
            scopeName.NameValue = InputScopeNameValue.CurrencyAmount;
            currencyScope.Names.Add(scopeName);
            addShoppingCost.InputScope = currencyScope;

            Image confirmListItem = new Image();
            confirmListItem.Source = new BitmapImage(new Uri("ms-appx:///Assets/AddIconWhite.png"));
            confirmListItem.Height = 40;
            confirmListItem.Width = 40;
            confirmListItem.SetValue(Grid.ColumnProperty, 3);
            confirmListItem.HorizontalAlignment = HorizontalAlignment.Right;
            confirmListItem.Margin = new Thickness(5);
            popUpAddItem.Children.Add(confirmListItem);

            popUpAddItem.Tapped += popUpAddItem_Tapped;
            addShoppingCost.TextChanged += addShoppingCost_TextChanged;
        }
        
        //add list item when add icon is tapped
        private void popUpAddItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //remove error message if found
            listGrid.Children.Remove(FindName("stackPanelError") as StackPanel);

            //gets textbox objects to get text from
            TextBox getInputText = FindName("listText") as TextBox;
            _inputText = getInputText.Text;

            TextBox getInputCost = FindName("listCost") as TextBox;
            _inputCost = getInputCost.Text;

            //counts the number of children to determine where to place list
            _countChildren = VisualTreeHelper.GetChildrenCount(listGrid);

            if (_countChildren <= 38)
            {
                switch (_countChildren)
                {
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                        //sets name for each list item to allow removal
                        _RowNum = 2;
                        _dividerBarName = "listDividerBar_0";
                        _inputTextName = "listTextBox_0";
                        _inputListCost = "listCost_0";
                        _deleteName = "deleteImage_0";
                        break;
                    case 14:
                    case 16:
                        _RowNum = 3;
                        _dividerBarName = "listDividerBar_1";
                        _inputTextName = "listTextBox_1";
                        _inputListCost = "listCost_1";
                        _deleteName = "deleteImage_1";
                        break;
                    case 18:
                    case 20:
                        _RowNum = 4;
                        _dividerBarName = "listDividerBar_2";
                        _inputTextName = "listTextBox_2";
                        _inputListCost = "listCost_2";
                        _deleteName = "deleteImage_2";
                        break;
                    case 22:
                    case 24:
                        _RowNum = 5;
                        _dividerBarName = "listDividerBar_3";
                        _inputTextName = "listTextBox_3";
                        _inputListCost = "listCost_3";
                        _deleteName = "deleteImage_3";
                        break;
                    case 26:
                    case 28:
                        _RowNum = 6;
                        _dividerBarName = "listDividerBar_4";
                        _inputTextName = "listTextBox_4";
                        _inputListCost = "listCost_4";
                        _deleteName = "deleteImage_4";
                        break;
                    case 30:
                    case 32:
                        _RowNum = 7;
                        _dividerBarName = "listDividerBar_5";
                        _inputTextName = "listTextBox_5";
                        _inputListCost = "listCost_5";
                        _deleteName = "deleteImage_5";
                        break;
                    case 34:
                    case 36:
                        _RowNum = 8;
                        _dividerBarName = "listDividerBar_6";
                        _inputTextName = "listTextBox_6";
                        _inputListCost = "listCost_6";
                        _deleteName = "deleteImage_6";
                        break;
                    default:
                        _RowNum = 9;
                        _dividerBarName = "listDividerBar_7";
                        _inputTextName = "listTextBox_7";
                        _inputListCost = "listCost_7";
                        _deleteName = "deleteImage_7";
                        break;
                } //switch

                //calls create list item method
                createListItem(_inputText);
                createListCostItem(_inputCost);

                //remove the add list text box from view
                listGrid.Children.Remove(FindName("stackPanelList") as StackPanel);

            } //if to limit list items to 9
        }

        //create list item and place in determined row
        private void createListItem(string inputText)
        {
            if (!(inputText == "") && !(_inputCost == "")) //input validation to check if list is not empty
            {
                //creates the dividing grey bar between list items
                Border dividerBar = new Border();
                dividerBar.Name = _dividerBarName; 
                dividerBar.Height = 2;
                dividerBar.Background = new SolidColorBrush(Colors.LightGray);
                dividerBar.SetValue(Grid.RowProperty, _RowNum);
                dividerBar.SetValue(Grid.ColumnProperty, 1);
                dividerBar.SetValue(Grid.ColumnSpanProperty, 3);
                dividerBar.VerticalAlignment = VerticalAlignment.Top;
                dividerBar.CornerRadius = new CornerRadius(1);
                listGrid.Children.Add(dividerBar);

                //adds a text block with input text
                TextBlock addInputText = new TextBlock();
                addInputText.Name = _inputTextName;
                addInputText.Text = inputText;
                addInputText.Foreground = new SolidColorBrush(Colors.Gray);
                addInputText.SetValue(Grid.RowProperty, _RowNum);
                addInputText.SetValue(Grid.ColumnProperty, 1);
                addInputText.Margin = new Thickness(15, 2, 10, 0);
                addInputText.VerticalAlignment = VerticalAlignment.Center;
                listGrid.Children.Add(addInputText);

                //adds delete icon image
                Image deleteList = new Image();
                deleteList.Source = new BitmapImage(new Uri("ms-appx:///Assets/DeleteIcon.png"));
                deleteList.Name = _deleteName;
                deleteList.Height = 35;
                deleteList.Width = 35;
                deleteList.SetValue(Grid.RowProperty, _RowNum);
                deleteList.SetValue(Grid.ColumnProperty, 3);
                deleteList.VerticalAlignment = VerticalAlignment.Center;
                deleteList.Margin = new Thickness(5);
                listGrid.Children.Add(deleteList);
                deleteList.Tapped += delete_Tapped;

                listData.Add(inputText); //add text to list to save to file
            } //if
        }

        //create list cost method and place in determined row
        private void createListCostItem(string inputCost)
        {
            double enteredCost;

            if (!(inputCost == "") && !(_inputText == "")) //input validation to check if list is not empty
            {
                //adds a text block with input text
                TextBlock addInputCost = new TextBlock();
                addInputCost.Name = _inputListCost;
                addInputCost.Text = inputCost;
                addInputCost.Foreground = new SolidColorBrush(Colors.Gray);
                addInputCost.SetValue(Grid.RowProperty, _RowNum);
                addInputCost.SetValue(Grid.ColumnProperty, 2);
                addInputCost.Margin = new Thickness(15, 2, 10, 0);
                addInputCost.VerticalAlignment = VerticalAlignment.Center;
                listGrid.Children.Add(addInputCost);
 
                listCost.Add(inputCost); //add cost to list to save to file
            } //if

          
            listGrid.Children.Remove(FindName("totalCostTextBlock") as TextBlock); //remove objects to add updated object
            listGrid.Children.Remove(FindName("currencyTextBlock") as TextBlock);

            //total cost calculation
            try
            {
                enteredCost = Convert.ToDouble(inputCost); //convert to double
                _totalCostEntered += enteredCost; //adds to total
                Debug.WriteLine(_totalCostEntered);
                addTotal(_totalCostEntered);
                addCurrency(); //add currency
            }
            catch (Exception)
            {
                Debug.WriteLine("Invalid entry");
                createErrorMessage();
            }
        }

        //validate if number 0-9 or "." is entered
        private void addShoppingCost_TextChanged(object sender, TextChangedEventArgs e)
        {
            //gets text box sender object to get text
            TextBox current = (TextBox)sender;

            if (System.Text.RegularExpressions.Regex.IsMatch(current.Text, "[^0-9^.]"))
            {
                current.Text = current.Text.Remove(current.Text.Length - 1); //takes away character if invalid
                createErrorMessage();
            }
        }

        // error method which displays error message when input is invalid
        private void createErrorMessage()
        {
            StackPanel popUpError = new StackPanel();
            popUpError.Name = "stackPanelError";
            popUpError.Height = 37;
            popUpError.Width = 400;
            popUpError.CornerRadius = new CornerRadius(5);
            popUpError.SetValue(Grid.RowProperty, 10);
            popUpError.SetValue(Grid.ColumnProperty, 1);
            popUpError.SetValue(Grid.ColumnSpanProperty, 3);
            popUpError.Background = new SolidColorBrush(Colors.Red);
            popUpError.Margin = new Thickness(5);
            popUpError.Orientation = Orientation.Horizontal;
            popUpError.VerticalAlignment = VerticalAlignment.Center;
            popUpError.HorizontalAlignment = HorizontalAlignment.Left;
            listGrid.Children.Add(popUpError);

            //adds a text block with input text
            TextBlock addErrorText = new TextBlock();
            addErrorText.Name = "textBlockError";
            addErrorText.Text = "Input must be a digit from 0-9 or include a '.'";
            addErrorText.Foreground = new SolidColorBrush(Colors.White);
            addErrorText.VerticalAlignment = VerticalAlignment.Center;
            popUpError.Children.Add(addErrorText);
        }

        //add total method which takes total calulation and outputs it
        private void addTotal(double totalCost)
        {
            String totalCostString;
            totalCostString = Convert.ToString(totalCost); //convert to string

            //adds a text block with input text
            TextBlock totalNum = new TextBlock();
            totalNum.Name = "totalCostTextBlock";
            totalNum.Text = totalCostString;
            totalNum.Foreground = new SolidColorBrush(Colors.Gray);
            totalNum.SetValue(Grid.RowProperty, 10);
            totalNum.SetValue(Grid.ColumnProperty, 2);
            totalNum.SetValue(Grid.ColumnSpanProperty, 2);
            totalNum.Margin = new Thickness(10, 0, 6, 0);
            totalNum.VerticalAlignment = VerticalAlignment.Center;
            listGrid.Children.Add(totalNum);
        }

        //add currency method which gets currency symbol from settings page and outputs it
        private void addCurrency()
        {
            String currencyString;

            ApplicationDataContainer localSetting = ApplicationData.Current.LocalSettings;

            currencyString = Convert.ToString(localSetting.Values["currencyChoice"]); //convert to string

            //adds a text block with currency
            TextBlock currencyTextBlock = new TextBlock();
            currencyTextBlock.Name = "currencyTextBlock";
            currencyTextBlock.Text = currencyString;
            currencyTextBlock.Foreground = new SolidColorBrush(Colors.Gray);
            currencyTextBlock.SetValue(Grid.RowProperty, 10);
            currencyTextBlock.SetValue(Grid.ColumnProperty, 2);
            currencyTextBlock.Margin = new Thickness(0, 0, 6, 0);
            currencyTextBlock.VerticalAlignment = VerticalAlignment.Center;
            listGrid.Children.Add(currencyTextBlock);
        }
        
        //when delete icon is tapped it removes list item
        private void delete_Tapped(object sender, TappedRoutedEventArgs e)
        {
            String soundString;

            //gets images sender object to get name
            Image currentImage = (Image)sender;
            String currentObj = currentImage.Name;
            string output = currentObj.Substring(currentObj.Length - 1, 1); //cuts off last character to get list position to add it to name
            int arrayPos = Convert.ToInt32(output); //convert to integer

            try
            {
                listData.RemoveAt(arrayPos); //remove list elements for text and cost
                listCost.RemoveAt(arrayPos); 
            }
            catch (Exception)
            {
                Debug.WriteLine("Exception");
            }

            //delete objects
            if (currentObj == "deleteImage_" + output)
            {
                //removes individual list element using name
                listGrid.Children.Remove(FindName("listDividerBar_" + output) as Border);
                listGrid.Children.Remove(FindName("listTextBox_" + output) as TextBlock);
                listGrid.Children.Remove(FindName("listCost_" + output) as TextBlock);
                listGrid.Children.Remove(FindName("deleteImage_" + output) as Image);
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
    }//main
}
