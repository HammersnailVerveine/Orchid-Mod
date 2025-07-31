using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Shapeshifter.Weapons.Predator;
using OrchidMod.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Predator
{
	public class PredatorGoblinProjSlash : OrchidModShapeshifterProjectile
	{
		private static Texture2D TextureMain;

		public override void SafeSetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 18;
			Projectile.scale = 0.9f;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			MeleeHit = true;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (target.whoAmI != (int)Projectile.ai[1]) return false;
			return base.CanHitNPC(target);
		}

		public override void AI()
		{
			if (!Initialized)
			{
				Initialized = true;
				Projectile.rotation = Projectile.ai[0];
				Projectile.velocity = Projectile.rotation.ToRotationVector2() * 0.1f;

				OrchidShapeshifter shapeshifter = Owner.GetModPlayer<OrchidShapeshifter>();
				if (shapeshifter.IsShapeshifted)
				{
					if (shapeshifter.Shapeshift is PredatorUndine)
					{
						shapeshifter.ShapeshiftAnchor.ai[0] = 15;
					}
				}

				for (int i = 0; i < 5; i ++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.position - new Vector2(8f, 8f), Projectile.width + 16, Projectile.height + 16, DustID.Shadowflame);
					dust.velocity = Projectile.velocity * Main.rand.NextFloat(11f, 17f);
					dust.scale *= Main.rand.NextFloat(0.5f, 0.75f);
				}

				SoundStyle soundStyle = SoundID.NPCHit18;
				soundStyle.Pitch -= Main.rand.NextFloat(0.5f, 1f);
				SoundEngine.PlaySound(soundStyle, Projectile.Center);
			}

			if (Projectile.timeLeft % 3 == 0 && Projectile.frame < 4)
			{
				Projectile.frame++;
			}

			Projectile.friendly = Projectile.timeLeft > 12;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			float colorMult = 1f;
			if (Projectile.timeLeft < 7) colorMult *= Projectile.timeLeft / 7f;

			Rectangle rectangle = TextureMain.Bounds;
			rectangle.Height /= 5;
			rectangle.Y += rectangle.Height * Projectile.frame;

			Vector2 drawPosition = Projectile.Center - Main.screenPosition;
			spriteBatch.Draw(TextureMain, drawPosition, rectangle, Color.White * colorMult, Projectile.rotation, rectangle.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}

		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidShapeshifter shapeshifter)
		{
			target.AddBuff(BuffID.ShadowFlame, 180);

			if (shapeshifter.IsShapeshifted && target.life < 0)
			{
				if (shapeshifter.Shapeshift is PredatorGoblin && shapeshifter.ShapeshiftAnchor.Projectile.ai[2] < 100f)
				{
					Vector2 velocity = Vector2.Normalize(Projectile.Center - player.Center).RotatedByRandom(1f) * 9f;
					int type = ModContent.ProjectileType<PredatorGoblinProjResource>();
					float charge = target.lifeMax;
					ShapeshifterNewProjectile(Projectile.Center, velocity, type, 0, 0, 0, player.whoAmI, charge, charge >= 100f ? 1f : 0f);
				}
			}
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			target.AddBuff(BuffID.ShadowFlame, 180);
		}
	}
}