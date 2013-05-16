using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Controls;

namespace Kalorilaskuri
{
    /// <summary>
    /// Edustaa sataa grammaa tiettyä ainetta tai ateriaa
    /// TODO: user-muuttuja
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

        [JsonProperty(PropertyName = "protein")]
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

        private float dl;
        public float Dl
        {
            get { return dl; }
            set
            {
                if (value < 0)
                {
                    throw new Exception("Amount must be greater than zero.");
                }

                dl = value;
                NotifyPropertyChanged("Dl");
            }
        }

        private int calories;
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

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                String help = (String)value;
                if (help == String.Empty)
                {
                    throw new Exception("Name must contain some letters");
                }

                name = value;
                NotifyPropertyChanged("Name");
            }
        }
        #endregion


        public Item() { }
        public Item(string name, int calories, float protein, float carbohydrates, float fat, float fibre, double dl)
        {
            Name = name;
            Calories = calories;
            Protein = protein;
            Carbohydrates = carbohydrates;
            Fat = fat;
            Fibre = fibre;
        }


        public Grid getAddGrid()
        {
            Grid add = new Grid();

            TextBox TextBoxname = new TextBox();
            
            
            return add;
        }

        public override string ToString()
        {
            return name + "100g cont. kcal:" + Calories + "prot:" + Protein + " carb:" + Carbohydrates + " fat:" + Fat + " fibre" + Fibre + " dl/g:" + Dl;
        }
    }
}
