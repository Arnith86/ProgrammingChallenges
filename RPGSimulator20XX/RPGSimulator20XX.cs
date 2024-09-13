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
	List<Weapon> weapons = new List<Weapon>();
	List<Protection> armors = new List<Protection>();
	List<Ring> rings = new List<Ring>();

	public RPGSimulator20XX() 
	{
		fillShop();
	}

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
			case "Weapon":	weapons.Add(new Weapon(type, cost, damage, armor));
				break;
			case "Armor":	armors.Add(new Protection(type, cost, damage, armor));
				break;
			case "Ring":	rings.Add(new Ring(type, cost, damage, armor));
				break;
			default:
				break;
		}
	}

	private void fillShop()
	{
		int nrOfLines = (File.ReadLines("ItemShop.txt").Count());
		StreamReader sr = new StreamReader("ItemShop.txt");

		string line;
		string equipmentType = "";

		while((line = sr.ReadLine()) != null)
		{
			if (line.Contains("Weapons:")) 
			{
				equipmentType = "Weapon";

				while (!string.IsNullOrEmpty(line = sr.ReadLine()))
				{
					putEquipmentInList(equipmentType, line);
				}
			}

			if (line.Contains("Armor:"))
			{
				equipmentType = "Armor";
				while (!string.IsNullOrEmpty(line = sr.ReadLine()))
				{
					putEquipmentInList(equipmentType, line);
				}
			}

			if (line.Contains("Rings:"))
			{
				equipmentType = "Ring";
				while (!string.IsNullOrEmpty(line = sr.ReadLine()))
				{
					putEquipmentInList(equipmentType, line);
				}
			}
		}
	}
}

public interface Equipment
{
	string Type { get; set;}
	int Cost { get; set; }
	int Damage { get; set; }
	int Armor { get; set; }
}

public class Weapon : Equipment
{
	public string Type { get; set; }
	public int Cost { get; set; }
	public int Damage { get; set; }
	public int Armor { get; set; }

	public Weapon(string type, int cost, int damage, int armor)
	{
		Type= type;
		Cost= cost;
		Damage= damage;
		Armor= armor;
	}
}

public class Protection : Equipment
{
	public string Type { get; set; }
	public int Cost { get; set; }
	public int Damage { get; set; }
	public int Armor { get; set; }

	public Protection(string type, int cost, int damage, int armor)
	{
		Type = type;
		Cost = cost;
		Damage = damage;
		Armor = armor;
	}
}

public class Ring : Equipment
{
	public string Type { get; set; }
	public int Cost { get; set; }
	public int Damage { get; set; }
	public int Armor { get; set; }

	public Ring (string type, int cost, int damage, int armor)
	{
		Type = type;
		Cost = cost;
		Damage = damage;
		Armor = armor;
	}
}