using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

// Directive for the data model.
using Kalorilaskuri.Model;
using System;
using System.Windows;


namespace Kalorilaskuri.ViewModel
{
    public class CalorieCounterViewModel : INotifyPropertyChanged
    {
        // LINQ to SQL data context for the local database.
        private CalorieCounterDataContext calorieCounterDB;

        // Class constructor, create the data context object.
        public CalorieCounterViewModel(string calorieCounterDBConnectionString)
        {
            calorieCounterDB = new CalorieCounterDataContext(calorieCounterDBConnectionString);
        }

        private ObservableCollection<Intake> _allIntakes;
        public ObservableCollection<Intake> AllIntakes
        {
            get { return _allIntakes; }
            set
            {
                _allIntakes = value;
                NotifyPropertyChanged("AllIntakes");
            }
        }

        /*
        private ObservableCollection<Ingredient> _allIngredients;
        public ObservableCollection<Ingredient> AllIngredients
        {
            get { return _allIngredients; }
            set
            {
                _allIngredients = value;
                NotifyPropertyChanged("AllIngredients");
            }
        }
         * */

        private List<Ingredient> _ingredientsList;
        public List<Ingredient> IngredientsList
        {
            get { return _ingredientsList; }
            set
            {
                _ingredientsList = value;
                NotifyPropertyChanged("IngredientsList");
            }
        }

        private List<Day> _daysList;
        public List<Day> DaysList
        {
            get { return _daysList; }
            set
            {
                _daysList = value;
                NotifyPropertyChanged("DaysList");
            }
        }

        // Add a to-do item to the database and collections.
        public void AddIntake(Intake newIntake)
        {
            // Add a to-do item to the data context.
            calorieCounterDB.Intakes.InsertOnSubmit(newIntake);

            // Save changes to the database.
            calorieCounterDB.SubmitChanges();

           
            // Add a to-do item to the "all" observable collection.
            AllIntakes.Add(newIntake);
        }

        /// <summary>
        /// Adds an intake to viewmodel and db,
        /// intake must contain ref to day, grams and calories
        /// </summary>
        /// <param name="newIntake"></param>
        /// <param name="ing"></param>
        public void AddIntake(Intake newIntake, Item ing)
        {
            Ingredient newIngredient = this.getIngredient(ing.Id);
            if (newIngredient == null)
            {
                newIngredient = new Ingredient();
                newIngredient.ItemId = ing.Id;
                newIngredient.Name = ing.Name;
                newIngredient.Calories = ing.Calories;
                newIngredient.Fat = ing.Fat;
                newIngredient.Carbohydrates = ing.Carbohydrates;
                newIngredient.Protein = ing.Protein;
                newIngredient.Portion = ing.Portion;
                newIngredient.PortionCalories = ing.PortionCalories;
                newIngredient.Portionweight = ing.Portionweight;
                this.AddIngredient(newIngredient);
            }

            newIntake.IngredientOfIntake = newIngredient;
            newIntake.DayIntake.Intakes.Add(newIntake); //voiko tää toimia???
            newIntake.DayIntake.Total = newIntake.DayIntake.Intakes.Select(i => i.Calories).Sum();
            MessageBox.Show("Total " + newIntake.DayIntake.Total);
            newIngredient.Intakes.Add(newIntake);
            AddIntake(newIntake);
            MessageBox.Show("lisätään " + newIntake.IngredientOfIntake.Name);
            //pitäis vielä päivättää dayslist
        }

        private Ingredient ingredientToAdd;

        private bool IsSameIngredient(Ingredient ingredientToCompare)
        {
            return ingredientToCompare.ItemId == ingredientToAdd.ItemId;
        }

        private bool IsSameIngredientByItemid(Ingredient ingredientToCompare)
        {
            return ingredientToCompare.ItemId == compareId;
        }

        /// <summary>
        /// Returns ingredient if one with same itemid
        /// already exists in database
        /// </summary>
        /// <param name="ingredient">Ingredient with itemid attached to find</param>
        /// <returns>Ingredient from db or null if not yet created</returns>
        public Ingredient getIngredient(Ingredient ingredient)
        {
            ingredientToAdd = ingredient;
            return IngredientsList.Find(IsSameIngredient);
        }

        int compareId;

        public Ingredient getIngredient(int itemid)
        {
            compareId = itemid;
            return IngredientsList.Find(IsSameIngredientByItemid);
        }

        /// <summary>
        /// Adds or updates ingredient to database
        /// </summary>
        /// <param name="newIngredient"></param>
        /// <returns></returns>
        public void AddIngredient(Ingredient newIngredient)
        {
            calorieCounterDB.Ingredients.InsertOnSubmit(newIngredient);
            calorieCounterDB.SubmitChanges();
            IngredientsList.Add(newIngredient);
        }

        /// <summary>
        /// Checks if the dates match comparing two
        /// different day-objects
        /// </summary>
        /// <param name="dayToCompare"></param>
        /// <returns></returns>
        private bool IsSameDay(Day dayToCompare)
        {
            int comparison = dayToCompare.Date.Day.CompareTo(dateToAdd.Day);
            if (comparison != 0) return false;
            else return true;
        }

        private DateTime dateToAdd = new DateTime();

        /// <summary>
        /// Returns a day from db if date
        /// is found
        /// </summary>
        /// <param name="date"></param>
        /// <returns>null if not found</returns>
        public Day getDay(DateTime date)
        {
            dateToAdd = date;
            return DaysList.Find(IsSameDay);
        }

        /// <summary>
        /// Adds a new day to the database if date is not found
        /// </summary>
        /// <param name="newDay"></param>
        /// <returns></returns>
        public Day AddDay(Day newDay)
        {
            dateToAdd = newDay.Date;
            Day find = getDay(newDay.Date);
            if (find != null)
            {
                return find;
            }

            newDay.Total = newDay.Intakes.Select(i => i.Calories).Sum(); //toimiiko?
            MessageBox.Show("Total: " + newDay.Total);
            calorieCounterDB.Days.InsertOnSubmit(newDay);
            calorieCounterDB.SubmitChanges();
            
            DaysList.Add(newDay);
            return newDay;
        }

        public void DeleteIntake(Intake intakeForDelete)
        {
            // Remove the to-do item from the "all" observable collection.
            intakeForDelete.IngredientOfIntake.Intakes.Remove(intakeForDelete);
            intakeForDelete.DayIntake.Intakes.Remove(intakeForDelete);
            AllIntakes.Remove(intakeForDelete);
            
            // Remove the to-do item from the data context.
            calorieCounterDB.Intakes.DeleteOnSubmit(intakeForDelete);

            // Save changes to the database.
            calorieCounterDB.SubmitChanges();
        }

        // Write changes in the data context to the database.
        public void SaveChangesToDB()
        {
            calorieCounterDB.SubmitChanges();
        }

        public void LoadCollectionsFromDatabase()
        {

            var intakesInDB = from Intake intake in calorieCounterDB.Intakes
                              select intake;

            AllIntakes = new ObservableCollection<Intake>(intakesInDB);
            /*
            var daysInDB = from Day day in calorieCounterDB.Days
                           select day;
             AllIngredients = new ObservableCollection<Ingredient>(ingredientsInDB);
             
             * var ingredientsInDB = from Ingredient ingredient in calorieCounterDB.Ingredients
                                  select ingredient;
             */

            DaysList = calorieCounterDB.Days.ToList();

            IngredientsList = calorieCounterDB.Ingredients.ToList();

        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the app that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}