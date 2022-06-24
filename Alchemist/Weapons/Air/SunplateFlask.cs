using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.Projectiles;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Air
{
	public class SunplateFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 15;
			Item.width = 30;
			Item.height = 30;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			this.potencyCost = 2;
			this.element = AlchemistElement.AIR;
			this.rightClickDust = 21;
			this.colorR = 190;
			this.colorG = 18;
			this.colorB = 148;
			this.secondaryDamage = 8;
			this.secondaryScaling = 4f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Talc");
			Tooltip.SetDefault("Creates orbiting stars on impact"
							+ "\nThe stars are considered as lingering particles");
		}

		public override void KillSecond(int timeLeft, Player player, OrchidAlchemist modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			int rand = Main.rand.Next(180);
			int rand2 = Main.rand.Next(3);
			for (int i = 0; i < 2; i++)
			{
				Vector2 vel = (new Vector2(0f, -5f).RotatedBy(MathHelper.ToRadians(rand + (180 * i))));
				int dmg = GetSecondaryDamage(player, alchProj.nbElements);
				int spawnProj = Projectile.NewProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, vel, ProjectileType<Alchemist.Projectiles.Air.SunplateFlaskProj>(), dmg, 0.1f, projectile.owner);
				Main.projectile[spawnProj].ai[1] = 180f * i;
				Main.projectile[spawnProj].ai[0] = rand2;
				Main.projectile[spawnProj].netUpdate = true;
			}
		}
	}
}
