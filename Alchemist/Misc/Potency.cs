using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Misc
{
	public class Potency : ModItem
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
			Lighting.AddLight(item.Center, Color.Green.ToVector3() * 0.4f * Main.essScale);
		}

		public override Color? GetAlpha(Color lightColor) {
			return Color.White;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Potency");
		}

		public override bool OnPickup(Player player) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			Color floatingTextColor = new Color(128, 255, 0);
			int val = 6;
			CombatText.NewText(player.Hitbox, floatingTextColor, val);
			modPlayer.alchemistPotency += modPlayer.alchemistPotency + val > modPlayer.alchemistPotencyMax ? modPlayer.alchemistPotencyMax : modPlayer.alchemistPotency;
			Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y, 85);
			return false;
		}
	}
}
