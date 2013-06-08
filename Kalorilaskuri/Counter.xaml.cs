using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.ComponentModel;
using Kalorilaskuri.Model;

namespace Kalorilaskuri
{
    /// <summary>
    /// Counter-class which handles counting calories
    /// by Items properties and usercontrols values
    /// </summary>
    public class CounterClass : INotifyPropertyChanged
    {
        public CounterClass() {}

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Property to indicate whether to count
        /// by grams or portion
        /// </summary>
        private bool isGramsSelected;
        public bool IsGramsSelected
        {
            get { return isGramsSelected; }
            set { isGramsSelected = value; countCalories(); }
        }

        /// <summary>
        /// Property to indicate the amount of portion/grams
        /// </summary>
        private float amount;
        public float Amount
        {
            get { return amount; }
            set
            {
                if (value >= 0.01)

                amount = value;
                NotifyPropertyChanged("Amount");
                countCalories();
            }
        }

        /// <summary>
        /// Item to the get the info
        /// </summary>
        private Item _item;
        public Item item
        {
            get { return _item; }
            set { _item = value; }
        }

        /// <summary>
        /// Property to store the value of
        /// counted calories
        /// </summary>
        private float countedCalories;
        public float CountedCalories
        {
            get { return countedCalories; }
            set { countedCalories = value; NotifyPropertyChanged("CountedCalories"); }
        }

        /// <summary>
        /// Counts calories by grams or portions by checking
        /// the isGramsSelected -property
        /// </summary>
        public void countCalories()
        {
            if (item != null)
            {
                if (isGramsSelected)
                {
                    CountedCalories = Amount * (item.Calories / 100F);
                }

                if (!isGramsSelected)
                {
                    CountedCalories = Amount * item.PortionCalories;
                    
                }
            }
        }
    }

    /// <summary>
    /// Usercontrol for adding calories to diary
    /// </summary>
    public partial class Counter : UserControl
    {

        private CounterClass CounterData;

        public CounterClass getCounterClass()
        {
            return CounterData;
        }

        /// <summary>
        /// Constructor sets the default values,
        /// binds datacontext and sets loaded event handler
        /// </summary>
        public Counter()
        {
            CounterData = new CounterClass();
            CounterData.Amount = 100;
            InitializeComponent();
            
            this.Loaded += Counter_Loaded;
            Counting.DataContext = CounterData;
        }

        /// <summary>
        /// Loaded event handler gets the binding item
        /// for counterclass from datacontext which is set in the main window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Counter_Loaded(object sender, RoutedEventArgs e)
        {
            CounterData.item = (Item)this.LayoutRoot.DataContext;
            MessageBox.Show("iteminä " + CounterData.item.Name);
        }

        /// <summary>
        /// Increase amount when clicking the plus button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonPlus_Click(object sender, RoutedEventArgs e)
        {
            CounterData.Amount++;   
        }

        /// <summary>
        /// Decrease the amount by clicking minus button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonMinus_Click(object sender, RoutedEventArgs e)
        {
            CounterData.Amount--;  
        }

        /// <summary>
        /// Set default portion as one (=>amount) and
        /// set the IsGramsSelected property false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioPortion_Checked(object sender, RoutedEventArgs e)
        {
            CounterData.Amount = 1;
            CounterData.IsGramsSelected = false; 
        }

        /// <summary>
        /// Set the default value to 100 when grams are chosen,
        /// set IsGramsSelected property true
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioGrams_Checked(object sender, RoutedEventArgs e)
        {
            CounterData.Amount = 100;
            CounterData.IsGramsSelected = true;
        }

        /// <summary>
        /// Add an intake to viewmodel when clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Lisätään " + CounterData.item.Name + " " + CounterData.CountedCalories);
        }
    }
}
