using System;

namespace GitPusher
{
    public static class UI
    {
        #region colours
        public static void white()
        {
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void yellow()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
        }
        public static void cyan()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
        }
        public static void red()
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        public static void green()
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }

        public static void blank(int n)
        {
            for (int i = 0; i < n; i++)
                Console.WriteLine();
        }
        #endregion
    }
}
