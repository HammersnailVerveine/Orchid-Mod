using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Misc
{
	public class Chip : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.maxStack = 1;
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
			ItemID.Sets.IsAPickup[Type] = true;

			// DisplayName.SetDefault("Chip");
		}

		public override bool OnPickup(Player player)
		{
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			Color floatingTextColor = new Color(255, 200, 0);
			CombatText.NewText(player.Hitbox, floatingTextColor, 1);
			modPlayer.AddGamblerChip();
			SoundEngine.PlaySound(SoundID.Item65, player.Center);
			return false;
		}
	}
}
