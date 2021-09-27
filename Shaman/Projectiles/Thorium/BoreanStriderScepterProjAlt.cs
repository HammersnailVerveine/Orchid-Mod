using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class BoreanStriderScepterProjAlt : OrchidModShamanProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Borean Icicle");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 14;
			projectile.height = 14;
			projectile.friendly = true;
			projectile.aiStyle = 1;
			projectile.timeLeft = 120;
			projectile.penetrate = 3;
		}

		public override void OnSpawn()
		{
			var trail = new Content.Trails.TriangularTrail(target: projectile, length: 16 * 5, width: (p) => 5 * (1 - p), color: (p) => BoreanStriderScepterProj.EffectColor * (1 - p) * 0.25f);
			trail.SetDissolveSpeed(0.35f);
			PrimitiveTrailSystem.NewTrail(trail);

			projectile.frame = Main.rand.Next(3);
		}

		public override void AI()
		{
			projectile.friendly = projectile.timeLeft < 170; // ???

			Lighting.AddLight(projectile.Center, BoreanStriderScepterProj.EffectColor.ToVector3() * 0.35f);

			if (Main.rand.Next(7) == 0)
			{
				var dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, 67)];
				dust.noGravity = true;
				dust.scale = 1.1f;
				dust.noLight = true;
				dust.velocity = projectile.velocity;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				var dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, 67)];
				dust.noGravity = true;
				dust.scale = 1.2f;
				dust.noLight = true;
				dust.velocity = new Vector2(0, Main.rand.NextFloat(2f, 4f)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				target.AddBuff((thoriumMod.BuffType("Freezing")), 2 * 60);
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			var texture = Main.projectileTexture[projectile.type];
			var drawPos = projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY);

			spriteBatch.Draw(texture, drawPos, new Rectangle(projectile.frame * 10, 0, 10, 18), projectile.GetAlpha(lightColor), projectile.rotation, new Vector2(5, 9), projectile.scale, SpriteEffects.None, 0);

			return false;
		}
	}
}