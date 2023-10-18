using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Big
{
	public class MonowaiExplosion : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 160;
			Projectile.height = 160;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 1;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
			Projectile.penetrate = 200;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Explosion");
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer)
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				target.AddBuff((thoriumMod.Find<ModBuff>("Singed").Type), 5 * 60);
			}

			if (!(target.boss || target.type == NPCID.TargetDummy) && target.knockBackResist > 0f)
			{
				target.velocity.Y = -15f;
				target.AddBuff((Mod.Find<ModBuff>("AquaBump").Type), 10 * 60);
			}
		}
	}
}