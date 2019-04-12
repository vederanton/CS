using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework28
{
    class Program
    {
        static void Main(string[] args)
        {
            Car car1 = new Car();
            Car car2 = new Car(CarsModels.Audi, CarsColors.Blue, 2015);
            Car car3 = new Car(CarsModels.Toyota, CarsColors.Red, 2018, 20000, 240);
            Car car4 = new Car(CarsModels.Mercedes, 260);
            Car car5 = new Car(CarsModels.Volkswagen);

            Car[] cars = { car1, car2, car3, car4, car5 };

            for(int i = 0; i < cars.Length; ++i)
            {
                cars[i].PrintCarInfo();
                Console.WriteLine();
            }
        }
    }

    public partial class Car
    {
        public void PrintCarInfo()
        {
            Console.WriteLine($"Car model: {CarModel}");
            Console.WriteLine($"Car color: {CarColor}");
            Console.WriteLine($"Year of car manufacture: {CarYearManuf}");
            Console.WriteLine($"Car mileage: {CarMileage}");
            Console.WriteLine($"Maximum car speed: {CarMaxSpeed}");
            ShowInform(ref _additionalInform);
        }
    }
}
