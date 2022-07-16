using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace OrchidMod.Alchemist.Misc
{
	public class Potency : OrchidModItem
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
			Lighting.AddLight(Item.Center, Color.Green.ToVector3() * 0.4f * Main.essScale);
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AltSetStaticDefaults()
		{
			DisplayName.SetDefault("Potency");
		}

		public override bool OnPickup(Player player)
		{
			OrchidAlchemist modPlayer = player.GetModPlayer<OrchidAlchemist>();
			Color floatingTextColor = new Color(128, 255, 0);
			int val = 6;
			CombatText.NewText(player.Hitbox, floatingTextColor, val);
			modPlayer.alchemistPotency += modPlayer.alchemistPotency + val > modPlayer.alchemistPotencyMax ? modPlayer.alchemistPotencyMax : modPlayer.alchemistPotency;
			SoundEngine.PlaySound(SoundID.Item85, player.Center); // (2, position, 85)
			return false;
		}
	}
}
