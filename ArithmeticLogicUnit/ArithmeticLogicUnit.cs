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

	Dictionary<int, ModelNr> MONAD;

	public ArithmeticLogicUnit() 
	{
		MONAD = new Dictionary<int, ModelNr>();
		alu = new ALU(MONAD, readInput(), NR_OF_ROWS);
		
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
	Dictionary<int, ModelNr> MONAD;
	string[] allInput; 
	int NR_OF_ROWS;

	public ALU(Dictionary<int, ModelNr> monad, string[] allInput, int nrOfRows)
	{
		MONAD = monad;
		this.allInput = allInput;
		NR_OF_ROWS = nrOfRows;

		// Here ttemporeraly for test 
		int modelNr = 0; 

		int index = 0;

		MONAD.Add(index, new ModelNr(/*command*/));

		for (int i = 0; i < NR_OF_ROWS; i++)
		{
			string[] enteredCommand = allInput[i].Split(' ');

			string command = enteredCommand[0];
			string variableNameA = enteredCommand[1];
			int a;
			int b;

			if (command.Equals("inp"))
			{
				// What happens if provided variable already exists?????
				//MONAD.Add(index, new ModelNr(/*command*/));
				if (MONAD[index].Variables.ContainsKey(variableNameA)) MONAD[index].Variables[variableNameA] = whichVariableGet(index, enteredCommand[1]);
				else setValue(index, enteredCommand[1], MONAD[index].Variables[variableNameA]);
				//int a = whichVariableGet(index, enteredCommand[1]);
				continue;
			}
			else
			{
				// Keep track of variable name and value
				a = whichVariableGet(index, enteredCommand[1]);

				if (enteredCommand[2].Any(Char.IsNumber)) b = int.Parse(enteredCommand[2]);
				else b = whichVariableGet(index, enteredCommand[2]);

				a = logic(index, command, a, b);

				MONAD[index].Variables[variableNameA] = a;
			}

			MONAD[index].buildNr();
			if (MONAD[index].Variables.Count() == 14)
			{
				Console.WriteLine("its 14");
			}
		}
	}

	public int logic(int index, string command, int a, int b)
	{
		// handle devison by 0, and modulo by 0 
		switch (command)
		{
			case "add":
				a += b; break;
			case "mul":
				a *= b; break;
			case "div":
				a /= b;  break;
			case "mod":
				a %= b;  break;
			case "eql":
				a = a == b ? 1 : 0; break;
			default:
				break;
		}
		return a;
	}

	private int whichVariableGet(int index, string variable)
	{
		return MONAD[index].Variables[variable];
	}

	//private void initiateVariable(int index, string variable, int value)
	//{
	//	MONAD[index].Variables.Add(variable, value);
	//}

	private void setValue(int index, string variable, int value)
	{
		MONAD[index].Variables[variable] = value;
	}
}

public class ModelNr
{
	public string Nr { get; set; }
	
	public Dictionary<string, int> Variables { get; set; }
	
	public ModelNr()
	{
		Variables = new Dictionary<string, int> 
		{
			{"w", 0},
			{"x", 0},
			{"y", 0},
			{"z", 0}
		};
	}

	public void buildNr()
	{
		Nr = $"{Variables["w"]}{Variables["x"]}{Variables["y"]}{Variables["z"]}";
	}
}

//switch (variable)
//{
//	case "w":
//		MONAD[index].Variables.Add(variable, value); break; // MONAD[index].W = value; break;
//	case "x":
//		MONAD[index].X = value; break;
//	case "y":
//		MONAD[index].Y = value; break;
//	case "z":
//		MONAD[index].Z = value break;
//	default:
//		break;
//}


//public int Add(int a, int b)
//{
//	return a += b;
//}

//public int Mul(int a, int b)
//{
//	return a = a * b;
//}

//public int Div(int a, int b)
//{
//	return a = a / b;
//}

//public int Mod(int a, int b)
//{
//	return a = a % b;
//}

//public int eql(int a, int b)
//{
//	return a == b ? 1 : 0;
//}