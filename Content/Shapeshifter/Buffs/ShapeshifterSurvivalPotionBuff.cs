using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Buffs
{
	public class ShapeshifterSurvivalPotionBuff : ModBuff
	{
		public override void Update(Player player, ref int buffIndex)
		{
			OrchidShapeshifter shapeshifter = player.GetModPlayer<OrchidShapeshifter>();
			if (shapeshifter.IsShapeshifted)
			{
				shapeshifter.ShapeshifterSurvival = true;
			}
		}
	}
}