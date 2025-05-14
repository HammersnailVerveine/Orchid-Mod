using Microsoft.Xna.Framework;
using OrchidMod.Common;
using OrchidMod.Common.Global.NPCs;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Shapeshifter.Weapons.Predator;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter
{
	public abstract class OrchidModShapeshifterProjectile : OrchidModProjectile
	{
		public ShapeshifterShapeshiftType ShapeshifterShapeshiftType;
		public virtual void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidShapeshifter shapeshifter) { }
		public virtual void SafeModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) { }

		public sealed override void AltSetDefaults()
		{
			Projectile.DamageType = ModContent.GetInstance<ShapeshifterDamageClass>();
			ShapeshifterShapeshiftType = ShapeshifterShapeshiftType.None;
			SafeSetDefaults();
		}

		public void ShapeshiftApplyBleed(NPC target, Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter, int timer, int potency, int maxCount, bool isGeneral = false)
		{ // Applies the bleed while in singleplayer, sends a packet for it while on a server
			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				ShapeshifterGlobalNPC globalNPC = target.GetGlobalNPC<ShapeshifterGlobalNPC>();
				ShapeshifterBleed bleed = globalNPC.ApplyBleed(player.whoAmI, timer, potency, maxCount, isGeneral);

				if (shapeshifter.IsShapeshifted)
				{
					shapeshifter.Shapeshift.ShapeshiftOnApplyBleed(target, projectile, anchor, player, shapeshifter, bleed);
				}
			}
			else
			{
				var packet = OrchidMod.Instance.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAPESHIFTERAPPLYBLEEDTONPC);
				packet.Write(player.whoAmI);
				packet.Write(target.whoAmI);
				packet.Write(potency);
				packet.Write(maxCount);
				packet.Write(timer);
				packet.Write(isGeneral);
				packet.Send();
			}
		}


		public sealed override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			OrchidShapeshifter shapeshifter = player.GetModPlayer<OrchidShapeshifter>();

			if (ShapeshifterShapeshiftType == ShapeshifterShapeshiftType.Predator)
			{
				if (shapeshifter.IsShapeshifted && shapeshifter.ShapeshifterPredatorBleedPotency > 0)
				{ // Applies bleeds on hit from predator attacks
					ShapeshifterShapeshiftAnchor anchor = shapeshifter.ShapeshiftAnchor;
					int potency = shapeshifter.ShapeshifterPredatorBleedPotency;
					int maxStacks = shapeshifter.ShapeshifterPredatorBleedMaxStacks;
					shapeshifter.Shapeshift.ShapeshiftApplyBleed(target, anchor.Projectile, anchor, player, shapeshifter, 900, potency, maxStacks, true);
				}
			}

			if (ShapeshifterShapeshiftType == ShapeshifterShapeshiftType.Sage)
			{
				if (shapeshifter.ShapeshifterSageDamageOnHit && IsValidTarget(target))
				{ // applies stacks increasing shapeshifter damage% for each unique enemy hit
					if (shapeshifter.ShapeshifterSageDamageOnHitCount < 10 && !shapeshifter.ShapeshifterSageDamageOnHitTargets.Contains(target.whoAmI))
					{
						shapeshifter.ShapeshifterSageDamageOnHitTargets[shapeshifter.ShapeshifterSageDamageOnHitCount] = target.whoAmI;
						shapeshifter.ShapeshifterSageDamageOnHitCount++;

						Rectangle hitbox = player.Hitbox;
						hitbox.Y -= 16;
						bool dramatic = false;

						if (shapeshifter.ShapeshifterSageDamageOnHitCount == 10)
						{
							dramatic = true;
							SoundEngine.PlaySound(SoundID.Item35, player.Center);
						}

						CombatText.NewText(hitbox, new Color(28, 216, 109), shapeshifter.ShapeshifterSageDamageOnHitCount, dramatic, true);
					}

					shapeshifter.ShapeshifterSageDamageOnHitTimer = 600;
				}
			}

			SafeOnHitNPC(target, hit, damageDone, player, shapeshifter);
		}

		public sealed override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			SafeModifyHitNPC(target, ref modifiers);
			OrchidGlobalNPC modTarget = target.GetGlobalNPC<OrchidGlobalNPC>();
			if (!modTarget.ShapeshifterHit)
			{
				modTarget.ShapeshifterHit = true;

				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					var packet = Mod.GetPacket();
					packet.Write((byte)OrchidModMessageType.NPCHITBYCLASS);
					packet.Write((byte)target.whoAmI);
					packet.Write((byte)4);
					packet.Send();
				}
			}
		}
	}
}