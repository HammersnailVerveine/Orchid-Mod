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
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 600;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			this.projectileTrail = true;
		}

		public override void AI()
		{
			Projectile.rotation += 0.1f;
			Projectile.velocity.X *= 0.99f;
			Projectile.velocity.Y += 0.1f;
		}

		public override void PostDraw(Color lightColor)
		{
			Texture2D texture = ModContent.GetTexture("OrchidMod/Alchemist/Projectiles/Nature/GlowingMushroomVialProj_Glow");
			OrchidModProjectile.DrawProjectileGlowmask(Projectile, spriteBatch, texture, Color.White);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			int dmg = Projectile.damage;
			Projectile.NewProjectile(Projectile.position.X, Projectile.position.Y, 0f, 0f, ProjectileType<Alchemist.Projectiles.Nature.GlowingMushroomVialProjAlt>(), dmg, 0f, Projectile.owner);
			Projectile.Kill();
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 172);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}
		}
	}
}