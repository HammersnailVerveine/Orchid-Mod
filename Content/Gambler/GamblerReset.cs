using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Gambler
{
	public class GamblerReset : ModItem
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
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();

			if (player.altFunctionUse == 2)
			{
				return false;
			}
			else
			{
				modPlayer.ClearGamblerCards();
				modPlayer.OnRespawn();
			}
			return base.CanUseItem(player);
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Playing Card : RESET");
			/* Tooltip.SetDefault("Test item : resets gambling cards"
							+ "\n[c/FF0000:Test Item]"); */
		}
	}
}
