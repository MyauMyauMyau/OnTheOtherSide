using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts
{
	class Dictionaries
	{
		public static readonly Dictionary<MonsterType, MonsterType> MonstersUpgradeDictionary = new Dictionary<MonsterType, MonsterType>
		{
			{MonsterType.Pumpkin1, MonsterType.Pumpkin2 },
			{MonsterType.Pumpkin2, MonsterType.Pumpkin3 },
			{MonsterType.Skeleton1, MonsterType.Skeleton2},
			{MonsterType.Skeleton2, MonsterType.Skeleton3},
			{MonsterType.Skeleton3, MonsterType.Skeleton4},
			{MonsterType.Skeleton4, MonsterType.Skeleton5},
		};
		public static readonly Dictionary<char, MonsterType> CharsToObjectTypes = new Dictionary<char, MonsterType>
		{
			{ 'Z', MonsterType.Zombie},
			{ 'S', MonsterType.Spider},
			{ 'V', MonsterType.Voodoo},
			{ 'B', MonsterType.Bat},
			{ 'G', MonsterType.Ghost},
			{ 'H', MonsterType.BlackHole},
			{ 'E', MonsterType.EmptyCell},
			{'1', MonsterType.WaterHorizontal },
			{'2', MonsterType.WaterVertical },
			{'3', MonsterType.WaterDiagonal },
			{'4', MonsterType.WaterHorizontal },
			{'5', MonsterType.WaterVertical },
			{'6', MonsterType.WaterDiagonal },
			{'C', MonsterType.Coocon},
			{'W', MonsterType.Bomb },
			{'R', MonsterType.Raft },
			{'x', MonsterType.Pumpkin1 },
			{'y', MonsterType.Pumpkin2 },
			{'z', MonsterType.Pumpkin3 },
			{'a', MonsterType.Skeleton1 },
			{'b', MonsterType.Skeleton2 },
			{'c', MonsterType.Skeleton3 },
			{'d', MonsterType.Skeleton4 },
			{'e', MonsterType.Skeleton5 },
		};

		public static Dictionary<MonsterType, Sprite> TypesToSprites;

		public static readonly List<MonsterType> MonsterTypes = new List<MonsterType>()
		{
			MonsterType.Zombie,
			MonsterType.Spider,
			MonsterType.Ghost,
			MonsterType.Voodoo,
			MonsterType.Bat,
			MonsterType.Pumpkin1,
			MonsterType.Pumpkin2,
			MonsterType.Pumpkin3,
			MonsterType.Skeleton4,
			MonsterType.Skeleton3,
			MonsterType.Skeleton2,
			MonsterType.Skeleton1,
			MonsterType.Skeleton5
		};

		public static readonly List<MonsterType> AnimatedTypes = new List<MonsterType>()
		{
			MonsterType.Zombie,
			MonsterType.Spider,
			MonsterType.Ghost,
			MonsterType.Voodoo,
			MonsterType.Bat,
		};

		public static List<char> MonsterGenerationList = new List<char>()
		{
			'Z',
			'S',
			'V',
			'B',
			'G',
		};

		public static Dictionary<MonsterType, int> MonsterCounter;
	}
}
