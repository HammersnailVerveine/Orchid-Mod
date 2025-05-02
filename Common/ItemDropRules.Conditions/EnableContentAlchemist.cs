using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace OrchidMod.Common.ItemDropRules.Conditions
{
	public static partial class OrchidDropConditions
	{
		public class EnableContentAlchemist : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info) => ModContent.GetInstance<OrchidServerConfig>().EnableContentAlchemist;
			public bool CanShowItemDropInUI() => ModContent.GetInstance<OrchidServerConfig>().EnableContentAlchemist;
			public string GetConditionDescription() => null;
		}
	}
}