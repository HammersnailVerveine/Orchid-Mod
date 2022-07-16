using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class BubbleCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			Item.damage = 13;
			Item.knockBack = 1f;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.shootSpeed = 5f;
			this.cardRequirement = 2;
			this.gamblerCardSets.Add("Elemental");
		}

		public override void AltSetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Bubbles");
			Tooltip.SetDefault("Summons bubbles, floating upwards"
							+ "\nReleasing your left click causes existing bubbles to dash"
							+ "\nBubbles gain in damage over time");
		}

		public override void GamblerShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockBack, bool dummy = false)
		{
			int projType = ProjectileType<Gambler.Projectiles.BubbleCardProj>();
			float scale = 1f - (Main.rand.NextFloat() * .3f);
			velocity = new Vector2(0f, -3f).RotatedByRandom(MathHelper.ToRadians(30)) * scale;
			DummyProjectile(Projectile.NewProjectile(source, position, velocity, projType, damage, knockBack, player.whoAmI), dummy);
			SoundEngine.PlaySound(SoundID.Item86);
		}
	}
}
