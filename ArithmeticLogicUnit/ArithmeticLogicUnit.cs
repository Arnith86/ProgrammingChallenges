using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingChallenges.ArithmeticLogicUnit;

public class ArithmeticLogicUnit
{
	int NR_OF_ROWS = 0;
	ALU alu;

	public ArithmeticLogicUnit() 
	{
		alu = new ALU(/*MONAD,*/ readInput(), NR_OF_ROWS);
	}

	private string[] readInput()
	{
		NR_OF_ROWS = File.ReadLines("ArithmeticLogicUnitInput.txt").Count();
		//string[] readAll = File.ReadAllLines("MiniInput.txt");
		//NR_OF_ROWS = File.ReadLines("MiniInput.txt").Count();

		return File.ReadAllLines("ArithmeticLogicUnitInput.txt"); //File.ReadAllLines("MiniInput.txt");
	}
}

public class ALU
{
	string[] allInput;
	int NR_OF_ROWS;

	public ALU(string[] allInput, int nrOfRows)
	{
		int[] modelNr = new int[14];

		this.allInput = allInput;
		NR_OF_ROWS = nrOfRows;

		Dictionary<string, int> variables = new Dictionary<string, int>
		{
			{"w", 0},
			{"x", 0},
			{"y", 0},
			{"z", 0}
		};


		int index = 0;
		for (long currentModelNr = 99999999999999; currentModelNr > 0; currentModelNr--)
		{
			Console.WriteLine(currentModelNr);

			// Convert the long to a string, then split it into individual digits, and convert back to int array
			modelNr = currentModelNr.ToString().Select(digit => int.Parse(digit.ToString())).ToArray();
			index = 0;
			bool containsZero = false;

			int currentNr = modelNr[index];

			for (int k = 0; k < NR_OF_ROWS; k++)
			{
				string[] enteredCommand = allInput[k].Split(' ');

				string command = enteredCommand[0];
				string variableNameA = enteredCommand[1];
				int a;
				int b;

				// Handles the input command  
				if (command.Equals("inp"))
				{
					if (modelNr[index] == 0)
					{
						containsZero = true;
						break;
					}
					else
					{
						variables[variableNameA] = modelNr[index];
						index++;
						continue;
					}
				}
				else
				{
					// Keep track of variable name and value
					a = variables[enteredCommand[1]];

					// Uses the number value provided or collects saved value of variable
					if (enteredCommand[2].Any(Char.IsNumber)) b = int.Parse(enteredCommand[2]);
					else b = variables[enteredCommand[2]];

					// Perform logic operation
					a = logic(command, a, b);

					// Save value in variable
					variables[variableNameA] = a;
				}
			}

			// Valid modelNr check 
			if (variables["z"] == 0 && !containsZero)
			{
				break;
			}
			else if (containsZero)
			{
				continue;
			}
		}

		// WILL "z" naturally contain 0?
		if (variables["z"] == 0)
		{
			for (int i = 0; i < 14; i++)
			{
				Console.WriteLine($"{modelNr[0]}");
			}
		}
	}

	public int logic(string command, int a, int b)
	{
		// handle devison by 0, and modulo by 0 
		switch (command)
		{
			case "add":
				a += b; break;
			case "mul":
				a *= b; break;
			case "div":
				a /= b; break;
			case "mod":
				a %= b; break;
			case "eql":
				a = a == b ? 1 : 0; break;
			default:
				break;
		}
		return a;
	}
}