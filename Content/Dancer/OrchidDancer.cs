using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Alchemist;
using OrchidMod.Content.Dancer;
using OrchidMod.Content.Gambler;
using OrchidMod.Content.Shaman;
using OrchidMod.Content.Guardian;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;
using OrchidMod.Common;

namespace OrchidMod
{
	public class OrchidDancer : ModPlayer
	{
		public int dancerPoise = 0;
		public int dancerPoiseConsume = 0;
		public int dancerPoiseMax = 20;
		public int dancerWeaponDamage = 0;
		public float dancerWeaponKnockback = 0f;
		public OrchidModDancerItemType dancerWeaponType = OrchidModDancerItemType.NULL;
		public int dancerDashTimer = 0;
		public int dancerInvincibility = 0;
		public Vector2 dancerVelocity = new Vector2(0f, 0f);



		public override void PostUpdateEquips()
		{
			if (dancerVelocity.X != 0f || dancerVelocity.Y != 0f)
			{
				Player.velocity *= 0f;

				int Height = !Player.onTrack ? Player.height : Player.height - 20;
				dancerVelocity = Collision.TileCollision(Player.position, dancerVelocity, Player.width, Height, true, false, (int)Player.gravDir);

				Player.position.X += dancerVelocity.X;
				Player.position.Y += dancerVelocity.Y;
				// Player.position = Vector2.op_Addition(player.position, currentVelocity);
			}
		}

		/*
		public override bool ImmuneTo(PlayerDeathReason damageSource, int cooldownCounter, bool dodgeable)
		{
			if (dancerInvincibility > 0)
			{
				return false;
			}
			return true;
		}
		*/

		public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
		{
			if (dancerWeaponType != OrchidModDancerItemType.NULL)
			{
				if (!(npc.boss || npc.type == NPCID.TargetDummy) && npc.knockBackResist > 0f)
				{
					npc.velocity.X = Player.velocity.X * dancerWeaponKnockback * npc.knockBackResist / 5;
					npc.velocity.Y = Player.velocity.Y * dancerWeaponKnockback * npc.knockBackResist / 5;
				}
				Player.ApplyDamageToNPC(npc, dancerWeaponDamage, 0f, Player.direction);

				switch (dancerWeaponType)
				{
					case OrchidModDancerItemType.IMPACT:
						Player.velocity = dancerVelocity * -0.5f;
						ClearDancerEffects();
						break;
					case OrchidModDancerItemType.PHASE:
						break;
					case OrchidModDancerItemType.MOMENTUM:
						Player.velocity = dancerVelocity * -1f;
						break;
					default:
						break;
				}

				modifiers.FinalDamage *= 0f;
				dancerInvincibility = 10;
			}
		}

		public override void ResetEffects()
		{
			dancerPoiseMax = 20;
			dancerPoiseConsume = 0;
			dancerInvincibility -= dancerInvincibility > 0 ? 1 : 0;

			if (dancerDashTimer > 0)
			{
				dancerDashTimer--;
				if (dancerDashTimer == 0)
				{
					Player.velocity *= 0f;
					ClearDancerEffects();
				}
			}
		}
		public void ClearDancerEffects()
		{
			dancerDashTimer = 0;
			dancerWeaponDamage = 0;
			dancerWeaponKnockback = 0f;
			dancerWeaponType = OrchidModDancerItemType.NULL;
			dancerVelocity = new Vector2(0f, 0f);
		}

		public void RemoveDancerPoise(int chance, int number)
		{
			for (int i = 0; i < number; i++)
			{
				if (Main.rand.Next(100) < (chance - dancerPoiseConsume))
				{
					dancerPoise--;
				}
			}
		}
	}
}
