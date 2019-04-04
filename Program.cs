using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework27
{
    class Program
    {
        static void Main(string[] args)
        {     
            // start task 1
            Console.WriteLine("Enter some symbols. Enter Point to stop: ");
            char input;
            int spaceCount = 0;
            do
            {
                input = Console.ReadKey().KeyChar;
                if (input == ' ')
                    spaceCount++;
            }
            while (input != '.');
            Console.WriteLine("\nQuantity of spaces: " + spaceCount);
            // end task 1
            
            // start task 2  
            string ticket = "";
            while (true)
            {
                Console.Write("\nEnter the six-digit ticket number: ");
                ticket = Console.ReadLine();
                if (ticket.Length == 6)
                    break;
                else
                {
                    Console.WriteLine("Error input! Please retry.");
                    continue;
                }
            }
            int firstHalfTicketSum = Convert.ToInt32(ticket[0] + ticket[1] + ticket[2]);
            int secondHalfTicketSum = Convert.ToInt32(ticket[3] + ticket[4] + ticket[5]);
            if(firstHalfTicketSum == secondHalfTicketSum)
                Console.WriteLine("You have a lucky ticket.");
            else
                Console.WriteLine("You have an unlucky ticket.");
            // end task 2

            // start task 3  
            int text = 0;
            char symbol;
            string value;
            while(text != 46)
            {
                Console.WriteLine("\nEnter text with no points and press enter. Enter Point to exit.");
                value = Console.ReadLine();
                for(int i = 0; i < value.Length; ++i)
                {
                    text = Convert.ToInt32(value[i]);
                    if (text >= 65 && text <= 90)
                        text += 32;
                    else if (text >= 97 && text <= 122)
                        text -= 32;
                    else if (text == 46) // 46 is Point in ASCII
                        break;

                    symbol = (char)text;
                    Console.Write(symbol);
                }  
            }
            // end task 3

            // start task 4   
            int a, b;
            while(true)
            {
                Console.Write("\n\rEnter A: ");
                a = Convert.ToInt32(Console.ReadLine());
                Console.Write("\rEnter B: ");
                b = Convert.ToInt32(Console.ReadLine());
                if(a >= b)
                    Console.WriteLine("A must be less than B.");
                else
                    break;
            }
            int count = 0;
            Console.WriteLine();
            for (; a <= b; ++a)
            {
                for (count = 0; count < a; ++count)
                {
                    Console.Write(a + " ");
                }
                Console.WriteLine("\r");
            }
            // end task 4 
            
            // start task 5
            Console.WriteLine("Enter any number.");
            int number = Convert.ToInt32(Console.ReadLine());
            string invNumber = "";
            while(number != 0)
            {
                int invNum = number % 10;
                number /= 10;
                invNumber += Convert.ToString(invNum);
            }
            number = Convert.ToInt32(invNumber);
            Console.WriteLine(number);
            // end task 5

            Console.ReadKey();
        }
    }
}
