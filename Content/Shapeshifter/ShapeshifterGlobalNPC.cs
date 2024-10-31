﻿using Microsoft.Xna.Framework;
using OrchidMod.Content.Shapeshifter.Buffs;
using OrchidMod.Content.Shapeshifter.Projectiles.Warden;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter
{
	internal class ShapeshifterGlobalNPC : GlobalNPC
	{
		public bool SageOwlDebuff;
		public bool WardenSpiderDebuff;
		
		public override bool InstancePerEntity => true;
		public override void ResetEffects(NPC npc) {
			SageOwlDebuff = false;
			WardenSpiderDebuff = false;
		}

		public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers) {
			if (SageOwlDebuff) {
				modifiers.FlatBonusDamage += 3;
			}
		}

		public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
		{
			if (WardenSpiderDebuff)
			{ // Executes low health enemies
				OrchidShapeshifter shapeshifter = Main.player[projectile.owner].GetModPlayer<OrchidShapeshifter>();
				if (projectile.type != ModContent.ProjectileType<WardenSpiderWeb>() && projectile.ModProjectile is OrchidModShapeshifterProjectile && shapeshifter.IsShapeshifted)
				{ // Higher threshold for the spider attacks
					if (npc.life - (projectile.damage - npc.defense * 0.5f) < (projectile.type == ModContent.ProjectileType<WardenSpiderProj>() ? 100 : 75))
					{ // 999 damage
						modifiers.FlatBonusDamage += 100;
						modifiers.SetCrit();

						shapeshifter.modPlayer.TryHeal(10);
						shapeshifter.Player.AddBuff(ModContent.BuffType<WardenSpiderBuff>(), 1800);
					}
				}
			}
		}

		public override void DrawEffects(NPC npc, ref Color drawColor) {
			if (SageOwlDebuff) {
				drawColor.R = 0;
			}

			if (WardenSpiderDebuff)
			{
				drawColor = Color.Gray.MultiplyRGBA(drawColor);
			}
		}
	}
}
