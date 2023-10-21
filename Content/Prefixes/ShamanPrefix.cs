using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace OrchidMod.Content.Prefixes
{
	public abstract class ShamanPrefix : ModPrefix
	{
		private static readonly List<ShamanPrefix> prefixes = new();
		public static IReadOnlyList<ShamanPrefix> GetPrefixes => prefixes;

		// ...

		private readonly float damage;
		private readonly int crit;
		private readonly float velocity;
		private readonly int bondDuration;
		private readonly float bondLoading;

		public override void Unload()
			=> prefixes.Clear();

		public override float RollChance(Item item)
			=> 500f;

		public override bool CanRoll(Item item)
			=> true;

		public override PrefixCategory Category
			=> PrefixCategory.Custom;

		public ShamanPrefix(float damage, float bondLoading, int bondDuration, int crit, float velocity)
		{
			this.damage = damage;
			this.bondLoading = bondLoading;
			this.bondDuration = bondDuration;
			this.crit = crit;
			this.velocity = velocity;
		}

		public override void Load()
			=> prefixes.Add(this);

		public override void Apply(Item item)
			=> item.GetGlobalItem<ShamanPrefixItem>().SetPrefixVariables(damage, bondLoading, bondDuration, crit, velocity);

		public override void ModifyValue(ref float valueMult)
		{
			float multiplier = 1f * (damage * 0.96f) * (bondLoading * 0.96f) * (velocity * 0.96f) + (crit * 0.0096f) + (bondDuration * 0.0072f);
			valueMult *= multiplier;
		}

		public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult,
		ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
		{
			damageMult = damage;
			critBonus = crit;
			shootSpeedMult = velocity;
		}
	}

	public class ShamanPrefixItem : GlobalItem
	{
		private float damage;
		private int crit;
		private float velocity;
		private int bondDuration;
		private float bondLoading;

		// ...

		public void SetPrefixVariables(float damage, float bondLoading, int bondDuration, int crit, float velocity)
		{
			this.damage = damage;
			this.bondLoading = bondLoading;
			this.bondDuration = bondDuration;
			this.crit = crit;
			this.velocity = velocity;
		}

		// ...

		public int GetBondDuration() => bondDuration;

		public ShamanPrefixItem()
		{
			damage = 0;
			crit = 0;
			bondDuration = 0;
			velocity = 0;
			bondLoading = 0;
		}

		public override bool InstancePerEntity
			=> true;

		public override GlobalItem Clone(Item item, Item itemClone)
		{
			ShamanPrefixItem myClone = (ShamanPrefixItem)base.Clone(item, itemClone);
			myClone.damage = damage;
			myClone.crit = crit;
			myClone.bondDuration = bondDuration;
			myClone.bondLoading = bondLoading;
			myClone.velocity = velocity;
			return myClone;
		}

		public override void PreReforge(Item item)
		{
			damage = 0;
			crit = 0;
			bondDuration = 0;
			velocity = 0;
			bondLoading = 0;
		}

		public override void HoldItem(Item item, Player player)
		{
			OrchidPlayer modPlayer = player.GetModPlayer<OrchidPlayer>();
			modPlayer.modPlayerShaman.ShamanBondDuration += bondDuration;
			if (bondLoading != 1f && bondLoading != 0f) modPlayer.modPlayerShaman.ShamanBondLoadRate += bondLoading - 1f;
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			int index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Material"));
			if (index == -1) index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Knockback"));
			if (index == -1) return;
			index += 2;

			if (bondDuration != 0)
			{
				tooltips.Insert(index, new TooltipLine(Mod, "BondDurationPrefix", (bondDuration > 0 ? "+" : "") + bondDuration + "s shamanic bond duration")
				{
					IsModifier = true,
					IsModifierBad = bondDuration < 0
				});
				index++;
			}

			if (bondLoading != 1f && bondLoading != 0f)
			{
				tooltips.Insert(index, new TooltipLine(Mod, "BondLoadingPrefix", (bondLoading > 1 ? "+" : "") + string.Format("{0:0}", ((bondLoading - 1f) * 100f)) + "% shamanic bond generation")
				{
					IsModifier = true,
					IsModifierBad = bondLoading < 1
				});
			}
		}

		public override int ChoosePrefix(Item item, UnifiedRandom rand)
		{
			if (item.damage <= 0 || item.accessory || item.type == ItemID.None) return -1;

			var globalItem = item.GetGlobalItem<OrchidModGlobalItem>();
			if (!globalItem.shamanWeapon) return -1;

			var prefixes = ShamanPrefix.GetPrefixes;
			while (true)
			{
				var prefix = prefixes[Main.rand.Next(prefixes.Count)];
				return prefix.Type;
			}
		}

		public override void NetSend(Item item, BinaryWriter writer)
		{
			writer.Write(damage);
			writer.Write(bondLoading);
			writer.Write(bondDuration);
			writer.Write(crit);
			writer.Write(velocity);
		}

		public override void NetReceive(Item item, BinaryReader reader)
		{
			damage = reader.ReadSingle();
			bondLoading = reader.ReadSingle();
			bondDuration = reader.ReadInt32();
			crit = reader.ReadInt32();
			velocity = reader.ReadSingle();
		}
	}
}