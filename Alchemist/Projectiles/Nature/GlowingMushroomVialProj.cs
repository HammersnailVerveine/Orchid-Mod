using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Projectiles.Nature
{
	public class GlowingMushroomVialProj : OrchidModAlchemistProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mushroom Spore");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.friendly = false;
			projectile.aiStyle = 0;
			projectile.timeLeft = 600;
			ProjectileID.Sets.Homing[projectile.type] = true;
			this.projectileTrail = true;
		}

		public override void AI()
		{
			projectile.rotation += 0.1f;
			projectile.velocity.X *= 0.99f;
			projectile.velocity.Y += 0.1f;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = ModContent.GetTexture("OrchidMod/Alchemist/Projectiles/Nature/GlowingMushroomVialProj_Glow");
			OrchidModProjectile.DrawProjectileGlowmask(projectile, spriteBatch, texture, Color.White);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			int dmg = projectile.damage;
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0f, 0f, ProjectileType<Alchemist.Projectiles.Nature.GlowingMushroomVialProjAlt>(), dmg, 0f, projectile.owner);
			projectile.Kill();
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 172);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}
		}
	}
}