using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons
{
    public class SurfaceFireShamanWeapon : ShamanWeaponItem
    {
		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.damage = 10;
			Item.knockBack = 1;
			Item.shoot = ModContent.ProjectileType<SurfaceFireShamanWeaponAnchor>();
			FireElement = true;
		}
	}

    public class SurfaceFireShamanWeaponAnchor : FireShamanWeaponAnchor
    {
		public override void SafeSetDefaults()
		{
			MoveSpeed = 5;
			AttackRate = 10;
			TargetRange = 400;
		}
		
		bool flip = false;

		public override bool StartAttackAnimation()
		{
			AttackAnimation = 60;
			TargetCooldown = 60;
			AttackCooldown = 10;
			flip = !flip;
			return false;
		}

		public override void Attack()
		{
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + new Vector2(32, 0).RotatedBy(Projectile.rotation), new Vector2(10, 0).RotatedBy(Projectile.rotation), ModContent.ProjectileType<SurfaceFireShamanProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner, CurrentTarget.X, CurrentTarget.Y, AttackAnimation switch {<= 30 => 48, <= 40 => 32, _ => 16});
			if (AttackAnimation <= 30) AttackCooldown = 30;
		}

		public override void Activate()
		{
			for (int i = 0; i < 20; i++) Dust.NewDustPerfect(Projectile.Center, DustID.Torch).noGravity = true;
		}

		public override void ExtraAIActive()
		{
			Projectile.frameCounter++;
			int animRate = AttackAnimation switch
			{
				0 => 8,
				< 10 => 12,
				< 25 or > 55 => 8,
				_ => 4
			};
			if (Projectile.frameCounter >= animRate)
			{
				Projectile.frame = (Projectile.frame + 1) % 24; //its dumb but the main body has 8 frames and the top has 3 frames so 
				Projectile.frameCounter = 0;
			}
		}

		int randomFlamePos;

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
            if (NotActivated) VisualOffset = GetInactiveAnimation();
			else
			{
				Vector2 visualTargetPosition = Projectile.Center;
				if (AttackAnimation > 0)
				{
					Vector2 offs = CurrentTarget - Projectile.Center;
					float swinginess = Math.Clamp(offs.LengthSquared() * 0.00004f + 0.5f, 1, 2);
					Projectile.rotation = offs.ToRotation() + (float)Math.Cos(AttackAnimation * 0.053f - 0.602f) * (flip ? swinginess : -swinginess);
					visualTargetPosition += new Vector2(24, 0).RotatedBy(Projectile.rotation);
				}
				VisualCenter = GetFloatAnimation(visualTargetPosition, OldVisualCenter);
			}
			Color backColor = lightColor * 0.6f;
			backColor.A = lightColor.A;
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			//back layer
            spriteBatch.Draw(texture, VisualCenter - Main.screenPosition, new Rectangle(Projectile.frame % 4 * 16, 14 - Projectile.frame / 4 % 2 * 14, 14, 12), backColor, 0, new Vector2(7, 4), Projectile.scale, SpriteEffects.FlipHorizontally, 0);
			//flame
			if (!NotActivated)
			{
				Color fireColor = Color.White with {A = 0};
				fireColor *= 0.5f;
				int frameY = Projectile.frame % 2 == 0 ? 0 : 12;
				spriteBatch.Draw(texture, VisualCenter - Main.screenPosition, new Rectangle(64, frameY, 6, 10), fireColor, 0, new Vector2(3, 4), Projectile.scale, Projectile.frame / 2 % 2 == 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
				//random flames like vanilla torches
				fireColor *= 0.5f;
				if (Main.rand.NextBool(6)) randomFlamePos = randomFlamePos / 25 + Main.rand.Next(25) * 25; //random offset in X and Y of -2 to 2, so 25 possible states, per flame. don't ask me why i overcomplicated this it just annoyed me to define two vector2s for a range so tiny
				spriteBatch.Draw(texture, VisualCenter + new Vector2(2 - randomFlamePos % 5, 2 - randomFlamePos / 5 % 5) - Main.screenPosition, new Rectangle(64, frameY, 6, 10), fireColor, 0, new Vector2(3, 4), Projectile.scale, Projectile.frame / 2 % 2 == 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
				spriteBatch.Draw(texture, VisualCenter + new Vector2(2 - randomFlamePos / 25 % 5, 2 - randomFlamePos / 125) - Main.screenPosition, new Rectangle(64, frameY, 6, 10), fireColor, 0, new Vector2(3, 4), Projectile.scale, Projectile.frame / 2 % 2 == 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
			}
			//front layer
			spriteBatch.Draw(texture, VisualCenter - Main.screenPosition, new Rectangle(Projectile.frame % 4 * 16, Projectile.frame / 4 % 2 * 14, 14, 12), lightColor, 0, new Vector2(7, 4), Projectile.scale, SpriteEffects.None, 0);
			//top
			spriteBatch.Draw(texture, VisualCenter - Main.screenPosition, new Rectangle(Projectile.frame % 3 * 20, 28, 18, 14), lightColor, 0, new Vector2(9, 14), Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}

	public class SurfaceFireShamanProjectile: OrchidModShamanProjectile
	{
		public override string Texture => $"Terraria/Images/Projectile_15";

		public override void SafeSetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 60;
			Projectile.hide = true;
			Projectile.penetrate = -1;
		}

		Vector2 target = Vector2.Zero;
		float startingDistance;
		float startingRotation;

		public override void AI()
		{
			if (target == Vector2.Zero)
			{
				target = new Vector2(Projectile.ai[0], Projectile.ai[1]);
				Vector2 offs = target - Projectile.Center; 
				startingDistance = offs.Length();
				startingRotation = offs.ToRotation();
				Projectile.rotation = MathHelper.WrapAngle(offs.ToRotation() - Projectile.velocity.ToRotation()) * 1.75f + offs.ToRotation();
				Projectile.velocity = Vector2.Zero;
				Projectile.oldPosition = Projectile.position; //fixes a visual issue when the projectile first spawns
			}
			if (Projectile.timeLeft == 1)
			{
				for (int i = 0; i < Projectile.ai[2]; i++)
				{
					Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Torch, Scale: Main.rand.NextFloat());
					if (!Main.rand.NextBool(3))
					{
						dust.fadeIn = Main.rand.NextFloat();
						dust.scale *= 1 + Projectile.ai[2] * 0.055f;
						dust.noGravity = true;
						dust.velocity.Y += 0.2f;
						dust.velocity *= 1 + Projectile.ai[2] * 0.11f;
						dust.velocity.X *= 1.2f;
					}
					else dust.velocity *= 1.5f;
				}
			}
			else if (Projectile.timeLeft >= 30)
			{
				float progress = Projectile.timeLeft / 30f - 1f;
				progress *= progress;
				Projectile.Center = target - new Vector2(MathHelper.Lerp(45f, startingDistance, progress), 0).RotatedBy(MathHelper.Lerp(Projectile.rotation, startingRotation, progress));
				Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Torch);
				dust.noGravity = true;
				dust.noLight = true;
				dust.velocity *= 0.15f;
				dust = Dust.NewDustPerfect(Projectile.Center, DustID.Torch);
				dust.noGravity = true;
				dust.noLight = true;
				dust.velocity *= 0.15f;
				dust.position += (Projectile.position - Projectile.oldPosition) * 0.5f;
				if (Projectile.timeLeft == 30)
				{
					Projectile.friendly = true;
					Projectile.velocity = new Vector2(1.5f, 0).RotatedBy(Projectile.rotation);
					for (int i = 0; i < 10; i++)
					{
						dust = Dust.NewDustPerfect(Projectile.Center, DustID.Torch, Scale: Main.rand.NextFloat(1f, 2f));
						dust.noGravity = true;
						dust.fadeIn += Main.rand.NextFloat(1f);
						dust.velocity *= 1.2f;
					}
				}
			}
			else
			{
				Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Torch, Scale: 2f);
				dust.noGravity = true;
				dust.velocity *= 0.1f;
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Projectile.timeLeft > 2)
				Projectile.timeLeft = 2;
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			if (Projectile.timeLeft == 1)
			{
				int size = (int)Projectile.ai[2];
				hitbox.X -= size;
				hitbox.Y -= size;
				hitbox.Width += size * 2;
				hitbox.Height += size * 2;
			}
		}
	}
}