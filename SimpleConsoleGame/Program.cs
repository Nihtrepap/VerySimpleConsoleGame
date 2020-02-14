using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleConsoleGame
{
    class Program
    {
        private static Random rdmXY = new Random();
        private static Stopwatch timer = new Stopwatch();
        private static string userInput;
        private static readonly int boardWidth = 12;
        private static readonly int boardHeight = 12;
        private static int xPlayerPos = 1;
        private static int yPlayerPos = 1;
        private static int points = 0;
        private static int rdm = rdmXY.Next(1, 10);
        private static int maze = rdmXY.Next(1, 11);
        private static bool play = true;
        private static bool on = true;
        private static bool menu = true;
        private static string[] life = new string[3] { "<3", "<3", "<3" };
        private static readonly char[,] gamePLan = new char[boardWidth, boardHeight];

        static void Main(string[] args)
        {
            GameEngine();
        }
        /// <summary>
        /// This method is the heart of this program.
        /// <br></br>
        /// <see cref="BuildGamePlanArray"/>
        /// <br></br>
        /// <see cref="CheckIfPlayerOnX(int, int)"/>
        /// <br></br>
        /// <see cref="TeleportPlayer"/>
        /// <br></br>
        /// <see cref="DrawGameplan"/>
        /// <br></br>
        /// <see cref="GameOverGameWon"/>
        /// <br></br>
        /// <see cref="MoveAround(ConsoleKeyInfo)"/>
        /// <br></br>
        /// <see cref="PointTaken"/>
        /// <br></br>
        /// <see cref="RemoveHeart"/>
        /// <br></br>
        /// <see cref="RestartGame"/>
        /// <br></br>
        /// <see cref="ShowPlayerStatus"/>
        /// </summary>
        static void GameEngine()
        {
            ConsoleKeyInfo cKey = new ConsoleKeyInfo();
            while (menu)
            {
                timer.Start();
                while (on)
                {
                    while (play)
                    {
                        Console.Clear();
                        ShowPlayerStatus();
                        BuildGamePlanArray();
                        gamePLan[xPlayerPos, yPlayerPos] = 'A';
                        PointTaken();
                        DrawGameplan();
                        play = false;
                        GameOverGameWon();
                    }
                    MoveAround(cKey);
                }
            }
        }
        static void ShowPlayerStatus()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Your Lifes: {life[0]} {life[1]} {life[2]}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Your score {points}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        static void RemoveHeart()
        {
            if (points < 0 && points > -20) life[2] = "  ";
            else if (points < -20 && points > -50) life[1] = "  ";
            else if (points < -50) life[0] = "  ";
        }
        static void RestartGame()
        {
            Console.WriteLine("[1] - play again\n[2] - exit");
            userInput = Console.ReadLine();
            if (userInput == "1")
            {
                on = true; play = true; points = 0;
            }
            else if (userInput == "2") menu = false;
        }
        static void GameOverGameWon()
        {
            if (life[0] == "  ")
            {
                timer.Stop();
                on = false;
                Console.Clear();
                Console.WriteLine($"Game Over you lost the game in {string.Format("{0:0.00}", timer.Elapsed.Seconds)}");
                for (int i = 0; i < life.Length; i++)
                {
                    life[i] = "<3";
                }
                timer.Reset();
                RestartGame();
            }
            else if (points >= 50)
            {
                timer.Stop();
                on = false;
                Console.Clear();
                Console.WriteLine($"You won the game in {string.Format("{0:0.00}", timer.Elapsed.TotalSeconds)} seconds");
                Console.ForegroundColor = ConsoleColor.Gray;
                if (timer.Elapsed.TotalSeconds > 30) Console.WriteLine($"You were very slow and got {points += 2} points");
                else if (timer.Elapsed.TotalSeconds > 20 && timer.Elapsed.TotalSeconds <= 30) Console.WriteLine($"You did OK with.. {points += 5} points");
                else if (timer.Elapsed.TotalSeconds > 11 && timer.Elapsed.TotalSeconds <= 20) Console.WriteLine($"Are you the flash? Well played! {points += 10} points");
                else if (timer.Elapsed.TotalSeconds > 0 && timer.Elapsed.TotalSeconds <= 11) Console.WriteLine($"Ludacris mode on you, nicely done!! {points += 5} points");
                Console.ForegroundColor = ConsoleColor.White;
                timer.Reset();
                RestartGame();
            }
        }
        /// <summary>
        /// This method checks if A is on X. Uses varaiables from BuildGamePlanArray.
        /// </summary>
        /// <param name="i">Int to check position.</param>
        /// <param name="j">Int to check position.</param>
        /// <returns>An int to adjust points.</returns>
        /// <see cref="BuildGamePlanArray"/>
        static int CheckIfPlayerOnX(int i, int j)
        {
            if (gamePLan[xPlayerPos, yPlayerPos] == gamePLan[i, j])
            {
                RemoveHeart(); return points -= 1;
            }
            return 0;
        }
        /// <summary>
        /// This method checks if A is on B
        /// </summary>
        static void PointTaken()
        {
            if (xPlayerPos == rdm && yPlayerPos == rdm)
            {
                rdm = rdmXY.Next(1, 10);
                maze = rdmXY.Next(1, 11);
                points += 10;
            }
            else
            {
                gamePLan[rdm, rdm] = 'B';
            }
        }
        static void TeleportPlayer()
        {
            if (yPlayerPos < 1) yPlayerPos = 10;
            else if (yPlayerPos > 10) yPlayerPos = 1;
            else if (xPlayerPos < 1) xPlayerPos = 10;
            else if (xPlayerPos > 10) xPlayerPos = 1;
        }
        static char[,] BuildGamePlanArray()
        {
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    if (j == 11 || j == 0)
                    {
                        gamePLan[i, j] = '|';
                    }
                    else if (i == 0)
                    {
                        gamePLan[i, j] = '_';
                    }
                    else if (i == 11)
                    {
                        gamePLan[i, j] = '-';
                    }
                    else if (i == maze - j)
                    {
                        gamePLan[i, j] = 'X';
                        CheckIfPlayerOnX(i, j);
                    }
                    else if (j == maze)
                    {
                        gamePLan[i, j] = 'X';
                        CheckIfPlayerOnX(i, j);
                    }
                    else
                    {
                        gamePLan[i, j] = ' ';
                    }
                }
            }
            return gamePLan;
        }
        static void DrawGameplan()
        {
            for (int row = 0; row < gamePLan.GetLength(0); row++)
            {
                Console.WriteLine();
                for (int col = 0; col < gamePLan.GetLength(1); col++)
                {
                    Console.Write(gamePLan[row, col]);
                }
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nPLAY WITH WASD\nPRESS ENTER TO QUIT");
            Console.ForegroundColor = ConsoleColor.White;
        }
        /// <summary>
        /// This method handles "movement" of A.
        /// </summary>
        /// <param name="cKey"></param>
        /// <see cref="TeleportPlayer"/>
        static void MoveAround(ConsoleKeyInfo cKey)
        {
            if (Console.KeyAvailable)
            {
                cKey = Console.ReadKey(true);
                switch (cKey.Key)
                {
                    case ConsoleKey.W:
                        xPlayerPos--;
                        TeleportPlayer();
                        play = true;
                        break;
                    case ConsoleKey.S:
                        xPlayerPos++;
                        TeleportPlayer();
                        play = true;
                        break;
                    case ConsoleKey.A:
                        yPlayerPos--;
                        TeleportPlayer();
                        play = true;
                        break;
                    case ConsoleKey.D:
                        yPlayerPos++;
                        TeleportPlayer();
                        play = true;
                        break;
                    case ConsoleKey.Enter:
                        play = false;
                        on = false;
                        menu = false;
                        break;
                }
            }
        }
    }
}
