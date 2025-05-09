using Microsoft.Xna.Framework;
using OrchidMod.Common;
using OrchidMod.Content.Shapeshifter.Buffs;
using OrchidMod.Content.Shapeshifter.Projectiles.Warden;
using OrchidMod.Content.Shapeshifter.Weapons.Sage;
using OrchidMod.Content.Shapeshifter.Weapons.Warden;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static System.Net.Mime.MediaTypeNames;

namespace OrchidMod.Content.Shapeshifter
{
	internal class ShapeshifterGlobalNPC : GlobalNPC
	{
		public bool SageOwlDebuff;
		public bool SageBatDebuff;
		public bool WardenSpiderDebuff;

		public int ShapeshifterBleed = 0; // Used by to count Shapshifter bleeds stacks. See PredatorFossil.ShapeshiftOnHitNPC() for sync Example
		public int ShapeshifterBleedTimer = 0; // Used to time Shapshifter bleeds
		public int ShapeshifterBleedPotency = 0; // Damage dealt by the active shapeshifter bleed

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

		public override void UpdateLifeRegen(NPC npc, ref int damage)
		{
			if (ShapeshifterBleedTimer > 0)
			{ // Used by the Predator Fossil for its bleeding effect on hit
				ShapeshifterBleedTimer--;

				if (ShapeshifterBleedTimer <= 0)
				{
					ShapeshifterBleed = 0;
				}

				if (npc.lifeRegen > 0)
				{
					npc.lifeRegen = 0;
				}

				npc.lifeRegen -= ShapeshifterBleed * 2 * ShapeshifterBleedPotency;
				if (damage < ShapeshifterBleed * ShapeshifterBleedPotency)
				{
					damage = ShapeshifterBleed * ShapeshifterBleedPotency;
				}

				if (Main.rand.NextBool(66 - ShapeshifterBleed * 6))
				{
					Main.dust[Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood)].velocity *= 0.75f;
				}
			}
			else
			{
				ShapeshifterBleedPotency = 0;
				ShapeshifterBleed = 0;
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

		public override void OnKill(NPC npc)
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				OnKillShapeshifterGlobalNPC(npc);
			}
			else
			{ // Sending a packet for effects that require the player to be aware of the NPC death
				var packet = OrchidMod.Instance.GetPacket();
				packet.Write((byte)OrchidModMessageType.SYNCONKILLNPC);
				packet.Write(npc.whoAmI);
				packet.Send();
			}
		}

		public void OnKillShapeshifterGlobalNPC(NPC npc)
		{
			if (!npc.friendly && !npc.CountsAsACritter && npc.aiStyle != NPCAIStyleID.Spell)
			{
				OrchidShapeshifter shapeshifter = Main.LocalPlayer.GetModPlayer<OrchidShapeshifter>();

				if (shapeshifter.IsShapeshifted)
				{
					if (shapeshifter.Shapeshift is SageImp impItem && npc.Center.Distance(shapeshifter.Player.Center) < 800f)
					{ // Imp attack speed buff when a nearby enemy dies
						impItem.FastAttackTimer = 300;
						impItem.FastAttack += 3;
						if (impItem.FastAttack > 9)
						{
							impItem.FastAttack = 9;
						}
					}

					if (shapeshifter.Shapeshift is WardenEater eaterItem)
					{ // Eater spawning a projectile going towards its closest fruit when a nearby enemy dies
						float distanceMax = 400f;
						int target = -1;
						foreach (Projectile projectile in Main.projectile)
						{
							float distance = npc.Center.Distance(projectile.Center);
							if (projectile.type == ModContent.ProjectileType<WardenEaterProjAlt>() && projectile.owner == shapeshifter.Player.whoAmI && distance < distanceMax && projectile.frame == 0)
							{
								distanceMax = distance;
								target = projectile.whoAmI;
							}
						}

						if (target >= 0)
						{
							int projectileType = ModContent.ProjectileType<WardenEaterProjAltDeath>();
							Projectile newProjectile = Projectile.NewProjectileDirect(shapeshifter.Player.GetSource_ItemUse(shapeshifter.Shapeshift.Item), npc.Center, Vector2.Zero, projectileType, 0, 0f, shapeshifter.Player.whoAmI, ai0: target);
						}
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
