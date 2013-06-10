using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Controls;
using System.Windows;

namespace Kalorilaskuri
{
    /// <summary>
    /// Represents 100g of some ingredient containing nutritional
    /// information and portions and portions calories
    /// </summary>
    public class Item : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region properties

        public int Id { get; set; }

        
        private float protein;
        [JsonProperty(PropertyName = "Protein")]
        public float Protein
        {
            get { return protein; }
            set
            {
                if (value < 0)
                {
                    throw new Exception("Amount must be greater than zero.");
                }

                protein = value;
                NotifyPropertyChanged("Protein");
            }
        }

        private float carbohydrates;
        [JsonProperty(PropertyName = "Carbohydrates")]
        public float Carbohydrates
        {
            get { return carbohydrates; }
            set
            {
                if (value < 0)
                {
                    throw new Exception("Amount must be greater than zero.");
                }

                carbohydrates = value;
                NotifyPropertyChanged("Carbohydrates");
            }
        }

        private float fat;
        [JsonProperty(PropertyName = "Fat")]
        public float Fat
        {
            get { return fat; }
            set
            {
                if (value < 0)
                {
                    throw new Exception("Amount must be greater than zero.");
                }

                fat = value;
                NotifyPropertyChanged("Fat");
            }
        }

        private float fibre;
        [JsonProperty(PropertyName = "Fibre")]
        public float Fibre
        {
            get { return fibre; }
            set
            {
                if (value < 0)
                {
                    throw new Exception("Amount must be greater than zero.");
                }

                fibre = value;
                NotifyPropertyChanged("Fibre");
            }
        }

        private int calories;
        [JsonProperty(PropertyName = "Calories")]
        public int Calories
        {
            get { return calories; }

            set
            {
                if (value < 0)
                {
                    throw new Exception("Amount must be greater than zero.");
                }

                calories = value;
                NotifyPropertyChanged("Calories");
            }
        }

        private float portionCalories;
        [JsonProperty(PropertyName = "PortionCalories")]
        public float PortionCalories
        {
            get { return portionCalories; }

            set
            {
                if (value < 0)
                {
                    throw new Exception("Amount must be greater than zero.");
                }

                portionCalories = value;
                NotifyPropertyChanged("PortionCalories");
            }
        }

        private string name;
        [JsonProperty(PropertyName = "Name")]
        public string Name
        {
            get { return name; }
            set
            {
                String help = (String)value;
                if (help == String.Empty || value == null)
                {
                    value = "";
                    throw new Exception("Name must contain some letters");
                }

                name = value;
                NotifyPropertyChanged("Name");
            }
        }

        private string portion;
        [JsonProperty(PropertyName = "Portion")]
        public string Portion
        {
            get { return portion; }
            set
            {
                String help = (String)value;
                if (help == String.Empty || value == null)
                {
                    value = "";
                    throw new Exception("Portion value is empty");
                }

                portion = value;
                NotifyPropertyChanged("Portion");
            }
        }

        private int portionweight;
        [JsonProperty(PropertyName = "Portionweight")]
        public int Portionweight
        {
            get { return portionweight; }
            set
            {
                if (value < 0)
                {
                    throw new Exception("Amount must be greater than zero.");
                }

                portionweight = value;

                NotifyPropertyChanged("Portionweight");
            }
        }

        
        #endregion

        public Item() { }

        public override string ToString()
        {
            return name + " /n" + calories + " kcal/100g";
        }

        /// <summary>
        /// Counts calories per portion (float) and changes
        /// the property
        /// </summary>
        public void countPortion()
        {
            float hundred = 100;
            float helpcalories = (float)Calories;
            PortionCalories = (helpcalories / hundred) * Portionweight;
        }
    }
}
