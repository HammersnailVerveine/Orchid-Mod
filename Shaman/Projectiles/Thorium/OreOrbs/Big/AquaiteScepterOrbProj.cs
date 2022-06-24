using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Big
{
	public class AquaiteScepterOrbProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 20;
			Projectile.scale = 1f;
			Projectile.alpha = 192;
			Projectile.tileCollide = false;
			Projectile.penetrate = 10;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Geyser");
		}

		public override void AI()
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 29);
				Main.dust[dust].velocity /= 10f;
				Main.dust[dust].scale = 1f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = false;

				int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 33);
				Main.dust[dust2].velocity /= 1f;
				Main.dust[dust2].scale = 1.7f;
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].noLight = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 29);
				Main.dust[dust].velocity = Projectile.velocity / 2;
				Main.dust[dust].noGravity = true;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer)
		{
			if (!(target.boss || target.type == NPCID.TargetDummy) && target.knockBackResist > 0f)
			{
				target.velocity.Y = -15f;
				target.AddBuff((Mod.Find<ModBuff>("AquaBump").Type), 10 * 60);
			}
		}
	}
}