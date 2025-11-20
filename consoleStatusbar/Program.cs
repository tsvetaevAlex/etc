using System;
using System.Threading;
using System.Threading.Tasks;

namespace consoleStatusbar
{
    class Program
    {
        private static readonly CancellationTokenSource StopToken = new();
        private static readonly StatusBar statusBar = new();
        private static bool cursorHidden = false;

        static void Main()
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            Console.Clear();
            Console.CursorVisible = true;

            await statusBar.LaunchAsync(
                message: "Heartbeat",
                hiColor: ConsoleColor.Red,
                lowColor: ConsoleColor.DarkRed,
                blinkDelay: 400,
                token: StopToken.Token
            );

            Console.WriteLine("Controls: F1-F4 = colors, F5 = hide cursor, F6 = pause/resume, F12 = exit");

            var keyTask = Task.Run(() =>
            {
                while (!StopToken.IsCancellationRequested)
                {
                    if (!Console.KeyAvailable)
                    {
                        Thread.Sleep(25);
                        continue;
                    }

                    var key = Console.ReadKey(true).Key;

                    switch (key)
                    {
                        case ConsoleKey.F12:
                            StopToken.Cancel();
                            break;
                        case ConsoleKey.F6:
                            statusBar.TogglePause();
                            break;
                        case ConsoleKey.F5:
                            cursorHidden = !cursorHidden;
                            Console.CursorVisible = !cursorHidden;
                            break;
                        case ConsoleKey.F1:
                            statusBar.SetColors(ConsoleColor.Red, ConsoleColor.DarkRed);
                            break;
                        case ConsoleKey.F2:
                            statusBar.SetColors(ConsoleColor.Green, ConsoleColor.DarkGreen);
                            break;
                        case ConsoleKey.F3:
                            statusBar.SetColors(ConsoleColor.Yellow, ConsoleColor.DarkYellow);
                            break;
                        case ConsoleKey.F4:
                            statusBar.SetColors(ConsoleColor.Blue, ConsoleColor.DarkBlue);
                            break;
                    }
                }
            });

            await keyTask;

            StopToken.Cancel();
            await statusBar.StopAsync();

            Console.CursorVisible = true;
            Console.WriteLine("\nApplication closed.");
        }
    }
}
