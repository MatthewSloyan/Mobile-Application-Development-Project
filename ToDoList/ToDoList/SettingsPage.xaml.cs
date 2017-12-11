using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ToDoList
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        //navigation
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            SplitViewMenu.IsPaneOpen = !SplitViewMenu.IsPaneOpen;
        }

        private void PersonalMenu_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void ShoppingMenu_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ShoppingPage));
        }

        private void WeeklyMenu_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(WeeklyPage));
        }

        //currency radiobuttons
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton current = (RadioButton)sender;

            //save the tag value as the setting and access the data container
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            //within the container, save the tag currency
            localSettings.Values["currencyChoice"] = current.Tag.ToString();
        }
    }
}
