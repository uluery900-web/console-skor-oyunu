using System;
using System.IO;
using System.Threading;

class ScoreGame
{
    static int width = 40;
    static int height = 20;

    static int playerX = width / 2;
    static int playerY = height - 2;

    static int itemX;
    static int itemY;

    static int score = 0;
    static int maxScore = 10;

    static Random rnd = new Random();
    static string logFile = "game_log.txt";

    static void Log(string message)
    {
        File.AppendAllText(logFile, DateTime.Now + " → " + message + Environment.NewLine);
    }

    static void SpawnItem()
    {
        itemX = rnd.Next(0, width);
        itemY = 0;
        Log($"UPDATE → itemSpawned x={itemX} y={itemY}");
    }

    static void Draw()
    {
        Console.Clear();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (x == playerX && y == playerY)
                    Console.Write("@");
                else if (x == itemX && y == itemY)
                    Console.Write("*");
                else
                    Console.Write(" ");
            }
            Console.WriteLine();
        }
        Console.WriteLine("Score: " + score);
    }

    static void MoveItem()
    {
        itemY++;
        Log($"UPDATE → itemMove x={itemX} y={itemY}");

        if (itemY > height)
            SpawnItem();
    }

    static void CheckCollision()
    {
        Log($"CHECK → playerX={playerX} playerY={playerY} itemX={itemX} itemY={itemY}");

        if (playerX == itemX && playerY == itemY)
        {
            score++;
            Log($"COLLISION → score={score}");
            SpawnItem();
        }
    }

    static void Input()
    {
        if (Console.KeyAvailable)
        {
            var key = Console.ReadKey(true).Key;
            Log($"INPUT → key={key} playerX={playerX} playerY={playerY}");

            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    if (playerX > 0) playerX--;
                    break;
                case ConsoleKey.RightArrow:
                    if (playerX < width - 1) playerX++;
                    break;
                case ConsoleKey.UpArrow:
                    if (playerY > 0) playerY--;
                    break;
                case ConsoleKey.DownArrow:
                    if (playerY < height - 1) playerY++;
                    break;
            }

            Log($"MOVE → playerX={playerX} playerY={playerY}");
        }
    }

    static void Main()
    {
        Console.CursorVisible = false;
        SpawnItem();

        DateTime startTime = DateTime.Now;
        int maxTime = 30; // oyun süresi saniye

        while (true)
        {
            Input();
            MoveItem();
            CheckCollision();
            Draw();

            if ((DateTime.Now - startTime).TotalSeconds >= maxTime || score >= maxScore)
            {
                Log($"GAME END → finalScore={score}");
                Console.WriteLine("Game Over! Final Score: " + score);
                break;
            }

            Thread.Sleep(150);
        }
    }
}
