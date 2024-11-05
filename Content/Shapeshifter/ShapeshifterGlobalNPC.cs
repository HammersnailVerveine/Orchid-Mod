using Microsoft.Xna.Framework;
using OrchidMod.Content.Shapeshifter.Buffs;
using OrchidMod.Content.Shapeshifter.Projectiles.Warden;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter
{
	internal class ShapeshifterGlobalNPC : GlobalNPC
	{
		public bool SageOwlDebuff;
		public bool SageBatDebuff;
		public bool WardenSpiderDebuff;
		
		public override bool InstancePerEntity => true;
		public override void ResetEffects(NPC npc) {
			SageOwlDebuff = false;
			SageBatDebuff = false;
			WardenSpiderDebuff = false;
		}

		public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers) {
			if (SageOwlDebuff) {
				modifiers.FlatBonusDamage += 3;
			}
		}

		public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
		{
			if (WardenSpiderDebuff && !npc.SpawnedFromStatue && npc.type != NPCID.Bee)
			{ // Executes low health enemies
				OrchidShapeshifter shapeshifter = Main.player[projectile.owner].GetModPlayer<OrchidShapeshifter>();
				if (projectile.type != ModContent.ProjectileType<WardenSpiderWeb>() && projectile.DamageType == ModContent.GetInstance<ShapeshifterDamageClass>() && shapeshifter.IsShapeshifted)
				{ // Higher threshold for the spider attacks
					bool spiderMelee = projectile.type == ModContent.ProjectileType<WardenSpiderProj>();
					if (npc.life - (projectile.damage - npc.defense * 0.5f) < (spiderMelee ? 100 : 75))
					{ // 999 damage
						modifiers.FlatBonusDamage += 100;
						modifiers.SetCrit();

						shapeshifter.modPlayer.TryHeal(spiderMelee ? 10 : 5);
						shapeshifter.Player.AddBuff(ModContent.BuffType<WardenSpiderBuff>(), 1800);
					}
				}
			}
		}

		public override void DrawEffects(NPC npc, ref Color drawColor) {
			if (SageOwlDebuff) {
				drawColor.R = 0;
			}

			if (SageBatDebuff) {
				drawColor.B = 0;
				if (drawColor.R < 128) drawColor.R = 128;
				if (drawColor.G < 128) drawColor.G = 128;
			}

			if (WardenSpiderDebuff)
			{
				drawColor = Color.Gray.MultiplyRGBA(drawColor);
			}
		}
	}
}
