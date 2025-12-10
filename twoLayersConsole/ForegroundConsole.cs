using System;
namespace twolayersConsole
{

    public class ForegroundConsole : ConsoleLayer
    {
        private static int linesCount = 0;

        public void println(string message)
        {
            Console.SetCursorPosition(0,linesCount);
            Console.WriteLine(message);
            linesCount++;
        }
        public void print(string message)
        {
            Console.Write(message);
            linesCount++;
        }
        public override void Render()
        {
            Console.ResetColor();

            for (int y = 0; y < Height; y++)
            {
                Console.SetCursorPosition(0, y);

                for (int x = 0; x < Width; x++)
                    Console.Write(Buffer[y, x]);
            }
        }
    }
}