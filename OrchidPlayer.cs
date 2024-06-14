using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace OrchidMod
{
	public class OrchidPlayer : ModPlayer
	{
		public OrchidShaman modPlayerShaman;
		public OrchidAlchemist modPlayerAlchemist;
		public OrchidGambler modPlayerGambler;
		public OrchidDancer modPlayerDancer;
		public OrchidGuardian modPlayerGuardian;

		public bool remoteCopterPet = false;
		public int originalSelectedItem;
		public bool autoRevertSelectedItem = false;

		public int timer120 = 0;
		public int Timer = 0;
		public int doubleTap = 0;
		public int doubleTapCooldown = 0;
		public bool doubleTapLock = false;
		public int keepSelected = -1;

		/*General*/

		public bool generalTools = false;
		public bool generalStatic = false;

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
			timer120++;
			if (timer120 == 120)
				timer120 = 0;

			remoteCopterPet = false;

			generalTools = false;
			generalStatic = false;

			if (this.keepSelected != -1)
			{
				Player.selectedItem = keepSelected;
				this.keepSelected = -1;
			}
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
