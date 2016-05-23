using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts.Skills
{
	class DeathSkills : ISkills
	{
		public void Skill1()
		{
			UnityEngine.Debug.Log("meow1");
		}

		public void Skill2()
		{
			UnityEngine.Debug.Log("meow2");
		}

		public void Skill3()
		{
			UnityEngine.Debug.Log("meow3");
		}
	}
}
