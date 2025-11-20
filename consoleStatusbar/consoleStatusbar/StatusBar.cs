using System;
using System.Threading;
using System.Threading.Tasks;

namespace consoleStatusbar
{
    public class StatusBar
    {
        private Task _task;
        private CancellationToken _token;
        private readonly object _lock = new();

        private string _message;
        private ConsoleColor _hiColor;
        private ConsoleColor _lowColor;
        private int _blinkDelay;
        private bool _running;
        private bool _paused;
        private int _heartbeatCounter;

        private const string HeartSymbol = "♥";

        public StatusBar()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
        }

        public void SetColors(ConsoleColor hi, ConsoleColor low)
        {
            lock (_lock)
            {
                _hiColor = hi;
                _lowColor = low;
            }
        }

        public void TogglePause()
        {
            _paused = !_paused;
        }

        public async Task LaunchAsync(string message, ConsoleColor hiColor, ConsoleColor lowColor,
                                      int blinkDelay, CancellationToken token)
        {
            if (_running) return;

            _running = true;
            _message = message;
            _hiColor = hiColor;
            _lowColor = lowColor;
            _blinkDelay = blinkDelay;
            _token = token;
            _heartbeatCounter = 0;

            _task = Task.Run(async () =>
            {
                bool hi = true;

                while (!_token.IsCancellationRequested)
                {
                    if (!_paused)
                    {
                        int lastRow = Console.BufferHeight - 1;
                        Console.SetCursorPosition(0, lastRow);

                        lock (_lock)
                        {
                            Console.ForegroundColor = hi ? _hiColor : _lowColor;
                            Console.Write($"\r {_message} {HeartSymbol} {_heartbeatCounter:D4}   ");
                            Console.ResetColor();
                        }

                        hi = !hi;
                        _heartbeatCounter++;
                    }

                    try
                    {
                        await Task.Delay(_blinkDelay, _token);
                    }
                    catch (TaskCanceledException)
                    {
                        break;
                    }
                }

                int finalRow = Console.BufferHeight - 1;
                Console.SetCursorPosition(0, finalRow);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, finalRow);

            }, _token);

            await Task.Yield();
        }

        public async Task StopAsync()
        {
            _running = false;
            if (_task != null)
            {
                try { await _task; } catch (TaskCanceledException) { }
            }
        }
    }
}
