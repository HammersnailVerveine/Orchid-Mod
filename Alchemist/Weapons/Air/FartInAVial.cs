using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.Projectiles;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Air
{
	public class FartInAVial : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 12;
			Item.width = 30;
			Item.height = 30;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			this.potencyCost = 1;
			this.element = AlchemistElement.AIR;
			this.rightClickDust = 16;
			this.colorR = 149;
			this.colorG = 229;
			this.colorB = 140;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Fart in a Flask");
			/* Tooltip.SetDefault("Launches hit enemy in the air"
							+ "\nIncreases the likelihood of spawning catalytic bubbles"); */
		}

		public override void KillSecond(int timeLeft, Player player, OrchidAlchemist modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			int nb = 2 + Main.rand.Next(3);
			for (int i = 0; i < nb; i++)
			{
				Vector2 vel = (new Vector2(0f, -(float)((3 * alchProj.nbElements) + Main.rand.Next(3))).RotatedByRandom(MathHelper.ToRadians(10)));
				int spawnProj = Main.rand.NextBool(3) ? ProjectileType<AlchemistSmoke3>() : ProjectileType<AlchemistSmoke2>();
				int smokeProj = SpawnProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, vel, spawnProj, 0, 0f, projectile.owner);
				Main.projectile[smokeProj].localAI[0] = alchProj.glowColor.R;
				Main.projectile[smokeProj].localAI[1] = alchProj.glowColor.G;
				Main.projectile[smokeProj].ai[1] = alchProj.glowColor.B;
			}

			if (alchProj.waterFlask.type == ItemType<Alchemist.Weapons.Water.BloodMoonFlask>())
			{
				for (int i = 0; i < nb; i++)
				{
					int dmg = GetSecondaryDamage(player, alchProj.nbElements);
					Vector2 vel = (new Vector2(0f, -(float)((3 * alchProj.nbElements) + Main.rand.Next(3))).RotatedByRandom(MathHelper.ToRadians(10)));
					int spawnProj = ProjectileType<Alchemist.Projectiles.Water.BloodMoonFlaskProj>();
					SpawnProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, vel, spawnProj, dmg, 0f, projectile.owner);
				}
			}

			int type = ProjectileType<Alchemist.Projectiles.Air.CloudInAVialProj>();
			int newProj = SpawnProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, Vector2.Zero, type, 0, 0.5f, projectile.owner);
			Main.projectile[newProj].ai[1] = alchProj.nbElements;
			Main.projectile[newProj].netUpdate = true;
		}
	}
}
