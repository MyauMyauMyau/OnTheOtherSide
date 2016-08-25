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
			{HeroType.Hunter, 10},
			{HeroType.Cleric, 10},
			{HeroType.Mummy, 10},
			{HeroType.Wolverine, 10},
			{HeroType.Vampire, 10}
		};

		public static Dictionary<HeroType, ISkills> HeroTypeToSkills;

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
			{'F', MonsterType.ClericFlag},
			{ 'J', MonsterType.EmptyCell2},
			{ 'I', MonsterType.CandleCell},
			{'X', MonsterType.BloodVortex},
			{'M', MonsterType.MagicSkull},
			{'P' , MonsterType.Plasm },
			{'N', MonsterType.Snake },
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
			MonsterType.Coocon,
			MonsterType.Pumpkin1,
			MonsterType.Pumpkin2,
			MonsterType.Pumpkin3,
			MonsterType.Skeleton4,
			MonsterType.Skeleton3,
			MonsterType.Skeleton2,
			MonsterType.Skeleton1,
			MonsterType.Bomb,
			MonsterType.Plasm,
			MonsterType.Snake,
		};

		public static readonly List<MonsterType> AnimatedTypes = new List<MonsterType>()
		{
			MonsterType.Zombie,
			MonsterType.Spider,
			MonsterType.Ghost,
			MonsterType.Voodoo,
			MonsterType.Bat,
			MonsterType.Plasm,
			MonsterType.Snake
		};

		public static List<char> MonsterGenerationList = new List<char>()
		{

		};

		public static Dictionary<MonsterType, int> MonsterCounter;

		public static Dictionary<HeroType, string> HeroNames = new Dictionary<HeroType, string>()
		{
			{HeroType.Hunter, "Hunter"},
			{HeroType.Cleric, "Cleric"},
			{HeroType.Mummy, "Mummy"},
			{HeroType.Wolverine, "Wolverine"},
			{HeroType.Vampire, "Vampire"},
			{HeroType.Death, "Death"}
		};
		public static Dictionary<HeroType, string> HeroDescriptions = new Dictionary<HeroType, string>()
		{
			{HeroType.Hunter, "A great shooter. He uses gun and bombs. Can build rafts on water levels."},
			{HeroType.Cleric, "Holy warrior! Can illuminate pumpkins and efficiently destroy undeads."},
			{HeroType.Mummy, "Former pharaoh. Controls scarabs and uses power of ancient gods."},
			{HeroType.Wolverine, "Not a human, not a wolf. He can scare enemies with his howl and summon friendly wolfs."},
			{HeroType.Vampire, "Immortal. Uses hypnosis to control his enemies. Master of blood magic."},
			{HeroType.Death, "A guy with a scythe. Uses different types of magic to make his work."}
		};

		public static Dictionary<string, string> SkillNames = new Dictionary<string, string>()
		{
			{"00", "Shot"},
			{"01", "Bombs" },
			{"02", "Raft" },
			{"10", "Exile"},
			{"11", "Holy Flag" },
			{"12", "Summon Light" },
			{"20", "Beam"},
			{"21", "Scarabs" },
			{"22", "Magic Skulls" },
			{"30", "Roar"},
			{"31", "Swipe" },
			{"32", "Summon Wolves" },
			{"40", "Hypnosis"},
			{"41", "Hunger" },
			{"42", "Blood Vortex" },
			{"50", "Fireball"},
			{"51", "Electroshot" },
			{"52", "Grave Ice" },
		};
		public static Dictionary<string, string> SkillDescriptions = new Dictionary<string, string>()
		{
			{"00", "Attack selected monsters and coocons.\n1 lvl. - 1 shot\n2 lvl. - 2 shots\n3 lvl. - 3 shots"},
			{"01", "Explode random monsters.\n1 lvl. - 2 monsters\n2 lvl. - 4 monsters\n3 lvl. - 9 monsters" },
			{"02", "Build river rafts and put them on water.\n1 lvl - 1 raft \n2 lvl - 2 rafts\n3 lvl - 3 rafts" },
			{"10", "Summon sparks that exile random undeads. \n1 lvl - 2 sparks\n2 lvl - 5 sparks\n3 lvl - 9 sparks"},
			{"11", "Put the burning flag on the selected cell for 10 turns.\n1 lvl - every 3rd turn\n2 lvl - every 2nd turn\n3 lvl - every turn" },
			{"12", "Send light to the selected pumpkin to light it up\n1 lvl - 1 pumpkin for a half\n2 lvl - 1 pumpkin\n3 lvl - 2 pumpkins" },
			{"20", "Beam destroys monsters on the selected cell and behind it.\n1 lvl - 1 monster\n2 lvl - 4 monsters\n3 lvl - 9 monsters"},
			{"21", "Send scarabs to destroy coocons.\n1 lvl - 1 scarab\n2 lvl - 2 scarabs\n3 lvl - 3 scarabs" },
			{"22", "Summon skull on cell with grave or monster. It will swap monsters next to it every turn.\n1 lvl - 1 skull\n2 lvl - 2 skulls\n3 lvl - 3 skulls" },
			{"30", "Shuffle monsters on selected lines\n1 lvl - 1 line\n2 lvl - 2 lines\n3 lvl - 3 lines"},
			{"31", "Remove web from selected cells\n1 lvl - 1 cell \n2 lvl - 3 cells\n3 lvl - 6 cells" },
			{"32", "Summon wolves that will attack monsters on selected lines\n1 lvl - 1 line\n2 lvl - 2 lines\n3 lvl - 3 lines" },
			{"40", "Swap selected monsters.\n1 lvl - 1 pair of monsters\n2 lvl - 2 pairs\n3 lvl - 3 pairs"},
			{"41", "Eat bats\n1 lvl - 3 bats\n2 lvl - 5 bats\n3 lvl - all bats" },
			{"42", "Place devastating blood vortex on the selected cell for 10 turns.It will move every 2nd turn.\n1 lvl - 1 vortex\n2 lvl - 2 vortices\n3 lvl - 3 vortices" },
			{"50", "Destroy monsters around the selected cell\n1 lvl - 1 monster\n2 lvl - 5 monsters\n3 lvl - 9 monsters "},
			{"51", "Destroy monsters of the selected type\n1 lvl - 1 monster\n2 lvl - 3 monsters\n3 lvl - all monsters" },
			{"52", "Freeze purple vortices\n1 lvl - 1 vortex\n2 lvl - 2 vortices\n3 lvl - 3 vortices" },
		};

		public static Dictionary<string, int> SkilPrices = new Dictionary<string, int>()
		{
			{"001", 10},
			{"011", 10},
			{"101", 10},
			{"111", 10},
			{"201", 10},
			{"211", 10},
			{"301", 10},
			{"311", 10},
			{"401", 10},
			{"411", 10},
			{"501", 10},
			{"511", 10},
			{"021", 10},
			{"121", 10},
			{"221", 10},
			{"321", 10},
			{"421", 10},
			{"521", 10},
			{"002", 10},
			{"012", 10},
			{"102", 10},
			{"112", 10},
			{"202", 10},
			{"212", 10},
			{"302", 10},
			{"312", 10},
			{"402", 10},
			{"412", 10},
			{"502", 10},
			{"512", 10},
			{"022", 10},
			{"122", 10},
			{"222", 10},
			{"322", 10},
			{"422", 10},
			{"522", 10},
		};		

	}
}
