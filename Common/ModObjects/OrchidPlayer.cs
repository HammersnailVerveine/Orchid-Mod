using Microsoft.Xna.Framework;
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

		// Equipment Fields (General)


		public float OrchidDamageReduction = 1f; // Multiplies all damage taken by the player. Should be modified by multiplicating it (eg : OrchidDamageReduction *= 0.7f)

		// Equipment Fields (Individual Items)

		public bool remoteCopterPet = false;

		public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
		{
			if (Player.ZoneSkyHeight && !attempt.inLava && !attempt.inHoney && Main.rand.NextBool(10) && Main.hardMode && attempt.rare)
			{
				itemDrop = ModContent.ItemType<Content.Shaman.Weapons.Hardmode.WyvernMoray>();
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

		public override void ResetEffects()
		{
			Timer++;
			Timer120++;
			if (Timer120 == 120)
				Timer120 = 0;

			remoteCopterPet = false;

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

			OrchidDamageReduction = 1f;
		}

		public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
		{
			modifiers.FinalDamage *= OrchidDamageReduction;
		}

		public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
		{
			modifiers.FinalDamage *= OrchidDamageReduction;
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
	}
}
