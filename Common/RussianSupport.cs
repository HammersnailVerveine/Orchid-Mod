using Terraria.Localization;
using Terraria.ModLoader;
using System.Collections.Generic;
using OrchidMod.Content.Guardian.Armors.Misc;
using OrchidMod.Content.Guardian.Armors.Bamboo;
using OrchidMod.Content.Guardian.Armors.Meteorite;
using OrchidMod.Content.Guardian.Armors.OreHelms;
using OrchidMod.Content.Guardian.Armors.Empress;
using OrchidMod.Content.Guardian.Armors.Horizon;
using OrchidMod.Content.Shapeshifter.Armors.Harpy;
using OrchidMod.Content.Shapeshifter.Armors.Ashwood;

namespace OrchidMod
{
	internal class RussianSupport : ModSystem
	{
		public override void PostSetupContent()
		{
			var orchid = Mod;

			if (!ModLoader.TryGetMod("CalamityRuTranslate", out Mod tru))
				return;

			//It would be better to add it directly to Orchid Mineshaft
			ModLoader.TryGetMod("OrchidMineshaft", out Mod orchidMineshaft);
			if (orchidMineshaft != null)
			{
				tru.Call("AddFeminineItems", orchidMineshaft, new[]
				{
					"MiniatureTools",
					"MineshaftPickaxe"
				});
			}

			tru.Call("AddFeminineItems", orchid, new[]
			{
				//Guardian accessories
				"BadgeBattlesPast",
				"GuardianEmblem",
				"GuardianTest",
				"HeavyChain",
				"StaffRocketBlue",
				"StaffRocketGreen",
				"StaffRocketRed",
				"StaffRocketYellow",
				"SturdySlab",
				//Shapeshifter accessories
				"HarnessYouxia",
				"ShapeshifterEmblem",
				"ShawlFeather",
				"ShawlOfTheWind",
				"ShawlPhoenix",
				//Guardian weapons
				"PaladinGauntlet",
				"ThoriumGraniteGauntlet",
				"GuardianNeedle",
				"PresentQuarterstaff",
				"ShardQuarterstaff",
				"SpectreQuarterstaff",
				"VerveineQuarterstaff",
				"BeeRune",
				"EnchantedRune",
				"FrostRune",
				"GoblinRune",
				"LivingRune",
				"MoonLordRune",
				"RuneRune",
				"AdamantiteShield",
				"AshWoodPavise",
				"ChlorophyteShield",
				"CrimtaneShield",
				"DemoniteShield",
				"IronPavise",
				"LeadPavise",
				"MahoganyPavise",
				"MeteoriteShield",
				"NightShield",
				"PearlwoodPavise",
				"SpectreShield",
				"TitaniumShield",
				"TrashPavise",
				"WoodenPavise",
				"IceStandard",
				"JungleWarhammer",
				"PumpkingWarhammer",
				//Shapeshifter weapons
				"PredatorFossil",
				"PredatorIceFox",
				"PredatorUndine",
				"SymbioteToad",
				//Other
				"HorizonPickaxe"
			});

			tru.Call("AddNeuterItems", orchid, new[]
			{
				//Guardian accessories
				"GuideShield",
				"GuideShieldBounce",
				"ParryingMailNinja",
				//Shapeshifter accessories
				"GuideTorches",
				"GuideTorchesInactive",
				//Guardian weapons
				"EmpressRune",
				"SpiderGauntlet",
				"ThoriumIllusionistPavise",
				"ThoriumLeafShield",
				"PlanteraStandard",
				"CrimsonWarhammer",
				//Shapeshifter weapons
				"PredatorHarpy",
				"SageBee",
				"SageCorruption",
				"SageCrimson",
				"SageImp",
				"WardenEater"
			});

			tru.Call("AddPluralItems", orchid, new[]
			{
				//Guardian accessories
				"ParryingMailFeral",
				"ParryingMailHoly",
				//Shapeshifter accessories
				"PlantEnzymes",
				//Guardian weapons
				"CactusGauntlet",
				"CobaltGauntlet",
				"CrystalGauntlet",
				"DukeGauntlet",
				"GlowingMushroomGauntlet",
				"JewelerGauntlet",
				"JungleGauntlet",
				"PalladiumGauntlet",
				"SilverGauntlet",
				"ThoriumDarksteelGauntlet",
				"ThoriumDeadwoodGauntlet",
				"ThoriumYewGauntlet",
				"TungstenGauntlet"
			});

			var prefixes = new List<string[]>
			{
                //Accessories
                new[] { "Сваренный", "Сваренная", "Сверенное", "Сверенные" },
				new[] { "Обеспеченный", "Обеспеченная", "Обеспеченное", "Обеспеченные" },
				new[] { "Блокирующий", "Блокирующая", "Блокирующее", "Блокирующие" },
                //Guardian
                new[] { "Непонятный", "Непонятная", "Непонятное", "Непонятные" },
				new[] { "Хлипкий", "Хлипкая", "Хлипкое", "Хлипкие" },
				new[] { "Хилый", "Хилая", "Хилое", "Хилые" },
				new[] { "Ветхий", "Ветхая", "Ветхое", "Ветхие" },
				new[] { "Твёрдый", "Твёрдая", "Твёрдое", "Твёрдые" },
				new[] { "Прочный", "Прочная", "Прочное", "Прочные" },
				new[] { "Несгибаемый", "Несгибаемая", "Несгибаемое", "Несгибаемые" },
				new[] { "Укреплённый", "Укреплённая", "Укреплённое", "Укреплённые" },
				new[] { "Неразрушимый", "Неразрушимая", "Неразрушимое", "Неразрушимые" },
				new[] { "Неприступный", "Неприступная", "Неприступное", "Неприступные" },
				new[] { "Превосходящий", "Превосходящая", "Превосходящее", "Превосходящие" },
				new[] { "Спартанский", "Спартанская", "Спартанское", "Спартанские" },
				new[] { "Ангельский", "Ангельская", "Ангельское", "Ангельские" },
				new[] { "Громадный", "Громадная", "Громадное", "Громадные" },
				new[] { "Возвышенный", "Возвышенная", "Возвышенное", "Возвышенные" },
                //Shapeshifter
                new[] { "Робкий", "Робкая", "Робкое", "Робкие" },
				new[] { "Кабанский", "Кабанская", "Кабанское", "Кабанские" },
				new[] { "Деформированный", "Деформированная", "Деформированное", "Деформированные" },
				new[] { "Разъярённый", "Разъярённая", "Разъярённое", "Разъярённые" },
				new[] { "Зверский", "Зверская", "Зверское", "Зверские" },
				new[] { "Прожорливый", "Прожорливая", "Прожорливое", "Прожорливые" },
				new[] { "Неукротимый", "Неукротимая", "Неукротимое", "Неукротимые" },
				new[] { "Лютый", "Лютая", "Лютое", "Лютые" },
				new[] { "Звериный", "Звериная", "Звериное", "Звериные" },
				new[] { "Монструозный", "Монструозная", "Монструозное", "Монструозные" },
				new[] { "Первобытный", "Первобытная", "Первобытное", "Первобытные" },
				new[] { "Святой", "Святая", "Святое", "Святые" }
			};
			tru.Call("RegisterPrefixes", prefixes);

			tru.Call("AddArmorSetBonusPreview", ModContent.ItemType<GuardianGitHelm>(), () =>
				Language.GetTextValue("Mods.OrchidMod.Items.GuardianGitHelm.SetBonus"));

			tru.Call("AddArmorSetBonusPreview", ModContent.ItemType<GuardianBambooHead>(), () =>
				Language.GetTextValue("Mods.OrchidMod.Items.GuardianBambooHead.SetBonus"));

			tru.Call("AddArmorSetBonusPreview", ModContent.ItemType<GuardianMeteoriteHead>(), () =>
				Language.GetTextValue("Mods.OrchidMod.Items.GuardianMeteoriteHead.SetBonus"));

			tru.Call("AddArmorSetBonusPreview", ModContent.ItemType<GuardianCrystalNinjaHelm>(), () =>
				Language.GetTextValue("ArmorSetBonus.CrystalNinja"));

			tru.Call("AddArmorSetBonusPreview", ModContent.ItemType<GuardianCobaltHead>(), () =>
				Language.GetTextValue("Mods.OrchidMod.Items.GuardianCobaltHead.SetBonus"));

			tru.Call("AddArmorSetBonusPreview", ModContent.ItemType<GuardianPalladiumHead>(), () =>
				Language.GetTextValue("ArmorSetBonus.Palladium"));

			tru.Call("AddArmorSetBonusPreview", ModContent.ItemType<GuardianMythrilHead>(), () =>
				Language.GetTextValue("Mods.OrchidMod.Items.GuardianMythrilHead.SetBonus"));

			tru.Call("AddArmorSetBonusPreview", ModContent.ItemType<GuardianOrichalcumHead>(), () =>
				Language.GetTextValue("ArmorSetBonus.Orichalcum"));

			tru.Call("AddArmorSetBonusPreview", ModContent.ItemType<GuardianAdamantiteHead>(), () =>
				Language.GetTextValue("Mods.OrchidMod.Items.GuardianAdamantiteHead.SetBonus"));

			tru.Call("AddArmorSetBonusPreview", ModContent.ItemType<GuardianTitaniumHead>(), () =>
				Language.GetTextValue("ArmorSetBonus.Titanium"));

			tru.Call("AddArmorSetBonusPreview", ModContent.ItemType<GuardianHallowedHead>(), () =>
				Language.GetTextValue("ArmorSetBonus.Hallowed"));

			tru.Call("AddArmorSetBonusPreview", ModContent.ItemType<GuardianAncientHallowedHead>(), () =>
				Language.GetTextValue("ArmorSetBonus.Hallowed"));

			tru.Call("AddArmorSetBonusPreview", ModContent.ItemType<GuardianChlorophyteHead>(), () =>
				Language.GetTextValue("ArmorSetBonus.Chlorophyte"));

			tru.Call("AddArmorSetBonusPreview", ModContent.ItemType<GuardianEmpressHead>(), () =>
				"Если надеты [i:OrchidMod/GuardianEmpressChest] латы валькирии рассвета:\n" +
				Language.GetTextValue("Mods.OrchidMod.Items.GuardianEmpressChest.SetBonus") + "\n" +
				"Если надет [i:OrchidMod/GuardianEmpressChestAlt] доспех надзирателя рассвета:\n" +
				Language.GetTextValue("Mods.OrchidMod.Items.GuardianEmpressChestAlt.SetBonus"));

			tru.Call("AddArmorSetBonusPreview", ModContent.ItemType<GuardianHorizonHead>(), () =>
				Language.GetTextValue("Mods.OrchidMod.Items.GuardianHorizonHead.SetBonus"));

			tru.Call("AddArmorSetBonusPreview", ModContent.ItemType<ShapeshifterHarpyHead>(), () =>
				Language.GetTextValue("Mods.OrchidMod.Items.ShapeshifterHarpyHead.SetBonus"));

			tru.Call("AddArmorSetBonusPreview", ModContent.ItemType<ShapeshifterAshwoodHead>(), () =>
				Language.GetTextValue("Mods.OrchidMod.Items.ShapeshifterAshwoodHead.SetBonus"));
		}
	}
}
