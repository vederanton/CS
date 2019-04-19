using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework29
{
    public interface IPart
    {
        void Build();
    }

    public class House
    {
        public HousePart Basement = new Basement();
        public HousePart Wall1 = new Wall();
        public HousePart Wall2 = new Wall();
        public HousePart Wall3 = new Wall();
        public HousePart Wall4 = new Wall();
        public HousePart Door = new Door();
        public HousePart Window1 = new Window();
        public HousePart Window2 = new Window();
        public HousePart Window3 = new Window();
        public HousePart Window4 = new Window();
        public HousePart Roof = new Roof();

        public HousePart[] HouseParts = new HousePart[11];

        public void CreateArrParts()
        {
            HouseParts[0] = Basement;
            HouseParts[1] = Wall1;
            HouseParts[2] = Wall2;
            HouseParts[3] = Wall3;
            HouseParts[4] = Wall4;
            HouseParts[5] = Door;
            HouseParts[6] = Window1;
            HouseParts[7] = Window2;
            HouseParts[8] = Window3;
            HouseParts[9] = Window4;
            HouseParts[10] = Roof;
        }
    }

    public abstract class HousePart : IPart
    {
        public string Name;
        public bool IsBuilt = false;
        public abstract void Build();
    }

    public class Basement : HousePart
    {
        public Basement()
        {
            Name = "Фундамент";
        }

        public override void Build()
        {
            IsBuilt = true;
        }
    }

    public class Wall : HousePart
    {
        public Wall()
        {
            Name = "Стена";
        }

        public override void Build()
        {
            IsBuilt = true;
        }
    }

    public class Door : HousePart
    {
        public Door()
        {
            Name = "Дверь";
        }

        public override void Build()
        {
            IsBuilt = true;
        }
    }

    public class Window : HousePart
    {
        public Window()
        {
            Name = "Окно";
        }

        public override void Build()
        {
            IsBuilt = true;
        }
    }

    public class Roof : HousePart
    {
        public Roof()
        {
            Name = "Крыша";
        }

        public override void Build()
        {
            IsBuilt = true;
        }
    }

    public interface IWorker
    {
        void Hire();
        void Work(House house, Team team);
    }

    public class Team
    {
        public TeamPart Worker1 = new Worker();
        public TeamPart Worker2 = new Worker();
        public TeamPart TeamLeader = new TeamLeader();

        public TeamPart[] TeamPartsWorkers = new TeamPart[2];

        public void CreateArrParts()
        {
            TeamPartsWorkers[0] = Worker1;
            TeamPartsWorkers[1] = Worker2;
        }
    }

    public abstract class TeamPart : IWorker
    {
        public bool IsHired = false;
        public abstract void Hire();
        public abstract void Work(House house, Team team);
    }

    public class Worker : TeamPart
    {
        public override void Hire()
        {
            IsHired = true;
        }

        public override void Work(House house, Team team)
        {
            team.CreateArrParts();
            house.CreateArrParts();
            Console.Clear();
            for (int i = 0; i < team.TeamPartsWorkers.Length; ++i)
            {
                if (team.TeamPartsWorkers[i].IsHired == true)
                {
                    int temp = 0;
                    foreach (var part in house.HouseParts)
                    {
                        if (part.IsBuilt == false)
                        {
                            part.Build();
                            Console.WriteLine($"Объект {part.Name} построен!\n");
                            temp = 1;
                            break;
                        }
                        if (temp == 1)
                            break;
                    }
                }
            }
        }
    }

    public class TeamLeader : TeamPart
    {
        public override void Hire()
        {
            IsHired = true;
        }

        public override void Work(House house, Team team)
        {
            house.CreateArrParts();

            if (team.TeamLeader.IsHired == true)
            {
                Console.Clear();
                Console.WriteLine("Отчет по строительству дома: \n");
                foreach (var part in house.HouseParts)
                {
                    if (part.IsBuilt == true)
                        Console.WriteLine($"Построен объект: {part.Name}");
                    else
                        Console.WriteLine($"Необходимо построить: {part.Name}");
                }
                Console.WriteLine();
            }
        }
    }

    class Program
    {
        static bool CheckGameWin(House house)
        {
            if (house.Roof.IsBuilt == true)
            {
                return true;
            }
            return false;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать в игру \"Строительство дома\"!");
            Console.WriteLine("\nДля продолжения нажмите любую кнопку...");
            Console.ReadKey();
            Console.Clear();
            char choice;
            do
            {
                Console.WriteLine("Прежде чем начать, нужно сформировать строительную команду. Сформировать?");
                Console.WriteLine("\nВ дальнейшем нажимайте соответствующую кнопку для выбора действия: ");
                Console.WriteLine("\n1 - нанять прораба");
                Console.WriteLine("2 - нет, стройка не для меня");
                choice = Console.ReadKey().KeyChar;
                switch (choice)
                {
                    case '1':
                        House house = new House();
                        Team team = new Team();
                        team.TeamLeader.Hire();
                        char howMany;
                        do
                        {
                            Console.Clear();
                            Console.WriteLine("Отлично! Но без работяг прораб не справится... Скольких рабочих нанять?");
                            Console.WriteLine("\n1 - одного");
                            Console.WriteLine("2 - двоих");
                            howMany = Console.ReadKey().KeyChar;
                            switch (howMany)
                            {
                                case '1':
                                    team.Worker1.Hire();
                                    break;
                                case '2':
                                    team.Worker1.Hire();
                                    team.Worker2.Hire();
                                    break;
                                default:
                                    continue;
                            }
                        }
                        while (howMany != '1' && howMany != '2');

                        char whatNext;
                        bool checkWin = false;
                        Console.Clear();
                        do
                        {
                            Console.WriteLine("Выберите дальнейшие действия: ");
                            Console.WriteLine("\n1 - строительство");
                            Console.WriteLine("2 - отчет от прораба");
                            if (team.Worker2.IsHired == false)
                                Console.WriteLine("\n3 - нанять еще одного строителя");
                            whatNext = Console.ReadKey().KeyChar;                           
                            switch (whatNext)
                            {
                                case '1':
                                    team.Worker1.Work(house, team);
                                    checkWin = CheckGameWin(house);
                                    break;
                                case '2':
                                    team.TeamLeader.Work(house, team);
                                    break;
                                case '3':
                                    if (team.Worker2.IsHired == false)
                                    {
                                        team.Worker2.Hire();
                                        Console.Clear();
                                        Console.WriteLine("Еще один строитель нанят на работу!\n");
                                    }
                                    else
                                        Console.Clear();
                                    break;
                                default:
                                    Console.Clear();
                                    continue;
                            }
                        }
                        while (checkWin != true);

                        Console.Clear();
                        Console.WriteLine(@"
                           ()
                          ()
                           ()
                          ()
                            )  )
                           ((                  /\
                            (_) /  \  /\
                    ________[_]________ /\/    \/  \
           /\      /\        ______    \    /   /\/\  /\/\
          /  \    //_\       \    /\    \  /\/\/    \/    \
   /\    / /\/\  //___\       \__/  \    \/
  /  \  /\/    \//_____\       \ |[]|     \
 /\/\/\/       //_______\       \|__|      \
/      \      / XXXXXXXXXX\                  \
        \    / _I_II  I__I_\__________________\
               I_I | I__I_____[]_ | _[]_____I
                 I_II  I__I_____[] _| _[]_____I
                   I II__I I     XXXXXXX I
            ~~~~~'   '~~~~~~~~~~~~~~~~~~~~~~~~");

                        break;
                    default:
                        Console.Clear();
                        continue;
                }
            }
            while (choice != '2' && choice != '1');
            if (choice == '2')
                Console.WriteLine("Игра окончена! Жаль...\n");
            else
                Console.WriteLine("\n\nВы построили этот роскошный дом! Поздравляем!\n");
        }
    }
}
