using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Snake
{
    class Snake
    {
        static void Main(string[] args)
        {
            List<Position> snakeElements = new List<Position>();
            Dictionary<string, Position> directions = new Dictionary<string, Position>();
            CreateDirections(directions);
            CreateSnake(snakeElements);
            string lastDirection = "Right";
            DrawSnake(snakeElements, lastDirection);

            Random randomNumbers = new Random();
            Position food = new Position(randomNumbers.Next(0, Console.WindowHeight),
                                         randomNumbers.Next(0, Console.WindowWidth));
            DrawFood(food);

            Position nextDirection = new Position(0, 1);
            Console.BufferHeight = Console.WindowHeight; //remove scroller
            double sleepTime = 100;

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo inputInfo = Console.ReadKey();
                    string directionName = inputInfo.Key.ToString().Replace("Arrow","");

                    if (inputInfo.Key == ConsoleKey.UpArrow && lastDirection != "Down")
                    {
                        lastDirection = directionName;
                        nextDirection = directions[directionName];
                        
                    }
                    else if (inputInfo.Key == ConsoleKey.DownArrow && lastDirection != "Up")
                    {
                        lastDirection = directionName;
                        nextDirection = directions[directionName];
                    }
                    else if (inputInfo.Key == ConsoleKey.LeftArrow && lastDirection != "Right")
                    {
                        lastDirection = directionName;
                        nextDirection = directions[directionName];
                    }
                    else if (inputInfo.Key == ConsoleKey.RightArrow && lastDirection != "Left")
                    {
                        lastDirection = directionName;
                        nextDirection = directions[directionName];
                    }
                }

                Position snakeHead = snakeElements.Last();
                Position snakeNewHead = new Position(snakeHead.Row + nextDirection.Row,
                                                     snakeHead.Col + nextDirection.Col);

                if (GameOver(snakeNewHead) || snakeElements.Contains(snakeNewHead))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine("Game over !");
                    Console.WriteLine($"Your score is : {(snakeElements.Count - 6) * 100}");
                    return;
                }

                snakeElements.Add(snakeNewHead);
                DrawSnake(snakeElements, lastDirection);

                if (snakeNewHead.Row == food.Row && snakeNewHead.Col == food.Col)
                {
                    do
                    {
                        food = new Position(randomNumbers.Next(0, Console.WindowHeight),
                                         randomNumbers.Next(0, Console.WindowWidth));
                    } 
                    while (snakeElements.Contains(food));
                    
                    DrawFood(food);
                    sleepTime--;
                }
                else
                {
                    Position last = snakeElements[0];
                    snakeElements.RemoveAt(0);
                    Console.SetCursorPosition(last.Col, last.Row);
                    Console.Write(" ");
                }

                DrawFood(food);

                sleepTime -= 0.01;

                Thread.Sleep((int)sleepTime);
            }
        }

        struct Position
        {
            public Position(int row, int col)
            {
                this.Row = row;
                this.Col = col;
            }
            public int Row { get; set; }
            public int Col { get; set; }
        }
        static List<Position> CreateSnake(List<Position> snakeElements)
        {
            for (int i = 0; i <= 5; i++)
            {
                snakeElements.Add(new Position(0, i));
            }

            return snakeElements;
        }

        static Dictionary<string, Position> CreateDirections(Dictionary<string, Position> input)
        {
            input.Add("Up", new Position(-1, 0));
            input.Add("Down", new Position(1, 0));
            input.Add("Left", new Position(0, -1));
            input.Add("Right", new Position(0, 1));

            return input;
        }
        static void DrawSnake(List<Position> snakeElements, string direction)
        {
            for(int i = 0; i < snakeElements.Count; i++)
            {
                Position position = snakeElements[i];
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.SetCursorPosition(position.Col, position.Row);

                if (i == snakeElements.Count - 1)
                {
                    if (direction == "Right")
                    {
                        Console.Write(">");
                    }
                    else if (direction == "Left")
                    {
                        Console.Write("<");
                    }
                    else if (direction == "Down")
                    {
                        Console.Write("v");
                    }
                    else if (direction == "Up")
                    {
                        Console.Write("^");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.SetCursorPosition(position.Col, position.Row);
                    Console.Write("*");
                }
            }
        }

        static void DrawFood(Position food)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(food.Col, food.Row);
            Console.Write("@");
        }

        static bool GameOver(Position headSnake) 
        {
            if (headSnake.Col < 0 ||
                headSnake.Row < 0||
                headSnake.Row >= Console.WindowHeight ||
                headSnake.Col >= Console.WindowWidth)
            {
                return true;
            }

            return false;
        
        }
    }
}
