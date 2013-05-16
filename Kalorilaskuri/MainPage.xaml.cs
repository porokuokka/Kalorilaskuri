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

namespace Kalorilaskuri
{

    public partial class MainPage : PhoneApplicationPage
    {
        /// <summary>
        /// Sisältää päivän yhteenlasketut kalorit
        /// </summary>
        private int total = 0;
        private Item add;
        private List<Item> hakutulokset;

        private MobileServiceCollectionView<Item> items;
        private IMobileServiceTable<Item> itemTable = App.MobileService.GetTable<Item>();

        /// <summary>
        /// Sisältää päivän yhteenlasketut kalorit,
        /// set-metodi päivittää labelin
        /// </summary>
        public int Total
        {
            get {return total;}
            set { total = value;
                  totalBlock.Text = total.ToString();
            }
        }

        public Grid annaGridi()
        {
            Grid add = new Grid();
            TextBlock testi = new TextBlock();
            testi.Text = "TESTAAAA";
            add.Children.Add(testi);
            return add;
        }

        private void lisaaGrid()
        {
            PanoramaItem uus = new PanoramaItem();
            uus.Content = annaGridi();
            Panorama.Items.Add(uus);
        }

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            add = new Item();
            GridAdd.DataContext = add;
            lisaaGrid();
            /**List<AlphaKeyGroup<Item>> DataSource = AlphaKeyGroup<Item>.CreateGroups(hakutulokset,
                System.Threading.Thread.CurrentThread.CurrentUICulture,
                (Item s) => { return s.Name; }, true);*/

            
            //hakutulokset = App.MobileService.GetTable("Item");
        }

        /// <summary>
        /// Lisää itemin pilveen, lopettaa prosessin poistamalla
        /// progressbarin näkyvistä ja pyyhkii lisäysolion.
        /// </summary>
        /// <param name="item"></param>
        private async void InsertItem(Item item)
        {
            TextBlockHakusana.Text = item.Name;
            await App.MobileService.GetTable<Item>().InsertAsync(item);
            Progressbar.IsVisible = false;

            //pitäisikö add-olio heittää jotenkin roskiin?
            add = new Item();
            GridAdd.DataContext = add;
            MessageBox.Show("Ruoka-aine lisätty!");
        }

        /// <summary>
        /// Valitsee hakukentän tekstin sitä klikatessa
        /// </summary>
        /// <param name="sender">TextBoxHaku</param>
        /// <param name="e">fokuksen saanti</param>
        private void TextBoxHaku_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBoxHaku.Focus();
            TextBoxHaku.SelectAll();
        }

        /// <summary>
        /// Laukeaa kun hakukentän teksti muuttuu
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
            
        }

        private void Haku(string hakusana)
        {
            
            TextBlockHakusana.Text = hakusana;

           items = itemTable.Where(item => item.Name == hakusana)
               .ToCollectionView();


            ListHakutulokset.ItemsSource = items;

            Progressbar.IsVisible = false;
            //hae hakusanalla molemmista kannoista ja lyö longlistselectoriin tms
        }


        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            Panorama.DefaultItem = Panorama.Items[1];
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            Panorama.DefaultItem = Panorama.Items[0];
            TextBoxHaku.Focus();
        }

        

        /// <summary>
        /// Tässä on tarkoitus hakea heti käyttäjän kirjoitettua jotain
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxHaku_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

            if (e.Key == System.Windows.Input.Key.Enter)
            {
                Progressbar.Text = "Haetaan...";
                Progressbar.IsVisible = true;
                hakutulokset = new List<Item>();
                TextBox text = (TextBox)sender;
                
                this.Focus();
                Haku(text.Text); 
            }
        }

        /// <summary>
        /// Testaa onko virheilmoituksia ja kutsuu sitten
        /// lisäysmetodia
        /// TODO: testaus, onko mitään lisätty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!val || add.Name == null)
                //TODO: tarkista onko nimi tyhjä
                MessageBox.Show("Lisäys ei onnistunut! Muistitko täyttää kaikki laatikot?");

            else {
                Progressbar.Text = "Lisätään ruoka-ainetta: " + add.Name;
                Progressbar.IsVisible = true;
                InsertItem(add); 
                }
        }

        private void TextBoxNimi_GotFocus(object sender, RoutedEventArgs e)
        {
    
        }

        private bool val = true;

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


    }
  
    
}
