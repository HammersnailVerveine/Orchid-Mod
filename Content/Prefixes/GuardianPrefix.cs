using System.Collections.Generic;
using System.IO;
using OrchidMod.Common.Global.Items;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Guardian;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace OrchidMod.Content.Prefixes
{
	public abstract class GuardianPrefix : ModPrefix
	{
		private static readonly List<GuardianPrefix> prefixes = new();
		public static IReadOnlyList<GuardianPrefix> GetPrefixes => prefixes;

		// ...

		private readonly float damage;
		private readonly float knockback;
		private readonly float blockDuration;
		private readonly int crit;
		private readonly float slamDistance;

		public override void Unload()
			=> prefixes.Clear();

		public override float RollChance(Item item)
			=> 500f;

		public override bool CanRoll(Item item)
			=> true;

		public override PrefixCategory Category
			=> PrefixCategory.Custom;

		public GuardianPrefix(float damage, float knockback, float blockDuration, int crit, float slamDistance)
		{
			this.damage = damage;
			this.knockback = knockback;
			this.blockDuration = blockDuration;
			this.crit = crit;
			this.slamDistance = slamDistance;
		}

		public override void Load()
			=> prefixes.Add(this);

		public override void Apply(Item item)
			=> item.GetGlobalItem<GuardianPrefixItem>().SetPrefixVariables(damage, knockback, blockDuration, crit, slamDistance);

		public override void ModifyValue(ref float valueMult)
		{
			float multiplier = 1f * (damage * 0.98f) * (knockback * 0.94f) * (blockDuration * 0.94f) * (slamDistance * 0.96f) + (crit * 0.0098f);
			valueMult *= multiplier;
		}

		public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult,
		ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
		{
			damageMult = damage;
			critBonus = crit;
			knockbackMult = knockback;
		}
	}

	public class GuardianPrefixItem : GlobalItem
	{
		private float damage;
		private float knockback;
		private float blockDuration;
		private int crit;
		private float slamDistance;

		// ...

		public void SetPrefixVariables(float damage, float knockback, float blockDuration, int crit, float slamDistance)
		{
			this.damage = damage;
			this.knockback = knockback;
			this.blockDuration = blockDuration;
			this.crit = crit;
			this.slamDistance = slamDistance;
		}

		// ...

		public float GetSlamDistance() => slamDistance != 0f ? slamDistance : 1f;
		public float GetBlockDuration() => blockDuration != 0f ? blockDuration : 1f;

		public GuardianPrefixItem()
		{
			damage = 0;
			knockback = 0;
			blockDuration = 0;
			crit = 0;
			slamDistance = 0;
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
			myClone.slamDistance = slamDistance;
			return myClone;
		}

		public override void PreReforge(Item item)
		{
			damage = 0;
			knockback = 0;
			blockDuration = 0;
			crit = 0;
			slamDistance = 0;
		}

		public override void HoldItem(Item item, Player player)
		{
			OrchidPlayer modPlayer = player.GetModPlayer<OrchidPlayer>();
			if (slamDistance != 1f && slamDistance != 0f) modPlayer.modPlayerGuardian.GuardianSlamDistance += slamDistance;
			if (blockDuration != 1f && blockDuration != 0f) modPlayer.modPlayerGuardian.GuardianBlockDuration += blockDuration;
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (blockDuration != 1f && blockDuration != 0f)
			{
				tooltips.Add(new TooltipLine(Mod, "BlockDurationPrefix", (blockDuration > 1 ? "+" : "") + string.Format("{0:0}", ((blockDuration - 1f) * 100f)) + "% block duration")
				{
					IsModifier = true,
					IsModifierBad = blockDuration < 1
				});
			}

			if (slamDistance != 1f && slamDistance != 0f)
			{
				tooltips.Add(new TooltipLine(Mod, "SlamDistancePrefix", (slamDistance > 1 ? "+" : "") + string.Format("{0:0}", ((slamDistance - 1f) * 100f)) + "% slam distance")
				{
					IsModifier = true,
					IsModifierBad = slamDistance < 1
				});
			}
		}

		public override int ChoosePrefix(Item item, UnifiedRandom rand)
		{
			var globalItem = item.GetGlobalItem<OrchidGlobalItemPerEntity>();
			if (item.ModItem is OrchidModGuardianShield)
			{
				var prefixes = GuardianPrefix.GetPrefixes;
				return prefixes[Main.rand.Next(prefixes.Count)].Type;
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
			writer.Write(slamDistance);
		}

		public override void NetReceive(Item item, BinaryReader reader)
		{
			damage = reader.ReadSingle();
			knockback = reader.ReadSingle();
			blockDuration = reader.ReadSingle();
			crit = reader.ReadInt32();
			slamDistance = reader.ReadSingle();
		}
	}
}