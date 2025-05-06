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
	
		//for (int i = 0; i < 257567848; i++)
		//{
		//	alu = new ALU(readInput(), NR_OF_ROWS, i);
		//}
		alu = new ALU(/*MONAD,*/ readInput(), NR_OF_ROWS);
	}

	private string[] readInput()
	{
		NR_OF_ROWS = File.ReadLines("ArithmeticLogicUnitInput.txt").Count();
		//string[] readAll = File.ReadAllLines("MiniInput.txt");
		//NR_OF_ROWS = File.ReadLines("MiniInput.txt").Count();

		return File.ReadAllLines("ArithmeticLogicUnitInput.txt"); /*File.ReadAllLines("MiniInput.txt")*/;
	}
}







public class ALU
{

	string[] allInput;
	int NR_OF_ROWS;

	public ALU(string[] allInput, int nrOfRows, int z = 0)
	{
		int[] modelNr = new int[14];
		int[] prevModelNr = new int[14] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

		this.allInput = allInput;
		NR_OF_ROWS = nrOfRows;

		int[] howMany26DivisionsLeft = new int[14] { 7, 7, 7, 7, 6, 6, 5, 5, 4, 3, 2, 1, 1, 0 };

		Dictionary<string, int> variables = new Dictionary<string, int>
		{
			{"w", 0},
			{"x", 0},
			{"y", 0},
			{"z", 0}
		};


		// 0 = w, 1 = x, 2 = y, 3 = z
		Dictionary<string, int>[] dynamicPVariables = new Dictionary<string, int>[14];

		for (int i = 0; i < 14; i++)
		{
			dynamicPVariables[i] = new Dictionary<string, int> {
				{"w", 0},
				{"x", 0},
				{"y", 0},
				{"z", 0}
			};
		}

		// remove after analasys of secret is complete
		//variables["z"] = z;

		int index = 0;

		for (long currentModelNr = 28392663863654; currentModelNr < 100000000000000; currentModelNr++)
		{
			bool toBeSkiped = false;
			bool[] setNumbers = new bool[14];

			// Convert the long to a string, then split it into individual digits, and convert back to int array
			modelNr = currentModelNr.ToString().Select(digit => int.Parse(digit.ToString())).ToArray();

			// Checks both if this number is to be skipped, and how far along the number (starting from the left) is the same as the previus number
			for (int i = 0; i < 14; i++)
			{
				if (modelNr[i] == 0) toBeSkiped = true;
				if (modelNr[i] == prevModelNr[i]) setNumbers[i] = true;
			}
			// Continue dynamic programin solution from here // we now know which index to get information from 
			if (toBeSkiped) continue;

			prevModelNr = modelNr;
			index = 0;
			bool containsZero = false;

			int currentNr = modelNr[index];
			int zStateOnInput = 0;


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
						zStateOnInput = variables["z"];
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

			//Console.WriteLine($"w: {variables["w"]} x: {variables["x"]} y: {variables["y"]}  z in: {zStateOnInput}  z out: {variables["z"]}");
			//string concatZ = $", {variables["z"]}";
			//zValues = string.Concat(zValues, concatZ);

			// Valid modelNr check 
			if (variables["z"] == 0)
			{
				Console.WriteLine($"{currentModelNr}");
			}


			int logic(string command, int a, int b)
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
	}
}




		//Console.WriteLine(zValues);
		//int index = 0;
		////for (long currentModelNr = 11111111111111/*99999999999999*//*99999895700000*/; currentModelNr > 0; currentModelNr--)
		//for (long currentModelNr = 1; currentModelNr < 10; currentModelNr++)
		//{
		//	Console.WriteLine(currentModelNr);

		//	// Convert the long to a string, then split it into individual digits, and convert back to int array
		//	modelNr = currentModelNr.ToString().Select(digit => int.Parse(digit.ToString())).ToArray();
		//	index = 0;
		//	bool containsZero = false;

		//	int currentNr = modelNr[index];

		//	for (int k = 0; k < NR_OF_ROWS; k++)
		//	{
		//		string[] enteredCommand = allInput[k].Split(' ');

		//		string command = enteredCommand[0];
		//		string variableNameA = enteredCommand[1];
		//		int a;
		//		int b;

		//		// Handles the input command  
		//		if (command.Equals("inp"))
		//		{
		//			if (modelNr[index] == 0)
		//			{
		//				containsZero = true;
		//				break;
		//			}
		//			else
		//			{
		//				variables[variableNameA] = modelNr[index];
		//				index++;
		//				continue;
		//			}
		//		}
		//		else
		//		{
		//			// Keep track of variable name and value
		//			a = variables[enteredCommand[1]];

		//			// Uses the number value provided or collects saved value of variable
		//			if (enteredCommand[2].Any(Char.IsNumber)) b = int.Parse(enteredCommand[2]);
		//			else b = variables[enteredCommand[2]];

		//			// Perform logic operation
		//			a = logic(command, a, b);

		//			// Save value in variable
		//			variables[variableNameA] = a;
		//		}
		//	}

		//	// Valid modelNr check 
		//	if (variables["z"] == 0 /*&& !containsZero*/)
		//	{
		//		break;
		//	}
		//	else if (containsZero)
		//	{
		//		continue;
		//	}
		//}

		//// WILL "z" naturally contain 0?
		//if (variables["z"] == 0)
		//{
		//	for (int i = 0; i < 14; i++)
		//	{
		//		Console.WriteLine($"{modelNr[0]}");
		//	}
		//}

		////////////string[] allInput;
		////////////int NR_OF_ROWS;

		////////////public ALU(string[] allInput, int nrOfRows, int z = 0)
		////////////{

		////////////	//int[] modelNr = new int[14];
		////////////	//int[] prevModelNr = new int[14] {0,0,0,0,0,0,0,0,0,0,0,0,0,0};

		////////////	//this.allInput = allInput;
		////////////	//NR_OF_ROWS = nrOfRows;

		////////////	int[] howMany26DivisionsLeft = new int[14] {7, 7, 7, 7, 6, 6, 5, 5, 4, 3, 2, 1, 1, 0};

		////////////	//Dictionary<string, int> variables = new Dictionary<string, int>
		////////////	//{
		////////////	//	{"w", 0},
		////////////	//	{"x", 0},
		////////////	//	{"y", 0},
		////////////	//	{"z", 0}
		////////////	//};


		////////////	//// 0 = w, 1 = x, 2 = y, 3 = z
		////////////	//Dictionary<string, int>[] dynamicPVariables = new Dictionary<string, int>[14];

		////////////	//for (int i = 0; i < 14; i++)
		////////////	//{
		////////////	//	dynamicPVariables[i] = new Dictionary<string, int> {
		////////////	//		{"w", 0},
		////////////	//		{"x", 0},
		////////////	//		{"y", 0},
		////////////	//		{"z", 0}
		////////////	//	};
		////////////	//}

		////////////	// FIGURE OUT HOW TO MAKETHIS WITH Dynamic programming !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

		////////////	HashSet<int> validNrs = new HashSet<int>();
		////////////	int[] zValues = new int[9999999];
		////////////	Array.Fill(zValues, -1);
		////////////	string completeNumber = "";

		////////////	numberVerifierRecursion(completeNumber, 0, howMany26DivisionsLeft, zValues, validNrs);

		////////////}
		////////////private void numberVerifierRecursion(string number, int z, int[] howManyDivisionsLeft, int[] zValues, HashSet<int> validNrs, int index = 0)
		////////////{
		////////////	int currentZ = z;
		////////////	string numberString = number;
		////////////	//string sevenFirstNumbers = number.Substring(0, 7);

		////////////	if (index > 7)
		////////////	{
		////////////		numberString = numberString.Substring(0, 7);
		////////////	}
		////////////	else
		////////////	{
		////////////		int howManyZerosToFill = 7 - numberString.Length;

		////////////		for (int i = 0; i < howManyZerosToFill; i++)
		////////////		{
		////////////			numberString = String.Concat(numberString, "0");
		////////////		}
		////////////	}

		////////////	int memoryNr = int.Parse(numberString);

		////////////	if (zValues[memoryNr] != -1) { }



		////////////	for (int i = 1; i < 10; i++)
		////////////	{
		////////////		if (index == 0 || index == 1 || index == 2 || index == 3 || index == 5 || index == 7 || index == 12)
		////////////		{
		////////////			currentZ = mulZ(currentZ, i, index);
		////////////		}
		////////////		else
		////////////		{
		////////////			currentZ = devZ(currentZ, i, index);
		////////////		}
		////////////		//Console.WriteLine(((int)(currentZ / Math.Pow(26, howManyDivisionsLeft[index]))));

		////////////		if (((int)(currentZ / Math.Pow(26, howManyDivisionsLeft[index]))) == 0)
		////////////		{
		////////////			numberVerifierRecursion(numberString+i, currentZ, howManyDivisionsLeft, validNrs, index + 1); Console.WriteLine(numberString);
		////////////		}

		////////////		if (index == 13 && currentZ == 0) 
		////////////		{ 
		////////////			validNrs.Add(int.Parse(numberString)); 
		////////////			Console.WriteLine(numberString); 
		////////////		}
		////////////	}
		////////////}

		////////////private int mulZ(int z, int nr, int index = 0)
		////////////{
		////////////	int[] addW = new int[14] {12, 8, 7, 4, 0, 1, 0, 8, 0, 0, 0, 0, 10, 0 };

		////////////	int tempZ = z;
		////////////	int tempW = nr;

		////////////	return tempZ += (addW[index] + nr );	

		////////////}


		////////////private int devZ(int z, int nr, int index = 0)
		////////////{
		////////////	int[] addX = new int[14] { 0, 0, 0, 0, -11, 0, -1, 0, -3, -4, -13, -8, 0, -11 };
		////////////	int[] addW = new int[14] { 0, 0, 0, 0,   4, 0, 10, 0, 12, 10,  15,  4, 0,   9 };

		////////////	int tempZ = z;
		////////////	int tempW = nr;
		////////////	int tempX = tempZ;

		////////////	tempX %= 26;
		////////////	tempZ /= 26;
		////////////	tempX += addX[index];
		////////////	tempX = logic("eql", tempX, tempW);
		////////////	tempX = logic("eql", tempX, 0);
		////////////	tempZ *= ((25 * tempX) + 1);

		////////////	return tempZ += ((tempW + addW[index]) * tempX);
		////////////}

		//////////////private void resetWYX(Dictionary<string, int> variables)
		//////////////{
		//////////////	variables["w"] = 0;
		//////////////	variables["x"] = 0;
		//////////////	variables["y"] = 0;
		//////////////}



	////////////	public int logic(string command, int a, int b)
	////////////{
	////////////	// handle devison by 0, and modulo by 0 
	////////////	switch (command)
	////////////	{
	////////////		case "add":
	////////////			a += b; break;
	////////////		case "mul":
	////////////			a *= b; break;
	////////////		case "div":
	////////////			a /= b; break;
	////////////		case "mod":
	////////////			a %= b; break;
	////////////		case "eql":
	////////////			a = a == b ? 1 : 0; break;
	////////////		default:
	////////////			break;
	////////////	}
	////////////	return a;
	////////////}

	//public class ALU
	//{
	//	string[] allInput;
	//	int NR_OF_ROWS;

	//public ALU(string[] allInput, int nrOfRows, int z = 0)
	//{
	//	int[] modelNr = new int[14];

	//	this.allInput = allInput;
	//	NR_OF_ROWS = nrOfRows;

	//	Dictionary<string, int> variables = new Dictionary<string, int>
	//	{
	//		{"w", 0},
	//		{"x", 0},
	//		{"y", 0},
	//		{"z", 0}
	//	};

	//	// remove after analasys of secret is complete
	//	bool toBeSkiped = false;

	//	int index = 0;

	//	//int w = 0;
	//	//int x = 0;
	//	//int y = 0;
	//	//int z = 0;

	//	// 0 = w, 1 = x, 2 = y, 3 = z
	//	Dictionary<string, int>[] dynamicPVariables = new Dictionary<string, int>[14];

	//	for (int i = 0; i < 14; i++)
	//	{
	//		dynamicPVariables[i] = new Dictionary<string, int> {
	//			{"w", 0},
	//			{"x", 0},
	//			{"y", 0},
	//			{"z", 0}
	//		}; 
	//	}


	//	for (int zero = 1; zero < 10; zero++)
	//	{
	//		dynamicPVariables[0]["w"];
	//		// nr 1
	//		variables["w"] = zero;
	//		variables["z"] += (variables["w"] + 12);

	//		for (int one = 1; one < 10; one++)
	//		{
	//			// nr 2 
	//			variables["w"] = modelNr[1];
	//			variables["z"] *= 26;
	//			variables["z"] += (variables["w"] + 8);

	//			for (int two = 1; two < 10; two++)
	//			{
	//				// nr 3
	//				variables["w"] = modelNr[2];
	//				variables["z"] *= 26;
	//				variables["z"] += (variables["w"] + 7);


	//				for (int three = 1; three < 10; three++)
	//				{
	//					// nr 4
	//					variables["w"] = modelNr[3];
	//					variables["z"] *= 26;
	//					variables["z"] += (variables["w"] + 4);

	//					for (int four = 1; four < 10; four++)
	//					{
	//						// nr 5
	//						variables["w"] = modelNr[4];
	//						variables["x"] = variables["z"];
	//						variables["x"] %= 26;
	//						variables["x"] /= 26;
	//						variables["x"] += -11;
	//						variables["x"] = logic("eql", variables["x"], variables["w"]);
	//						variables["x"] = logic("eql", variables["x"], 0);
	//						variables["z"] *= ((25 * variables["x"]) + 1);
	//						variables["z"] += ((variables["w"] + 4) * variables["x"]);

	//						for (int five = 1; five < 10; five++)
	//						{
	//							// nr 6
	//							variables["w"] = modelNr[5];
	//							variables["z"] *= 26;
	//							variables["z"] += (variables["w"] + 1);

	//							for (int six = 1; six < 10; six++)
	//							{
	//								// nr 7
	//								variables["w"] = modelNr[6];
	//								variables["x"] = variables["z"];
	//								variables["x"] %= 26;
	//								variables["x"] /= 26;
	//								variables["x"] += -1;
	//								variables["x"] = logic("eql", variables["x"], variables["w"]);
	//								variables["x"] = logic("eql", variables["x"], 0);
	//								variables["z"] *= ((25 * variables["x"]) + 1);
	//								variables["z"] += ((variables["w"] + 10) * variables["x"]);

	//								for (int seven = 1; seven < 10; seven++)
	//								{
	//									// nr 8
	//									variables["w"] = modelNr[7];
	//									variables["z"] *= 26;
	//									variables["z"] += (variables["w"] + 8);

	//									for (int eight = 1; eight < 10; eight++)
	//									{
	//										// nr 9
	//										variables["w"] = modelNr[8];
	//										variables["x"] = variables["z"];
	//										variables["x"] %= 26;
	//										variables["x"] /= 26;
	//										variables["x"] += -3;
	//										variables["x"] = logic("eql", variables["x"], variables["w"]);
	//										variables["x"] = logic("eql", variables["x"], 0);
	//										variables["z"] *= ((25 * variables["x"]) + 1);
	//										variables["z"] += ((variables["w"] + 12) * variables["x"]);

	//										for (int nine = 1; nine < 10; nine++)
	//										{
	//											// nr 10
	//											variables["w"] = modelNr[9];
	//											variables["x"] = variables["z"];
	//											variables["x"] %= 26;
	//											variables["x"] /= 26;
	//											variables["x"] += -4;
	//											variables["x"] = logic("eql", variables["x"], variables["w"]);
	//											variables["x"] = logic("eql", variables["x"], 0);
	//											variables["z"] *= ((25 * variables["x"]) + 1);
	//											variables["z"] += ((variables["w"] + 10) * variables["x"]);

	//											for (int ten = 1; ten < 10; ten++)
	//											{
	//												// nr 11
	//												variables["w"] = modelNr[10];
	//												variables["x"] = variables["z"];
	//												variables["x"] %= 26;
	//												variables["x"] /= 26;
	//												variables["x"] += -13;
	//												variables["x"] = logic("eql", variables["x"], variables["w"]);
	//												variables["x"] = logic("eql", variables["x"], 0);
	//												variables["z"] *= ((25 * variables["x"]) + 1);
	//												variables["z"] += ((variables["w"] + 15) * variables["x"]);

	//												for (int eleven = 1; eleven < 10; eleven++)
	//												{
	//													// nr 12
	//													variables["w"] = modelNr[11];
	//													variables["x"] = variables["z"];
	//													variables["x"] %= 26;
	//													variables["x"] /= 26;
	//													variables["x"] += -8;
	//													variables["x"] = logic("eql", variables["x"], variables["w"]);
	//													variables["x"] = logic("eql", variables["x"], 0);
	//													variables["z"] *= ((25 * variables["x"]) + 1);
	//													variables["z"] += ((variables["w"] + 4) * variables["x"]);

	//													for (int twelve = 1; twelve < 10; twelve++)
	//													{
	//														// nr 13
	//														variables["w"] = modelNr[12];
	//														variables["z"] *= 26;
	//														variables["z"] += (variables["w"] + 10);

	//														for (int thirteen = 1; thirteen < 10; thirteen++)
	//														{
	//															// nr 14
	//															variables["w"] = modelNr[13];
	//															variables["x"] = variables["z"];
	//															variables["x"] %= 26;
	//															variables["x"] /= 26;
	//															variables["x"] += -11;
	//															variables["x"] = logic("eql", variables["x"], variables["w"]);
	//															variables["x"] = logic("eql", variables["x"], 0);
	//															variables["z"] *= ((25 * variables["x"]) + 1);
	//															variables["z"] += ((variables["w"] + 4) * variables["x"]);

	//															// Valid modelNr check 
	//															if (z == 0)
	//															{
	//																Console.WriteLine($"{currentModelNr}");
	//															}
	//														}
	//													}
	//												}
	//											}
	//										}
	//									}
	//								}
	//							}
	//						}
	//					}
	//				}
	//			}

	//		}
	//	}
	//}

	//public class ALU
	//{
	//	string[] allInput;
	//	int NR_OF_ROWS;

	//	public ALU(string[] allInput, int nrOfRows, int z = 0)
	//	{
	//		int[] modelNr = new int[14];
	//		int[] prevModelNr = new int[14] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

	//		this.allInput = allInput;
	//		NR_OF_ROWS = nrOfRows;

	//		int[] howMany26DivisionsLeft = new int[14] { 7, 7, 7, 7, 6, 6, 5, 5, 4, 3, 2, 1, 1, 0 };

	//		Dictionary<string, int> variables = new Dictionary<string, int>
	//	{
	//		{"w", 0},
	//		{"x", 0},
	//		{"y", 0},
	//		{"z", 0}
	//	};


	//		// 0 = w, 1 = x, 2 = y, 3 = z
	//		Dictionary<string, int>[] dynamicPVariables = new Dictionary<string, int>[14];

	//		for (int i = 0; i < 14; i++)
	//		{
	//			dynamicPVariables[i] = new Dictionary<string, int> {
	//			{"w", 0},
	//			{"x", 0},
	//			{"y", 0},
	//			{"z", 0}
	//		};
	//		}

	//		// remove after analasys of secret is complete
	//		//variables["z"] = z;

	//		//int index = 0;

	//		for (long currentModelNr = /*11111111111111*//*99999999999999*/11184156282655; currentModelNr < 100000000000000; currentModelNr++)
	//		{
	//			bool toBeSkiped = false;
	//			bool[] setNumbers = new bool[14];

	//			// Convert the long to a string, then split it into individual digits, and convert back to int array
	//			modelNr = currentModelNr.ToString().Select(digit => int.Parse(digit.ToString())).ToArray();

	//			// Checks both if this number is to be skipped, and how far along the number (starting from the left) is the same as the previus number
	//			for (int i = 0; i < 14; i++)
	//			{
	//				if (modelNr[i] == 0) toBeSkiped = true;
	//				if (modelNr[i] == prevModelNr[i]) setNumbers[i] = true;
	//			}
	//			// Continue dynamic programin solution from here // we now know which index to get information from 
	//			if (toBeSkiped) continue;

	//			prevModelNr = modelNr;
	//			//index = 0;
	//			//bool containsZero = false;

	//			//int currentNr = modelNr[index];
	//			//int zStateOnInput = 0;

	//			for (int i = 0; i < 14; i++)
	//			{
	//				if (setNumbers[i] == false)
	//				{
	//					int prevZ = 0;
	//					if (!((i - 1) < 0)) prevZ = i - 1;


	//					switch (i)
	//					{
	//						case 0: setNumber0(dynamicPVariables[i], modelNr[i], dynamicPVariables[prevZ]["z"]); break;
	//						case 1: setNumber1(dynamicPVariables[i], modelNr[i], dynamicPVariables[prevZ]["z"]); break;
	//						case 2: setNumber2(dynamicPVariables[i], modelNr[i], dynamicPVariables[prevZ]["z"]); break;
	//						case 3: setNumber3(dynamicPVariables[i], modelNr[i], dynamicPVariables[prevZ]["z"]); break;
	//						case 4: setNumber4(dynamicPVariables[i], modelNr[i], dynamicPVariables[prevZ]["z"]); break;
	//						case 5: setNumber5(dynamicPVariables[i], modelNr[i], dynamicPVariables[prevZ]["z"]); break;
	//						case 6: setNumber6(dynamicPVariables[i], modelNr[i], dynamicPVariables[prevZ]["z"]); break;
	//						case 7: setNumber7(dynamicPVariables[i], modelNr[i], dynamicPVariables[prevZ]["z"]); break;
	//						case 8: setNumber8(dynamicPVariables[i], modelNr[i], dynamicPVariables[prevZ]["z"]); break;
	//						case 9: setNumber9(dynamicPVariables[i], modelNr[i], dynamicPVariables[prevZ]["z"]); break;
	//						case 10: setNumber10(dynamicPVariables[i], modelNr[i], dynamicPVariables[prevZ]["z"]); break;
	//						case 11: setNumber11(dynamicPVariables[i], modelNr[i], dynamicPVariables[prevZ]["z"]); break;
	//						case 12: setNumber12(dynamicPVariables[i], modelNr[i], dynamicPVariables[prevZ]["z"]); break;
	//						case 13: setNumber13(dynamicPVariables[i], modelNr[i], dynamicPVariables[prevZ]["z"]); break;
	//						default: break;
	//					}

	//					variables["w"] = dynamicPVariables[i]["w"];
	//					variables["x"] = dynamicPVariables[i]["x"];
	//					variables["y"] = dynamicPVariables[i]["y"];
	//					variables["z"] = dynamicPVariables[i]["z"];
	//				}
	//			}

	//			//for (int k = 0; k < NR_OF_ROWS; k++)
	//			//{
	//			//	string[] enteredCommand = allInput[k].Split(' ');

	//			//	string command = enteredCommand[0];
	//			//	string variableNameA = enteredCommand[1];
	//			//	int a;
	//			//	int b;

	//			//	// Handles the input command  
	//			//	if (command.Equals("inp"))
	//			//	{
	//			//		if (modelNr[index] == 0)
	//			//		{
	//			//			containsZero = true;
	//			//			zStateOnInput = variables["z"];
	//			//			break;
	//			//		}
	//			//		else
	//			//		{
	//			//			variables[variableNameA] = modelNr[index];
	//			//			index++;
	//			//			continue;
	//			//		}
	//			//	}
	//			//	else
	//			//	{
	//			//		// Keep track of variable name and value
	//			//		a = variables[enteredCommand[1]];

	//			//		// Uses the number value provided or collects saved value of variable
	//			//		if (enteredCommand[2].Any(Char.IsNumber)) b = int.Parse(enteredCommand[2]);
	//			//		else b = variables[enteredCommand[2]];

	//			//		// Perform logic operation
	//			//		a = logic(command, a, b);

	//			//		// Save value in variable
	//			//		variables[variableNameA] = a;
	//			//	}
	//			//}

	//			//Console.WriteLine($"w: {variables["w"]} x: {variables["x"]} y: {variables["y"]}  z in: {zStateOnInput}  z out: {variables["z"]}");
	//			//string concatZ = $", {variables["z"]}";
	//			//zValues = string.Concat(zValues, concatZ);

	//			// Valid modelNr check 
	//			if (variables["z"] == 0)
	//			{
	//				Console.WriteLine($"{currentModelNr}");
	//			}
	//			// Valid modelNr check 
	//			//if (variables["z"] == 0 /*&& !containsZero*/)
	//			//{
	//			//	Console.WriteLine($"w: {variables["w"]} x: {variables["x"]} y: {variables["y"]}  z in: {zStateOnInput}  z out: {variables["z"]}");
	//			//	break;
	//			//}
	//			//else if (containsZero)
	//			//{
	//			//	continue;
	//			//}
	//		}

	//		//Console.WriteLine(zValues);
	//		//int index = 0;
	//		////for (long currentModelNr = 11111111111111/*99999999999999*//*99999895700000*/; currentModelNr > 0; currentModelNr--)
	//		//for (long currentModelNr = 1; currentModelNr < 10; currentModelNr++)
	//		//{
	//		//	Console.WriteLine(currentModelNr);

	//		//	// Convert the long to a string, then split it into individual digits, and convert back to int array
	//		//	modelNr = currentModelNr.ToString().Select(digit => int.Parse(digit.ToString())).ToArray();
	//		//	index = 0;
	//		//	bool containsZero = false;

	//		//	int currentNr = modelNr[index];

	//		//	for (int k = 0; k < NR_OF_ROWS; k++)
	//		//	{
	//		//		string[] enteredCommand = allInput[k].Split(' ');

	//		//		string command = enteredCommand[0];
	//		//		string variableNameA = enteredCommand[1];
	//		//		int a;
	//		//		int b;

	//		//		// Handles the input command  
	//		//		if (command.Equals("inp"))
	//		//		{
	//		//			if (modelNr[index] == 0)
	//		//			{
	//		//				containsZero = true;
	//		//				break;
	//		//			}
	//		//			else
	//		//			{
	//		//				variables[variableNameA] = modelNr[index];
	//		//				index++;
	//		//				continue;
	//		//			}
	//		//		}
	//		//		else
	//		//		{
	//		//			// Keep track of variable name and value
	//		//			a = variables[enteredCommand[1]];

	//		//			// Uses the number value provided or collects saved value of variable
	//		//			if (enteredCommand[2].Any(Char.IsNumber)) b = int.Parse(enteredCommand[2]);
	//		//			else b = variables[enteredCommand[2]];

	//		//			// Perform logic operation
	//		//			a = logic(command, a, b);

	//		//			// Save value in variable
	//		//			variables[variableNameA] = a;
	//		//		}
	//		//	}

	//		//	// Valid modelNr check 
	//		//	if (variables["z"] == 0 /*&& !containsZero*/)
	//		//	{
	//		//		break;
	//		//	}
	//		//	else if (containsZero)
	//		//	{
	//		//		continue;
	//		//	}
	//		//}

	//		//// WILL "z" naturally contain 0?
	//		//if (variables["z"] == 0)
	//		//{
	//		//	for (int i = 0; i < 14; i++)
	//		//	{
	//		//		Console.WriteLine($"{modelNr[0]}");
	//		//	}
	//		//}
	//	}
	//}