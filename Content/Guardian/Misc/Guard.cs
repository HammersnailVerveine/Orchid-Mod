using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Misc
{
	public class Guard : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.maxStack = 1;
			Item.rare = ItemRarityID.White;
		}

		public override void SetStaticDefaults()
		{
			ItemID.Sets.IsAPickup[Type] = true;
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.Brown.ToVector3() * 0.4f * Main.essScale);
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override bool CanPickup(Player player)
		{
			OrchidGuardian guardian = player.GetModPlayer<OrchidGuardian>();
			if (guardian.GuardianGuard >= guardian.GuardianGuardMax) return false;
			return base.CanPickup(player);
		}

		public override bool OnPickup(Player player)
		{
			OrchidGuardian guardian = player.GetModPlayer<OrchidGuardian>();
			guardian.AddGuard(1);
			SoundEngine.PlaySound(SoundID.Item53, player.Center); // (2, position, 85)
			return false;
		}
	}
}
