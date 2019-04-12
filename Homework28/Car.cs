using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework28
{
    public enum CarsModels
    {
        Mercedes,
        BMW,
        Audi,
        Volkswagen,
        Toyota
    }

    public enum CarsColors
    {
        White,
        Black,
        Blue,
        Gray,
        Red
    }

    public partial class Car
    {
        private CarsModels _carModel;
        private CarsColors _carColor;
        private int _carYearManuf;
        private int _carMileage;
        private int _carMaxSpeed;
        private string _additionalInform;

        private static int _сarsCreated;
        private static bool _allCarsIsWorked;

        static Car()
        {
            _сarsCreated = 0;
            _allCarsIsWorked = true;
        }

        public Car()
        {
            this._carModel = 0;
            this._carColor = 0;
            this._carYearManuf = DateTime.Today.Year;
            this._carMileage = 0;
            this._carMaxSpeed = 200;
            this._additionalInform = "";
            _сarsCreated++;
        }

        public Car(CarsModels model, CarsColors color, int year, int mileage = 0, int maxSpeed = 200, string info = "")
        {
            this._carModel = model;
            this._carColor = color;
            this._carYearManuf = year;
            this._carMileage = mileage;
            this._carMaxSpeed = maxSpeed;
            this._additionalInform = info;
            _сarsCreated++;
        }

        public Car(CarsModels model, int maxSpeed = 0)
        {
            this._carModel = model;
            this._carColor = 0;
            this._carYearManuf = DateTime.Today.Year;
            this._carMileage = 0;
            this._carMaxSpeed = maxSpeed;
            this._additionalInform = "";
            _сarsCreated++;
        }

        public CarsModels CarModel
        {
            get { return _carModel; }
            set { _carModel = value; }
        }
        public CarsColors CarColor
        {
            get { return _carColor; }
            set { _carColor = value; }
        }
        public int CarYearManuf
        {
            get { return _carYearManuf; }
            set { _carYearManuf = value; }
        }
        public int CarMileage
        {
            get { return _carMileage; }
            set { _carMileage = value; }
        }
        public int CarMaxSpeed
        {
            get { return _carMaxSpeed; }
            set { _carMaxSpeed = value; }
        }
        public string AditionalInform
        {
            get { return _additionalInform; }
            set { _additionalInform = value; }
        }
        public static int CarsCreated
        {
            get { return _сarsCreated; }
        }
        public static bool AllCarsIsWorked
        {
            get { return _allCarsIsWorked; }
            set { _allCarsIsWorked = value; }
        }   

        public void ShowInform(ref string inform)
        {
            Console.WriteLine("Additional Information for this car: ");
            if(inform != "")
                Console.WriteLine(inform);
            else
                Console.WriteLine("there is nothing :(");
        }     

        protected void ChangeInform(ref string inform)
        {
            Console.WriteLine("Change information and press Enter:");
            inform = Console.ReadLine();
        }

        public void ShowAndChangeInform()
        {
            int choice = 0;
            while (choice != 3)
            {
                Console.WriteLine("\nPress 1 to show information or press 2 to change information. Press 3 to exit. ");
                choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        ShowInform(ref _additionalInform);
                        break;
                    case 2:
                        ChangeInform(ref _additionalInform);
                        break;
                    default:
                        continue;
                }
            }           
        }
    }
}
