using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;

namespace OrchidMod.Alchemist.Misc
{
	public class Potency : OrchidModItem
	{
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.maxStack = 99;
			Item.rare = 0;
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.Green.ToVector3() * 0.4f * Main.essScale);
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Potency");
		}

		public override bool OnPickup(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			Color floatingTextColor = new Color(128, 255, 0);
			int val = 6;
			CombatText.NewText(player.Hitbox, floatingTextColor, val);
			modPlayer.alchemistPotency += modPlayer.alchemistPotency + val > modPlayer.alchemistPotencyMax ? modPlayer.alchemistPotencyMax : modPlayer.alchemistPotency;
			SoundEngine.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 85);
			return false;
		}
	}
}
