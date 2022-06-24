using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

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
			OrchidModPlayerGambler modPlayer = player.GetModPlayer<OrchidModPlayerGambler>();
			Color floatingTextColor = new Color(255, 200, 0);
			CombatText.NewText(player.Hitbox, floatingTextColor, 1);
			modPlayer.AddGamblerChip(100);
			SoundEngine.PlaySound(SoundID.Item65, player.Center);
			return false;
		}
	}
}
