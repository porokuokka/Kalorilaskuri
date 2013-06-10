using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Kalorilaskuri.Resources;
using System.Windows.Data;
using System.Windows.Media;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Kalorilaskuri.Model;
using Kalorilaskuri.ViewModel;

namespace Kalorilaskuri
{
    /// <summary>
    /// Includes all of UI elements except for counting
    /// </summary>
    public partial class MainPage : PhoneApplicationPage
    {
        /// <summary>
        /// Sisältää päivän yhteenlasketut kalorit
        /// </summary>
        private Item add;
        //private List<Item> hakutulokset;
        private DateTime selectedDate;
        //private Day SelectedDay;

        private MobileServiceCollectionView<Item> items;
        private MobileServiceCollectionView<Item> search;
        private IMobileServiceTable<Item> itemTable = App.MobileService.GetTable<Item>();


        public static Item Selected
        {
            get;
            set;
        }

        public static Day SelectedDay
        {
            get;
            set;
        }

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
            items = itemTable.ToCollectionView();
            MessageBox.Show("Itemtable: " + itemTable.ToString());
            add = new Item();
            GridAdd.DataContext = add;
            
            //Get today as default
            selectedDate = new DateTime();
            selectedDate = DateTime.Today;
            SelectedDay = new Day();
            SelectedDay.Date = selectedDate;
            SelectedDay = App.ViewModel.AddDay(SelectedDay);
   
            if (SelectedDay.Intakes != null) LongListDiary.ItemsSource = SelectedDay.Intakes;

            Diary.DataContext = SelectedDay;
            
            MessageBox.Show("Itemcount" + items.Count);
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        /// <summary>
        /// Loads items from mobile service, sets datacontext for
        /// adding new ingredients, selects today as default day in diary
        /// and loads intakes of today if any
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //ListPickerDiary.ItemsSource = App.ViewModel.DaysList;
        }

        /// <summary>
        /// Inserts item to mobileservice table, removes
        /// progressbar when finished and clears the new item
        /// space from additem grid
        /// </summary>
        /// <param name="item">Item to insert</param>
        private async void InsertItem(Item item)
        {
            await App.MobileService.GetTable<Item>().InsertAsync(item);
            Progressbar.IsVisible = false;

            //the old add item goes to garbage automatically?
            add = new Item();
            GridAdd.DataContext = add;
            MessageBox.Show("Ruoka-aine lisätty!");
        }

        /// <summary>
        /// Chooses text when textbox is clicked
        /// </summary>
        /// <param name="sender">TextBoxHaku</param>
        /// <param name="e">fokuksen saanti</param>
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Focus();
            tb.SelectAll();
        }

        /// <summary>
        /// Fires when search-textbox textproperty is changed
        /// </summary>
        /// <param name="sender">hakukenttä</param>
        /// <param name="e">tekstin muuttuminen</param>
        private void TextBoxHaku_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBlockHakusana.Text = TextBoxHaku.Text + "...";
        }

        /// <summary>
        /// Laukeaa kun hakukentästä lähtee fokus,
        /// nyt vaan poistetaan progressbar näkyvistä testauksen takia
        /// </summary>
        /// <param name="sender">hakukenttä</param>
        /// <param name="e"></param>
        private void TextBoxHaku_LostFocus(object sender, RoutedEventArgs e)
        {
            //TODO: progressbar should be visible until search is ready
        }

        /// <summary>
        /// Searches from mobileservice table for items containing
        /// searchcriteria
        /// </summary>
        /// <param name="hakusana"></param>
        private void Haku(string hakusana)
        {
           TextBlockHakusana.Text = hakusana + " ...";
         
           search = App.MobileService.GetTable<Item>().Where(item => item.Name.Contains(hakusana))
               .ToCollectionView();
           
           ListHakutulokset.ItemsSource = search;
           
           Progressbar.IsVisible = false;
        }


        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            Panorama.DefaultItem = Panorama.Items[1];
        }

        private void ApplicationBarSearchButton_Click(object sender, EventArgs e)
        {
            Panorama.DefaultItem = Panorama.Items[0];
            TextBoxHaku.Focus();
        }

        

        /// <summary>
        /// TODO: start searching immediately when user sets input,
        /// not just say you do...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxHaku_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Progressbar.Text = "Haetaan...";

            Progressbar.IsVisible = true;

            if (e.Key == System.Windows.Input.Key.Enter)
            {
                TextBox text = (TextBox)sender;
                this.Focus();
                Haku(text.Text); 
            }
        }

        /// <summary>
        /// Test if there's errors and adds item to db
        /// TODO: test if adding was a success
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ListPickerItem selectedItem = (ListPickerItem)Portionpicker.SelectedItem;
            string content = (string)selectedItem.Content;
            add.countPortion();
            
            MessageBox.Show(add.PortionCalories.ToString());
            add.Portion = content;

            if (!val || add.Name == null)
                //TODO: tarkista onko nimi tyhjä ja laita täähän kieliasetukset ja parempi virheentarkistus(ainakin info)
                MessageBox.Show("Lisäys ei onnistunut! Muistitko täyttää kaikki laatikot?");

            else {
                Progressbar.Text = "Lisätään ruoka-ainetta: " + add.Name;
                Progressbar.IsVisible = true;
                InsertItem(add); 
                }
        }

        private bool val = true;

        /// <summary>
        /// Käsittelee syötteentarkistusvirheen
        /// TODO: tarkista sotiiko accent-värit jonkin teeman kanssa
        /// + lisää tooltip tms
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_BindingValidationError(object sender, ValidationErrorEventArgs e)
        {
            TextBox MyTextBox = (TextBox)e.OriginalSource;

            if (e.Action == ValidationErrorEventAction.Added)
            {
                MyTextBox.Background = new SolidColorBrush(Colors.Red);
                val = false;
            }
            else if (e.Action == ValidationErrorEventAction.Removed)
            {
                MyTextBox.Background = new SolidColorBrush(Colors.White);
                val = true;
            }

        }

        private void TextBoxNimi_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                this.Focus();
            }
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            ExpanderView ex = (ExpanderView)sender;
            ex.Header = "-";
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            ExpanderView ex = (ExpanderView)sender;
            ex.Header = "+";

        }

        private void ButtonAddIntake_Click(object sender, RoutedEventArgs e)
        {
            ApplicationBarSearchButton_Click(sender, e);
        }

        #region Diary
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatePickerDiary_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            changeDay((DateTime)e.NewDateTime);
        }

        private void changeDay(DateTime date)
        {
            SelectedDay = new Day();
            SelectedDay.Date = date;
            SelectedDay = App.ViewModel.AddDay(SelectedDay);
            MessageBox.Show("Haettu selectedDay");
            if (SelectedDay.Intakes != null) LongListDiary.ItemsSource = SelectedDay.Intakes;
            MessageBox.Show("Haettu selectedDay");
            Diary.DataContext = SelectedDay;
        }

        private void StackPanelIntake_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            StackPanel intk = (StackPanel)sender;
            Intake remove = (Intake)intk.DataContext;
            MessageBox.Show("Remove intake " + remove.IngredientOfIntake.Name);
        }

        private void ExpanderDiary_Expanded(object sender, RoutedEventArgs e)
        {
            ExpanderView exd = (ExpanderView)sender;
            exd.Header = "-";
        }

        private void ExpanderDiary_Collapsed(object sender, RoutedEventArgs e)
        {
            ExpanderView exd = (ExpanderView)sender;
            exd.Header = "+";
        }

        /*
       private void Button_Click_1(object sender, RoutedEventArgs e)
       {
           MessageBox.Show("Painoit shittiä");

           ///Etsitään item paikallisesta kannasta ja muodostetaan uusi jos sitä ei ole aiemmin lisätty
           Item selectedItem = (Item)ListHakutulokset.SelectedItem;
           Ingredient newIngredient = App.ViewModel.getIngredient(selectedItem.Id);
           if (newIngredient != null) MessageBox.Show("Löytyi listasta: " + newIngredient.Name);
           if (newIngredient == null)
           {
               newIngredient = new Ingredient();
               newIngredient.ItemId = selectedItem.Id;
               newIngredient.Name = selectedItem.Name;
               newIngredient.Calories = selectedItem.Calories;
               newIngredient.Fat = selectedItem.Fat;
               newIngredient.Carbohydrates = selectedItem.Carbohydrates;
               newIngredient.Protein = selectedItem.Protein;
               newIngredient.Portion = selectedItem.Portion;
               newIngredient.PortionCalories = selectedItem.PortionCalories;
               newIngredient.Portionweight = selectedItem.Portionweight;
               App.ViewModel.AddIngredient(newIngredient);
           }

           //Tehdään uusi intake
           Intake newIntake = new Intake();
           newIntake.Grams = 300;
           newIntake.IngredientOfIntake = newIngredient;
           MessageBox.Show("Intaken ingredientin nimi: " + newIntake.IngredientOfIntake.Name);
           newIntake.DayIntake = SelectedDay;
            
           SelectedDay.Intakes.Add(newIntake);
           newIngredient.Intakes.Add(newIntake);
            
            
           App.ViewModel.AddIntake(newIntake);

           LongListDiary.ItemsSource = SelectedDay.Intakes;
            
           //Lisätään intake kantaan
            
          /* newIntake._ingredientId = getIngredient.IngredientId;
           newIntake._dayId = SelectedDay.Id;*/


        //MessageBox.Show("Oisit ny lisäämässä" + newItem.Name);

        //}


        /*
        private void DiaryListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDate = (DateTime)sender;
            SelectedDay.Date = selectedDate;
            SelectedDay = App.ViewModel.AddDay(SelectedDay);
            
            LongListDiary.ItemsSource = SelectedDay.Intakes;
            TotalIntake.Text = SelectedDay.Total.ToString();
            MessageBox.Show(SelectedDay.Total.ToString());
        }*/
        #endregion

    }
  
    
}
