using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace OrchidMod.Common.ItemDropRules.Conditions
{
	public static partial class OrchidDropConditions
	{
		public class EnableContentAlchemistNotExpert : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info) => ModContent.GetInstance<OrchidServerConfig>().EnableContentAlchemist && !Main.expertMode;
			public bool CanShowItemDropInUI() => ModContent.GetInstance<OrchidServerConfig>().EnableContentAlchemist;
			public string GetConditionDescription() => null;
		}
	}
}