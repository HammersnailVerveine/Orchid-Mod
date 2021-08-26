using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class CoznixScepterProjLaser : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Beam");
		}
		public override void SafeSetDefaults()
		{
			projectile.width = 24;
			projectile.height = 24;
			projectile.aiStyle = -1;
			projectile.penetrate = -1;
			projectile.alpha = 255;
			projectile.timeLeft = 7200;
			projectile.tileCollide = false;
		}

		internal const float charge = 20f;
		public float LaserLength { get { return projectile.localAI[1]; } set { projectile.localAI[1] = value; } }
		public const float LaserLengthMax = 350f;
		int multiplier = 1;

		public override bool ShouldUpdatePosition()
		{
			return false;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			projectile.ai[1]++;
			if (projectile.ai[1] == charge)
			{
				projectile.friendly = true;
			}
			if (projectile.ai[1] >= charge + 7200 && multiplier == 1)
			{
				multiplier = -1;
			}
			if (multiplier == -1 && projectile.ai[1] <= 0)
				projectile.Kill();

			if (!Main.projectile[(int)projectile.ai[0]].active)
				projectile.Kill();

			projectile.Center = Main.projectile[(int)projectile.ai[0]].Center;

			projectile.gfxOffY = player.gfxOffY;

			projectile.rotation = projectile.velocity.ToRotation() - 1.57079637f;
			projectile.velocity = Vector2.Normalize(projectile.velocity);

			float[] sampleArray = new float[2];
			Collision.LaserScan(projectile.Center, projectile.velocity, 0, LaserLengthMax, sampleArray);
			float sampledLength = 0f;
			for (int i = 0; i < sampleArray.Length; i++)
			{
				sampledLength += sampleArray[i];
			}
			sampledLength /= sampleArray.Length;
			float amount = 0.75f; // last prism is 0.75 rather than 0.5?
			LaserLength = MathHelper.Lerp(LaserLength, sampledLength, amount);

			#region Dusts
			Vector2 endPoint = projectile.Center + projectile.velocity * (projectile.localAI[1] - 14f);
			if (Main.rand.Next(5) == 0)
			{
				float num809 = projectile.velocity.ToRotation() + ((Main.rand.Next(2) == 1) ? -1f : 1f) * 1.57079637f;
				float num810 = (float)Main.rand.NextDouble() * 2f + 2f;
				Vector2 vector79 = new Vector2((float)Math.Cos((double)num809) * num810, (float)Math.Sin((double)num809) * num810);
				int num811 = Dust.NewDust(endPoint, 0, 0, 90, vector79.X, vector79.Y, 0, default(Color), 1f);
				Main.dust[num811].noGravity = true;
				Main.dust[num811].scale = 1.7f;
			}
			if (Main.rand.Next(5) == 0)
			{
				Vector2 value29 = projectile.velocity.RotatedBy(1.5707963705062866, default(Vector2)) * ((float)Main.rand.NextDouble() - 0.5f) * (float)projectile.width;
				int num812 = Dust.NewDust(endPoint + value29 - Vector2.One * 4f, 8, 8, 31, 0f, 0f, 100, default(Color), 1.5f);
				Dust dust3 = Main.dust[num812];
				dust3.velocity *= 0.5f;
				Main.dust[num812].velocity.Y = -Math.Abs(Main.dust[num812].velocity.Y);
			}
			#endregion
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float collisionPoint = 0f;
			return (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + projectile.velocity * LaserLength, projHitbox.Width, ref collisionPoint));
		}

		public override bool? CanCutTiles()
		{
			DelegateMethods.tilecut_0 = Terraria.Enums.TileCuttingContext.AttackProjectile;
			Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * LaserLength, (float)projectile.width * projectile.scale * 2, new Utils.PerLinePoint(CutTilesAndBreakWalls));
			return true;
		}

		private bool CutTilesAndBreakWalls(int x, int y)
		{
			return DelegateMethods.CutTiles(x, y);
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) { }

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (projectile.velocity == Vector2.Zero) return true;

			Texture2D texture2D19 = Main.projectileTexture[projectile.type];
			Texture2D texture2D20 = mod.GetTexture("Shaman/Projectiles/Thorium/CoznixScepterProjLaser_Beam");
			Texture2D texture2D21 = mod.GetTexture("Shaman/Projectiles/Thorium/CoznixScepterProjLaser_End");
			float num228 = LaserLength;
			Color color44 = Color.White * 0.8f;
			Texture2D arg_AF99_1 = texture2D19;
			Vector2 arg_AF99_2 = projectile.Center + new Vector2(0, projectile.gfxOffY) - Main.screenPosition;
			Rectangle? sourceRectangle2 = null;
			spriteBatch.Draw(arg_AF99_1, arg_AF99_2, sourceRectangle2, color44, projectile.rotation, texture2D19.Size() / 2f, new Vector2(Math.Min(projectile.ai[1], charge) / charge, 1f), SpriteEffects.None, 0f);
			num228 -= (float)(texture2D19.Height / 2 + texture2D21.Height) * projectile.scale;
			Vector2 value20 = projectile.Center + new Vector2(0, projectile.gfxOffY);
			value20 += projectile.velocity * projectile.scale * (float)texture2D19.Height / 2f;
			if (num228 > 0f)
			{
				float num229 = 0f;
				Microsoft.Xna.Framework.Rectangle rectangle7 = new Microsoft.Xna.Framework.Rectangle(0, 16 * (projectile.timeLeft / 3 % 5), texture2D20.Width, 16);
				while (num229 + 1f < num228)
				{
					if (num228 - num229 < (float)rectangle7.Height)
					{
						rectangle7.Height = (int)(num228 - num229);
					}
					Main.spriteBatch.Draw(texture2D20, value20 - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(rectangle7), color44, projectile.rotation, new Vector2((float)(rectangle7.Width / 2), 0f), new Vector2(Math.Min(projectile.ai[1], charge) / charge, 1f), SpriteEffects.None, 0f);
					num229 += (float)rectangle7.Height * projectile.scale;
					value20 += projectile.velocity * (float)rectangle7.Height * projectile.scale;
					rectangle7.Y += 16;
					if (rectangle7.Y + rectangle7.Height > texture2D20.Height)
					{
						rectangle7.Y = 0;
					}
				}
			}
			SpriteBatch arg_B1FF_0 = Main.spriteBatch;
			Texture2D arg_B1FF_1 = texture2D21;
			Vector2 arg_B1FF_2 = value20 - Main.screenPosition;
			sourceRectangle2 = null;
			arg_B1FF_0.Draw(arg_B1FF_1, arg_B1FF_2, sourceRectangle2, color44, projectile.rotation, texture2D21.Frame(1, 1, 0, 0).Top(), new Vector2(Math.Min(projectile.ai[1], charge) / charge, 1f), SpriteEffects.None, 0f);

			return true;
		}
	}
}
