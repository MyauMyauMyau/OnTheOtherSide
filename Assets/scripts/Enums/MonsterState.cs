using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.scripts
{
	public enum MonsterState
	{
		Default,
		Clicked,
		Moving,
		Dropping,
		WaitingForInitialising,
		WaitingForActivation,
		Growing,
		Decreasing,
		Destroying,
		Deactivated
	}
}
