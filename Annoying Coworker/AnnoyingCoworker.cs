/*
 * Worker is lazy, instead of doing the work himself he asks his coworkers to help him. Each time he asks for help 
 * they get more annoyed. How must he distribute his asking for help to keep the annoyance level of the coworker with highest as low as possible, 
 * if a project takes "h" helps to complete? 
 * 
 * c = number of coworkers (1 - 100 000)
 * 
 * a = initial annoyance level (1 - 1·000·000·000)
 * d = how much more annoyed they become each time you ask them for help (1 - 1·000·000·000)
 * 
 * h = number of times you have to ask for help to complete project (1 - 100 000)
 * 
 * annoyance level increase:
 * a += d
 * 
 * Problem: keep the annoyance level as even as possible, there by keeping the overall annoyance level as low as possible. 
 * 
 * Output: the highest annoyance level. 
 * 
 * To do this we make use of a min heap, so that the 
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Linq;
using System.ComponentModel.DataAnnotations;

class AnnoyingCoworker
{
    string input;
    long helpNeeded = 0;
    long numberOfCoworkers = 0;
    long addedCoworkers = 0; 

    public AnnoyingCoworker()
    {
		start();
    }

    private void start()
    {
		bool firstInputSubmitted = false;
		var priorityQueue1 = new PriorityQueue<Coworker, long>();


		while ((input = Console.ReadLine()) != null)
		{
			string[] inputToArray = input.Split(' ');

			if (!firstInputSubmitted)
			{
				firstInputSubmitted = true;
				helpNeeded = Int64.Parse(inputToArray[0]);
				numberOfCoworkers = Int64.Parse(inputToArray[1]);
			}
			else
			{
				Coworker coworker = new Coworker(Int64.Parse(inputToArray[0]), Int64.Parse(inputToArray[1]));
				coworker.setWithIncrease();
				priorityQueue1.Enqueue(coworker, coworker.getWithIncrease());

				addedCoworkers++;

				if (numberOfCoworkers == addedCoworkers)
				{
					for (int i = 0; i < helpNeeded; i++)
					{
						coworker = priorityQueue1.Dequeue();
						coworker.CurrentLevel = (coworker.CurrentLevel + coworker.Increase);
						coworker.setWithIncrease();
						priorityQueue1.Enqueue(coworker, coworker.getWithIncrease());

					}

					Coworker coworkerHighestAnnoyance = new Coworker(0, 0);
					long highestValue = 0;

					for (int i = 0; i < numberOfCoworkers; i++)
					{
						coworkerHighestAnnoyance = priorityQueue1.Dequeue();
						if (coworkerHighestAnnoyance.CurrentLevel > highestValue)
						{
							highestValue = coworkerHighestAnnoyance.CurrentLevel;
						}
					}
					Console.WriteLine(highestValue);
				}
			}
		}
	}
}

public class Coworker
{
    public long CurrentLevel { get; set; }
    public long Increase { get; set; }
    public long withIncrease; 

    public Coworker(long annoyancelevel, long annoyanceIncrease)
    {
        CurrentLevel = annoyancelevel;
        Increase = annoyanceIncrease;
    }

    public long getWithIncrease()
    {
        return this.withIncrease;
    }

    public void setWithIncrease()
    {
        this.withIncrease = (CurrentLevel + Increase);
    }
}