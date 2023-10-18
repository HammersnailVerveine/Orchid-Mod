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

		private readonly string displayName;
		private readonly float damage;
		private readonly float crit;
		private readonly float useTime;
		private readonly float velocity;
		private readonly float knockback;

		public bool UseTimeHasDefaultValue
			=> useTime == 1f;

		public bool VelocityHasDefaultValue
			=> velocity == 1f;

		public override void Unload()
			=> prefixes.Clear();

		public override float RollChance(Item item)
			=> 500f;

		public override bool CanRoll(Item item)
			=> true;

		public override PrefixCategory Category
			=> PrefixCategory.Custom;

		public ShamanPrefix(string displayName, float damage, float knockback, float useTime, float crit, float velocity)
		{
			this.displayName = displayName;
			this.damage = damage;
			this.knockback = knockback;
			this.useTime = useTime;
			this.crit = crit;
			this.velocity = velocity;
		}

		public override void Load()
			=> prefixes.Add(this);

		/* public override void SetStaticDefaults()
			=> DisplayName.SetDefault(displayName); */

		public override void Apply(Item item)
			=> item.GetGlobalItem<ShamanPrefixItem>().SetPrefixVariables(damage, knockback, useTime, crit, velocity);

		public override void ModifyValue(ref float valueMult)
		{
			float multiplier = 1f * (damage * 0.96f) * (knockback * 0.96f) * (crit * 0.96f) * ((2f - useTime) * 0.96f) * (velocity * 0.96f);
			valueMult *= multiplier;
		}

		public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult,
		ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
		{
			damageMult = damage;
			knockbackMult = knockback;
			useTimeMult = useTime;
			critBonus = (int)(crit * 100 - 100);
			shootSpeedMult = velocity;
		}
	}

	public class ShamanPrefixItem : GlobalItem
	{
		private float damage;
		private float crit;
		private float useTime;
		private float velocity;
		private float knockback;

		// ...

		public void SetPrefixVariables(float damage, float knockback, float useTime, float crit, float velocity)
		{
			this.damage = damage;
			this.knockback = knockback;
			this.useTime = useTime;
			this.crit = crit;
			this.velocity = velocity;
		}

		// ...

		public ShamanPrefixItem()
		{
			damage = 0;
			crit = 0;
			useTime = 0;
			velocity = 0;
			knockback = 0;
		}

		public override bool InstancePerEntity
			=> true;

		public override GlobalItem Clone(Item item, Item itemClone)
		{
			ShamanPrefixItem myClone = (ShamanPrefixItem)base.Clone(item, itemClone);
			myClone.damage = damage;
			myClone.crit = crit;
			myClone.useTime = useTime;
			myClone.velocity = velocity;
			myClone.knockback = knockback;
			return myClone;
		}

		public override void PreReforge(Item item)
		{
			damage = 0;
			crit = 0;
			useTime = 0;
			velocity = 0;
			knockback = 0;
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

				if (globalItem.shamanWeaponNoUsetimeReforge && !prefix.UseTimeHasDefaultValue) continue;
				if (globalItem.shamanWeaponNoVelocityReforge && !prefix.VelocityHasDefaultValue) continue;

				return prefix.Type;
			}
		}

		public override void NetSend(Item item, BinaryWriter writer)
		{
			writer.Write(damage);
			writer.Write(knockback);
			writer.Write(useTime);
			writer.Write(crit);
			writer.Write(velocity);
		}

		public override void NetReceive(Item item, BinaryReader reader)
		{
			damage = reader.ReadSingle();
			knockback = reader.ReadSingle();
			useTime = reader.ReadSingle();
			crit = reader.ReadSingle();
			velocity = reader.ReadSingle();
		}
	}
}