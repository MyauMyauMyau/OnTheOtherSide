using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Assets.scripts.Enums;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.scripts
{
	static class WaterField
	{
		public static Monster[,] Map;
		public static Dictionary<Coordinate, Direction> Bridges = new Dictionary<Coordinate, Direction>();
		public static List<Coordinate> River;
		public static List<Monster> BridgeObjects;
		public static bool IsBridgeOrNull(int x, int y)
		{
			return (Map[x, y] == null || Bridges.Keys.Contains(new Coordinate(x, y)));
		}
														
		public static void GenerateRiver()
		{
			Coordinate currentCoordinate = new Coordinate();
			for (int i = 0; i < Game.MAP_SIZE; i++)
			{
				if (Map[0,i] != null)
				{
					var cnt = 0;
					if (Map[1,i] != null)
						cnt++;
					if (i > 0 && Map[0, i - 1] != null)
						cnt++;
					if (i < Game.MAP_SIZE - 1 && Map[0, i + 1] != null)
						cnt++;
					if (cnt == 1)
					{
						currentCoordinate = new Coordinate(0,i);
						River.Add(currentCoordinate);
						break;
					}
				}
			}
			if (River.Count == 0)
				return;
			while (true)
			{
				if (currentCoordinate.Y > 0 && Map[currentCoordinate.X, currentCoordinate.Y - 1] != null 
					&& !River.Contains(new Coordinate(currentCoordinate.X, currentCoordinate.Y-1)))
				{
					currentCoordinate = new Coordinate(currentCoordinate.X, currentCoordinate.Y - 1);
					River.Add(currentCoordinate);
					continue;	
				}
				if (currentCoordinate.Y < Game.MAP_SIZE - 1 && Map[currentCoordinate.X, currentCoordinate.Y + 1] != null
					&& !River.Contains(new Coordinate(currentCoordinate.X, currentCoordinate.Y + 1)))
				{
					currentCoordinate = new Coordinate(currentCoordinate.X, currentCoordinate.Y + 1);
					River.Add(currentCoordinate);
					continue;
				}
				if (currentCoordinate.X < Game.MAP_SIZE - 1 && Map[currentCoordinate.X + 1, currentCoordinate.Y] != null
					&& !River.Contains(new Coordinate(currentCoordinate.X + 1, currentCoordinate.Y)))
				{
					currentCoordinate = new Coordinate(currentCoordinate.X + 1, currentCoordinate.Y);
					River.Add(currentCoordinate);
					continue;
				}
				if (currentCoordinate.X > 0 && Map[currentCoordinate.X-1, currentCoordinate.Y] != null
					&& !River.Contains(new Coordinate(currentCoordinate.X - 1, currentCoordinate.Y)))
				{
					currentCoordinate = new Coordinate(currentCoordinate.X-1, currentCoordinate.Y);
					River.Add(currentCoordinate);
					continue;
				}
				break;
			}

		}
		public static void ShiftBridges()
		{
			Debug.Log("bridgesCount " + Bridges.Count);
			Game.PlayerIsBlocked = true;
			var dict = new Dictionary<Coordinate, Direction>();
			for (int i = 0; i < Bridges.Count; i++)
			{
				Debug.Log(Bridges.Count);
				var k = River.FindIndex(x => x == Bridges.ElementAt(i).Key);
				if (Bridges.ElementAt(i).Value == Direction.Forward)
				{
					if (k == River.Count - 1)
					{
						if (Bridges.ContainsKey(River.ElementAt(River.Count - 2)) || dict.ContainsKey(River.ElementAt(River.Count - 2)))
							continue;
						dict.Add(River.ElementAt(River.Count - 2), Direction.Backward);
						Bridges.Remove(Bridges.ElementAt(i).Key);
						MoveBridge(River.ElementAt(k), River.ElementAt(River.Count - 2));
					}
					else
					{
						if (Bridges.ContainsKey(River.ElementAt(k + 1)) || dict.ContainsKey(River.ElementAt(k + 1)))
						{
							if (k == 0 || Bridges.ContainsKey(River.ElementAt(k - 1)) || dict.ContainsKey(River.ElementAt(k - 1)))
								continue;
							dict.Add(River.ElementAt(k - 1), Direction.Backward);
							Bridges.Remove(Bridges.ElementAt(i).Key);
							MoveBridge(River.ElementAt(k), River.ElementAt(k - 1));
						}
						else
						{
							dict.Add(River.ElementAt(k + 1), Direction.Forward);
							Bridges.Remove(Bridges.ElementAt(i).Key);
							MoveBridge(River.ElementAt(k), River.ElementAt(k + 1));
						}
					}
				}
				else
				{
					if (k == 0)
					{
						if (Bridges.ContainsKey(River.ElementAt(1)) || dict.ContainsKey(River.ElementAt(1)))
							continue;
						dict.Add(River.ElementAt(1), Direction.Forward);
						Bridges.Remove(Bridges.ElementAt(i).Key);
						MoveBridge(River.ElementAt(0), River.ElementAt(1));
					}
					else
					{
						if (Bridges.ContainsKey(River.ElementAt(k - 1)) || dict.ContainsKey(River.ElementAt(k - 1)))
						{
							if (k == 0 || Bridges.ContainsKey(River.ElementAt(k + 1)) || dict.ContainsKey(River.ElementAt(k + 1)))
								continue;
							dict.Add(River.ElementAt(k + 1), Direction.Forward);
							Bridges.Remove(Bridges.ElementAt(i).Key);
							MoveBridge(River.ElementAt(k), River.ElementAt(k + 1));
						}
						else
						{
							dict.Add(River.ElementAt(k - 1), Direction.Backward);
							Bridges.Remove(Bridges.ElementAt(i).Key);
							MoveBridge(River.ElementAt(k), River.ElementAt(k - 1));
						}
					}
				}
				i--;
			}
			for (int i = 0; i < Bridges.Count; i++)
			{
				dict.Add(Bridges.ElementAt(i).Key, Bridges.ElementAt(i).Value);
			}
			Bridges = dict;
			Game.PlayerIsBlocked = false;
		}

		private static void MoveBridge(Coordinate from, Coordinate to)
		{
			var bridge = BridgeObjects.First(x => x.GridPosition == from);
			Debug.Log(from.X +" "+ from.Y);
			Debug.Log(to.X + " " + to.Y);
			bridge.Move(to);
			GameField.Map[from.X, from.Y].Move(to);
			GameField.Map[to.X, to.Y] = GameField.Map[from.X, from.Y];
			GameField.Map[from.X, from.Y] = null;
		}
	}
}
