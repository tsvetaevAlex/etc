using System;
using System.Threading;
using System.Threading.Tasks;

namespace twolayersConsole
{
    class Program
    {
        private static readonly CancellationTokenSource cts = new CancellationTokenSource();
        private static readonly BackgroundConsole backgroundConsole = new BackgroundConsole();
        private static readonly ForegroundConsole foregroundConsole = new ForegroundConsole();
        private static StatusBar statusBar;

        static async Task Main(string[] args)
        {
            statusBar = new StatusBar(
                message: "Heartbeat: ",
                hColor: ConsoleColor.Gray,
                lColor: ConsoleColor.DarkGray,
                blinkDelay: 500,
                stopSign: cts
            );

            for (int i = 0; i < 10; i++)
            {
                foregroundConsole.println($"Hello World line {i}");
                Thread.Sleep( 2000 );
            }
            await Task.Delay(5000);
            cts.Cancel();
            await Task.Delay(500);
        }
    }
}
