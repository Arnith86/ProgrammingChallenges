using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProgrammingChallenges.RPGSimulator20XX;

public class RPGSimulator20XX
{
	List<Equipment> weapons = new List<Equipment>();
	List<Equipment> armors = new List<Equipment>();
	List<Equipment> rings = new List<Equipment>();

	Character boss;
	Character player;

	public RPGSimulator20XX() 
	{
		fillShop();
		int[] bossStats = readBossStats();
		
		boss = new Character(bossStats[0], bossStats[1], bossStats[2]);
		player = new Character(100, 0, 0);

		int finalCost = knapsackProblem();

		Console.WriteLine($"The lowest cost was: {finalCost}");
	}

	private int knapsackProblem()
	{
		doBattle();
		return 0; 
	}

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
			if ((bossHP -= playerAttacks) <= 0) break;
			if ((playerHP -= bossAttacks) <= 0) break;
		}

		if (bossHP < 0)
		{
			return true;
		}
		else return false;
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
		// Replace multiple spaces with a single space
		string normalizedReadLine = Regex.Replace(readLine, @"\s+", "  ");
		string[] lineToArray = normalizedReadLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);

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
