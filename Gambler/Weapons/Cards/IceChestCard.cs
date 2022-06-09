using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class IceChestCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = 1;
			Item.damage = 32;
			Item.crit = 4;
			Item.knockBack = 4f;
			Item.useAnimation = 25;
			Item.useTime = 25;
			Item.shootSpeed = 12.5f;
			this.cardRequirement = 2;
			this.gamblerCardSets.Add("Elemental");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Icicle");
			Tooltip.SetDefault("Summons icicles, falling from the ceiling above your cursor");
		}

		public override void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false)
		{
			int projType = ProjectileType<Gambler.Projectiles.IceChestCardProj>();
			Vector2 newPos = Main.MouseWorld;
			Vector2 offSet = new Vector2(0f, -15f);
			for (int i = 0; i < 50; i++)
			{
				offSet = Collision.TileCollision(newPos, offSet, 14, 32, true, false, (int)player.gravDir);
				newPos += offSet;
				if (offSet.Y > -15f)
				{
					break;
				}
			}
			newPos.Y = player.position.Y - newPos.Y > Main.screenHeight / 2 ? player.position.Y - Main.screenHeight / 2 : newPos.Y;
			OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(newPos.X, newPos.Y, 0f, 12.5f, projType, damage, knockBack, player.whoAmI), dummy);
			SoundEngine.PlaySound(2, (int)player.Center.X, (int)player.Center.Y - 200, 30);
		}
	}
}
