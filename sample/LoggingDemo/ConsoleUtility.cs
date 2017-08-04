using System;
using static System.Console;

namespace LoggingDemo
{
    public static class ConsoleUtility
    {
        public const string UNKNOWN = "unknown";

        public static string Option(string question, params string[] options)
        {
            if(options == null || options.Length == 0) return UNKNOWN;

            WriteLine(question);
            for(int i = 1; i <= options.Length; i++)
            {
                WriteLine($"{i}) {options[i-1]}");
            }
            WriteLine();
            WriteLine("Select the number of the option you wish to choose...");
            if(int.TryParse(ReadLine(), out int result) && result <= options.Length)
            {
                return options[result - 1];
            }

            return UNKNOWN;
        }

        public static string Question(string question)
        {
            WriteLine(question);
            return ReadLine();
        }

        public static T Question<T>(string question)
        {
            return (T)Convert.ChangeType(Question(question), typeof(T));
        }
    }
}
