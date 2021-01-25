using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Misc
{
	public class Chip : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.maxStack = 99;
			item.rare = 0;
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(item.Center, Color.Gold.ToVector3() * 0.2f * Main.essScale);
		}

		public override Color? GetAlpha(Color lightColor) {
			return Color.White;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chip");
		}

		public override bool OnPickup(Player player) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			Color floatingTextColor = new Color(255, 200, 0);
			CombatText.NewText(player.Hitbox, floatingTextColor, 1);
			OrchidModGamblerHelper.addGamblerChip(100, player, modPlayer);
			Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y, 65);
			return false;
		}
	}
}
