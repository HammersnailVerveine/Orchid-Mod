using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;

namespace OrchidMod.Gambler.Misc
{
	public class Chip : OrchidModItem
	{
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.maxStack = 99;
			Item.rare = ItemRarityID.White;
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.Gold.ToVector3() * 0.2f * Main.essScale);
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chip");
		}

		public override bool OnPickup(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			Color floatingTextColor = new Color(255, 200, 0);
			CombatText.NewText(player.Hitbox, floatingTextColor, 1);
			OrchidModGamblerHelper.addGamblerChip(100, player, modPlayer);
			SoundEngine.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 65);
			return false;
		}
	}
}
