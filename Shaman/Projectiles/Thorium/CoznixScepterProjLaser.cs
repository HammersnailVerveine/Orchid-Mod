using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

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
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.timeLeft = 7200;
			Projectile.tileCollide = false;
		}

		internal const float charge = 20f;
		public float LaserLength { get { return Projectile.localAI[1]; } set { Projectile.localAI[1] = value; } }
		public const float LaserLengthMax = 350f;
		int multiplier = 1;

		public override bool ShouldUpdatePosition()
		{
			return false;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			Projectile.ai[1]++;
			if (Projectile.ai[1] == charge)
			{
				Projectile.friendly = true;
			}
			if (Projectile.ai[1] >= charge + 7200 && multiplier == 1)
			{
				multiplier = -1;
			}
			if (multiplier == -1 && Projectile.ai[1] <= 0)
				Projectile.Kill();

			if (!Main.projectile[(int)Projectile.ai[0]].active)
				Projectile.Kill();

			Projectile.Center = Main.projectile[(int)Projectile.ai[0]].Center;

			Projectile.gfxOffY = player.gfxOffY;

			Projectile.rotation = Projectile.velocity.ToRotation() - 1.57079637f;
			Projectile.velocity = Vector2.Normalize(Projectile.velocity);

			float[] sampleArray = new float[2];
			Collision.LaserScan(Projectile.Center, Projectile.velocity, 0, LaserLengthMax, sampleArray);
			float sampledLength = 0f;
			for (int i = 0; i < sampleArray.Length; i++)
			{
				sampledLength += sampleArray[i];
			}
			sampledLength /= sampleArray.Length;
			float amount = 0.75f; // last prism is 0.75 rather than 0.5?
			LaserLength = MathHelper.Lerp(LaserLength, sampledLength, amount);

			#region Dusts
			Vector2 endPoint = Projectile.Center + Projectile.velocity * (Projectile.localAI[1] - 14f);
			if (Main.rand.Next(5) == 0)
			{
				float num809 = Projectile.velocity.ToRotation() + ((Main.rand.Next(2) == 1) ? -1f : 1f) * 1.57079637f;
				float num810 = (float)Main.rand.NextDouble() * 2f + 2f;
				Vector2 vector79 = new Vector2((float)Math.Cos((double)num809) * num810, (float)Math.Sin((double)num809) * num810);
				int num811 = Dust.NewDust(endPoint, 0, 0, 90, vector79.X, vector79.Y, 0, default(Color), 1f);
				Main.dust[num811].noGravity = true;
				Main.dust[num811].scale = 1.7f;
			}
			if (Main.rand.Next(5) == 0)
			{
				Vector2 value29 = Projectile.velocity.RotatedBy(1.5707963705062866, default(Vector2)) * ((float)Main.rand.NextDouble() - 0.5f) * (float)Projectile.width;
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
			return (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.velocity * LaserLength, projHitbox.Width, ref collisionPoint));
		}

		public override bool? CanCutTiles()
		{
			DelegateMethods.tilecut_0 = Terraria.Enums.TileCuttingContext.AttackProjectile;
			Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * LaserLength, (float)Projectile.width * Projectile.scale * 2, new Utils.TileActionAttempt(CutTilesAndBreakWalls));
			return true;
		}

		private bool CutTilesAndBreakWalls(int x, int y)
		{
			return DelegateMethods.CutTiles(x, y);
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayerShaman modPlayer) { }

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (Projectile.velocity == Vector2.Zero) return true;

			Texture2D texture2D19 = TextureAssets.Projectile[Projectile.type].Value;
			Texture2D texture2D20 = ModContent.Request<Texture2D>("Shaman/Projectiles/Thorium/CoznixScepterProjLaser_Beam").Value;
			Texture2D texture2D21 = ModContent.Request<Texture2D>("Shaman/Projectiles/Thorium/CoznixScepterProjLaser_End").Value;
			float num228 = LaserLength;
			Color color44 = Color.White * 0.8f;
			Texture2D arg_AF99_1 = texture2D19;
			Vector2 arg_AF99_2 = Projectile.Center + new Vector2(0, Projectile.gfxOffY) - Main.screenPosition;
			Rectangle? sourceRectangle2 = null;
			spriteBatch.Draw(arg_AF99_1, arg_AF99_2, sourceRectangle2, color44, Projectile.rotation, texture2D19.Size() / 2f, new Vector2(Math.Min(Projectile.ai[1], charge) / charge, 1f), SpriteEffects.None, 0f);
			num228 -= (float)(texture2D19.Height / 2 + texture2D21.Height) * Projectile.scale;
			Vector2 value20 = Projectile.Center + new Vector2(0, Projectile.gfxOffY);
			value20 += Projectile.velocity * Projectile.scale * (float)texture2D19.Height / 2f;
			if (num228 > 0f)
			{
				float num229 = 0f;
				Microsoft.Xna.Framework.Rectangle rectangle7 = new Microsoft.Xna.Framework.Rectangle(0, 16 * (Projectile.timeLeft / 3 % 5), texture2D20.Width, 16);
				while (num229 + 1f < num228)
				{
					if (num228 - num229 < (float)rectangle7.Height)
					{
						rectangle7.Height = (int)(num228 - num229);
					}
					Main.spriteBatch.Draw(texture2D20, value20 - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(rectangle7), color44, Projectile.rotation, new Vector2((float)(rectangle7.Width / 2), 0f), new Vector2(Math.Min(Projectile.ai[1], charge) / charge, 1f), SpriteEffects.None, 0f);
					num229 += (float)rectangle7.Height * Projectile.scale;
					value20 += Projectile.velocity * (float)rectangle7.Height * Projectile.scale;
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
			arg_B1FF_0.Draw(arg_B1FF_1, arg_B1FF_2, sourceRectangle2, color44, Projectile.rotation, texture2D21.Frame(1, 1, 0, 0).Top(), new Vector2(Math.Min(Projectile.ai[1], charge) / charge, 1f), SpriteEffects.None, 0f);

			return true;
		}
	}
}
