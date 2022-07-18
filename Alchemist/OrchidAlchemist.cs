using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Alchemist;
using OrchidMod.Dancer;
using OrchidMod.Gambler;
using OrchidMod.Shaman;
using OrchidMod.Guardian;
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
	public class OrchidAlchemist : ModPlayer
	{
		public OrchidPlayer modPlayer;

		public List<string> alchemistKnownReactions = new List<string>();
		public List<string> alchemistKnownHints = new List<string>();
		public float alchemistVelocity = 1.0f;
		public bool[] alchemistElements = new bool[6];
		public Item[] alchemistFlasks = new Item[6];
		public int alchemistFlaskDamage = 0;
		public int alchemistNbElements = 0; // mp Sync ?
		public int alchemistNbElementsMax = 2;
		public int alchemistPotencyMax = 8;
		public int alchemistPotency = 100;
		public int alchemistRegenPotency = 60;
		public int alchemistPotencyWait = 0;
		public int alchemistPotencyDisplayTimer = 0;
		public int alchemistResetTimer = 300;
		public int alchemistColorR = 50; // mp Sync ?
		public int alchemistColorG = 100; // mp Sync ?
		public int alchemistColorB = 255; // mp Sync ?
		public int alchemistColorRDisplay = 0;
		public int alchemistColorGDisplay = 0;
		public int alchemistColorBDisplay = 0;
		public bool alchemistSelectUIDisplay = true;
		public bool alchemistSelectUIItem = false;
		public bool alchemistSelectUIInitialize = false;
		public bool alchemistSelectUIKeysDisplay = true;
		public bool alchemistSelectUIKeysItem = false;
		public bool alchemistSelectUIKeysInitialize = false;
		public bool alchemistShootProjectile = false;
		public bool alchemistBookUIDisplay = false;
		public bool alchemistBookUIItem = false;
		public bool alchemistBookUIInitialize = false;
		public bool alchemistDailyHint = false;
		public bool alchemistEntryTextCooldown = false;
		public int alchemistLastAttackDelay = 0;

		public int alchemistFlower = 0;
		public int alchemistFlowerTimer = 0;
		
		public bool alchemistMeteor = false;
		public bool alchemistFlowerSet = false;
		public bool alchemistMushroomSpores = false;
		public bool alchemistReactiveVials = false;
		public bool alchemistCovent = false;

		public void ClearAlchemistColors()
		{
			alchemistColorR = 50;
			alchemistColorG = 100;
			alchemistColorB = 255;
		}

		public void ClearAlchemistFlasks()
		{
			alchemistFlasks = new Item[6];

			for (int i = 0; i < 6; i++)
			{
				alchemistFlasks[i] = new Item();
			}
		}

		public void ClearAlchemistElements()
		{
			alchemistElements = new bool[6];

			for (int i = 0; i < 6; i++)
			{
				alchemistElements[i] = false;
			}
		}

		public int GetNbAlchemistFlasks()
		{
			int val = 0;
			for (int i = 0; i < 6; i++)
			{
				val += alchemistFlasks[i].type != 0 ? 1 : 0;
			}
			return val;
		}

		public bool ContainsAlchemistFlask(int flaskType)
		{
			for (int i = 0; i < 6; i++)
			{
				if (alchemistFlasks[i].type == flaskType)
				{
					return true;
				}
			}
			return false;
		}

		public int GetSecondaryDamage(int itemType, int bonusDamage = 0, bool bonusDamageScaling = true)
		{
			Item item = new Item();
			item.SetDefaults(itemType);
			OrchidModGlobalItem globalItem = item.GetGlobalItem<OrchidModGlobalItem>();
			float dmg = (int)(globalItem.alchemistSecondaryDamage + (int)(bonusDamage * (bonusDamageScaling ? globalItem.alchemistSecondaryScaling : 1f)));
			dmg = Player.GetDamage<AlchemistDamageClass>().ApplyTo(dmg);
			dmg = Player.GetDamage<GenericDamageClass>().ApplyTo(dmg);
			return (int)dmg;
		}

		public int GetProgressLevel()
		{
			int progression = 1;
			progression = NPC.downedBoss2 ? 2 : progression;
			progression = NPC.downedBoss3 ? 3 : progression;
			progression = Main.hardMode ? 4 : progression;
			progression = NPC.downedMechBossAny ? 5 : progression;
			progression = NPC.downedGolemBoss ? 6 : progression;
			return progression;
		}

		public void Reset()
		{
			alchemistPotency = alchemistPotencyMax;
			alchemistFlaskDamage = 0;
			alchemistNbElements = 0;
			alchemistPotencyDisplayTimer = 0;
			ClearAlchemistElements();
			ClearAlchemistFlasks();
			ClearAlchemistColors();
		}

		public override void Initialize()
		{
			modPlayer = Player.GetModPlayer<OrchidPlayer>();

			Reset();

			this.alchemistKnownReactions = new List<string>();
			this.alchemistKnownHints = new List<string>();
		}

		public override void SaveData(TagCompound tag)
		{
			tag.Add("ChemistHint", alchemistDailyHint);
			tag.Add("AlchemistKnownReactions", alchemistKnownReactions.ToList());
			tag.Add("AlchemistKnownHints", alchemistKnownHints.ToList());
		}

		public override void LoadData(TagCompound tag)
		{
			alchemistDailyHint = tag.GetBool("ChemistHint");
			alchemistKnownReactions = tag.Get<List<string>>("AlchemistKnownReactions");
			alchemistKnownHints = tag.Get<List<string>>("AlchemistKnownHints");
		}

		public override void PostUpdateEquips()
		{
			int regenComparator = alchemistPotencyWait <= 120 ? alchemistPotencyWait <= 0 ? (int)(alchemistRegenPotency / 3) : (int)(alchemistRegenPotency / 2) : alchemistRegenPotency;
			if (alchemistPotency < alchemistPotencyMax && ((alchemistRegenPotency > 0) ? modPlayer.timer120 % regenComparator == 0 : true))
			{
				alchemistPotencyDisplayTimer = 180;
				alchemistPotency++;
				alchemistFlowerTimer = alchemistFlowerSet ? 600 : 0;
			}
			else
			{
				if (alchemistPotency > alchemistPotencyMax)
				{
					alchemistPotency = alchemistPotencyMax;
				}
				alchemistPotencyDisplayTimer--;
			}

			alchemistResetTimer = (alchemistPotencyDisplayTimer > 0) ? 300 : alchemistResetTimer - 1;
			alchemistResetTimer = (alchemistResetTimer == -1) ? 0 : alchemistResetTimer;
			alchemistPotencyWait -= alchemistPotencyWait > 0 ? 1 : 0;

			if (alchemistResetTimer == 1)
			{
				alchemistFlaskDamage = 0;
				alchemistNbElements = 0;
				ClearAlchemistElements();
				ClearAlchemistFlasks();
				ClearAlchemistColors();
			}

			if (alchemistColorRDisplay != alchemistColorR)
			{
				bool superior = alchemistColorRDisplay > alchemistColorR;
				int abs = Math.Abs(alchemistColorRDisplay - alchemistColorR);
				int val = (int)(abs / 10);
				if (abs > val)
				{
					alchemistColorRDisplay += superior ? -val : val;
				}
				alchemistColorRDisplay += superior ? -1 : 1;
			}

			if (alchemistColorGDisplay != alchemistColorG)
			{
				bool superior = alchemistColorGDisplay > alchemistColorG;
				int abs = Math.Abs(alchemistColorGDisplay - alchemistColorG);
				int val = (int)(abs / 10);
				if (abs > val)
				{
					alchemistColorGDisplay += superior ? -val : val;
				}
				alchemistColorGDisplay += superior ? -1 : 1;
			}

			if (alchemistColorBDisplay != alchemistColorB)
			{
				bool superior = alchemistColorBDisplay > alchemistColorB;
				int abs = Math.Abs(alchemistColorBDisplay - alchemistColorB);
				int val = (int)(abs / 10);
				if (abs > val)
				{
					alchemistColorBDisplay += superior ? -val : val;
				}
				alchemistColorBDisplay += superior ? -1 : 1;
			}
		}

		public override void PostUpdate()
		{
			alchemistFlowerTimer -= alchemistFlowerTimer > 0 ? 1 : 0;
			alchemistFlower = alchemistFlowerTimer == 0 ? 0 : alchemistFlower;

			if (alchemistShootProjectile)
			{
				float shootSpeed = 10f * alchemistVelocity;
				int projType = ProjectileType<Alchemist.Projectiles.AlchemistProj>();
				Vector2 target = Main.MouseWorld;
				Vector2 heading = target - Player.Center;
				heading.Normalize();
				heading *= shootSpeed;
				Projectile.NewProjectile(Player.GetSource_FromThis("Alchemist Main"), Player.Center.X, Player.Center.Y, heading.X, heading.Y, projType, 1, 1f, Player.whoAmI);
				alchemistShootProjectile = false;
			}
		}
		public override void ModifyHitNPCWithProj(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			OrchidModGlobalProjectile modProjectile = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
			if (modProjectile.alchemistProjectile)
			{
				/*
				if (Main.rand.Next(101) <= this.alchemistCrit + modProjectile.baseCritChance)
					crit = true;
				else crit = false;
				*/
			}
		}

		public override void ProcessTriggers(TriggersSet triggersSet)
		{
			if (OrchidKeybindLoader.AlchemistCatalyst.JustPressed && Player.itemAnimation == 0)
			{
				for (int i = 0; i < Main.InventorySlotsTotal; i++)
				{
					Item item = Main.LocalPlayer.inventory[i];
					if (item.type != 0)
					{
						OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
						if (orchidItem.alchemistCatalyst)
						{
							modPlayer.originalSelectedItem = Player.selectedItem;
							modPlayer.autoRevertSelectedItem = true;
							Player.selectedItem = i;
							Player.controlUseItem = true;
							Player.ItemCheck(Player.whoAmI);
							return;
						}
					}
				}
			}

			if (OrchidKeybindLoader.AlchemistReaction.JustPressed)
			{
				if (this.alchemistNbElements < 2 || Player.FindBuffIndex(Mod.Find<ModBuff>("ReactionCooldown").Type) > -1)
				{
					return;
				}
				else
				{
					AlchemistHiddenReactionHelper.triggerAlchemistReaction(Mod, Player, this);
				}
			}
		}

		public override void ResetEffects()
		{
			if (GetNbAlchemistFlasks() == 0)
			{
				ClearAlchemistFlasks();
				ClearAlchemistElements();
				ClearAlchemistColors();
			}

			alchemistDailyHint = (Main.dayTime && Main.time == 0) ? false : alchemistDailyHint;
			alchemistLastAttackDelay += alchemistLastAttackDelay < 3600 ? 1 : 0;

			alchemistPotencyMax = 8;
			alchemistRegenPotency = 60;
			alchemistNbElementsMax = 2;
			alchemistVelocity = 1.0f;
			alchemistSelectUIDisplay = alchemistSelectUIItem ? alchemistSelectUIDisplay : false;
			alchemistSelectUIKeysDisplay = alchemistSelectUIKeysItem ? alchemistSelectUIKeysDisplay : false;
			alchemistSelectUIItem = false;
			alchemistSelectUIKeysItem = false;
			alchemistBookUIDisplay = alchemistBookUIItem ? alchemistBookUIDisplay : false;
			alchemistBookUIItem = false;
			alchemistEntryTextCooldown = false;

			alchemistMeteor = false;
			alchemistFlowerSet = false;
			alchemistMushroomSpores = false;
			alchemistReactiveVials = false;
			alchemistCovent = false;
		}

		public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
		{
			Reset();
		}

		public override void OnRespawn(Player player)
		{
			Reset();
		}
	}
}
