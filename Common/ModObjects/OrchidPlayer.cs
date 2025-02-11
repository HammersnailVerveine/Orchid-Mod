using System.Collections.Generic;
using Microsoft.Xna.Framework;
using OrchidMod.Content.General.Projectiles;
using OrchidMod.Content.Guardian.Weapons.Shields;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Common.ModObjects
{
	public class OrchidPlayer : ModPlayer
	{
		public OrchidShaman modPlayerShaman;
		public OrchidAlchemist modPlayerAlchemist;
		public OrchidGambler modPlayerGambler;
		public OrchidDancer modPlayerDancer;
		public OrchidGuardian modPlayerGuardian;
		public OrchidShapeshifter modPlayerShapeshifter;

		public int Timer120 = 0; // Used for various AIs. I'll eventually get rid of this
		// public int doubleTap = 0;
		// public int doubleTapCooldown = 0;
		// public bool doubleTapLock = false;

		// Gameplay Fields

		public int Timer = 0; // Used for various AIs. Increased by 1 every frame
		public int keepSelected = -1;
		public int originalSelectedItem;
		public bool autoRevertSelectedItem = false;
		public int PlayerImmunity = 0; // Player is immune if this is >0
		public Vector2 ForcedVelocityVector = Vector2.Zero; // vector the player will be moved every frame if ForcedVelocityTimer > 0, ignoring normal velocity
		public float ForcedVelocityUpkeep = 0f; // Should the forced velocity be applied to the player velocity when it ends
		public int ForcedVelocityTimer = 0; // How long should the forced velocity be kept
		public bool OrchidDoubleDash = false;
		public int OrchidDoubleDashCD = 0;
		/// <summary> Set to 15 after a tap, decremented every frame. Registers a double tap and resets to 0 if another tap is input while above 0.</summary>
		/// <remarks> Up = 0, Right = 1, Down = 2, Left = 3</remarks>
		public int[] DoubleTapping = new int[4]; 
		/// <summary> Set to 15 after a double tap, decremented every frame.</summary>
		/// <remarks> Up = 0, Right = 1, Down = 2, Left = 3</remarks>
		public int[] DoubleTapped = new int[4];
		/// <summary> The player has double tapped Up on this frame.</summary>
		public bool DoubleTapUp => DoubleTapped[0] == 15;
		/// <summary> The player has double tapped Down on this frame.</summary>
		public bool DoubleTapDown => DoubleTapped[1] == 15;
		/// <summary> The player has double tapped Down on this frame.</summary>
		public bool DoubleTapRight => DoubleTapped[2] == 15;
		/// <summary> The player has double tapped Right on this frame.</summary>
		public bool DoubleTapLeft => DoubleTapped[3] == 15;
		/// <summary> The player has double tapped their Set Bonus key on this frame.</summary>
		public bool DoubleTapSetBonus => (DoubleTapDown && !Main.ReversedUpDownArmorSetBonuses) || (DoubleTapUp && Main.ReversedUpDownArmorSetBonuses);
		/// <summary> The player has double tapped a direction on this frame.</summary>
		public bool DoubleTapAny => DoubleTapUp || DoubleTapDown || DoubleTapRight || DoubleTapLeft; 
		/// <summary> Set to 15 after a double tap. Decremented every frame.</summary>
		public ref int DoubleTappedUp => ref DoubleTapped[0];
		/// <summary> Set to 15 after a double tap. Decremented every frame.</summary>
		public ref int DoubleTappedDown => ref DoubleTapped[1];
		/// <summary> Set to 15 after a double tap. Decremented every frame.</summary>
		public ref int DoubleTappedRight => ref DoubleTapped[2];
		/// <summary> Set to 15 after a double tap. Decremented every frame.</summary>
		public ref int DoubleTappedLeft => ref DoubleTapped[3];
		/// <summary>List of current Orchid Titanium Shards owned by this player.</summary>
		public List<Projectile> TitaniumShards = new List<Projectile>();

		// Equipment Fields (General)

		/// <summary> Divides damage taken by the player by the sum of all damage resistance bonuses.</summary>
		public float OrchidDamageResistance = 1f;

		// Equipment Fields (Individual Items)

		public bool remoteCopterPet = false;

		public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
		{
			/*
			if (Player.ZoneSkyHeight && !attempt.inLava && !attempt.inHoney && Main.rand.NextBool(10) && Main.hardMode && attempt.rare)
			{
				itemDrop = ModContent.ItemType<Content.Shaman.Weapons.Hardmode.WyvernMoray>();
			}
			*/

			if (attempt.fishingLevel < 50 && Main.rand.NextBool(5 + (int)(attempt.fishingLevel / 2f))) 
			{
				itemDrop = ModContent.ItemType<TrashPavise>();
			}
		}

		public override void Initialize()
		{
			modPlayerShaman = Player.GetModPlayer<OrchidShaman>();
			modPlayerAlchemist = Player.GetModPlayer<OrchidAlchemist>();
			modPlayerGambler = Player.GetModPlayer<OrchidGambler>();
			modPlayerDancer = Player.GetModPlayer<OrchidDancer>();
			modPlayerGuardian = Player.GetModPlayer<OrchidGuardian>();
			modPlayerShapeshifter = Player.GetModPlayer<OrchidShapeshifter>();
		}

		public override void PreUpdate()
		{
			if (Player.whoAmI == Main.myPlayer)
			{
				if (autoRevertSelectedItem)
				{
					if (Player.itemTime == 0 && Player.itemAnimation == 0)
					{
						Player.selectedItem = originalSelectedItem;
						autoRevertSelectedItem = false;
					}
				}
			}
		}

		public override void PreUpdateMovement()
		{
			if ((DoubleTapLeft || DoubleTapRight) && OrchidDoubleDashCD <= 0 && OrchidDoubleDash)
			{
				OrchidDoubleDashCD = 60;
				Player.dashDelay = 60;
				SoundEngine.PlaySound(SoundID.Item19, Player.Center);
				Player.velocity.X = DoubleTapRight ? 15f : -15f;
			}
		}

		public override void ResetEffects()
		{
			Timer++;
			Timer120++;
			if (Timer120 == 120)
				Timer120 = 0;

			remoteCopterPet = false;
			OrchidDoubleDash = false;

			if (OrchidDoubleDashCD > 0)
			{
				OrchidDoubleDashCD--;
				if (OrchidDoubleDashCD > 30)
				{
					Player.velocity.X *= 0.95f;
				}
			}

			if (keepSelected != -1)
			{
				Player.selectedItem = keepSelected;
				keepSelected = -1;
			}

			if (PlayerImmunity > 0) PlayerImmunity--;

			if (ForcedVelocityTimer > 0)
			{
				if (ForcedVelocityTimer <= 0)
				{
					ForcedVelocityVector = Vector2.Zero;
					ForcedVelocityUpkeep = 0f;
				}
				else
				{
					Player.velocity = Vector2.Zero;
					Player.velocity = ForcedVelocityVector * ForcedVelocityUpkeep;
					Vector2 addedVelocity = Vector2.Zero;
					for (int i = 0; i < 10; i++)
						addedVelocity += Collision.TileCollision(Player.position + addedVelocity, ForcedVelocityVector * 0.1f, Player.width, Player.height, false, false, (int)Player.gravDir);

					Player.position += addedVelocity;
				}

				ForcedVelocityTimer--;
			}

			OrchidDamageResistance = 1f;
			
			for (int i = 0; i < 4; i++)
			{
				bool tapKey = false;
				switch(i)
				{
					case 0:
						tapKey = Player.controlUp && Player.releaseUp;
						break;
					case 1:
						tapKey = Player.controlDown && Player.releaseDown;
						break;
					case 2:
						tapKey = Player.controlRight && Player.releaseRight;
						break;
					case 3:
						tapKey = Player.controlLeft && Player.releaseLeft;
						break;
				}
				if (DoubleTapped[i] > 0) DoubleTapped[i]--;
				if (tapKey)
				{
					if (DoubleTapping[i] > 0)
					{
						DoubleTapping[i] = 0;
						DoubleTapped[i] = 15;
					}
					else DoubleTapping[i] = 15;
				}
				else if (DoubleTapping[i] > 0) DoubleTapping[i]--;
			}
			TitaniumShards.RemoveAll(p => !p.active || p.type != ModContent.ProjectileType<OrchidTitaniumShard>());
			int index = 0;
			foreach (Projectile shard in TitaniumShards)
			{
				shard.ai[0] = index / (float)TitaniumShards.Count;
				index++;
			}
		}

		public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
		{
			if (OrchidDamageResistance > 0) modifiers.FinalDamage /= OrchidDamageResistance;
			else modifiers.FinalDamage *= 9999;
			//idk if we'd ever have a situation where it's possible to hit -100% damage resistance but this makes it kill the player instead of throwing an exception
			//seems fitting anyway
		}

		public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
		{
			if (OrchidDamageResistance > 0) modifiers.FinalDamage /= OrchidDamageResistance;
			else modifiers.FinalDamage *= 9999;
		}

		public override bool FreeDodge(Player.HurtInfo info)
		{
			if (PlayerImmunity > 0)
			{
				SoundEngine.PlaySound(SoundID.Item1, Player.Center);
				return true;
			}

			return false;
		}

		public void TryHeal(int amount)
		{
			if (!Player.moonLeech && Player.whoAmI == Main.myPlayer)
			{
				int damage = Player.statLifeMax2 - Player.statLife;
				if (amount > damage)
				{
					amount = damage;
				}
				if (amount > 0)
				{
					Player.HealEffect(amount, true);
					Player.statLife += amount;
				}
			}
		}

		///<summary>Spawns custom Orchid Titanium Shards, and refreshes the player's Titanium Barrier buff.</summary>
		public void SpawnTitaniumShards(IEntitySource source, int count = 1, int maxCount = 8)
		{
			Player.AddBuff(BuffID.TitaniumStorm, 600);
			if (TitaniumShards.Count + count > maxCount) count = maxCount - TitaniumShards.Count;
			if (count < 1) return;
			for (int i = 0; i < count; i++)
			{
				TitaniumShards.Add(Projectile.NewProjectileDirect(source, Player.Center, Vector2.Zero, ModContent.ProjectileType<OrchidTitaniumShard>(), 50, 15f, Player.whoAmI));
			}
		}
	}
}
