using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace OrchidMod.Common.ItemDropRules.Conditions
{
	public static partial class OrchidDropConditions
	{
		public class DownedEyeOfCthulhu : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info) => NPC.downedBoss1;
			public bool CanShowItemDropInUI() => true;
			public string GetConditionDescription() => null;
		}
	}
}