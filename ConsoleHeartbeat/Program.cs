using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Приложение запущено.");
        Console.WriteLine("Нажмите F12 для выхода.");

        using CancellationTokenSource cts = new();

        // Запускаем фоновую задачу
        var backgroundTask = Task.Run(() => BackgroundWorker(cts.Token));

        // Основной цикл (асинхронный)
        while (true)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(intercept: true).Key;
                if (key == ConsoleKey.F12)
                {
                    Console.WriteLine("\nЗавершение работы...");
                    cts.Cancel(); // остановить фоновый worker
                    break;
                }
            }

            // Имитация основной работы
            await Task.Delay(200);
        }

        // Дожидаемся завершения фонового потока
        await backgroundTask;

        Console.WriteLine("Фоновый поток завершён. Программа закрыта.");
    }

    static async Task BackgroundWorker(CancellationToken token)
    {
        int counter = 0;

        try
        {
            while (!token.IsCancellationRequested)
            {
                counter++;
                Console.Write($"\r[Фоновый поток] Выполнено циклов: {counter}");
                await Task.Delay(1000, token); // каждую секунду
            }
        }
        catch (TaskCanceledException)
        {
            // ожидаемая отмена — ничего страшного
        }

        Console.WriteLine("\nФоновый поток остановлен.");
    }
}
