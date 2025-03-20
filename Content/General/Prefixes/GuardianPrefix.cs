using System;
using System.Collections.Generic;
using System.IO;
using OrchidMod.Common.Global.Items;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Guardian;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace OrchidMod.Content.General.Prefixes
{
	public abstract class GuardianPrefixNoBlockDuration : GuardianPrefix
	{
		private static readonly List<GuardianPrefix> prefixes = new();
		public static new IReadOnlyList<GuardianPrefix> GetPrefixes => prefixes;
		public GuardianPrefixNoBlockDuration(float damage, float knockback, float blockDuration, int crit, float speed) : base(damage, knockback, blockDuration, crit, speed) { }
		public override void Load() => prefixes.Add(this);
	}

	public abstract class GuardianPrefix : ModPrefix
	{
		private static readonly List<GuardianPrefix> prefixes = new();
		public static IReadOnlyList<GuardianPrefix> GetPrefixes => prefixes;
		public float GetSpeed => speed;

		// ...

		private readonly float damage;
		private readonly float knockback;
		private readonly float blockDuration;
		private readonly int crit;
		private readonly float speed;

		public override void Unload() => prefixes.Clear();

		public override float RollChance(Item item)
			=> 500f;

		public override bool CanRoll(Item item)
			=> true;

		public override PrefixCategory Category
			=> PrefixCategory.Custom;

		public GuardianPrefix(float damage, float knockback, float blockDuration, int crit, float speed)
		{
			this.damage = damage;
			this.knockback = knockback;
			this.blockDuration = blockDuration;
			this.crit = crit;
			this.speed = speed;
		}

		public override void Load() => prefixes.Add(this);

		public override void Apply(Item item)
			=> item.GetGlobalItem<GuardianPrefixItem>().SetPrefixVariables(damage, knockback, blockDuration, crit, speed);

		public override void ModifyValue(ref float valueMult)
		{
			float multiplier = 1f + (damage - 1f) * 0.05f + (knockback - 1f) * 0.05f + (blockDuration - 1f) * 0.05f + (speed - 1f) * 0.05f + crit * 0.0015f;
			valueMult *= multiplier;
		}

		public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult,
		ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
		{
			damageMult = damage;
			critBonus = crit;
			knockbackMult = knockback;
			//useTimeMult = 1f + (speed - 1f) * -1f;
		}
	}

	public class GuardianPrefixItem : GlobalItem
	{
		private float damage;
		private float knockback;
		private float blockDuration;
		private int crit;
		private float speed;

		// ...

		public void SetPrefixVariables(float damage, float knockback, float blockDuration, int crit, float speed)
		{
			this.damage = damage;
			this.knockback = knockback;
			this.blockDuration = blockDuration;
			this.crit = crit;
			this.speed = speed;
		}

		// ...

		public float GetSlamDistance() => speed != 0f ? speed : 1f;
		public float GetBlockDuration() => blockDuration != 0f ? blockDuration : 1f;

		public GuardianPrefixItem()
		{
			damage = 0;
			knockback = 0;
			blockDuration = 0;
			crit = 0;
			speed = 0;
		}

		public override bool InstancePerEntity
			=> true;

		public override GlobalItem Clone(Item item, Item itemClone)
		{
			GuardianPrefixItem myClone = (GuardianPrefixItem)base.Clone(item, itemClone);
			myClone.damage = damage;
			myClone.knockback = knockback;
			myClone.blockDuration = blockDuration;
			myClone.crit = crit;
			myClone.speed = speed;
			return myClone;
		}

		public override void PreReforge(Item item)
		{
			damage = 0;
			knockback = 0;
			blockDuration = 0;
			crit = 0;
			speed = 0;
		}

		public override void HoldItem(Item item, Player player)
		{
			OrchidPlayer modPlayer = player.GetModPlayer<OrchidPlayer>();
			if (blockDuration != 1f && blockDuration != 0f) modPlayer.modPlayerGuardian.GuardianBlockDuration += blockDuration;
			if (speed != 1f && speed != 0f)
			{
				if (item.ModItem is OrchidModGuardianShield)
				{
					modPlayer.modPlayerGuardian.GuardianPaviseScale += speed - 1f;
				}
				else
				{
					modPlayer.modPlayerGuardian.GuardianMeleeSpeed += speed - 1f;
				}
			}
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if ((blockDuration != 1f && blockDuration != 0f) || (speed != 1f && speed != 0f))
			{
				string block = Language.GetTextValue("Mods.OrchidMod.Prefixes.AddParry");

				if (item.ModItem is OrchidModGuardianShield)
				{
					block = Language.GetTextValue("Mods.OrchidMod.Prefixes.AddBlock");
				}

				// I have no clue how to do this in a clean way
				int index = -1;
				for (int i = 5; i >= 0; i--)
				{
					index = tooltips.FindIndex(ttip => ttip.Name.Equals("Tooltip" + i));
					if (index != -1)
					{
						index++;
						break;
					}
				}

				if (index == -1)
				{
					index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("PrefixDamage"));
					if (index == -1) index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("PrefixCritChance"));
					if (index == -1) index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("PrefixKnockback"));
					if (index == -1) index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Material"));
					if (index == -1) index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Knockback"));
					if (index == -1) return;
					index += 3;
				}


				if (blockDuration != 1f && blockDuration != 0f)
				{
					tooltips.Insert(index, new TooltipLine(Mod, "BlockDurationPrefix", (blockDuration > 1 ? "+" : "") + string.Format("{0:0}", (blockDuration - 1f) * 100f) + "% " + Language.GetTextValue("Mods.OrchidMod.Prefixes.AddDuration", block))
					{
						IsModifier = true,
						IsModifierBad = blockDuration < 1
					});
				}

				if (speed != 1f && speed != 0f)
				{
					string statname = item.ModItem is OrchidModGuardianShield ? Language.GetTextValue("Mods.OrchidMod.Prefixes.AddSize") : Language.GetTextValue("Mods.OrchidMod.Prefixes.AddSpeed");
					tooltips.Insert(index, new TooltipLine(Mod, "SpeedPrefix", (speed > 1 ? "+" : "") + string.Format("{0:0}", (speed - 1f) * 100f) + "% " + statname)
					{
						IsModifier = true,
						IsModifierBad = speed < 1
					});
				}
			}
		}

		public override int ChoosePrefix(Item item, UnifiedRandom rand)
		{
			var globalItem = item.GetGlobalItem<OrchidGlobalItemPerEntity>();
			if (item.ModItem is OrchidModGuardianItem)
			{
				List<int> UniversalPrefixesIDs = [36, 37, 38, 39, 40, 41, 53, 54, 55, 56, 57, 59, 60, 61]; // Has to be hardcoded

				if (item.ModItem is not OrchidModGuardianHammer && item.ModItem is not OrchidModGuardianRune)
				{
					foreach (var prefix in GuardianPrefix.GetPrefixes)
					{
						if (prefix is not HaidexPrefix || Main.rand.NextBool(100))
						{
							UniversalPrefixesIDs.Add(prefix.Type);
						}
					}
				}

				foreach (var prefix in GuardianPrefixNoBlockDuration.GetPrefixes) UniversalPrefixesIDs.Add(prefix.Type);
				return UniversalPrefixesIDs[Main.rand.Next(UniversalPrefixesIDs.Count)];
			}

			/*
			if (item.ModItem is OrchidModGuardianHammer)
			{
				List<int> UniversalPrefixesIDs = [36, 37, 38, 39, 40, 41, 53, 54, 55, 56, 57, 59, 60, 61]; // Has to be hardcoded
				return Main.rand.Next(UniversalPrefixesIDs);
			}
			*/

			return -1;
		}

		public override void NetSend(Item item, BinaryWriter writer)
		{
			writer.Write(damage);
			writer.Write(knockback);
			writer.Write(blockDuration);
			writer.Write(crit);
			writer.Write(speed);
		}

		public override void NetReceive(Item item, BinaryReader reader)
		{
			damage = reader.ReadSingle();
			knockback = reader.ReadSingle();
			blockDuration = reader.ReadSingle();
			crit = reader.ReadInt32();
			speed = reader.ReadSingle();
		}
	}
}