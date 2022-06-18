using Terraria;
using Terraria.ID;

namespace OrchidMod.Gambler
{
	public class GamblerReset : OrchidModItem
	{
		public override void SetDefaults()
		{
			Item.noMelee = true;
			Item.maxStack = 1;
			Item.width = 20;
			Item.height = 26;
			Item.useStyle = 4;
			Item.UseSound = SoundID.Item64;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.rare = -12;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			if (player.altFunctionUse == 2)
			{
				return false;
			}
			else
			{
				OrchidModGamblerHelper.clearGamblerCards(player, modPlayer);
				OrchidModGamblerHelper.onRespawnGambler(player, modPlayer);
			}
			return base.CanUseItem(player);
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : RESET");
			Tooltip.SetDefault("Test item : resets gambling cards"
							+ "\n[c/FF0000:Test Item]");
		}
	}
}
