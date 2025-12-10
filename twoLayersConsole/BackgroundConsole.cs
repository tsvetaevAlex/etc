using System;
namespace twolayersConsole
{

    public class BackgroundConsole : ConsoleLayer
    {
        public override void Render()
        {
            int bottom = Console.WindowHeight - 1;

            Console.SetCursorPosition(0, bottom);
            Console.ForegroundColor = ConsoleColor.DarkGray;

            for (int x = 0; x < Width; x++)
                Console.Write(Buffer[bottom, x]);

            Console.ResetColor();
        }
    }
}