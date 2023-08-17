using System;
using System.Collections.Generic;

namespace SnakeGame
{
    internal class Program
    {
		
		Direction? direction = null;
		Random? random = null;
		Tile[,]? map;

		bool closeRequested = false;
		int width;
		int height;

		static void Main(string[] args)
        {
			var obj = new Program();

			Exception? exception = null;
			obj.width = Console.WindowWidth;
			obj.height = Console.WindowHeight;
			obj.map = new Tile[obj.width, obj.height];
			obj.random = new();

			char[] DirectionChars = { '^', 'v', '<', '>', };
            TimeSpan sleep = TimeSpan.FromMilliseconds(100);
            
            
            
            
            Queue<(int X, int Y)> snake = new();
            (int X, int Y) = (obj.width / 2, obj.height / 2);

			try
			{
				Console.CursorVisible = false;
				Console.Clear();
				snake.Enqueue((X, Y));
				obj.map[X, Y] = Tile.Snake;
				
				obj.PositionFood();
				Console.SetCursorPosition(X, Y);
				Console.Write('@');
				while (!obj.direction.HasValue && !obj.closeRequested)
				{
					obj.GetDirection();
				}
				while (!obj.closeRequested)
				{
					if (Console.WindowWidth != obj.width || Console.WindowHeight != obj.height)
					{
						Console.Clear();
						Console.Write("Console was resized. Snake game has ended.");
						return;
					}
					switch (obj.direction)
					{
						case Direction.Up: Y--; break;
						case Direction.Down: Y++; break;
						case Direction.Left: X--; break;
						case Direction.Right: X++; break;
					}
					if (X < 0 || X >= obj.width ||
						Y < 0 || Y >= obj.height ||
						obj.map[X, Y] is Tile.Snake)
					{
						Console.Clear();
						Console.Write("Game Over. Score: " + (snake.Count - 1) + ".");
						return;
					}
					Console.SetCursorPosition(X, Y);
					Console.Write(DirectionChars[(int)obj.direction!]);
					snake.Enqueue((X, Y));
					if (obj.map[X, Y] == Tile.Food)
					{
						obj.PositionFood();
					}
					else
					{
						(int x, int y) = snake.Dequeue();
						obj.map[x, y] = Tile.Open;
						Console.SetCursorPosition(x, y);
						Console.Write(' ');
					}
					obj.map[X, Y] = Tile.Snake;
					if (Console.KeyAvailable)
					{
						obj.GetDirection();
					}
					System.Threading.Thread.Sleep(sleep);
				}
			}
			catch (Exception e)
			{
				exception = e;
				throw;
			}
			finally
			{
				Console.CursorVisible = true;
				Console.Clear();
				Console.WriteLine(exception?.ToString() ?? "Snake was closed.");
			}
		}

		void GetDirection()
		{
			switch (Console.ReadKey(true).Key)
			{
				case ConsoleKey.UpArrow: direction = Direction.Up; break;
				case ConsoleKey.DownArrow: direction = Direction.Down; break;
				case ConsoleKey.LeftArrow: direction = Direction.Left; break;
				case ConsoleKey.RightArrow: direction = Direction.Right; break;
				case ConsoleKey.Escape: closeRequested = true; break;
			}
		}

		void PositionFood()
		{
			List<(int X, int Y)> possibleCoordinates = new();
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					if (map[i, j] is Tile.Open)
					{
						possibleCoordinates.Add((i, j));
					}
				}
			}
			int index = random.Next(possibleCoordinates.Count);
			(int X, int Y) = possibleCoordinates[index];
			map[X, Y] = Tile.Food;
			Console.SetCursorPosition(X, Y);
			Console.Write('+');
		}

		enum Direction
		{
			Up = 0,
			Down = 1,
			Left = 2,
			Right = 3,
		}

		enum Tile
		{
			Open = 0,
			Snake,
			Food,
		}
	}


}