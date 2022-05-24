﻿
struct Position
{
    public int row;
    public int col;

    public Position(int row,int col)
    {
        this.row = row; 
        this.col = col;
    }
}

class Program
{
    static void Main(string[] args)
    {
        byte right = 0;
        byte left = 1;
        byte down = 2;
        byte up = 3;
        int lastFoodTime = 0;
        int foodDissapearTime = 8000;
        int negativePoints = 0;

        Position[]directions = new Position[]
        {
            new Position(0, 1), // right
            new Position(0, -1), // left
            new Position(1, 0), // down
            new Position(-1, 0), // up
        };

        int direction = right;
        double sleepTime = 100;
        Console.BufferHeight = Console.WindowHeight;
        Random randomNumbersGenerator = new Random();
        Position food = new Position(randomNumbersGenerator.Next(0,Console.WindowHeight),
            randomNumbersGenerator.Next(0,Console.WindowWidth));
        lastFoodTime = Environment.TickCount;
        Console.SetCursorPosition(food.col,food.row);
        Console.Write("@");


        Queue<Position> snakeElements = new Queue<Position>();

        for (int i = 0; i <= 5; i++)
        {
            snakeElements.Enqueue(new Position(0, i));
        }
        foreach (Position position in snakeElements)
        {
            Console.SetCursorPosition(position.col, position.row);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("*");
        }


        while (true)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo userInput = Console.ReadKey();
                if (userInput.Key == ConsoleKey.LeftArrow)
                {
                    if (direction != right) direction = left;
                }
                if (userInput.Key == ConsoleKey.RightArrow)
                {
                    if (direction != left) direction = right;
                }
                if (userInput.Key == ConsoleKey.UpArrow)
                {
                    if (direction != down) direction = up;
                }
                if (userInput.Key == ConsoleKey.DownArrow)
                {
                    if (direction != up) direction = down;
                }
            }

            
            Position snakeHead = snakeElements.Last();
            Position newDirection = directions[direction];
            Position snakeNewHead = new Position(snakeHead.row + newDirection.row, snakeHead.col + newDirection.col);
            if(snakeNewHead.row < 0 || 
               snakeNewHead.col < 0 || 
               snakeNewHead.row >= Console.WindowHeight || 
               snakeNewHead.col >= Console.WindowWidth ||
               snakeElements.Contains(snakeNewHead))
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Game Over!");
                Console.WriteLine("Your points are: {0}", (snakeElements.Count-6) * 100);
                return;
            }
            snakeElements.Enqueue(snakeNewHead);
            Console.SetCursorPosition(snakeNewHead.col, snakeNewHead.row); 
            Console.Write("*");
            
            if (snakeNewHead.col == food.col && snakeNewHead.row == food.row)
            {

                do
                {
                    food = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                       randomNumbersGenerator.Next(0, Console.WindowWidth));
                }
                while (snakeElements.Contains(food));
                lastFoodTime = Environment.TickCount;
                Console.SetCursorPosition(food.col, food.row);
                Console.Write("@");
                sleepTime--;
            }
            else
            {
                Position last = snakeElements.Dequeue();
                Console.SetCursorPosition(last.col, last.row);
                Console.Write(" ");
            }

            if (Environment.TickCount - lastFoodTime >= foodDissapearTime)
            {
                negativePoints = negativePoints + 50;
                Console.SetCursorPosition(food.col, food.row);
                Console.Write(" ");
                do
                {
                    food = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                        randomNumbersGenerator.Next(0, Console.WindowWidth));
                }
                while (snakeElements.Contains(food));
                lastFoodTime = Environment.TickCount;
            }
            Console.SetCursorPosition(food.col, food.row);
            Console.Write("@");
            sleepTime -= 0.01;

            Thread.Sleep((int)sleepTime);
        }
    }
}
