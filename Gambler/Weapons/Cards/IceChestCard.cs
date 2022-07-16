using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class IceChestCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			Item.damage = 32;
			Item.knockBack = 4f;
			Item.useAnimation = 25;
			Item.useTime = 25;
			Item.shootSpeed = 12.5f;

			this.cardRequirement = 2;
			this.cardSets = GamblerCardSets.Elemental;
		}

		public override void AltSetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Icicle");
			Tooltip.SetDefault("Summons icicles, falling from the ceiling above your cursor");
		}

		public override void GamblerShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback, bool dummy = false)
		{
			int projType = ProjectileType<Gambler.Projectiles.IceChestCardProj>();
			Vector2 newPosition = Main.MouseWorld;
			Vector2 offSet = new Vector2(0f, -15f);
			for (int i = 0; i < 50; i++)
			{
				offSet = Collision.TileCollision(newPosition, offSet, 14, 32, true, false, (int)player.gravDir);
				newPosition += offSet;
				if (offSet.Y > -15f)
				{
					break;
				}
			}
			newPosition.Y = player.position.Y - newPosition.Y > Main.screenHeight / 2 ? player.position.Y - Main.screenHeight / 2 : newPosition.Y;
			velocity = new Vector2(0f, 12.5f);
			DummyProjectile(Projectile.NewProjectile(source, newPosition, velocity, projType, damage, knockback, player.whoAmI), dummy);
			SoundEngine.PlaySound(SoundID.Item30);
		}
	}
}
