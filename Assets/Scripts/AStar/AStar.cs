using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AStar
{
    private static Dictionary<Point, Node> nodes;

	private static void CreateNodes()
	{
		nodes = new Dictionary<Point, Node>();

		foreach (TileScript tile in LevelManager.Instance.Tiles.Values)
		{
			nodes.Add(tile.GridPosition, new Node(tile));
		}
	}

	public static void GetPath(Point start, Point goal)
	{
		if (nodes == null)
		{
			CreateNodes();
		}

		HashSet<Node> openList = new HashSet<Node>();

		HashSet<Node> closeList = new HashSet<Node>();

		Stack<Node> finalPath = new Stack<Node>();

		Node currentNode = nodes[start];

		//Step 1
		openList.Add(currentNode);

		while (openList.Count > 0)
		{
			for (int x = -1; x <= 1; x++)
			{
				for (int y = -1; y <= 1; y++)
				{
					Point neighbourPos = new Point(currentNode.GridPosition.X - x, currentNode.GridPosition.Y - y);

					if (LevelManager.Instance.Inbounds(neighbourPos) && LevelManager.Instance.Tiles[neighbourPos].Walkable && neighbourPos != currentNode.GridPosition)
					{
						int gCost = 0;

						//Score 10 in we are moving to the side or up or down
						if (Math.Abs(x - y) == 1)
						{
							gCost = 10;
						}

						//scores 14 if we are moving diagonally 
						else
						{
							if (!ConnectedDiagonally(currentNode, nodes[neighbourPos]))
							{
								continue;
							}

							gCost = 14;
						}

						//Step 3
						Node neighbour = nodes[neighbourPos];

						if (openList.Contains(neighbour))
						{
							if (currentNode.G + gCost < neighbour.G)
							{
								//Step 9.4
								neighbour.CalcValues(currentNode, nodes[goal], gCost);
							}
						}

						else if (!closeList.Contains(neighbour))
						{
							//Step 9.2
							openList.Add(neighbour);

							//Step 9.3
							neighbour.CalcValues(currentNode, nodes[goal], gCost);
						}
					}
				}
			}

			//Step 5 and 8 
			openList.Remove(currentNode);
			closeList.Add(currentNode);

			if (openList.Count > 0)
			{
				//sort F lists by value and select first one
				currentNode = openList.OrderBy(n => n.F).First();
			}

			if (currentNode == nodes[goal])
			{
				while (currentNode.GridPosition != start)
				{
					finalPath.Push(currentNode);
					currentNode = currentNode.Parent;
				}

				break;
			}
		}		

		//THIS IS ONLY FOR DEBUGGING NEEDS TO BE REMOVED LATER
		GameObject.Find("AStarDebugger").GetComponent<AStarDebugger>().DebugPath(openList, closeList, finalPath);
	}

	private static bool ConnectedDiagonally(Node currentNode, Node neighbor)
	{
		Point direction = neighbor.GridPosition - currentNode.GridPosition;

		Point first = new Point(currentNode.GridPosition.X + direction.X, currentNode.GridPosition.Y);

		Point second = new Point(currentNode.GridPosition.X, currentNode.GridPosition.Y + direction.Y);

		if (LevelManager.Instance.Inbounds(first) && !LevelManager.Instance.Tiles[first].Walkable)
		{
			return false;
		}

		if (LevelManager.Instance.Inbounds(second) && !LevelManager.Instance.Tiles[second].Walkable)
		{
			return false;
		}

		return true;
	}
}
