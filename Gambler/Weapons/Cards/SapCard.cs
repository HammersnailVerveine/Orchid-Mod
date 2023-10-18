using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class SapCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			Item.damage = 10;
			Item.knockBack = 0.5f;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.shootSpeed = 3f;
			Item.UseSound = SoundID.Item1;
			Item.channel = true;

			this.cardRequirement = 0;
			this.cardSets = GamblerCardSets.Elemental;
		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Playing Card : Unstable Sap");
			/* Tooltip.SetDefault("Releases a slow-moving sap bubble, following the cursor"
							+ "\nUpon releasing the mouse click, the bubble will explode"
							+ "\nThe longer the bubble exists, the more explosion damage"); */
		}

		public override void GamblerShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback, bool dummy = false)
		{
			int projType = ModContent.ProjectileType<Gambler.Projectiles.SapCardProj>();

			if (player.ownedProjectileCounts[projType] == 0 && player.channel)
			{
				DummyProjectile(Projectile.NewProjectile(source, position, velocity, projType, damage, knockback, player.whoAmI), dummy);
				SoundEngine.PlaySound(SoundID.Item1);
			}
			else
				SoundEngine.PlaySound(SoundID.Item7);
		}
	}
}
