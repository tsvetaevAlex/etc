using System;

class Program
{
    static void Main()
    {
        string[] currencies = { "USD", "GBP", "RUB" };
        int selectedIndex = 0;

        ConsoleKey key;

        do
        {
            Console.Clear();
            Console.WriteLine("Выберите валюту (← → для выбора, Enter для подтверждения):");
            Console.WriteLine();

            for (int i = 0; i < currencies.Length; i++)
            {
                if (i == selectedIndex)
                {
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Gray;
                }

                Console.Write($" [{currencies[i]}] ");
                Console.ResetColor();
            }

            Console.WriteLine();
            key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.LeftArrow)
            {
                selectedIndex = (selectedIndex - 1 + currencies.Length) % currencies.Length;
            }
            else if (key == ConsoleKey.RightArrow)
            {
                selectedIndex = (selectedIndex + 1) % currencies.Length;
            }

        } while (key != ConsoleKey.Enter);

        Console.Clear();
        Console.WriteLine($"✅ Вы выбрали валюту: {currencies[selectedIndex]}");
    }
}
