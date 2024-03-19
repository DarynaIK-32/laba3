/*Скласти опис класу для представлення часу. Передбачити можливості установки часу 
 * і зміни його окремих полів (година, хвилина, секунда) з перевіркою допустимості 
 * введених значень. У разі недопустимих значень полів викинути помилку. Створити 
 * методи зміни часу на задану кількість годин, хвилин і секунд.*/

using System;
using System.IO;
using Newtonsoft.Json;

namespace TimeApp
{
    class Time
    {
        private int hour;
        private int minute;
        private int second;

        public int Hour
        {
            get { return hour; }
            set
            {
                if (value >= 0 && value < 24)
                    hour = value;
                else
                    throw new ArgumentOutOfRangeException(nameof(value), "Hour must be between 0 and 23.");
            }
        }

        public int Minute
        {
            get { return minute; }
            set
            {
                if (value >= 0 && value < 60)
                    minute = value;
                else
                    throw new ArgumentOutOfRangeException(nameof(value), "Minute must be between 0 and 59.");
            }
        }

        public int Second
        {
            get { return second; }
            set
            {
                if (value >= 0 && value < 60)
                    second = value;
                else
                    throw new ArgumentOutOfRangeException(nameof(value), "Second must be between 0 and 59.");
            }
        }

        public Time(int hour, int minute, int second)
        {
            Hour = hour;
            Minute = minute;
            Second = second;
        }

        public void AddHours(int hours)
        {
            Hour = (Hour + hours) % 24;
        }

        public void AddMinutes(int minutes)
        {
            int totalMinutes = Hour * 60 + Minute + minutes;
            Hour = (totalMinutes / 60) % 24;
            Minute = totalMinutes % 60;
        }

        public void AddSeconds(int seconds)
        {
            int totalSeconds = Hour * 3600 + Minute * 60 + Second + seconds;
            Hour = (totalSeconds / 3600) % 24;
            Minute = (totalSeconds / 60) % 60;
            Second = totalSeconds % 60;
        }

        public override string ToString()
        {
            return $"{Hour:D2}:{Minute:D2}:{Second:D2}";
        }

        public void SaveToJson(string filePath)
        {
            string json = JsonConvert.SerializeObject(this);
            File.WriteAllText(filePath, json);
        }

        public static Time LoadFromJson(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found.", filePath);

            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Time>(json);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Введіть години:");
                int hours = int.Parse(Console.ReadLine());

                Console.WriteLine("Введіть хвилини:");
                int minutes = int.Parse(Console.ReadLine());

                Console.WriteLine("Введіть секунди:");
                int seconds = int.Parse(Console.ReadLine());

                Time currentTime = new Time(hours, minutes, seconds);
                Console.WriteLine("Початковий час: " + currentTime);

                Console.WriteLine("Введіть кількість годин для додавання:");
                int addHours = int.Parse(Console.ReadLine());
                currentTime.AddHours(addHours);

                Console.WriteLine("Введіть кількість хвилин для додавання:");
                int addMinutes = int.Parse(Console.ReadLine());
                currentTime.AddMinutes(addMinutes);

                Console.WriteLine("Введіть кількість секунд для додавання:");
                int addSeconds = int.Parse(Console.ReadLine());
                currentTime.AddSeconds(addSeconds);

                Console.WriteLine("Новий час: " + currentTime);

                currentTime.SaveToJson(@"C:\Users\1\Desktop\laba3\ConsoleApp1\time.json.txt");

                Time loadedTime = Time.LoadFromJson(@"C:\Users\1\Desktop\laba3\ConsoleApp1\time.json.txt");
                Console.WriteLine("Час з файлу: " + loadedTime);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка: " + ex.Message);
            }
        }
    }
}
