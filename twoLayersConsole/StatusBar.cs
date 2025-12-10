using System;
using System.Threading;
using System.Threading.Tasks;

namespace twolayersConsole
{
    class StatusBar
    {
        private readonly ConsoleColor hiColor;
        private readonly ConsoleColor lowColor;
        private readonly string message;
        private readonly int blinkDelay;
        private readonly CancellationToken token;

        private Task backgroundTask;

        public StatusBar(
            string message,
            ConsoleColor hColor,
            ConsoleColor lColor,
            int blinkDelay,
            CancellationTokenSource stopSign
        )
        {
            this.message = message;
            this.hiColor = hColor;
            this.lowColor = lColor;
            this.blinkDelay = blinkDelay;
            this.token = stopSign.Token;

            // 🟡 Стартуем фонового работника ПРЯМО в конструкторе
            backgroundTask = Task.Run(() => HeartbeatAsync());
        }

        private async Task HeartbeatAsync()
        {
            int counter = 0;
            int bottom = Console.WindowHeight - 1;

            Console.CursorVisible = false;

            while (!token.IsCancellationRequested)
            {
                Console.SetCursorPosition(0, bottom);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(message);
                Console.ResetColor();

                Console.ForegroundColor = (counter % 2 == 0 ? hiColor : lowColor);
                Console.Write("█");
                Console.ResetColor();

                counter++;

                try
                {
                    await Task.Delay(blinkDelay, token);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }

            Console.SetCursorPosition(0, bottom);
            Console.Write("End.".PadRight(Console.WindowWidth));
        }
    }
}
