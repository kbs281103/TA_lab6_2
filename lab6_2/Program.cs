using System;
using System.Collections.Generic;

namespace TramStopDatabase
{
    // Інтерфейс для операцій зупинок трамваю
    public interface ITramStopOperations
    {
        void AddRecord(Hour hour);
        void EditRecord(int index, Hour newHour);
    }

    // Батьківський клас: Трамвайна зупинка
    public class TramStop
    {
        public string Name { get; set; }
        public List<int> RouteNumbers { get; set; }

        public TramStop(string name, List<int> routeNumbers)
        {
            Name = name;
            RouteNumbers = routeNumbers;
        }

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Трамвайна зупинка: {Name}");
            Console.WriteLine("Номери маршрутів: " + string.Join(", ", RouteNumbers));
        }
    }

    // Похідний клас: Година
    public class Hour : TramStop
    {
        public int PassengerCount { get; set; }
        public string Comment { get; set; }

        public Hour(string name, List<int> routeNumbers, int passengerCount, string comment)
            : base(name, routeNumbers)
        {
            PassengerCount = passengerCount;
            Comment = comment;
        }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Кількість пасажирів: {PassengerCount}");
            Console.WriteLine($"Коментар: {Comment}");
        }
    }

    // Клас для управління базою даних
    public class Database : ITramStopOperations
    {
        private List<Hour> hours;

        public Database()
        {
            hours = new List<Hour>();
            LoadData();
        }

        private void LoadData()
        {
            // Статичні дані для початкового масиву
            hours.Add(new Hour("Зупинка 1", new List<int> { 1, 2 }, 100, "Ранок"));
            hours.Add(new Hour("Зупинка 2", new List<int> { 3, 4 }, 50, "Полудень"));
            hours.Add(new Hour("Зупинка 3", new List<int> { 5, 6 }, 30, "Вечір"));
            hours.Add(new Hour("Зупинка 4", new List<int> { 7, 8 }, 75, "Ніч"));
            hours.Add(new Hour("Зупинка 5", new List<int> { 9, 10 }, 10, "Пізня ніч"));
        }

        public void AddRecord(Hour hour)
        {
            hours.Add(hour);
        }

        public void EditRecord(int index, Hour newHour)
        {
            if (index >= 0 && index < hours.Count)
            {
                hours[index] = newHour;
            }
            else
            {
                Console.WriteLine("Некоректний індекс.");
            }
        }

        public void DeleteRecord(int index)
        {
            if (index >= 0 && index < hours.Count)
            {
                hours.RemoveAt(index);
            }
            else
            {
                Console.WriteLine("Некоректний індекс.");
            }
        }

        public void DisplayAllRecords()
        {
            foreach (var hour in hours)
            {
                hour.DisplayInfo();
                Console.WriteLine();
            }
        }

        public void CalculateAndDisplayResults()
        {
            int totalPassengers = 0;
            int minPassengers = int.MaxValue;
            string longestComment = string.Empty;
            string minHour = string.Empty;

            foreach (var hour in hours)
            {
                totalPassengers += hour.PassengerCount;
                if (hour.PassengerCount < minPassengers)
                {
                    minPassengers = hour.PassengerCount;
                    minHour = hour.Name;
                }
                if (hour.Comment.Length > longestComment.Length)
                {
                    longestComment = hour.Comment;
                }
            }

            Console.WriteLine($"Загальна кількість пасажирів: {totalPassengers}");
            Console.WriteLine($"Година з найменшою кількістю пасажирів: {minHour} ({minPassengers} пасажирів)");
            Console.WriteLine($"Найдовший коментар: {longestComment}");
        }
    }

    class Program
    {
        static void Main()
        {
            // Встановлення кодування UTF-8 для консолі
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Database db = new Database();

            while (true)
            {
                Console.WriteLine("Меню:");
                Console.WriteLine("1 - Додати запис");
                Console.WriteLine("2 - Редагувати запис");
                Console.WriteLine("3 - Видалити запис");
                Console.WriteLine("4 - Показати всі записи");
                Console.WriteLine("5 - Обчислити та показати результати");
                Console.WriteLine("Enter - Вихід");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddRecord(db);
                        break;
                    case "2":
                        EditRecord(db);
                        break;
                    case "3":
                        DeleteRecord(db);
                        break;
                    case "4":
                        db.DisplayAllRecords();
                        break;
                    case "5":
                        db.CalculateAndDisplayResults();
                        break;
                    case "":
                        return;
                    default:
                        Console.WriteLine("Некоректний вибір.");
                        break;
                }
            }
        }

        static void AddRecord(Database db)
        {
            Console.Write("Введіть назву зупинки: ");
            string name = Console.ReadLine();
            Console.Write("Введіть номери маршрутів (через кому): ");
            List<int> routes = new List<int>();
            foreach (var num in Console.ReadLine().Split(','))
            {
                if (int.TryParse(num.Trim(), out int route))
                {
                    routes.Add(route);
                }
            }

            int passengers;
            do
            {
                Console.Write("Введіть кількість пасажирів: ");
            } while (!int.TryParse(Console.ReadLine(), out passengers) || passengers < 0);

            Console.Write("Введіть коментар: ");
            string comment = Console.ReadLine();

            db.AddRecord(new Hour(name, routes, passengers, comment));
        }


        static void EditRecord(Database db)
        {
            Console.Write("Введіть індекс запису для редагування: ");
            int index = int.Parse(Console.ReadLine());

            Console.Write("Введіть нову назву зупинки: ");
            string name = Console.ReadLine();
            Console.Write("Введіть нові номери маршрутів (через кому): ");
            List<int> routes = new List<int>();
            foreach (var num in Console.ReadLine().Split(','))
            {
                if (int.TryParse(num.Trim(), out int route))
                {
                    routes.Add(route);
                }
            }
            Console.Write("Введіть нову кількість пасажирів: ");
            int passengers = int.Parse(Console.ReadLine());
            Console.Write("Введіть новий коментар: ");
            string comment = Console.ReadLine();

            db.EditRecord(index, new Hour(name, routes, passengers, comment));
        }

        static void DeleteRecord(Database db)
        {
            Console.Write("Введіть індекс запису для видалення: ");
            int index = int.Parse(Console.ReadLine());
            db.DeleteRecord(index);
        }
    }
}

