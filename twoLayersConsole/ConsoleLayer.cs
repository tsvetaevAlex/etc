using System;
using System.Text;
namespace twolayersConsole
{

    public abstract class ConsoleLayer
    {
        protected readonly int Width;
        protected readonly int Height;
        protected readonly char[,] Buffer;

        protected ConsoleLayer()
        {
            Width = Console.WindowWidth;
            Height = Console.WindowHeight - 1; // не трогаем статус-бар! (полмедняя строка это статус-бар.)
            Buffer = new char[Height, Width];
            Clear();
        }

        public void Clear()
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    Buffer[y, x] = ' ';
        }

        public void Write(int x, int y, string text)
        {
            if (y < 0 || y >= Height) return;

            for (int i = 0; i < text.Length && x + i < Width; i++)
                Buffer[y, x + i] = text[i];
        }

        public abstract void Render();
    }
}