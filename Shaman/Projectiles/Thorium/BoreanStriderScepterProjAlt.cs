using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
using Terraria;
using Terraria.GameContent;
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
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 1;
			Projectile.timeLeft = 120;
			Projectile.penetrate = 3;
		}

		public override void OnSpawn()
		{
			/* [SP]
			var trail = new Content.Trails.TriangularTrail(target: Projectile, length: 16 * 5, width: (p) => 5 * (1 - p), color: (p) => BoreanStriderScepterProj.EffectColor * (1 - p) * 0.25f);
			trail.SetDissolveSpeed(0.35f);
			PrimitiveTrailSystem.NewTrail(trail);
			*/

			Projectile.frame = Main.rand.Next(3);
		}

		public override void AI()
		{
			Projectile.friendly = Projectile.timeLeft < 170; // ???

			Lighting.AddLight(Projectile.Center, BoreanStriderScepterProj.EffectColor.ToVector3() * 0.35f);

			if (Main.rand.Next(7) == 0)
			{
				var dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 67)];
				dust.noGravity = true;
				dust.scale = 1.1f;
				dust.noLight = true;
				dust.velocity = Projectile.velocity;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				var dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 67)];
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
				target.AddBuff((thoriumMod.Find<ModBuff>("Freezing").Type), 2 * 60);
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			var texture = TextureAssets.Projectile[Projectile.type].Value;
			var drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);

			spriteBatch.Draw(texture, drawPos, new Rectangle(Projectile.frame * 10, 0, 10, 18), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(5, 9), Projectile.scale, SpriteEffects.None, 0);

			return false;
		}
	}
}