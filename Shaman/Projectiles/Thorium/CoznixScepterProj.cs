using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class CoznixScepterProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.aiStyle = 1;
			Projectile.timeLeft = 55;
			Projectile.scale = 1f;
			Projectile.alpha = 128;
			AIType = ProjectileID.Bullet;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Bolt");
		}

		public override void AI()
		{
			int DustID = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 60, Projectile.velocity.X * 0.75f, Projectile.velocity.Y * 0.75f, 125, default(Color), 1.25f);
			Main.dust[DustID].scale *= 1.5f;
			Main.dust[DustID].noGravity = true;

			DustID = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, Projectile.velocity.X, Projectile.velocity.Y * 0.75f, 125, default(Color), 1.25f);
			Main.dust[DustID].scale *= 1.5f;
			Main.dust[DustID].noGravity = true;

			DustID = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 90, Projectile.velocity.X * 1.5f, Projectile.velocity.Y * 1.5f, 125, default(Color), 1.25f);
			Main.dust[DustID].scale *= 1f;
			Main.dust[DustID].noGravity = true;
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == Mod.Find<ModProjectile>("CoznixScepterProjPortal").Type && proj.owner == player.whoAmI)
				{
					proj.active = false;
				}
			}

			if (OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod) > 2)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y - 300, 0f, 0f, Mod.Find<ModProjectile>("CoznixScepterProjPortal").Type, 0, 0.0f, Projectile.owner, 0.0f, 0.0f);
			}
		}
	}
}