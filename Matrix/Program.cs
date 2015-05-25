using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    class Program
    {
        private static Random random = new Random();
        private static bool time = false;

        static void Main(string[] args)
        {
            Console.Title = "Matrix";
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WindowLeft = Console.WindowTop = 0;
            Console.WindowHeight = Console.BufferHeight = Console.LargestWindowHeight;
            Console.WindowWidth = Console.BufferWidth = Console.LargestWindowWidth;

            int width, height;

            int[] a, b;

            Initialize(out width, out height, out a, out b);

            while (true)
            {
                MatrixSteep(width, height, a, b);

                System.Threading.Thread.Sleep(10);

                if (Console.KeyAvailable && Console.ReadKey().Key == ConsoleKey.F5)
                    Initialize(out width, out height, out a, out b);
            }
        }

        private static void Initialize(out int width, out int height, out int[] a, out int[] b)
        {
            int height1, height2;

            height = Console.WindowHeight;

            height1 = height / 2;

            height2 = height1 / 2;

            width = Console.WindowWidth - 1;

            a = new int[width];

            b = new int[width];

            Console.Clear();

            for (int x = 0; x < width; x++)
            {
                a[x] = random.Next(height);

                b[x] = random.Next(height2 * (x % 11 != 10 ? 2 : 1), height1 * (x % 11 != 10 ? 2 : 1));
            }
        }

        private static void MatrixSteep(int width, int height, int[] a, int[] b)
        {
            time = !time;

            for (int x = 0; x < width; x++)
            {
                if (x % 11 == 10)
                {
                    if (!time)
                        continue;

                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;

                    Console.SetCursorPosition(x, InBoxY(a[x] - 2 - (b[x] / 40 * 2), height));

                    Console.Write(RandomChar());

                    Console.ForegroundColor = ConsoleColor.Green;
                }

                Console.SetCursorPosition(x, a[x]);

                Console.Write(RandomChar());

                a[x] = InBoxY(a[x] + 1, height);

                Console.SetCursorPosition(x, InBoxY(a[x] - b[x], height));

                Console.Write(' ');
            }
        }

        private static int InBoxY(int n, int height)
        {
            n = n % height;

            return n < 0 ? n + height : n;
        }

        private static char RandomChar()
        {
            switch (random.Next(5))
            {

                case 0:

                    return (char)('0' + random.Next(10));

                case 2:

                    return (char)('a' + random.Next(27));

                case 4:

                    return (char)('A' + random.Next(27));

                default:

                    int i = random.Next(32, 255);

                    if (i == 183)
                        i = 200;

                    return (char)i;
            }

        }
    }
}
