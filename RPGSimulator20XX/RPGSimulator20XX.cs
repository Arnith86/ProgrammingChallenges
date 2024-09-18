using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProgrammingChallenges.RPGSimulator20XX;

/**
 * To solve this problem, we use dynamic programming and a variation of the knapsack problem. 
 * The key idea is to avoid evaluating each possible combination of equipment in battle. 
 * Instead, we evaluate each unique combination of stats (Damage and Armor). For each unique combination, 
 * we conduct a single battle and store the result in a memoization matrix. If the same combination of stats 
 * recurs, we retrieve the outcome from the matrix, which significantly reduces the number of battles.
 * 
 * --- Part Two ---
 * It turns out the shopkeeper is working with the boss and can persuade you to buy any items he wants. 
 * The other rules remain the same, and he still only has one of each item. 
 * The question is: what is the maximum amount of gold you can spend and still lose the fight?
 * 
 * Solving part two builds on part one. We can reuse the knapsackProblem method, but now return an array 
 * (instead of just an integer, as in part 1). Using the same nested loops as before, we now check if the 
 * battle simulation is lost, and if so, whether the cost is higher than the current highest losing cost. 
 * This way, we perform both checks simultaneously.
 **/



public class RPGSimulator20XX
{
	List<Equipment> weapons = new List<Equipment>();
	List<Equipment> armors = new List<Equipment>();
	List<Equipment> rings = new List<Equipment>();

	Character boss;
	Character player;

	int[,] memo;  

	public RPGSimulator20XX() 
	{
		fillShop();
		int[] bossStats = readBossStats();
		
		boss = new Character(bossStats[0], bossStats[1], bossStats[2]);
		player = new Character(100, 0, 0);

		int[] finalCost = knapsackProblem();

		Console.WriteLine($"The lowest cost while still winning was: {finalCost[0]} \nThe Highest cost while still losing was: {finalCost[1]}");
	}


	private int[] knapsackProblem()
	{
		// Find the highest possible damage and armor
		int highestDamage = weapons.Max(x => x.Damage) + (rings.Max(x => x.Damage) * 2);
		int highestArmor = armors.Max(x => x.Armor) + (rings.Max(x => x.Armor) * 2);

		// Initializing the memo matrix
		// -1: no memory yet, 0: lose, 1: win
		memo = new int[highestDamage + 1, highestArmor + 1];

		for (int i = 0; i < highestDamage + 1; i++)
		{
			for (int j = 0; j < highestArmor + 1; j++)
			{
				memo[i, j] = -1;
			}
		}

		// Convert lists to array for ease of access and add option for not wearing equipments
		Equipment[] weapon = new Equipment[weapons.Count];
		Equipment[] armor = new Equipment[armors.Count + 1];
		Equipment[] ring = new Equipment[rings.Count + 1];

		armor[0] = new Equipment("", 0, 0, 0);
		ring[0] = new Equipment("", 0, 0, 0);
		
		weapon = weapons.ToArray();
		armors.ToArray().CopyTo(armor, 1);
		rings.ToArray().CopyTo(ring, 1);
		

		int lowestCost = int.MaxValue;
		int highestCost = int.MinValue;
		int totalCost = 0; 

		// Equipes every weapon, one at a time
		for (int i = 0; i < weapon.Length; i++)
		{
			player.Damage = weapon[i].Damage;
			totalCost = weapon[i].Cost;

			// Equipes every armore, one at a time, staring with no armore
			for (int j = 0; j < armor.Length; j++)
			{
				player.Armor = armor[j].Armor;
				totalCost += armor[j].Cost;

				// Equipes 0 to two rings 
				for (int k = 0; k < ring.Length; k++)
				{
					player.Damage += ring[k].Damage;
					player.Armor += ring[k].Armor;
					totalCost += ring[k].Cost;
					
					// Single or no ring equiped
					if (memo[player.Damage, player.Armor] == -1)
					{
						if (doBattle() == true) memo[player.Damage, player.Armor] = 1;
						else memo[player.Damage, player.Armor] = 0;
					}

					if (memo[player.Damage, player.Armor] == 1) lowestCost = Math.Min(lowestCost, totalCost);	// Part 1	
					if (memo[player.Damage, player.Armor] == 0) highestCost = Math.Max(highestCost, totalCost);	// Part 2

					// Second ring equiped
					//foreach (Equipment secondRing in ring)
					for (int l = 0; l < ring.Length; l++)
					{
						// Cannot wear same ring on two hands
						if (!(ring[k] == ring[l]))
						{
							player.Damage += ring[l].Damage;
							player.Armor += ring[l].Armor;
							totalCost += ring[l].Cost;

							if (memo[player.Damage, player.Armor] == -1)
							{
								if (doBattle() == true) memo[player.Damage, player.Armor] = 1;
								else memo[player.Damage, player.Armor] = 0;
							}

							if (memo[player.Damage, player.Armor] == 1) lowestCost = Math.Min(lowestCost, totalCost);   // Part 1	
							if (memo[player.Damage, player.Armor] == 0) highestCost = Math.Max(highestCost, totalCost); // Part 2

							player.Damage -= ring[l].Damage;
							player.Armor -= ring[l].Armor;
							totalCost -= ring[l].Cost;
						}
					}

					player.Damage -= ring[k].Damage;
					player.Armor -= ring[k].Armor;
					totalCost -= ring[k].Cost;
				}

				player.Armor -= armor[j].Armor;
				totalCost -= armor[j].Cost;
			}

			player.Damage -= weapon[i].Damage;
			totalCost -= weapon[i].Cost;
		}

		
		return new int[] { lowestCost, highestCost }; 
	}

	// Performas a battle simulation 
	private bool doBattle()
	{
		int bossHP = boss.HP;
		int playerHP = player.HP;
		int bossAttacks = 0;
		int playerAttacks = 0;

		bossAttacks = Math.Max(1, boss.Damage - player.Armor);
		playerAttacks = Math.Max(1, player.Damage - boss.Armor);

		while (true)
		{
			if ((bossHP -= playerAttacks) <= 0) return true;
			if ((playerHP -= bossAttacks) <= 0)
			{ return false; }
		}
	}

	// Fills shop with equipments found in a file.
	private void fillShop()
	{
		StreamReader sr = new StreamReader("ItemShop.txt");

		string line;
		string equipmentType = "";

		while((line = sr.ReadLine()) != null)
		{
			if (line.Contains("Weapons:")) 
			{
				equipmentType = "Weapon";
				while (!string.IsNullOrEmpty(line = sr.ReadLine()))	putEquipmentInList(equipmentType, line);
			}

			if (line.Contains("Armor:"))
			{
				equipmentType = "Armor";
				while (!string.IsNullOrEmpty(line = sr.ReadLine())) putEquipmentInList(equipmentType, line);
			}

			if (line.Contains("Rings:"))
			{
				equipmentType = "Ring";
				while (!string.IsNullOrEmpty(line = sr.ReadLine())) putEquipmentInList(equipmentType, line);
			}
		}
	}

	// Places equipments in its appropriate list 
	private void putEquipmentInList(string equipmentType, string readLine)
	{
		string[] lineToArray = readLine.Split("  ", StringSplitOptions.RemoveEmptyEntries);

		string type = lineToArray[0];
		int cost = int.Parse(lineToArray[1]);
		int damage = int.Parse(lineToArray[2]);
		int armor = int.Parse(lineToArray[3]);
		
		switch (equipmentType)
		{
			case "Weapon":
				weapons.Add(new Equipment(type, cost, damage, armor));
				break;
			case "Armor":
				armors.Add(new Equipment(type, cost, damage, armor));
				break;
			case "Ring":
				rings.Add(new Equipment(type, cost, damage, armor));
				break;
			default:
				break;
		}
	}

	// Gets the stats for the boss
	private int[] readBossStats()
	{
		int[] bossStats= new int[3];
		StreamReader sr = new StreamReader("RPGSimulator20XXInput.txt");
		string line;

		// This regex will match one or more digits (i.e., an integers)
		Regex regex = new Regex(@"\d+");

		for (int i = 0; i < 3; i++)
		{
			line = sr.ReadLine();
			// finds integers in the line
			Match integers = regex.Match(line);
			if (integers.Success) bossStats[i] = int.Parse(integers.Value); 
		}

		return bossStats;
	}
}

public class Equipment{
	public string Type { get; set; }
	public int Cost { get; set; }
	public int Damage { get; set; }
	public int Armor { get; set; }

	public Equipment(string type, int cost, int damage, int armor)
	{
		Type= type;
		Cost= cost;
		Damage= damage;
		Armor= armor;
	}
}

public class Character
{
	public int HP { get; set; }
	public int Damage { get; set; }
	public int Armor { get; set;}

	public Character(int hP, int damage, int armor)
	{
		HP = hP;
		Damage = damage;
		Armor= armor;
	}
}
