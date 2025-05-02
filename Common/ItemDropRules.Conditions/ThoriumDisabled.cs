using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace OrchidMod.Common.ItemDropRules.Conditions
{
	public static partial class OrchidDropConditions
	{
		public class ThoriumDisabled : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info) => OrchidMod.ThoriumMod == null;
			public bool CanShowItemDropInUI() => OrchidMod.ThoriumMod == null;
			public string GetConditionDescription() => null;
		}
	}
}