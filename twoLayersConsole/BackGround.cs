using System;
using System.Threading;
using System.Threading.Tasks;

namespace twoLayers
{
    internal class BackGround
    {
        private static  readonly int _blinkDelay = 10;

        public BackGround() { }
        public async Task LaunchASync()
        {
            int i = 0;
            while (true)
            {
                Console.WriteLine($"output line number: {i}");
                    i++;
                try
                {
                    await Task.Delay(_blinkDelay);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }

        }


    }
}
