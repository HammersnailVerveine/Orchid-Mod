using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.Global.Projectiles;
using OrchidMod.Common.Global.NPCs;
using OrchidMod.Common.ModObjects;
using System;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Gambler
{
	public abstract class OrchidModGamblerProjectile : OrchidModProjectile
	{
		public bool bonusTrigger = false;

		public virtual void SafeAI() { }

		public virtual void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidGambler modPlayer) { }

		public virtual void BonusProjectiles(Player player, OrchidGambler modPlayer, Projectile projectile, OrchidGlobalProjectile modProjectile, bool dummy = false) { }

		public sealed override void AltSetDefaults()
		{
			OrchidGlobalProjectile modProjectile = Projectile.GetGlobalProjectile<OrchidGlobalProjectile>();
			Projectile.timeLeft = 1500;
			SafeSetDefaults();
			modProjectile.gamblerProjectile = true;
			modProjectile.gamblerBonusTrigger = this.bonusTrigger;
			modProjectile.gamblerBonusProjectilesDelegate = BonusProjectiles;
		}

		public override void AI()
		{
			this.SafeAI();
			OrchidGlobalProjectile modProjectile = Projectile.GetGlobalProjectile<OrchidGlobalProjectile>();
			modProjectile.gamblerInternalCooldown -= modProjectile.gamblerInternalCooldown > 0 ? 1 : 0;
		}

		public sealed override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			OrchidGlobalNPC modTarget = target.GetGlobalNPC<OrchidGlobalNPC>();
			if (target.type != NPCID.TargetDummy)
			{
				modPlayer.TryAddGamblerChip();
			}
			modTarget.GamblerHit = true;
			SafeOnHitNPC(target, hit.Damage, hit.Knockback, hit.Crit, player, modPlayer);
		}

		public void DrawOutline(Texture2D outline, SpriteBatch spriteBatch, Color lightColor)
		{
			float lightMult = 0.25f + Math.Abs((1f * Main.player[Main.myPlayer].GetModPlayer<OrchidPlayer>().Timer120 - 60) / 90f);
			spriteBatch.Draw(outline, Projectile.position - new Vector2(2, 2) - Main.screenPosition, null, lightColor * lightMult, Projectile.rotation, Vector2.Zero, Projectile.scale, SpriteEffects.None, 0f);
		}

		public bool getDummy()
		{
			return Projectile.GetGlobalProjectile<OrchidGlobalProjectile>().gamblerDummyProj;
		}
		
		public int getCardType(OrchidGambler modPlayer) {
			return Projectile.GetGlobalProjectile<OrchidGlobalProjectile>().gamblerDummyProj ? modPlayer.gamblerCardDummy.type : modPlayer.gamblerCardCurrent.type;
		}

		public int DummyProjectile(int projectile, bool dummy) => OrchidGambler.DummyProjectile(projectile, dummy);

		public bool homingCheckGambler(NPC target)
		{
			bool dummy = getDummy();
			return target.active && !target.dontTakeDamage && !target.friendly && target.lifeMax > 5 && ((!dummy && target.type != NPCID.TargetDummy) || (dummy && target.type == NPCID.TargetDummy));
		}
	}
}