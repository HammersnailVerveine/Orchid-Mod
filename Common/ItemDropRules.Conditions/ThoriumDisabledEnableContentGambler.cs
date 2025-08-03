using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace OrchidMod.Common.ItemDropRules.Conditions
{
	public static partial class OrchidDropConditions
	{
		public class ThoriumDisabledEnableContentGambler : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info) => OrchidMod.ThoriumMod == null && ModContent.GetInstance<OrchidServerConfig>().EnableContentGambler;
			public bool CanShowItemDropInUI() => OrchidMod.ThoriumMod == null && ModContent.GetInstance<OrchidServerConfig>().EnableContentGambler;
			public string GetConditionDescription() => null;
		}
	}
}