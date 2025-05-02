using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace OrchidMod.Common.ItemDropRules.Conditions
{
	public static partial class OrchidDropConditions
	{
		public class EnableContentGamblerNotExpert : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info) => ModContent.GetInstance<OrchidServerConfig>().EnableContentGambler && !Main.expertMode;
			public bool CanShowItemDropInUI() => ModContent.GetInstance<OrchidServerConfig>().EnableContentGambler;
			public string GetConditionDescription() => null;
		}
	}
}