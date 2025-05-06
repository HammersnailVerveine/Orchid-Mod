using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Shapeshifter.Weapons.Warden;
using System.Collections.Generic;
using Terraria;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Warden
{
	public class WardenSlimeProj : OrchidModShapeshifterProjectile
	{
		private static Texture2D TextureMain;
		public List<float> OldAI;
		public Color drawColor;

		public override void SafeSetDefaults()
		{
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 2;
			Projectile.scale = 1f;
			Projectile.alpha = 96;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
			Projectile.friendly = false;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (IsLocalOwner)
			{  // Can't hit an enemy already hit by the falling player
				OrchidShapeshifter shapeshifter = Main.LocalPlayer.GetModPlayer<OrchidShapeshifter>();
				if (shapeshifter.IsShapeshifted)
				{
					if (shapeshifter.ShapeshiftAnchor.ShapeshifterItem.ModItem is WardenSlime slime)
					{
						if (slime.HitNPCs.Contains(target.whoAmI))
						{
							return false;
						}
					}
				}
			}

			return base.CanHitNPC(target);
		}

		public override void AI()
		{
			if (Projectile.ai[0] >= 2f)
			{ // full charge slam, big hitbox
				Projectile.width = 112;
				Projectile.height = 112;
				Projectile.ai[1] = 15f;
			} 
			else
			{ // normal hitbox
				Projectile.width = 80;
				Projectile.height = 80;
				Projectile.ai[1] = 4f * Projectile.ai[0];
			}

			Projectile.friendly = true;
			Projectile.position = Main.player[Projectile.owner].Center - new Vector2(Projectile.width, Projectile.height) * 0.5f;

			foreach (NPC npc in Main.npc)
			{ // yeet
				if (IsValidTarget(npc) && npc.Hitbox.Intersects(Projectile.Hitbox) && npc.knockBackResist > 0f)
				{
					npc.velocity = new Vector2(Projectile.ai[1] * (npc.Center.X > Projectile.Center.X ? 1 : -1), - Projectile.ai[1]) * npc.knockBackResist;
					npc.netUpdate = true;
				}
			}
		}
	}
}