using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.Projectiles;
using OrchidMod.Common.Globals.NPCs;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Fire
{
	public class EmberVial : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 7;
			Item.width = 30;
			Item.height = 30;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			this.potencyCost = 1;
			this.element = AlchemistElement.FIRE;
			this.rightClickDust = 6;
			this.colorR = 253;
			this.colorG = 62;
			this.colorB = 3;
			this.secondaryDamage = 8;
			this.secondaryScaling = 2f;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ember Flask");
			/* Tooltip.SetDefault("Burns your target briefly"
							+ "\nReleases lingering embers"); */
		}

		public override void KillSecond(int timeLeft, Player player, OrchidAlchemist modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			int nb = 2 + Main.rand.Next(3);
			for (int i = 0; i < nb; i++)
			{
				Vector2 vel = (new Vector2(0f, -(float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(80)));
				int spawnProj = ProjectileType<Alchemist.Projectiles.Fire.EmberVialProjAlt>();
				SpawnProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, vel, spawnProj, 0, 0f, projectile.owner);
			}
			int dmg = GetSecondaryDamage(player, 0);
			int rand = alchProj.nbElements + Main.rand.Next(2);
			for (int i = 0; i < rand; i++)
			{
				Vector2 vel = (new Vector2(0f, -3f).RotatedByRandom(MathHelper.ToRadians(60)));
				SpawnProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, vel, ProjectileType<Alchemist.Projectiles.Fire.EmberVialProj>(), dmg, 0f, projectile.owner);
			}
		}

		public override void OnHitNPCSecond(NPC target, int damage, float knockback, bool crit, Player player, OrchidAlchemist modPlayer,
		OrchidModAlchemistNPC modTarget, OrchidGlobalNPC modTargetGlobal, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			target.AddBuff(BuffID.OnFire, (60 * 3) + (60 * (alchProj.nbElements)));
		}
	}
}
