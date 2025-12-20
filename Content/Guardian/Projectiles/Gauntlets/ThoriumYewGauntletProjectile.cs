using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Guardian.Weapons.Gauntlets;
using OrchidMod.Content.Shapeshifter.Accessories;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Projectiles.Gauntlets
{
	public class ThoriumYewGauntletProjectile : OrchidModGuardianProjectile
	{
		Vector2 InitialVelocity = Vector2.Zero;
		Vector2 NPCImpactPoint = Vector2.Zero;
		Vector2 NPCImpactVelocity = Vector2.Zero;
		int TimeSpent = 0;
		bool Flip = false;
		bool QuickPull = false;

		public override void SafeSetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 120;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.alpha = 255;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (Projectile.ai[1] > -1 || TimeSpent > 25)
			{
				return false;
			}

			return base.CanHitNPC(target);
		}

		public override void AI()
		{
			TimeSpent++;

			if (!Initialized)
			{
				Initialized = true;
				InitialVelocity = Projectile.velocity;
				Flip = Projectile.velocity.X < 0;
				SoundEngine.PlaySound(SoundID.DD2_BallistaTowerShot.WithPitchOffset(0.5f), Projectile.Center);
			}

			Vector2 target = Owner.Center;
			Projectile gauntlet = Main.projectile[(int)Projectile.ai[2]];
			if (gauntlet.active && gauntlet.ModProjectile is GuardianGauntletAnchor && gauntlet.owner == Owner.whoAmI)
			{
				target = gauntlet.Center;
			}
			else if (IsLocalOwner)
			{
				Projectile.Kill();
			}

			if (QuickPull)
			{ // this field exists for neysync purposes
				QuickPull = false;
				Owner.velocity = Vector2.Normalize(Projectile.Center - Owner.MountedCenter) * 15f;
			}

			if (Projectile.ai[1] >= 0)
			{
				Projectile.tileCollide = false;
				NPC npc = Main.npc[(int)Projectile.ai[1]];
				if (npc.active && !npc.friendly && npc.life > 0)
				{
					if (NPCImpactPoint == Vector2.Zero)
					{
						SoundEngine.PlaySound(SoundID.DD2_JavelinThrowersAttack, Projectile.Center);
						NPCImpactPoint = Projectile.Center - npc.Center;
						NPCImpactVelocity = Projectile.velocity;
						Projectile.timeLeft = 60;
						Projectile.friendly = false;

						if (IsLocalOwner)
						{
							Main.SetCameraLerp(0.1f, 10);
						}
					}

					Projectile.Center = NPCImpactPoint - NPCImpactVelocity + npc.Center;
					NPCImpactVelocity *= 0.75f;
					Owner.RemoveAllGrapplingHooks();

					Owner.velocity = Vector2.Normalize(Projectile.Center - Owner.MountedCenter) * 15f;

					if (Owner.Center.Distance(Projectile.Center) < 32f)
					{
						Projectile.ai[1] = -1;
						TimeSpent = 35;
						Projectile.velocity = Vector2.Normalize(target - Projectile.Center) * 15f;
						Owner.velocity.X *= 0.5f;
						Owner.velocity.Y *= 0.75f;
					}
				}
				else
				{
					Projectile.ai[1] = -1;
					TimeSpent = 35;
					Projectile.velocity = Vector2.Normalize(target - Projectile.Center) * 15f;
				}
			}
			else if (TimeSpent > 20)
			{
				if (TimeSpent <= 35)
				{
					Projectile.velocity -= InitialVelocity * (TimeSpent - 20) * 0.02f;
				}
				else
				{
					Projectile.velocity = Vector2.Normalize(target - Projectile.Center) * Projectile.velocity.Length();
				}

				if (target.Distance(Projectile.Center) < 32f)
				{
					Projectile.Kill();
				}
			}
			else
			{
				Projectile.rotation = Projectile.velocity.ToRotation();
			}
		}

		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidGuardian guardian)
		{
			if (Projectile.timeLeft > 20 && !target.CountsAsACritter)
			{
				if (target.life > 0)
				{
					Projectile.ai[1] = target.whoAmI;
					Projectile.netUpdate = true;
					guardian.AddGuard();
				}
				else if (Owner.HeldItem.ModItem is ThoriumYewGauntlet gauntlet && gauntlet.PullOnKill)
				{
					QuickPull = true;
					Projectile.netUpdate = true;
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (TimeSpent < 35)
			{
				TimeSpent = 35;
				SoundEngine.PlaySound(SoundID.Dig, Projectile.position);

				Vector2 target = Owner.Center;
				Projectile gauntlet = Main.projectile[(int)Projectile.ai[2]];
				if (gauntlet.active && gauntlet.ModProjectile is GuardianGauntletAnchor && gauntlet.owner == Owner.whoAmI)
				{
					target = gauntlet.Center;
				}
				else if (IsLocalOwner)
				{
					Projectile.Kill();
				}

				Projectile.velocity = Vector2.Normalize(target - Projectile.Center) * 15f;
			}

			return false;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(QuickPull);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			QuickPull = reader.ReadBoolean();
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Flip)
			{
				spriteEffects = SpriteEffects.FlipVertically;
			}

			Texture2D projTexture = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawPosition = Projectile.Center - Main.screenPosition;
			spriteBatch.Draw(projTexture, drawPosition, null, lightColor, Projectile.rotation, projTexture.Size() * 0.5f, Projectile.scale, spriteEffects, 0f);

			return false;
		}
	}
}