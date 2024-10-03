using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian
{
	public class GauntletPunchProjectile : OrchidModGuardianProjectile
	{
		private static Texture2D TextureMain;
		public OrchidModGuardianGauntlet GauntletItem;
		public bool ChargedHit => Projectile.ai[0] == 1f;
		public bool OffHand => Projectile.ai[1] == 1f;

		public bool FirstHit = false;
		public bool Initialized = false;

		public override void Load()
		{
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
		}

		public override void AltSetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 20;
			Projectile.tileCollide = false;
			Projectile.scale = 1f;
			Projectile.alpha = 96;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
		}

		public override void AI()
		{

			Player owner = Main.player[Projectile.owner];
			if (!Initialized)
			{
				Initialized = true;
				Projectile.rotation = (Projectile.velocity - owner.velocity * 1.5f).ToRotation();

				if (!IsLocalOwner)
				{
					foreach (Projectile projectile in Main.projectile)
					{ // This cannot be reliably synced with packets (?)
						if (projectile.ModProjectile is GuardianGauntletAnchor anchor && projectile.owner == Projectile.owner)
						{
							GauntletItem = anchor.GauntletItem.ModItem as OrchidModGuardianGauntlet;
						}
					}

					owner.GetModPlayer<OrchidGuardian>().GuardianGauntletCharge = 0; // probably not the best place to put this but it works. (fixes a minor visual issue)
					SoundEngine.PlaySound(ChargedHit ? SoundID.DD2_MonkStaffGroundMiss : SoundID.DD2_MonkStaffSwing, owner.Center);
				}
			}


			if (GauntletItem.ProjectileAI(owner, Projectile, ChargedHit))
			{
				Projectile.velocity *= 0.8f;
			}
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
				if (!FirstHit)
				{
					FirstHit = true;
					GauntletItem.OnHitFirst(owner, guardian, target, Projectile, hit, ChargedHit);
				}
				GauntletItem.OnHit(owner, guardian, target, Projectile, hit, ChargedHit);
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

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
				Vector2 drawPosition = Vector2.Transform(Projectile.Center - offsetVector - Main.screenPosition, Main.GameViewMatrix.EffectMatrix);
				spriteBatch.Draw(TextureMain, drawPosition, null, GauntletItem.GetColor(OffHand) * colorMult, Projectile.rotation, TextureMain.Size() * 0.5f, scale, effect, 0f);

				// Draw code ends here

				spriteBatch.End();
				spriteBatch.Begin(spriteBatchSnapshot);
			}
			return false;
		}
	}
}	