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
		public class ShroomKeyCondition : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info) => info.npc.value > 0f && Main.hardMode && !info.IsInSimulation && info.player.ZoneGlowshroom;
			public bool CanShowItemDropInUI() => true;
			public string GetConditionDescription() => "Drops in the Hardmode Glowing Mushroom Fields";
		}
	}
}