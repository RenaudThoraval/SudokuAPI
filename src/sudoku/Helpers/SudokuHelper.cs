using System;
using System.Collections.Generic;
using System.Linq;

namespace sudoku.Helpers
{

	public class SudokuHelper
	{
		// Store the sudoku grid
		private int[,] _grid { get; set; }

		private static Random _random = new Random();

		// Initialize the helper with an empty grid
		public SudokuHelper()
		: this(new int[9, 9])
		{ }

		// Initialize the helper with input grid
		public SudokuHelper(int[,] grid)
		{
			this._grid = grid;
		}

		// Will validate the whole grid values. Return true if everything is OK.
		public bool ValidateGridValues()
		{
			// We check if all lines, rows and squares are good.
			return GetLines().All(CheckOneSerie)
				&& GetRows().All(CheckOneSerie)
				&& GetSquares().All(CheckOneSerie);
		}

		// Function which generate a valid completed grid
		public int[,] GenerateValidGrid()
		{

			// This collection will store possible values for every tile.
			var availableValues = new List<int>[9, 9];

			// Initialize all possible values with a list of all number from 1 to 9.
			for (var x = 0; x < 9; x++)
			{
				for (var y = 0; y < 9; y++)
				{
					availableValues[x, y] = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
				}
			}

			// Creation of the index object.
			var position = new Position();

			// We'll keep looping until the index reaches the last tile.
			while (!position.IsAtEnd)
			{
				//Console.WriteLine("Current position : [" + position.x + "," + position.y + "]");
				if (availableValues[position.x, position.y].Count == 0)
				{
					// We have tried every possibilities for this tile without success.
					// We reset the possible values for current tile and we move backward.
					FillList(availableValues[position.x, position.y]);
					position.MovePrev();

					// We'll try another value for previous tile.
					this._grid[position.x, position.y] = 0;
				}
				else
				{
					// We still have possible values for current tile.
					// We pick one randomly in possible values.
					var possibleValue = PopRandomValue(availableValues[position.x, position.y]);
					if (CheckValue(possibleValue, position))
					{
						// The value fits in this tile without breaking the rules.
						// We'll try to move forward with this value.
						this._grid[position.x, position.y] = possibleValue;
						position.MoveNext();
					};
				}
			}

			// The grid has been generated successfully.
			return this._grid;
		}

		#region Technical functions

		// This function will check input value for this position with the current grid.
		private bool CheckValue(int testedValue, Position position)
		{
			return !(GetSelectedLine(position.x).Contains(testedValue)
			 || GetSelectedRow(position.y).Contains(testedValue)
			 || GetSelectedSquare(position.squareX, position.squareY).Contains(testedValue));
		}

		// Helper function which renew values of a list.
		private void FillList(List<int> list)
		{
			for (var i = 1; i <= 9; i++)
			{
				list.Add(i);
			}
		}

		// Will return a random value in input list and remove from it.
		private int PopRandomValue(List<int> list)
		{
			var index = SudokuHelper._random.Next(0, list.Count - 1);
			var result = list.ElementAt(index);
			list.RemoveAt(index);
			return result;
		}

		// Return all the squares (3 x 3) inside the grid.
		private IEnumerable<IEnumerable<int>> GetSquares()
		{
			return Enumerable.Range(0, 3).SelectMany(x =>
			{
				return Enumerable.Range(0, 3).Select(y =>
				{
					return GetSelectedSquare(x, y);
				});
			});
		}

		// Return the selected square thanks to its coordonates.
		// 0,0  0,1  0,2
		// 1,0  1,1  1,2
		// 2,0  2,1  2,2
		private IEnumerable<int> GetSelectedSquare(int x, int y)
		{
			return Enumerable.Range(0, 3).SelectMany(i =>
			{
				return Enumerable.Range(0, 3).Select(j =>
				{
					return this._grid[x * 3 + i, y * 3 + j];
				});
			});
		}

		// Return all lines of the grid.
		private IEnumerable<IEnumerable<int>> GetLines()
		{
			return Enumerable.Range(0, 9).Select(index =>
			{
				return GetSelectedLine(index);
			});
		}

		// Return targeted line in the grid.
		private IEnumerable<int> GetSelectedLine(int lineNumber)
		{
			return Enumerable.Range(0, 9).Select(index =>
			{
				return this._grid[lineNumber, index];
			});
		}

		// Return all rows of the grid.
		private IEnumerable<IEnumerable<int>> GetRows()
		{
			return Enumerable.Range(0, 9).Select(index =>
			{
				return GetSelectedRow(index);
			});
		}

		// Return selected row of the grid.
		private IEnumerable<int> GetSelectedRow(int rowNumber)
		{
			return Enumerable.Range(0, 9).Select(j =>
			{
				return this._grid[j, rowNumber];
			});
		}

		// List of values required in a line/row/square (1, 2, 3, 4, 5, 6, 7, 8, 9).
		private static List<int> AllowedValues = Enumerable.Range(1, 9).ToList();

		// Check if the values match with the sudoku rules.
		private bool CheckOneSerie(IEnumerable<int> values)
		{
			return values != null && values.Count() == 9 && values.Intersect(AllowedValues).Count() == 9;
		}

		#endregion
	}

	// Index object for grid navigation
	class Position
	{

		public int x { get; private set; }

		public int y { get; private set; }

		public int squareX
		{
			get
			{
				decimal result = x / 3;
				return (int)Math.Truncate(result);
			}
		}

		public int squareY
		{
			get
			{
				decimal result = y / 3;
				return (int)Math.Truncate(result);
			}
		}

		private bool _isAtEnd;

		public bool IsAtEnd
		{
			get
			{
				return _isAtEnd;
			}
			private set
			{
				this._isAtEnd = value;
			}
		}

		public Position()
		{
			this.x = 0;
			this.y = 0;
		}

		public Position MoveNext()
		{
			if (y == 8)
			{
				if (x < 8)
				{
					y = 0;
					x++;
				}
				else
				{
					this.IsAtEnd = true;
				}
			}
			else
			{
				y++;
			}

			return this;
		}

		public Position MovePrev()
		{
			if (y == 0)
			{
				if (x > 0)
				{
					y = 8;
					x--;
				}
			}
			else
			{
				y--;
			}

			return this;
		}
	}
}
