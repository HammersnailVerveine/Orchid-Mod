using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using Terraria.Utilities;

namespace OrchidMod.Content.Prefixes
{
	public class ShamanPrefix : ModPrefix
	{
		private static readonly Dictionary<string, ShamanPrefix> prefixesByName = new();
		public static IReadOnlyDictionary<string, ShamanPrefix> AllPrefixesByName => prefixesByName;

		// ...

		private readonly string name;
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
			=> prefixesByName.Clear();

		public override float RollChance(Item item)
			=> 500f;

		public override bool CanRoll(Item item)
			=> true;

		public override PrefixCategory Category
			=> PrefixCategory.Custom;

		public ShamanPrefix() { }

		public ShamanPrefix(string name, float damage, float knockback, float useTime, float crit, float velocity)
		{
			this.name = name;
			this.damage = damage;
			this.knockback = knockback;
			this.useTime = useTime;
			this.crit = crit;
			this.velocity = velocity;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(name);
		}

		public override bool IsLoadingEnabled(Mod mod)
		{
			void AddPrefix(string name, float damage, float knockback, float useTime, float crit, float velocity)
			{
				var prefix = new ShamanPrefix(name, damage, knockback, useTime, crit, velocity);
				Mod.AddContent(prefix);
				prefixesByName.Add(name, prefix);
			}

			AddPrefix("Voodoo", 1.00f, 1.00f, 1.00f, 1.00f, 1.05f);
			AddPrefix("Superior", 1.10f, 1.10f, 1.00f, 1.00f, 1.00f);
			AddPrefix("Forceful", 1.00f, 1.15f, 1.00f, 1.00f, 1.00f);
			AddPrefix("Broken", 0.70f, 0.80f, 1.00f, 1.00f, 1.00f);
			AddPrefix("Damaged", 0.85f, 1.00f, 1.00f, 1.00f, 1.00f);
			AddPrefix("Shoddy", 0.90f, 0.85f, 1.00f, 1.00f, 1.00f);
			AddPrefix("Hurtful", 1.10f, 1.00f, 1.00f, 1.00f, 1.00f);
			AddPrefix("Strong", 1.00f, 1.15f, 1.00f, 1.00f, 1.00f);
			AddPrefix("Unpleasant", 1.05f, 1.15f, 1.00f, 1.00f, 1.00f);
			AddPrefix("Weak", 1.00f, 0.80f, 1.00f, 1.00f, 1.00f);
			AddPrefix("Ruthless", 1.18f, 0.90f, 1.00f, 1.00f, 1.00f);
			AddPrefix("Occult", 1.15f, 1.15f, 1.00f, 1.00f, 1.10f);
			AddPrefix("Diabolic", 1.15f, 1.00f, 1.00f, 1.00f, 1.10f);
			AddPrefix("Spirited", 1.00f, 1.00f, 1.00f, 1.00f, 1.10f);

			AddPrefix("Quick", 1.00f, 1.00f, 0.90f, 1.00f, 1.00f);
			AddPrefix("Deadly", 1.10f, 1.00f, 0.90f, 1.00f, 1.00f);
			AddPrefix("Magnetic", 1.00f, 1.00f, 0.90f, 1.00f, 1.05f);
			AddPrefix("Nimble", 1.00f, 1.00f, 0.95f, 1.00f, 1.00f);
			AddPrefix("Runic", 1.07f, 1.00f, 0.94f, 1.00f, 1.05f);
			AddPrefix("Slow", 1.00f, 1.00f, 1.15f, 1.00f, 1.00f);
			AddPrefix("Sluggish", 1.00f, 1.00f, 1.20f, 1.00f, 1.00f);
			AddPrefix("Lazy", 1.00f, 1.00f, 1.18f, 1.00f, 1.00f);
			AddPrefix("Annoying", 0.80f, 1.00f, 1.15f, 1.00f, 1.00f);
			AddPrefix("Conjuring", 1.05f, 0.90f, 0.90f, 1.00f, 1.05f);

			AddPrefix("Studious", 1.10f, 1.00f, 1.00f, 1.15f, 1.00f);
			AddPrefix("Unique", 1.15f, 1.05f, 1.00f, 1.20f, 1.00f);
			AddPrefix("Balanced", 1.00f, 1.10f, 0.90f, 1.00f, 1.00f);
			AddPrefix("Hopeful", 1.00f, 1.00f, 1.00f, 1.15f, 1.00f);
			AddPrefix("Enraged", 1.15f, 1.15f, 1.00f, 1.00f, 1.00f);
			AddPrefix("Effervescent", 1.10f, 1.10f, 1.10f, 1.10f, 1.00f); // :(
			AddPrefix("Ethereal", 1.15f, 1.15f, 0.90f, 1.10f, 1.10f);
			AddPrefix("Focused", 1.10f, 1.00f, 1.00f, 1.15f, 1.00f);
			AddPrefix("Complex", 0.90f, 1.00f, 0.90f, 1.10f, 1.00f);

			return false;
		}

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

		public override bool InstancePerEntity => true;

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

		public override bool PreReforge(Item item)
		{
			damage = 0;
			crit = 0;
			useTime = 0;
			velocity = 0;
			knockback = 0;

			return base.PreReforge(item);
		}

		public override int ChoosePrefix(Item item, UnifiedRandom rand)
		{
			if (item.damage <= 0 || item.accessory || item.type == ItemID.None) return -1;

			var globalItem = item.GetGlobalItem<OrchidModGlobalItem>();
			if (!globalItem.shamanWeapon) return -1;

			var prefixes = ShamanPrefix.AllPrefixesByName;
			while (true)
			{
				var prefix = prefixes.ElementAt(Main.rand.Next(0, prefixes.Count)).Value;

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