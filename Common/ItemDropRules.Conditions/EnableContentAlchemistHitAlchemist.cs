using OrchidMod.Common.Global.NPCs;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace OrchidMod.Common.ItemDropRules.Conditions
{
	public static partial class OrchidDropConditions
	{
		public class EnableContentAlchemistHitAlchemist : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info) => ModContent.GetInstance<OrchidServerConfig>().EnableContentAlchemist && info.npc.GetGlobalNPC<OrchidGlobalNPC>().AlchemistHit;
			public bool CanShowItemDropInUI() => false;
			public string GetConditionDescription() => null;
		}
	}
}