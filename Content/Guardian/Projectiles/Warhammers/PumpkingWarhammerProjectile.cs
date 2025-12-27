using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Projectiles.Warhammers
{
	public class PumpkingWarhammerProjectile : OrchidModGuardianProjectile
	{
		float EmbeddedRotation = 0f;
		float EmbeddedRotation2 = 0f;

		public override void SafeSetDefaults()
		{
			Projectile.width = 46;
			Projectile.height = 46;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 600;
			Projectile.scale = 1f;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			behindNPCs.Add(index);
		}


		public override bool? CanHitNPC(NPC target)
		{
			if (target.whoAmI != (int)Projectile.ai[0] || Projectile.localAI[1] > 0 || Projectile.timeLeft > 580) return false;
			return base.CanHitNPC(target);
		}

		public override void AI()
		{
			NPC npc = Main.npc[(int)Projectile.ai[0]];

			if (Projectile.localAI[1] > 0)
			{
				Projectile.localAI[1]++;

				if (Projectile.localAI[1] > 25)
				{
					Projectile.Kill();
				}
			}

			if (!npc.active)
			{
				Projectile.Kill();
			}
			else
			{
				if (!Initialized)
				{
					Initialized = true;
					Vector2 offset = npc.Center - Projectile.Center;
					EmbeddedRotation = npc.rotation;
					if (offset.X < 0)
					{ // flip
						Projectile.localAI[0] = 1;
						Projectile.rotation = (npc.Center - Projectile.Center).ToRotation() + MathHelper.Pi * 0.25f;
					}
					else
					{
						Projectile.rotation = (npc.Center - Projectile.Center).ToRotation() - MathHelper.Pi * 0.25f;
					}

					EmbeddedRotation2 = Projectile.rotation;

					for (int i = 0; i < 5; i++)
					{
						Gore gore = Gore.NewGoreDirect(Projectile.GetSource_FromAI(), Projectile.Center + new Vector2(Main.rand.NextFloat(-8f, 0f), Main.rand.NextFloat(-8f, 0f)), Vector2.UnitY.RotatedByRandom(MathHelper.Pi), 99);
						gore.rotation = Main.rand.NextFloat(MathHelper.Pi);
						gore.scale *= Main.rand.NextFloat(0.7f, 1f);
					}

					for (int i = 0; i < 10; i++)
					{
						Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Wraith);
						dust.scale *= Main.rand.NextFloat(1f, 1.5f);
						dust.noGravity = true;
					}

					SoundEngine.PlaySound(SoundID.NPCDeath52.WithPitchOffset(0.5f), Projectile.Center);
				}

				Projectile.Center = npc.Center + new Vector2(Projectile.ai[1], Projectile.ai[2]).RotatedBy(npc.rotation - EmbeddedRotation + EmbeddedRotation2 * 0.1f);

				Projectile.rotation += 0.35f * (Projectile.localAI[0] == 1 ? -1 : 1);
				//Projectile.rotation = EmbeddedRotation2 + npc.rotation - EmbeddedRotation;
				EmbeddedRotation2 += (Projectile.localAI[0] == 1 ? -1 : 1);

				if (Main.rand.NextBool())
				{
					Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Wraith);
					dust.scale *= Main.rand.NextFloat(1f, 1.5f);
					dust.noGravity = true;

					if (Main.rand.NextBool(10))
					{
						Gore gore = Gore.NewGoreDirect(Projectile.GetSource_FromAI(), Projectile.Center + new Vector2(Main.rand.NextFloat(-8f, 0f), Main.rand.NextFloat(-8f, 0f)), Vector2.UnitY.RotatedByRandom(MathHelper.Pi), 99);
						gore.rotation = Main.rand.NextFloat(MathHelper.Pi);
						gore.scale *= Main.rand.NextFloat(0.4f, 0.7f);
					}
				}
			}
		}

		public override void OnKill(int timeLeft)
		{
			if (Projectile.localAI[1] == 0f)
			{
				for (int i = 0; i < 5; i++)
				{
					Gore gore = Gore.NewGoreDirect(Projectile.GetSource_FromAI(), Projectile.Center + new Vector2(Main.rand.NextFloat(-8f, 0f), Main.rand.NextFloat(-8f, 0f)), Vector2.UnitY.RotatedByRandom(MathHelper.Pi), 99);
					gore.rotation = Main.rand.NextFloat(MathHelper.Pi);
					gore.scale *= Main.rand.NextFloat(0.7f, 1f);
				}

				for (int i = 0; i < 10; i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Wraith);
					dust.scale *= Main.rand.NextFloat(1f, 1.5f);
					dust.noGravity = true;
				}

				NPC npc = Main.npc[(int)Projectile.ai[0]];
				
				if (Projectile.localAI[2] < 1f)
				{ // gives minimum 1 guard
					Projectile.localAI[2] = 1f;
				}

				if (npc.life <= 0)
				{ // gives 2 additional guards if the enemy died
					Projectile.localAI[2] += 2;
				}

				SoundEngine.PlaySound(SoundID.NPCHit54.WithPitchOffset(0.5f), Projectile.Center);
				Owner.GetModPlayer<OrchidGuardian>().AddGuard((int)Projectile.localAI[2]);
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			Color drawColor = lightColor * (1f - Projectile.localAI[1] * 0.04f);
			SpriteEffects effects = Projectile.localAI[0] == 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Texture2D projTexture = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawPosition = Projectile.Center - Main.screenPosition;
			spriteBatch.Draw(projTexture, drawPosition, null, drawColor, Projectile.rotation, projTexture.Size() * 0.5f, Projectile.scale, effects, 0f);
			return false;
		}
	}
}