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
			case "Weapon":	weapons.Add(new Equipment(type, cost, damage, armor));
				break;
			case "Armor":	armors.Add(new Equipment(type, cost, damage, armor));
				break;
			case "Ring":	rings.Add(new Equipment(type, cost, damage, armor));
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
