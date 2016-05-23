using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.scripts.Skills;
using UnityEngine;

namespace Assets.scripts
{
	class Dictionaries
	{
		public static readonly Dictionary<HeroType, int> HeroPrices = new Dictionary<HeroType, int>
		{
			{HeroType.Death, 10},
		};

		public static readonly Dictionary<HeroType, ISkills> HeroTypeToSkills = new Dictionary<HeroType, ISkills>
		{
			{HeroType.Death, new DeathSkills()},
		};

		public static Dictionary<HeroType, GameObject> HeroTypesToPrefabs;
		public static readonly Dictionary<MonsterType, MonsterType> MonstersUpgradeDictionary = new Dictionary<MonsterType, MonsterType>
		{
			{MonsterType.Pumpkin1, MonsterType.Pumpkin2 },
			{MonsterType.Pumpkin2, MonsterType.Pumpkin3 },
			{MonsterType.Skeleton1, MonsterType.Skeleton2},
			{MonsterType.Skeleton2, MonsterType.Skeleton3},
			{MonsterType.Skeleton3, MonsterType.Skeleton4},
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
			{'e', MonsterType.WaterHorizontal },
			{'f', MonsterType.WaterVertical },
			{'g', MonsterType.WaterUpperRight },
			{'h', MonsterType.WaterUpperLeft },
			{'i', MonsterType.WaterDownRight },
			{'j', MonsterType.WaterDownLeft },
			//bridges
			{'k', MonsterType.WaterHorizontal },
			{'l', MonsterType.WaterVertical },
			{'m', MonsterType.WaterUpperRight },
			{'n', MonsterType.WaterUpperLeft },
			{'o', MonsterType.WaterDownRight },
			{'p', MonsterType.WaterDownLeft },

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
