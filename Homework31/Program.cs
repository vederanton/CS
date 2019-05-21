using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _12_05_2019
{
    public abstract class Car
    {
        public string CarName { get; set; }
        public int Speed { get; set; }
        public int DrivingDistance { get; set; }

        public delegate void CarStateHandler(object sender, CarEvenArgs e);
        public virtual event CarStateHandler SportsCarDriveEvent;
        public virtual event CarStateHandler PassengerCarDriveEvent;
        public virtual event CarStateHandler TruckDriveEvent;
        public virtual event CarStateHandler BusDriveEvent;

        public Car(string name)
        {
            CarName = name;
            var rand = new Random();
            Speed = rand.Next(1, 99);
            DrivingDistance = 0;
        }
        public abstract void Drive();

        public static void ShowMessage(object sender, CarEvenArgs e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine($"Скорость автомобиля составила {e.DriveSpeed} км\\ч.");
            Console.WriteLine();
        }

        public void ChangeSpeed()
        {
            var rand = new Random();
            Speed = rand.Next(1, 99);
        }
    }

    public class CarEvenArgs : EventArgs
    {
        public string Message;
        public int DriveSpeed;
        public CarEvenArgs(string m, int s)
        {
            Message = m;
            DriveSpeed = s;
        }
    } 

    public class SportsСar : Car
    {
        public override event CarStateHandler SportsCarDriveEvent;

        public SportsСar(string name) : base(name)
        {
        }

        public override void Drive()
        {
            DrivingDistance += Speed;
            ChangeSpeed();
            if(SportsCarDriveEvent != null)
            {
                SportsCarDriveEvent(this, new CarEvenArgs($"{CarName} проехала всего {DrivingDistance} километров.", Speed));
            }
        }
    }

    public class PassengerCar : Car
    {
        public override event CarStateHandler PassengerCarDriveEvent;
        public PassengerCar(string name) : base(name)
        {
        }

        public override void Drive()
        {
            DrivingDistance += Speed;
            ChangeSpeed();
            if (PassengerCarDriveEvent != null)
            {
                PassengerCarDriveEvent(this, new CarEvenArgs($"{CarName} промчалась {DrivingDistance} километров.", Speed));
            }
        }
    }

    public class Truck : Car
    {
        public override event CarStateHandler TruckDriveEvent;
        public Truck(string name) : base(name)
        {
        }

        public override void Drive()
        {
            DrivingDistance += Speed;
            ChangeSpeed();
            if (TruckDriveEvent != null)
            {
                TruckDriveEvent(this, new CarEvenArgs($"Путь, который проехал {CarName} составил {DrivingDistance} километров.", Speed));
            }
        }
    }

    public class Bus : Car
    {
        public override event CarStateHandler BusDriveEvent;
        public Bus(string name) : base(name)
        {
        }

        public override void Drive()
        {
            DrivingDistance += Speed;
            ChangeSpeed();
            if (BusDriveEvent != null)
            {
                BusDriveEvent(this, new CarEvenArgs($"{CarName} спокойно проехал {DrivingDistance} километров.", Speed));
            }
        }
    }

    public class CarComparer : IComparer<Car>
    {
        public int Compare(Car x, Car y)
        {
            if (x.DrivingDistance > y.DrivingDistance)
                return -1;
            else if (x.DrivingDistance < y.DrivingDistance)
                return 1;
            else
                return 0;
        }
    }

    public class Game
    {
        public Car SportsCar;
        public Car PassengerCar;
        public Car Truck;
        public Car Bus;
        public int GameDistance;
        public Game()
        {
            SportsCar = new SportsСar("Спортивная тачка");
            PassengerCar = new PassengerCar("Такси");
            Truck = new Truck("Грузовик");
            Bus = new Bus("Автобус");
            GameDistance = 200;
            SportsCar.SportsCarDriveEvent += Car.ShowMessage;
            PassengerCar.PassengerCarDriveEvent += Car.ShowMessage;
            Truck.TruckDriveEvent += Car.ShowMessage;
            Bus.BusDriveEvent += Car.ShowMessage;
        }

        public void StartGame()
        {
            while(SportsCar.DrivingDistance <= GameDistance || PassengerCar.DrivingDistance <= GameDistance ||
                Truck.DrivingDistance <= GameDistance || Bus.DrivingDistance <= GameDistance)
            {
                SportsCar.Drive();
                PassengerCar.Drive();
                Truck.Drive();
                Bus.Drive();
            }
            Results();
        }

        public void Results()
        {
            Car[] cars = { SportsCar, PassengerCar, Truck, Bus };
            Array.Sort(cars, new CarComparer());
            Console.WriteLine("Гонка окончена! Результаты:");
            Console.WriteLine($"1 место: {cars[0].CarName} проехал всего {cars[0].DrivingDistance} км.");
            Console.WriteLine($"2 место: {cars[1].CarName} проехал всего {cars[1].DrivingDistance} км.");
            Console.WriteLine($"3 место: {cars[2].CarName} проехал всего {cars[2].DrivingDistance} км.");
            Console.WriteLine($"4 место: {cars[3].CarName} проехал всего {cars[3].DrivingDistance} км.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Game MyGame = new Game();
            MyGame.StartGame();
        }
    }
}
