/* Lots of runners use personal Global Positioning System (GPS) receivers to track how many miles
they run. No GPS is perfect, though: it only records its position periodically rather than
continuously, so it can miss parts of the true running path. For this problem we'll consider a
GPS that works in the following way when tracking a run:

 - At the beginning of the run, the GPS first records the runner's starting position at time 0.
 - It then records the position every t units of time.
 - It always records the position at the end of the run, even if the total running time is not a multiple of t.

The GPS assumes that the runner goes in a straight line between each consecutive pair of recorded positions. Because of 
this, a GPS can underestimate the total distance run. For example, suppose someone runs in straight lines and at constant speed
between the positions on the left side of Table 1. The time they reach each position is shown next to the position. 
They stopped running at time 11. If the GPS records a position every 2 units of time, its readings would be the
records on the right side of Table 1.*/

/*
Time	Position  	| Position	Time
0		(0,0)		|	0		(0,0)
3		(0,3)		|	2		(0,2)
5		(-2,5)		|	4		(-1,4)
7		(0,7)		|	6		(-1,6)
9		(2,5)		|	8		(1,6)
11		(0,3)		|	10		(1,4)
					|   11      (0,3)


Table 1: Actual Running Path on the left, GPS readings on the right.

The total distance run is approximately 14.313708 units, while the GPS measures the distance as 
approximately 11.650281 units. The difference between the actual and GPS distance is approximately 2.663427 
units, or approximately 18.607525% of the total run distance.

Given a sequence of positions and times for a running path, as well as the GPS recording time interval t, 
calculate the percentage of the total run distance that is lost by the GPS. Your computations should assume that 
the runner goes at a constant speed in a straight line between consecutive positions.
*/


/* time        pos
 * start :   0
 * t
 * tx2
 * tx3
 * end   :   t does not mater 	
 * 
 * GPS assumes straight line between positions
 * 
 * first input: n t
 * n = total number of positions (2 - 100)
 * t = the time interval of recordings (1 - 100)
 *
 * next n inputs: x_i y_i t_i
 * x_i = the coordinant (-1 000 000 - 1 000 000) 
 * y_i = time of coordinant in seconds (-1 000 000 - 1 000 000) 
 * t_i = (0 - 1 000 000)
 *
 * Problem: calculate the total run distance that is lost by the GPS
 * 
 * Underlying problem: Calculate the correct positioning of the gps from the values of the actuall input
 *
 * Example input
 * 6 2
 * 0 0 0
 * 0 3 3
 * -2 5 5
 * 0 7 7
 * 2 5 9
 * 0 3 11
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;

class InaccurrateGPS
{
	string input;
	bool positionsAndIntervalSet = false;

	int numberOfReadings = 0;
	int timeIntervall = 0;

	int currentRealReading = 0;
	int howLongWasRun = 0;

	public InaccurrateGPS()
	{
		start();
	}
	private void start()
	{
		Queue<Coordinant> actualRun = new Queue<Coordinant>();
		Queue<Coordinant> actualRun2 = new Queue<Coordinant>();
		Coordinant[] everySecondReading = new Coordinant[0];

		while ((input = Console.ReadLine()) != null)
		{
			string[] inputToArray = input.Split(' ');

			// Register how many readings the actual run has and the gps timing
			if (!positionsAndIntervalSet)
			{
				numberOfReadings = int.Parse(inputToArray[0]);
				timeIntervall = int.Parse(inputToArray[1]);

				everySecondReading = new Coordinant[numberOfReadings];
				actualRun = new Queue<Coordinant>();
				actualRun2 = new Queue<Coordinant>();

				positionsAndIntervalSet = true;
			}
			// register each new coordinant for the actual run
			else
			{
				int readAt = int.Parse(inputToArray[2]);
				int x = int.Parse(inputToArray[0]);
				int y = int.Parse(inputToArray[1]);

				actualRun.Enqueue(new Coordinant(readAt, x, y));
				actualRun2.Enqueue(new Coordinant(readAt, x, y));

				currentRealReading++;

				// used to keep track of how long the run lasted.
				if (readAt > howLongWasRun)
				{
					howLongWasRun = readAt;
				}
			}

			// Start calculating second by second array
			// When as many entries as expected are registered 
			if (numberOfReadings == currentRealReading)
			{
				Coordinant firstCoordinant = null;
				Coordinant secondCoordinant = null;

				int sizeOfActuallRunQueue = actualRun.Count;
				everySecondReading = new Coordinant[howLongWasRun + 1];

				// keeps track of last seconds coordinants 
				int currentX = 0;
				int currentY = 0;

				// Used to not recalculate every time.
				int xAjust = 0;
				int yAjust = 0;

				// ensures that the first entrie gets registered 
				if (sizeOfActuallRunQueue == currentRealReading)
				{
					firstCoordinant = actualRun.Dequeue();
				}

				bool secondCoordinantSet = false;
				int secondCoordinantReadAt = 0;

				// Calculates current seconds position.
				for (int i = 1; i < howLongWasRun + 1; i++)
				{
					if (i == 1)
					{
						everySecondReading[0] = firstCoordinant;
						firstCoordinant.Second = 0;
						currentX = firstCoordinant.X;
						currentY = firstCoordinant.Y;
					}

					if ((secondCoordinantReadAt + 1) == i || !secondCoordinantSet)
					{

						if (!secondCoordinantSet)
						{
							secondCoordinant = actualRun.Dequeue();
							secondCoordinantSet = true;
						}
						else
						{
							firstCoordinant = secondCoordinant;
							secondCoordinant = actualRun.Dequeue();
						}
						secondCoordinantReadAt = secondCoordinant.ReadAtSecond;
						sizeOfActuallRunQueue--;

						int xDifference = secondCoordinant.X - firstCoordinant.X;
						int yDifference = secondCoordinant.Y - firstCoordinant.Y;
						int readAtSecondDifferece = secondCoordinant.ReadAtSecond - firstCoordinant.ReadAtSecond;


						// makes sure that we never divid by 0.
						// or if it by some reason produces a negative time
						if (readAtSecondDifferece == 0)
						{
							xAjust = xDifference;
							yAjust = yDifference;
						}
						else
						{
							xAjust = xDifference / readAtSecondDifferece;
							yAjust = yDifference / readAtSecondDifferece;
						}
					}

					// update the current values and place this seconds Coordinants in the second by second array
					if (i != 0)
					{
						currentX = (currentX + xAjust);
						currentY = (currentY + yAjust);
						everySecondReading[i] = new Coordinant(i, currentX, currentY);
						everySecondReading[i].Second = i;
					}

				}

				// get the coordinants for gps 
				Queue<Coordinant> gpsReadings = new Queue<Coordinant>();

				int currentSecond = 0;

				// Registers the position of the gps run, with the exeption of the last coordinent if it is not same as time intervall 
				while (true)
				{
					Coordinant gpsCoordinant = new Coordinant(currentSecond, everySecondReading[currentSecond].X, everySecondReading[currentSecond].Y);
					gpsReadings.Enqueue(gpsCoordinant);

					if (!((currentSecond + timeIntervall) > howLongWasRun))
					{
						currentSecond += timeIntervall;
					}
					else
					{
						break;
					}
				}

				if (currentSecond < howLongWasRun)
				{
					Coordinant gpsCoordinant = new Coordinant(howLongWasRun, everySecondReading[howLongWasRun].X, everySecondReading[howLongWasRun].Y);
					gpsReadings.Enqueue(gpsCoordinant);
				}

				DistanceCalculator distanceCalculator = new DistanceCalculator(gpsReadings, actualRun2, howLongWasRun, timeIntervall);
			}
		}
	}
}


// handles the calculation of the distance 
public class DistanceCalculator
{
	Queue<Coordinant> gpsReadings;
	Queue<Coordinant> actuallReadings;

	int timeIntervall = 0;
	int howLongWasRun = 0;

	public DistanceCalculator(Queue<Coordinant> gpsReadings, Queue<Coordinant> actuallReadings, int howLongWasRun, int timeIntervall)
	{
		this.gpsReadings = gpsReadings;
		this.actuallReadings = actuallReadings;
		this.timeIntervall = timeIntervall;
		this.howLongWasRun = howLongWasRun;

		double gpsDistance = calculateDistance(gpsReadings, timeIntervall);
		double actuallDistance = calculateDistance(actuallReadings);
		double distanceDifference = actuallDistance - gpsDistance;
		double procent = (distanceDifference / actuallDistance) * 100;
		Console.WriteLine(procent);
	}

	// Calculates the distance between coordinants
	public double howFar(Coordinant firstCoordinant, Coordinant secondCoordinant)
	{
		double howFar = 0;
		bool isDiagonal = false;

		int xDifference = secondCoordinant.X - firstCoordinant.X;
		int yDifference = secondCoordinant.Y - firstCoordinant.Y;

		if (!(xDifference == 0) && !(yDifference == 0))
		{
			isDiagonal = true;
		}

		// if distance is diagonal, the distance will be the hypotenuse
		if (isDiagonal)
		{
			double combinedX = (secondCoordinant.X) - (firstCoordinant.X);
			double combinedY = (secondCoordinant.Y) - (firstCoordinant.Y);
			double hypotenuse = Math.Sqrt((Math.Pow(combinedX, 2)) + (Math.Pow(combinedY, 2)));
			howFar += hypotenuse;
		}
		// if not diagonal then distance between coordinants 
		else
		{
			int tempDistance = 0;
			switch ((xDifference, yDifference))
			{
				case (0, 0): tempDistance += xDifference; break;
				case (0, _): tempDistance += yDifference; break;
				case (_, 0): tempDistance += xDifference; break;

				default: break;
			}

			// handles negative distances
			if (tempDistance < 0)
			{
				tempDistance *= -1;
			}

			howFar += tempDistance;
		}
		return howFar;
	}

	// Iterates trough the coordinants and sends to a method that calculates the distance between coordinates
	public double calculateDistance(Queue<Coordinant> readings, int timeIntervall = 1)
	{
		double distance = 0;

		int numberOfReadings = readings.Count;
		int currentSecond = 0;

		Coordinant firstCoordinant = null;
		Coordinant secondCoordinant = null;

		if (numberOfReadings != 0)
		{
			firstCoordinant = readings.Dequeue();
		}

		if (numberOfReadings > 1)
		{
			while ((currentSecond+timeIntervall) <= this.howLongWasRun)
			{
				secondCoordinant = readings.Dequeue();
				distance += howFar(firstCoordinant, secondCoordinant);
				currentSecond = secondCoordinant.ReadAtSecond;
				firstCoordinant = secondCoordinant;
			}

			if ((numberOfReadings > 0) && (currentSecond < this.howLongWasRun))
			{
				firstCoordinant = secondCoordinant;
				secondCoordinant = readings.Dequeue();
				distance += howFar(firstCoordinant, secondCoordinant);
			}
		} 
		return distance;
	}
}

// houses the coordinant information 
public class Coordinant
{
	public int Second { get; set; }
	public bool IsDiagonal { get; set; }
	public int X { get; set; }
	public int Y { get; set; }
	public int ReadAtSecond { get; set; }

	public Coordinant(int readAt, int x, int y, bool isDiagonal = false)
	{
		X = x;
		Y = y;
		ReadAtSecond = readAt;
	}
}