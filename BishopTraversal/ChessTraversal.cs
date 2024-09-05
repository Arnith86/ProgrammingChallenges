using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using System.IO;
using System.Runtime.CompilerServices;


namespace ProgrammingChallenges.ChessTraversal
{
	/* 
	    In chess the bishop is the chessman, which can only move diagonal. It is well known that bishops can reach
		only fields of one color but all of them in some number of moves (assuming no other figures are on
		the field). You are given two coordinates on a chessfield and should determine, if a bishop can reach the 
		one field from the other and how. Coordinates in chess are given by a letter ('A' to 'H') and a number (1 to 8).
		The letter specifies the column, the number the row on the chessboard.
	    
	    Input

		The input starts with the number of test cases. Each test case consists of one line, containing the start
		position X and end position Y. Each position is given by two space separated characters. A letter for the 
		column and a number for the row. There are no duplicate test cases in one input.
	    
	    Output

		Output one line for every test case. If it's not possible to move a bishop from X to Y in any number of moves
		output 'Impossible'. Otherwise output one possible move sequence from X to Y. Output the number n of 
		moves first (allowed to be 4 at most). Followed by n +1 positions, which describe the path the bishop
		has to go. Every character is separated by one space. There are many possible solutions. Any with at most 4
		moves will be accepted. Remember that in a chess move one chessman (the bishop in this case) has to
		change his position to be a valid move (i.e. two consecutive positions in the output must differ).
	  
	 	Expected output:
	    Impossible
	    2 F1 B 5 E 8
	    0 A 3

		Sample input: 
	    3
	    E 2 E 3
	    F 1 E 8
	    A 3 A 3

		first input: 
	    n : number of test cases
	   
	    Each row after: X:1 X:2 Y:1 Y:2 
	    X: start position
	    Y: end position 
	 */

	// Used to convert column values between numbers and letters
	enum ChessToNum
	{
		A = 1,
		B = 2,
		C = 3,
		D = 4,
		E = 5,
		F = 6,
		G = 7,
		H = 8
	}

	//class Program
	//{
	//	static void Main()
	//	{
	//		ChessTraversal chessTraversal = new ChessTraversal();
	//		chessTraversal.collectData();
	//	}
	//}

	class ChessTraversal
	{
		const int bordWidth = 8;
		const int bordHeight = 8;

		// Used to end the recursion early
		bool found = false;

		// Keeps track of the cells color 
		// True = white, false = black
		bool[,] blackWhiteBord = new bool[8, 8];

		string input;
		bool nrOfCasesRecorded = false;

		long nrOfCases = 0;
		long currentCase = 0;

		Dictionary<long, Position[]> caseInfo = new Dictionary<long, Position[]>();

		public ChessTraversal()
		{
			// True = white, false = black
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					if ((i + j) % 2 == 0)
					{
						blackWhiteBord[i, j] = false;
					}
					else
					{
						blackWhiteBord[i, j] = true;
					}
				}
			}
			
			collectData();
		}

		public void collectData()
		{
			while ((input = Console.ReadLine()) != null)
			{
				string[] inputToArray = this.input.Split(' ');

				// Registers the number of cases
				if (!this.nrOfCasesRecorded)
				{
					this.nrOfCases = Int64.Parse(inputToArray[0]);
					nrOfCasesRecorded = true;
				}
				// Registers start and end position for each case.
				else
				{
					currentCase++;

					if (!caseInfo.ContainsKey(currentCase))
					{
						caseInfo.Add(currentCase, new Position[2]);
					}

					// Start Position 
					// This will parse the letter to its enum value
					string EnumX = inputToArray[0];
					ChessToNum xNum = (ChessToNum)Enum.Parse(typeof(ChessToNum), EnumX);

					// Converts the enum value to an int
					int x = (int)xNum;
					int y = int.Parse(inputToArray[1]);

					caseInfo[currentCase][0] = new Position(x, y);

					// End position 
					EnumX = inputToArray[2];
					xNum = (ChessToNum)Enum.Parse(typeof(ChessToNum), EnumX);

					x = (int)xNum;
					y = int.Parse(inputToArray[3]);

					caseInfo[currentCase][1] = new Position(x, y);

					if (currentCase == nrOfCases)
					{
						break;
					}
				}
			}

			foreach (KeyValuePair<long, Position[]> caseData in caseInfo)
			{
				string sequencePrint = "";
				string startColumnLetter = "";
				string intermediateColumnLetter = "";
				string endColumnLetter = "";

				Dictionary<int, Position> moveSequence = new Dictionary<int, Position>();

				int xStart = caseData.Value[0].X;
				int yStart = caseData.Value[0].Y;
				int xEnd = caseData.Value[1].X;
				int yEnd = caseData.Value[1].Y;

				// The positions are of different colors, impossible ro reach
				if (this.blackWhiteBord[(xStart - 1), (yStart - 1)] != this.blackWhiteBord[(xEnd - 1), (yEnd - 1)])
				{
					Console.WriteLine("Impossible");
					continue;
				}

				// Bishop starts at end possition, no move needed.
				if (xStart == xEnd && yStart == yEnd)
				{
					startColumnLetter = ((ChessToNum)xStart).ToString();
					sequencePrint = $"0 {startColumnLetter} {yStart}";
					Console.WriteLine(sequencePrint);
					continue;
				}

				// If the absolut difference of the Start and end position are the same.
				// They exist on the same diagonal line. Only a single move is needed.  
				if (Math.Abs(xStart - xEnd) == Math.Abs(yStart - yEnd))
				{
					startColumnLetter = ((ChessToNum)xStart).ToString();
					endColumnLetter = ((ChessToNum)xEnd).ToString();
					sequencePrint = $"1 {startColumnLetter} {yStart} {endColumnLetter} {yEnd}";
					Console.WriteLine(sequencePrint);
					continue;
				}

				int[,] direction = new int[,]
				{
					{ -1,  1 },  // North west
					{ -1, -1 },  // South west
					{  1, -1 },	 // South east	
					{  1,  1 }   // North east 
				};

				Position intermediatePosition = chessDFS(direction, xStart, yStart, xEnd, yEnd, 0, 0, true);

				startColumnLetter = ((ChessToNum)xStart).ToString();
				endColumnLetter = ((ChessToNum)xEnd).ToString();
				intermediateColumnLetter = ((ChessToNum)intermediatePosition.X).ToString();
				sequencePrint = $"2 {startColumnLetter} {yStart} {intermediateColumnLetter} {intermediatePosition.Y} {endColumnLetter} {yEnd}";
				Console.WriteLine(sequencePrint);

				this.found = false;
			}
		}

		// Search for the intermediate Position
		// First recursion goes in all diagonal directions
		// Subsequent recursions continues in the started direction
		private Position chessDFS(int[,] direction, int x, int y, int endX, int endY,
								  int xNextDirection = -2, int yNextDirection = -2, bool firstRecursion = false)
		{

			Position position = null;

			// Stops the recursion if found
			if (this.found)
			{
				return null;
			}

			// End position is diagonal to current position
			// Return current position
			if (Math.Abs(x - endX) == Math.Abs(y - endY))
			{
				this.found = true;
				return position = new Position(x, y);
			}

			// Recursion in all four diagonal directions
			if (firstRecursion)
			{
				for (int i = 0; i < 4; i++)
				{
					if (this.found)
					{
						break;
					}

					// Expected next move 
					int nextStepX = x + direction[i, 0];
					int nextStepY = y + direction[i, 1];

					// Prevents movements outside of chess borad
					if (nextStepX == 0 || nextStepY == 0 || nextStepX == bordHeight + 1 || nextStepY == bordHeight + 1)
					{
						continue;
					}

					// Continue recursion
					position = chessDFS(direction, nextStepX, nextStepY, endX, endY, direction[i, 0], direction[i, 1]);
				}
			}
			else
			{
				// Only moves in a single direction
				int nextStepX = x + xNextDirection;
				int nextStepY = y + yNextDirection;

				// Prevents movements outside of chess borad
				if (nextStepX == 0 || nextStepY == 0 || nextStepX == bordHeight + 1 || nextStepY == bordHeight + 1)
				{
					return null;
				}

				// Continue recursion
				position = chessDFS(direction, nextStepX, nextStepY, endX, endY, xNextDirection, yNextDirection);
			}

			// Returns position if found
			if (position != null)
			{
				return position;
			}

			return null;
		}
	}


	class Position
	{
		public int X { get; set; }
		public int Y { get; set; }

		public Position(int x, int y)
		{
			X = x;
			Y = y;
		}
	}


}
