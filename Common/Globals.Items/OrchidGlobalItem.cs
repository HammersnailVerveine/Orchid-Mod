using OrchidMod.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Common.Globals.Items
{
	public partial class OrchidGlobalItem : GlobalItem, IPostSetupContent
	{
		private static readonly Dictionary<int, ClassTags> tagsByItemType = new();

		// ...

		void IPostSetupContent.PostSetupContent(Mod mod)
		{
			foreach (var modItem in mod.GetContent<ModItem>())
			{
				var sacrificeCountDict = CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId;

				if (modItem.Mod.Equals(mod) && !sacrificeCountDict.ContainsKey(modItem.Type))
				{
					int sacrificeCount = GetAutoSacrificeCount(modItem);

					if (sacrificeCount > 0)
					{
						sacrificeCountDict.Add(modItem.Type, sacrificeCount);
					}
				}

				if (tagsByItemType.ContainsKey(modItem.Type)) continue;

				var atr = modItem.GetType().GetCustomAttribute<ClassTagAttribute>();

				if (atr is null) continue; 

				tagsByItemType.Add(modItem.Type, atr.Tag);
			}
		}

		public override void Unload()
		{
			tagsByItemType.Clear();
		}

		// ...

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (ModContent.GetInstance<OrchidClientConfig>().ShowClassTags && tagsByItemType.ContainsKey(item.type))
			{
				AddClassTagToTooltips(item, tooltips);
			}

			if (ModContent.GetInstance<OrchidServerConfig>().LoadCrossmodContentWithoutRequiredMods)
			{
				var atr = item.ModItem?.GetType().GetCustomAttribute<CrossmodContentAttribute>();

				if (atr is not null)
				{
					AddCrossmodInfoToTooltips(item, tooltips, atr.Mods);
				}
			}
		}

		// ...

		private void AddClassTagToTooltips(Item item, List<TooltipLine> tooltips)
		{
			var index = tooltips.FindIndex(i => i.Mod.Equals("Terraria") && i.Name.Equals("ItemName"));

			if (index < 0) return;

			var tagType = tagsByItemType[item.type];

			tooltips.Insert(index + 1, new TooltipLine(Mod, "ClassTag", GetClassTagText(tagType))
			{
				OverrideColor = OrchidColors.GetClassTagColor(tagType)
			});
		}

		private void AddCrossmodInfoToTooltips(Item item, List<TooltipLine> tooltips, string[] mods)
		{
			var text = "This is a crossmod item: " + String.Join(", ", mods);

			tooltips.Add(new TooltipLine(Mod, "CrossmodInfo", text)
			{
				OverrideColor = OrchidColors.CrossmodContentWarning
			});
		}

		// ...

		private static string GetClassTagText(ClassTags tag)
			=> $"-{tag} Class-";

		private static int GetAutoSacrificeCount(ModItem modItem)
		{
			// https://terraria.fandom.com/wiki/Journey_Mode#Research

			var item = modItem.Item;
			var itemType = item.type;

			// Heart, star, potency, ...
			if (ItemID.Sets.IsAPickup[itemType]) return 0;

			// How can I put more than 1 item in the slot?
			if (item.maxStack <= 1) return 1;

			// Consumables
			if (item.consumable)
			{
				// Placeables (Walls)
				if (item.createWall > WallID.None) return 400;

				// Placeables (Tiles)
				if (item.createTile > TileID.Dirt)
				{
					var tileType = item.createTile;

					// Platforms
					if (TileID.Sets.Platforms[tileType]) return 200;

					// Furniture
					if (!Main.tileSolid[tileType]) return 1;

					// Ores, blocks, torches, ropes, empty bullets, coins and other...
					return 100;
				}

				// Potions, food, and ...
				if (item.buffType > 0)
				{
					var buffType = item.buffType;

					// Food
					if (BuffID.Sets.IsWellFed[buffType]) return 10;

					// Potions and other...
					return 20;
				}
			}

			// Baits
			if (item.bait > 0) return 5;

			// Dyes
			if (item.dye > 0) return 3;

			// ...
			if (item.damage > 0 || item.accessory || item.useStyle > ItemUseStyleID.None ||
				item.defense > 0 || item.vanity || item.mountType > 0) return 1;

			// If nothing fits the condition, crafting materials?
			return 25;
		}
	}
}