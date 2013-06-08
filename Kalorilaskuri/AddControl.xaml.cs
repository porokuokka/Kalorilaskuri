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
    public class AmountChanger : INotifyPropertyChanged
    {
        public AmountChanger() { }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private float amount;

        public float Amount
        {
            get { return amount; }
            set
            {
                if (value >= 0.01)

                    amount = value;
                NotifyPropertyChanged("Amount");
            }
        }

        private float addCalories;
        public float AddCalories
        {
            get { return addCalories; }
            set
            {
                addCalories = value;
            }
        }

        private int portionIndex;
        public int PortionIndex
        {
            get { return portionIndex; }
            set
            {
                portionIndex = value;
            }
        }

        private float grams;
        public float Grams
        {
            get { return grams; }
            set
            {
                grams = value;
            }
        }

        private float cals;
        public float Cals
        {
            get { return cals; }
            set
            {
                cals = value;
            }
        }
    }

    
    public partial class AddControl : UserControl
    {
        private AmountChanger amountchanger;
        private Item item;

        public AddControl()
        {
            InitializeComponent();
            amountchanger = new AmountChanger();
            amountchanger.Amount = 1.0F;
            GridAmount.DataContext = amountchanger;
            this.Loaded += AddControl_Loaded;
            TextAmount.TextChanged += TextAmount_TextChanged;
        }

        void AddControl_Loaded(object sender, RoutedEventArgs e)
        {
            item = (Item)this.LayoutRoot.DataContext;
            PickPortion.SelectionChanged += PickPortion_SelectionChanged;
        }

        private void ButtonMinus_Click(object sender, RoutedEventArgs e)
        {
            amountchanger.Amount--;
            countCalories();
        }

        private void ButtonPlus_Click(object sender, RoutedEventArgs e)
        {
            amountchanger.Amount++;
            countCalories();
        }

        private void countCalories()
        {
            if (amountchanger.PortionIndex == 0)
            {
                amountchanger.Cals = (item.Calories / 100.0F) * amountchanger.Amount;
                amountchanger.Grams = amountchanger.Amount;
                
            }

            if (amountchanger.PortionIndex == 1)
            {
                amountchanger.Cals = item.PortionCalories * amountchanger.Amount;
                amountchanger.Grams = item.Portionweight * amountchanger.Amount;
                
            }
        }

        private void PickPortion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
                amountchanger.PortionIndex = PickPortion.SelectedIndex;
                countCalories();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Intake newIntake = new Intake();
            newIntake.DayIntake = MainPage.SelectedDay; //tuleeko staattisuudesta ongelmia???
            newIntake.Grams = (int)amountchanger.Grams;
            newIntake.Calories = (int)amountchanger.Cals;
            App.ViewModel.AddIntake(newIntake, item);
        }

        private void TextAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            countCalories();
        }
    }
}
