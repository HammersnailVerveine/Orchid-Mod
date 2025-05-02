using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace OrchidMod.Common.ItemDropRules.Conditions
{
	public static partial class OrchidDropConditions
	{
		public class EnableContentShapeshifter : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info) => ModContent.GetInstance<OrchidServerConfig>().EnableContentShapeshifter;
			public bool CanShowItemDropInUI() => ModContent.GetInstance<OrchidServerConfig>().EnableContentShapeshifter;
			public string GetConditionDescription() => null;
		}
	}
}