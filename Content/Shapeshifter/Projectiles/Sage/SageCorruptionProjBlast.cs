using Microsoft.Xna.Framework;
using OrchidMod.Common.ModObjects;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Sage
{
	public class SageCorruptionProjBlast : OrchidModShapeshifterProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 100;
			Projectile.height = 100;
			Projectile.friendly = false;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 10;
			Projectile.scale = 1f;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
		}

		public override void AI()
		{
			if (!Initialized)
			{
				Initialized = true;

				int dustType1 = DustID.ToxicBubble;
				int dustType2 = DustID.CorruptGibs;
				int dustType3 = DustID.CorruptGibs;
				if (Projectile.ai[2] != 0)
				{ // crimson version
					dustType1 = DustID.Crimson;
					dustType2 = DustID.CrimsonPlants;
					dustType3 = 120;
				}

				for (int i = 0; i < 15; i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Smoke);
					dust.scale = Main.rand.NextFloat(1.5f, 2f);
					dust.noGravity = true;
					dust.velocity *= 0.5f;
					dust.velocity += Vector2.Normalize(Projectile.velocity).RotatedByRandom(MathHelper.ToRadians(40f)) * Main.rand.NextFloat(2f, 8f);
				}

				for (int i = 0; i < 10; i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, dustType3);
					dust.scale = Main.rand.NextFloat(1.5f, 2f);
					dust.noGravity = true;
					dust.velocity *= 0.5f;
					dust.velocity += Vector2.Normalize(Projectile.velocity).RotatedByRandom(MathHelper.ToRadians(30f)) * Main.rand.NextFloat(5f, 8f);
				}

				for (int i = 0; i < 10; i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, dustType2);
					dust.scale = Main.rand.NextFloat(1.5f, 2f);
					dust.noGravity = true;
					dust.velocity *= 0.5f;
					dust.velocity += Vector2.Normalize(Projectile.velocity).RotatedByRandom(MathHelper.ToRadians(30f)) * Main.rand.NextFloat(2f, 8f);
				}

				for (int i = 0; i < 15; i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, dustType1);
					dust.scale = Main.rand.NextFloat(1.5f, 2f);
					dust.noGravity = true;
					dust.velocity *= 0.5f;
					dust.velocity += Vector2.Normalize(Projectile.velocity).RotatedByRandom(MathHelper.ToRadians(40f)) * Main.rand.NextFloat(5f, 8f);
				}

				for (int i = 0; i < 10; i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, dustType1);
					dust.scale = Main.rand.NextFloat(0.5f, 0.75f);
					dust.velocity *= 0.5f;
					dust.velocity += Vector2.Normalize(Projectile.velocity).RotatedByRandom(MathHelper.ToRadians(30f)) * Main.rand.NextFloat(5f, 8f);
				}

				for (int i = 0; i < 5; i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, dustType3);
					dust.scale = Main.rand.NextFloat(1.5f, 2f);
					dust.noGravity = true;
					dust.velocity *= 0.5f;
					dust.velocity += Vector2.Normalize(Projectile.velocity).RotatedByRandom(MathHelper.ToRadians(20f)) * Main.rand.NextFloat(10f, 15f);
				}

				for (int i = 0; i < 5; i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, dustType1);
					dust.scale = Main.rand.NextFloat(1.5f, 2f);
					dust.noGravity = true;
					dust.velocity *= 0.5f;
					dust.velocity += Vector2.Normalize(Projectile.velocity).RotatedByRandom(MathHelper.ToRadians(20f)) * Main.rand.NextFloat(10f, 15f);
				}

				foreach(NPC npc in Main.npc)
				{
					if (npc.knockBackResist > 0f && IsValidTarget(npc) && npc.Hitbox.Intersects(Projectile.Hitbox))
					{
						npc.velocity = Vector2.Normalize(Projectile.velocity) * (12.5f + 7.5f * Projectile.ai[0]) * npc.knockBackResist;
						npc.velocity.Y -= 3f;
						npc.netUpdate = true;
					}
				}

				SoundEngine.PlaySound(SoundID.Item100, Projectile.Center);
				SoundEngine.PlaySound(SoundID.Item95, Projectile.Center);
				Projectile.friendly = true;
				Projectile.Center += Projectile.velocity * 48f;
				Projectile.velocity *= 0f;
			}
		}
	}
}