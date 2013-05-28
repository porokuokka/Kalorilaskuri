using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Kalorilaskuri.Model
{
    public class CalorieCounterDataContext : DataContext
    {
        // Pass the connection string to the base class.
        public CalorieCounterDataContext(string connectionString)
            : base(connectionString)
        { }

        // Specify a table for the to-do items.
        public Table<Intake> Intakes;

        // Specify a table for the categories.
        public Table<Day> Days;

        public Table<Ingredient> Ingredients;
    }

    #region IntakeTable
    [Table]
    public class Intake : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _IntakeId;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int IntakeId
        {
            get { return _IntakeId; }
            set
            {
                if (_IntakeId != value)
                {
                    NotifyPropertyChanging("IntakeId");
                    _IntakeId = value;
                    NotifyPropertyChanged("IntakeId");
                }
            }
        }

        private int _Calories;

        [Column]
        public int Calories
        {
            get { return _Calories; }
            set
            {
                if (_Calories != value)
                {
                    NotifyPropertyChanging("Calories");
                    _Calories = value;
                    NotifyPropertyChanged("Calories");
                }
            }
        }

        private int _grams;

        [Column]
        public int Grams
        {
            get { return _grams; }
            set
            {
                if (_grams != value)
                {
                    NotifyPropertyChanging("Grams");
                    _grams = value;
                    NotifyPropertyChanged("Grams");
                }
            }
        }

        [Column]
        internal int _ingredientId;

        private EntityRef<Ingredient> _ingredientOfIntake;
        [Association(Storage = "_ingredientOfIntake", ThisKey = "_ingredientId", OtherKey = "IngredientId", IsForeignKey = true)]
        public Ingredient IngredientOfIntake
        {
            get { return _ingredientOfIntake.Entity; }
            set
            {
                NotifyPropertyChanging("DayIntake");
                _ingredientOfIntake.Entity = value;

                if (value != null)
                {
                    _ingredientId = value.IngredientId;
                }

                NotifyPropertyChanging("IngredientOfIntake");
            }
        }
        

        [Column]
        internal int _dayId;

        // Entity reference, to identify the ToDoCategory "storage" table
        private EntityRef<Day> _dayIntake;

        // Association, to describe the relationship between this key and that "storage" table
        [Association(Storage = "_dayIntake", ThisKey = "_dayId", OtherKey = "Id", IsForeignKey = true)]
        public Day DayIntake
        {
            get { return _dayIntake.Entity; }
            set
            {
                NotifyPropertyChanging("DayIntake");
                _dayIntake.Entity = value;

                if (value != null)
                {
                    _dayId = value.Id;
                }

                NotifyPropertyChanging("DayIntake");
            }
        }

        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    #endregion
    }

    #region IngredientTable
    [Table]
    public class Ingredient : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _ingredientId;
        [Column(DbType = "INT NOT NULL IDENTITY", IsDbGenerated = true, IsPrimaryKey = true)]
        public int IngredientId
        {
            get { return _ingredientId; }
            set
            {
                NotifyPropertyChanging("IngredientId");
                _ingredientId = value;
                NotifyPropertyChanged("IngredientId");
            }
        }

        [Column]
        private int _itemId;
        public int ItemId
        {
            get { return _itemId; }
            set
            {
                NotifyPropertyChanging("ItemId");
                _itemId = value;
                NotifyPropertyChanged("ItemId");
            }
        }

        [Column]
        private float protein;
        public float Protein
        {
            get { return protein; }
            set
            {
                NotifyPropertyChanging("Protein");
                protein = value;
                NotifyPropertyChanged("Protein");
            }
        }

        [Column]
        private float carbohydrates;
        public float Carbohydrates
        {
            get { return carbohydrates; }
            set
            {
                NotifyPropertyChanging("Carbohydrates");
                carbohydrates = value;
                NotifyPropertyChanged("Carbohydrates");
            }
        }

        [Column]
        private float fat;
        public float Fat
        {
            get { return fat; }
            set
            {
                NotifyPropertyChanging("Fat");
                fat = value;
                NotifyPropertyChanged("Fat");
            }
        }

        [Column]
        private float fibre;
        public float Fibre
        {
            get { return fibre; }
            set
            {
                NotifyPropertyChanging("Fibre");
                fibre = value;
                NotifyPropertyChanged("Fibre");
            }
        }

        [Column]
        private int calories;
        public int Calories
        {
            get { return calories; }

            set
            {
                NotifyPropertyChanging("Calories");
                calories = value;
                NotifyPropertyChanged("Calories");
            }
        }

        [Column]
        private float portionCalories;
        public float PortionCalories
        {
            get { return portionCalories; }

            set
            {
                NotifyPropertyChanging("PortionCalories");
                portionCalories = value;
                NotifyPropertyChanged("PortionCalories");
            }
        }

        [Column]
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                NotifyPropertyChanging("Name");
                name = value;
                NotifyPropertyChanged("Name");
            }
        }

        [Column]
        private string portion;
        public string Portion
        {
            get { return portion; }
            set
            {
                NotifyPropertyChanging("Portion");
                portion = value;
                NotifyPropertyChanged("Portion");
            }
        }

        [Column]
        private int portionweight;
        public int Portionweight
        {
            get { return portionweight; }
            set
            {
                NotifyPropertyChanging("Portionweight");
                portionweight = value;
                NotifyPropertyChanged("Portionweight");
            }
        }

       private EntitySet<Intake> _intakes;

        [Association(Storage = "_intakes", OtherKey = "_ingredientId", ThisKey = "IngredientId")]
        public EntitySet<Intake> Intakes
        {
            get { return this._intakes; }
            set { this._intakes.Assign(value); }
        }


        // Assign handlers for the add and remove operations, respectively.
        public Ingredient()
        {
            _intakes = new EntitySet<Intake>(
                new Action<Intake>(this.attach_Intake),
                new Action<Intake>(this.detach_Intake)
                );
        }

        // Called during an add operation
        private void attach_Intake(Intake intake)
        {
            NotifyPropertyChanging("Intake");
            intake.IngredientOfIntake = this;
        }

        // Called during a remove operation
        private void detach_Intake(Intake intake)
        {
            NotifyPropertyChanging("Intake");
            intake.IngredientOfIntake = null;
        }

        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    #endregion
    }

    #region DayTable
    [Table]
    public class Day : INotifyPropertyChanged, INotifyPropertyChanging
    {

        // Define ID: private field, public property, and database column.
        private int _id;

        [Column(DbType = "INT NOT NULL IDENTITY", IsDbGenerated = true, IsPrimaryKey = true)]
        public int Id
        {
            get { return _id; }
            set
            {
                NotifyPropertyChanging("Id");
                _id = value;
                NotifyPropertyChanged("Id");
            }
        }

        // Define category name: private field, public property, and database column.
        private DateTime _date;

        [Column]
        public DateTime Date
        {
            get { return _date; }
            set
            {
                NotifyPropertyChanging("Date");
                _date = value;
                NotifyPropertyChanged("Date");
            }
        }

        private int _total;

        [Column]
        public int Total
        {
            get { return _total; }
            set
            {
                NotifyPropertyChanging("Total");
                _total = value;
                NotifyPropertyChanged("Total");
            }
        }

        private float _carbohydrates;

        [Column]
        public float Carbohydrates
        {
            get { return _carbohydrates; }
            set
            {
                NotifyPropertyChanging("Carbohydrates");
                _carbohydrates = value;
                NotifyPropertyChanged("Carbohydrates");
            }
        }

        private float _protein;

        [Column]
        public float Protein
        {
            get { return _protein; }
            set
            {
                NotifyPropertyChanging("Protein");
                _protein = value;
                NotifyPropertyChanged("Protein");
            }
        }

        private float _fat;

        [Column]
        public float Fat
        {
            get { return _fat; }
            set
            {
                NotifyPropertyChanging("Fat");
                _fat = value;
                NotifyPropertyChanged("Fat");
            }
        }

        private float _fibre;

        [Column]
        public float Fibre
        {
            get { return _fibre; }
            set
            {
                NotifyPropertyChanging("Fibre");
                _fibre = value;
                NotifyPropertyChanged("Fibre");
            }
        }

        private EntitySet<Intake> _intakes;

        [Association(Storage = "_intakes", OtherKey = "_dayId", ThisKey = "Id")]
        public EntitySet<Intake> Intakes
        {
            get { return this._intakes; }
            set { this._intakes.Assign(value); }
        }


        // Assign handlers for the add and remove operations, respectively.
        public Day()
        {
            _intakes = new EntitySet<Intake>(
                new Action<Intake>(this.attach_Intake),
                new Action<Intake>(this.detach_Intake)
                );
        }

        // Called during an add operation
        private void attach_Intake(Intake intake)
        {
            NotifyPropertyChanging("Intake");
            intake.DayIntake = this;
        }

        // Called during a remove operation
        private void detach_Intake(Intake intake)
        {
            NotifyPropertyChanging("Intake");
            intake.DayIntake = null;
        }

        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }
    #endregion
}