using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingChallenges.ClumsyCrucible;

public class ClumsyCrucible
{
	int NR_OF_ROWS;
	int NR_OF_COLUMNS;

	int[,] heatMap;  
	const string ROW = "row";
	const string COLUMN = "column";

	Node[,] heatMap;
	int[,] adjacencyMatrix;

	public ClumsyCrucible()
	{
		readInput();
		adjacencyMatrixCreator();
		Console.WriteLine( dijkstrasAlgorithm());
		Console.WriteLine("");
	}
	
	
	private int dijkstrasAlgorithm()
	{
		int combination = (NR_OF_ROWS * NR_OF_COLUMNS);
		
		// Keeps the node with the shortest distance at the top
		PriorityQueue<Node, int> leastHeatLoss = new PriorityQueue<Node, int>();
		Node[] visited = new Node[combination];
		
		// Index: NodeNr, 0: Least Heatloss, 1: Previous Node
		int[,] heatLoss = new int[combination, 2];

		int index = 0;
		// Initializes the variables, adds all nodes to unVisited and only first node gets shortest distance and prev node.
		for (int i = 0; i < NR_OF_ROWS; i++)
		{
			for (int j = 0; j < NR_OF_COLUMNS; j++)
			{
				Node tempNode = heatMap[i, j];
				if ( tempNode.NodeNr == 0)
				{
					heatLoss[tempNode.NodeNr, 0] = 0;
					heatLoss[tempNode.NodeNr, 1] = 0;
				}
				else
				{
					heatLoss[tempNode.NodeNr, 0] = int.MaxValue;
					heatLoss[tempNode.NodeNr, 1] = -1;
				}
				visited[index++] = heatMap[i, j];
			}
		}

		// Adds start node to queue
		Node currNode = heatMap[0, 0];
		leastHeatLoss.Enqueue(currNode, currNode.HeatReduction);
		
		while(true)
		{
			if (leastHeatLoss.Count != 0) currNode = leastHeatLoss.Dequeue();
			else break;

			// Skips the currNode if it has already been visited
			if (currNode.Visited) continue;
			// Checks if the algorithm have taken 3 steps in a direction
			bool threeSteps = (threeStepsTaken(visited, heatLoss, currNode.NodeNr));
			// Total heatloss to currNode
			int heatlossToCurrNode = heatLoss[currNode.NodeNr, 0];

			foreach (KeyValuePair<int,int> node in currNode.costToNeighbor)
			{
				Node nextNode = visited[node.Key];

				// Checks if algorithm is trying to go back the same way it came, and if we have taken 3 steps in a direction
				//if (nextNode.Visited == true) continue;
				if (isNodeParent(currNode, nextNode, visited, heatLoss)) continue;
				if (threeSteps)
				{
					// If row or column of nextNode is the same as the previousNode of currNode 
					// then the nextNode continues in the same direction, therefore it is skiped.
					Node previousNode = visited[heatLoss[currNode.NodeNr, 1]];
					if (onSameRowOrColumn(ROW, nextNode, previousNode)) continue;
					if (onSameRowOrColumn(COLUMN, nextNode, previousNode)) continue;
				}

				// Total heatloss from start to neighbor node
				int heatLossToNextNode = heatlossToCurrNode + node.Value;

				// If heatloss is less to nextNode then current heatloss, then set this node as new lest heatloss node
				if ((heatLoss[node.Key, 0] > heatLossToNextNode)) 
				{
					heatLoss[node.Key, 0] = heatLossToNextNode;
					heatLoss[node.Key, 1] = currNode.NodeNr;
					//leastHeatLoss.
					// If neighbor node has not been visited, add it to the priority queue
					if (!visited[nextNode.NodeNr].Visited) leastHeatLoss.Enqueue(nextNode, heatLossToNextNode);
				}

				

			}

			visited[currNode.NodeNr].Visited = true;
		}

		
		
		int cNode = combination - 1; 
		Node lastVisitedNode = visited[cNode];
		Node prevVisitedNode = null;
		Node tempNode2 = lastVisitedNode;
		int steps = 0;

		// retrives the node 3 steps back
		while(true)
		{
			// Gets the node visited before the last visited node (takes a step back in the search)
			prevVisitedNode = visited[heatLoss[cNode, 1]];

			// If prevNode and tempNode is same then both are node 0, resulting in less then three steps (prev node of 0 is 0)
			if (prevVisitedNode == tempNode2) break;

			tempNode2 = prevVisitedNode;
			cNode = heatLoss[cNode, 1];
			steps++;
		}

		cNode = combination - 1;
		lastVisitedNode = visited[cNode];
		prevVisitedNode = null;
		tempNode2 = lastVisitedNode;

		//int[,] sequence = new int[steps, 2];
		int[] sequence = new int[steps];


		// retrives the node 3 steps back
		for (int i = 0; i < steps; i++)
		{
			// Gets the node visited before the last visited node (takes a step back in the search)
			prevVisitedNode = visited[heatLoss[cNode, 1]];

			// If prevNode and tempNode is same then both are node 0, resulting in less then three steps (prev node of 0 is 0)
			if (prevVisitedNode == tempNode2) break;

			//sequence[i, 0] = heatLoss[cNode, 0];
			sequence[i] = heatLoss[cNode, 1];
			//sequence[i, 1] = heatLoss[cNode, 1];

			tempNode2 = prevVisitedNode;
			cNode = heatLoss[cNode, 1];
		}

		return heatLoss[combination -1, 0];
	}

	// Checks if the attempted direction is the parent node to current node
	private bool isNodeParent(Node currNode, Node nextNode, Node[] visited, int[,] heatLoss)
	{
		return (nextNode.NodeNr == heatLoss[currNode.NodeNr, 1]);
	}
	
	// Checks how many steps have been taken. 
	private bool threeStepsTaken(Node[] visited, int[,] heatLoss, int currNode) 
	{
		int rowSteps = 0;
		int columnSteps = 0; 

		Node lastVisitedNode = visited[currNode];
		Node prevVisitedNode = null;
		Node tempNode = lastVisitedNode;

		// retrives the node 3 steps back
		for (int i = 0; i < 3; i++)
		{
			// Gets the node visited before the last visited node (takes a step back in the search)
			prevVisitedNode = visited[heatLoss[currNode, 1]];

			// If the previous node is the same as the current one, it means it's the start (0)
			if (prevVisitedNode.NodeNr == tempNode.NodeNr) return false;

			// If current node and prev node is on same row 
			if (onSameRowOrColumn(ROW, tempNode, prevVisitedNode)) rowSteps++;
			if (onSameRowOrColumn(COLUMN, tempNode, prevVisitedNode)) columnSteps++;
		
			// Move a step back 
			tempNode = prevVisitedNode;
			currNode = heatLoss[currNode, 1];
		}

		// Only return true if all three steps were in the same direction
		return (rowSteps == 3 || columnSteps == 3);
	}

	// Checks if two nodes are in the same row or column 
	private bool onSameRowOrColumn(string axis, Node node1, Node node2)
	{
		if (axis.Equals(ROW)) return (node1.Row == node2.Row);
		else return (node1.Column == node2.Column);
	}

	// Make sure this returns an array 
	private void adjacencyMatrixCreator()
	{

		Queue<Node> bfsQueue = new Queue<Node>();
		bool[,] visited = new bool[NR_OF_ROWS, NR_OF_COLUMNS];
		int combination = (NR_OF_ROWS * NR_OF_COLUMNS);
		adjacencyMatrix = new int[combination, combination];

		int[,] direction = new int[,]
		{
			{  1,  0 },  // down
			{ -1,  0 },  // upp
			{  0, -1 },	 // left
			{  0,  1 }   // right
		};

		// Start position of the bfs inserted 
		bfsQueue.Enqueue(heatMap[0,0]);
		visited[0, 0] = true;

		while (true)
		{
			// All nodes that can be visited has been visited
			if (!(bfsQueue.Count > 0)) break;

			Node currNode = bfsQueue.Dequeue();
			
			for (int i = 0; i < 4; i++)
			{
				int nextRow = currNode.Row + direction[i, 0];
				int nextColumn = currNode.Column + direction[i, 1];

				// Next coordinants for node is outside matrix
				if (nextRow == NR_OF_ROWS || nextColumn == NR_OF_COLUMNS || nextRow < 0 || nextColumn < 0) continue;
			
				Node nextNode = heatMap[nextRow, nextColumn];

				currNode.costToNeighbor.Add(nextNode.NodeNr, nextNode.HeatReduction);
				adjacencyMatrix[currNode.NodeNr, nextNode.NodeNr] = nextNode.HeatReduction;
									
				// Adds new node to queue, if not yet visited
				if (!visited[nextRow, nextColumn])
				{
					visited[nextRow, nextColumn] = true;
					bfsQueue.Enqueue(nextNode);
				}
			}
		}
	}

	// Gets the proportion of the input matrix
	private void getInputSize()
	{
		StreamReader sr = new StreamReader("ExampleInput.txt");
		//StreamReader sr = new StreamReader("ClumsyCrucibleInput.txt");
		NR_OF_ROWS = (File.ReadLines("ExampleInput.txt").Count());
		//NR_OF_ROWS = (File.ReadLines("ClumsyCrucibleInput.txt").Count());
		NR_OF_COLUMNS = sr.ReadLine().Count();
	}

	// Creates the map that will be used to map the possible routes.
	private void readInput()
	{
		getInputSize();
		StreamReader sr = new StreamReader("ExampleInput.txt");
		//StreamReader sr = new StreamReader("ClumsyCrucibleInput.txt");

		heatMap = new Node[NR_OF_ROWS, NR_OF_COLUMNS];
		int nodeNr = 0;

		for (int i = 0; i < NR_OF_ROWS; i++)
		{
			string line = sr.ReadLine();
			int index = 0; 
			foreach (char charInString in line)
			{
				heatMap[i, index] = new Node((int)char.GetNumericValue(charInString), nodeNr++, i, index++);
			}
		}
	}
}
public class Node
{
	public int HeatReduction { get; set; }
	public int NodeNr { get; set; }
	public int Row { get; set; }
	public int Column { get; set; }
	public bool Visited { get; set; }
	public Dictionary<int, int> costToNeighbor { get; set; }

	public Node(int heatReduction, int nodeNr, int row, int column, bool visited = false)
	{
		HeatReduction = heatReduction;
		NodeNr = nodeNr;
		Row = row;
		Column = column;
		costToNeighbor = new Dictionary<int, int>();
		Visited = visited;
	}
}


