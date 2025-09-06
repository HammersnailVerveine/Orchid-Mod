using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian
{
	public class GauntletPunchProjectile : OrchidModGuardianAnchor
	{
		private static Texture2D TextureMain;
		public OrchidModGuardianGauntlet GauntletItem;
		public bool ChargedHit => Projectile.ai[0] == 1f;
		public bool OffHand => Projectile.ai[1] == 1f;
		public bool FirstFrame = false;

		public override void Load()
		{
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 81;
			Projectile.tileCollide = false;
			Projectile.scale = 1f;
			Projectile.alpha = 96;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 3;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 90;
		}

		public override void AI()
		{

			Player owner = Main.player[Projectile.owner];
			if (!Initialized)
			{
				Initialized = true;
				Projectile.rotation = (Projectile.velocity - owner.velocity * 0.375f).ToRotation();
				if (ChargedHit) Strong = true;
				if (!IsLocalOwner)
				{
					foreach (Projectile projectile in Main.projectile)
					{ // This cannot be reliably synced with packets (?)
						if (projectile.ModProjectile is GuardianGauntletAnchor anchor && projectile.owner == Projectile.owner && projectile.active)
						{
							GauntletItem = anchor.GauntletItem.ModItem as OrchidModGuardianGauntlet;
						}
					}

					owner.GetModPlayer<OrchidGuardian>().GuardianGauntletCharge = 0; // probably not the best place to put this but it works. (fixes a minor visual issue)
					SoundEngine.PlaySound(ChargedHit ? SoundID.DD2_MonkStaffGroundMiss : SoundID.DD2_MonkStaffSwing, owner.Center);
				}
			}
			else
			{
				if (!FirstFrame)
				{
					FirstFrame = true;
					Projectile.position += Projectile.velocity * 0.5f;
					Projectile.width = 20;
					Projectile.height = 20;
					Projectile.position.X += 5;
					Projectile.position.Y += 5;
				}

				if (GauntletItem.ProjectileAI(owner, Projectile, ChargedHit))
				{
					Projectile.velocity *= 0.94574f;
				}
			}
		}

		public override void SafeModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			var owner = Main.player[Projectile.owner];
			GauntletItem.ModifyHitNPCGauntlet(owner, target, Projectile, ref modifiers, ChargedHit);
		}

		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidGuardian guardian)
		{
			var owner = Main.player[Projectile.owner];
			if (!owner.active || owner.dead || GauntletItem == null)
			{
				return;
			}
			else
			{
				if (FirstHit)
				{
					guardian.GuardianGuardRecharging += ChargedHit? 0.5f : 0.25f;
					GauntletItem.OnHitFirst(owner, guardian, target, Projectile, hit, ChargedHit);
				}
				GauntletItem.OnHit(owner, guardian, target, Projectile, hit, ChargedHit);
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			if (!FirstFrame) return false;

			var owner = Main.player[Projectile.owner];
			if (GauntletItem != null)
			{
				spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
				spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

				// Draw code here
				float colorMult = 0.8f;
				Vector2 offsetVector = new Vector2(0f, 12f).RotatedBy(Projectile.rotation - MathHelper.PiOver2);
				if (Projectile.timeLeft < 10) colorMult *= Projectile.timeLeft / 10f;
				SpriteEffects effect = SpriteEffects.None;
				if (Projectile.velocity.X < 0f) effect = SpriteEffects.FlipVertically;

				float scale = Projectile.scale * (ChargedHit ? 1.2f : 1f);
				Vector2 drawPosition = Projectile.Center - offsetVector - Main.screenPosition;
				spriteBatch.Draw(TextureMain, drawPosition, null, GauntletItem.GetColor(OffHand) * colorMult, Projectile.rotation, TextureMain.Size() * 0.5f, scale, effect, 0f);

				// Draw code ends here

				spriteBatch.End();
				spriteBatch.Begin(spriteBatchSnapshot);
			}
			return false;
		}
	}
}	