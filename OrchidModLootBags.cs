using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod
{
	public class OrchidModLootBags : GlobalItem
	{
		public override void OpenVanillaBag(string context, Player player, int arg)
		{
			if (context == "bossBag" && arg == ItemID.QueenBeeBossBag)
			{
				if (Main.rand.Next(2) == 0)
				{
					if (Main.rand.Next(2) == 0)
					{
						player.QuickSpawnItem(ItemType<Gambler.Weapons.Cards.QueenBeeCard>(), 1);
					}
					else
					{
						player.QuickSpawnItem(ItemType<Gambler.Weapons.Dice.HoneyDie>(), 1);
					}
				}

				if (Main.rand.Next(2) == 0)
				{
					int rand = Main.rand.Next(3);
					if (rand == 0)
					{
						player.QuickSpawnItem(ItemType<Shaman.Weapons.BeeSeeker>(), 1);
					}
					else if (rand == 1)
					{
						player.QuickSpawnItem(ItemType<Shaman.Accessories.WaxyVial>(), 1);
					}
					else
					{
						player.QuickSpawnItem(ItemType<Alchemist.Weapons.Air.QueenBeeFlask>(), 1);
					}
				}
			}
			if (context == "bossBag" && arg == ItemID.MoonLordBossBag)
			{
				if (Main.rand.Next(5) == 0)
				{
					if (Main.rand.Next(2) == 0) player.QuickSpawnItem(ItemType<Shaman.Weapons.Hardmode.Nirvana>(), 1);
					else player.QuickSpawnItem(ItemType<Shaman.Weapons.Hardmode.TheCore>(), 1);
				}
			}
			if (context == "bossBag" && arg == ItemID.WallOfFleshBossBag)
			{
				if (Main.rand.Next(4) == 0)
				{
					player.QuickSpawnItem(ItemType<Shaman.Accessories.ShamanEmblem>(), 1);
				}
				player.QuickSpawnItem(ItemType<General.Items.Misc.OrchidEmblem>(), 1);
			}
			if (context == "bossBag" && arg == ItemID.PlanteraBossBag)
			{
				if (Main.rand.Next(3) == 0)
				{
					if (Main.rand.Next(2) == 0) player.QuickSpawnItem(ItemType<Shaman.Weapons.Hardmode.BulbScepter>(), 1);
					else player.QuickSpawnItem(ItemType<Shaman.Accessories.FloralStinger>(), 1);
				}

				if (Main.rand.Next(20) == 0)
				{
					player.QuickSpawnItem(ItemType<General.Items.Vanity.OrnateOrchid>(), 1);
				}
			}
			if (context == "bossBag" && arg == ItemID.GolemBossBag)
			{
				if (Main.rand.Next(6) == 0)
				{
					player.QuickSpawnItem(ItemType<Shaman.Weapons.Hardmode.SunRay>(), 1);
				}
			}
			if (context == "bossBag" && arg == ItemID.KingSlimeBossBag)
			{
				if (Main.rand.Next(3) == 0)
				{
					player.QuickSpawnItem(ItemType<Alchemist.Weapons.Water.KingSlimeFlask>(), 1);
				}
				if (Main.rand.Next(3) == 0)
				{
					player.QuickSpawnItem(ItemType<Gambler.Weapons.Cards.KingSlimeCard>(), 1);
				}
			}
			if (context == "bossBag" && arg == ItemID.EaterOfWorldsBossBag)
			{
				if (Main.rand.Next(3) == 0)
				{
					player.QuickSpawnItem(ItemType<Alchemist.Accessories.PreservedCorruption>(), 1);
				}
				if (Main.rand.Next(3) == 0)
				{
					player.QuickSpawnItem(ItemType<Gambler.Weapons.Cards.EaterCard>(), 1);
				}
			}
			if (context == "bossBag" && arg == ItemID.BrainOfCthulhuBossBag)
			{
				if (Main.rand.Next(3) == 0)
				{
					player.QuickSpawnItem(ItemType<Alchemist.Accessories.PreservedCrimson>(), 1);
				}
				if (Main.rand.Next(3) == 0)
				{
					player.QuickSpawnItem(ItemType<Gambler.Weapons.Cards.BrainCard>(), 1);
				}
			}
			if (context == "bossBag" && arg == ItemID.EyeOfCthulhuBossBag)
			{
				if (Main.rand.Next(3) == 0)
				{
					player.QuickSpawnItem(ItemType<Gambler.Weapons.Cards.EyeCard>(), 1);
				}
			}
			if (context == "bossBag" && arg == ItemID.SkeletronBossBag)
			{
				player.QuickSpawnItem(ItemType<Gambler.Weapons.Cards.SkeletronCard>(), 1);
			}

			// BOXES 

			if (context == "crate" && arg == ItemID.WoodenCrate)
			{
				if (Main.rand.Next(45) == 0)
				{
					player.QuickSpawnItem(ItemType<Shaman.Weapons.AdornedBranch>(), 1);
				}
				if (Main.rand.Next(45) == 0)
				{
					player.QuickSpawnItem(ItemType<Alchemist.Weapons.Fire.EmberVial>(), 1);
				}
				if (Main.rand.Next(45) == 0)
				{
					player.QuickSpawnItem(ItemType<Gambler.Weapons.Cards.EmbersCard>(), 1);
				}
				if (Main.rand.Next(40) == 0)
				{
					player.QuickSpawnItem(ItemType<Alchemist.Weapons.Air.TsunamiInAVial>(), 1);
				}
			}
			if (context == "crate" && arg == 2335)
			{ // IRON CRATE
				if (Main.rand.Next(20) == 0)
				{
					player.QuickSpawnItem(ItemType<Gambler.Weapons.Cards.BubbleCard>(), 1);
				}
				if (Main.rand.Next(20) == 0)
				{
					player.QuickSpawnItem(ItemType<Alchemist.Weapons.Water.SeafoamVial>(), 1);
				}
				if (Main.rand.Next(20) == 0)
				{
					player.QuickSpawnItem(ItemType<Alchemist.Weapons.Air.TsunamiInAVial>(), 1);
				}

				if (Main.rand.Next(20) == 0)
				{
					player.QuickSpawnItem(ItemType<Gambler.Decks.DeckEnchanted>(), 1);
				}
			}
			if (context == "lockBox")
			{ // GOLDEN LOCK BOX
				if (Main.rand.Next(2) == 0)
				{
					player.QuickSpawnItem(ItemType<Gambler.Misc.TiamatRelic>(), 1);
				}

				if (Main.rand.Next(2) == 0)
				{
					int rand = Main.rand.Next(4);
					if (rand == 0)
					{
						player.QuickSpawnItem(ItemType<Shaman.Accessories.SpiritedWater>(), 1);
					}
					else if (rand == 1)
					{
						player.QuickSpawnItem(ItemType<Alchemist.Weapons.Water.DungeonFlask>(), 1);
					}
					else if (rand == 2)
					{
						player.QuickSpawnItem(ItemType<Alchemist.Weapons.Catalysts.DungeonCatalyst>(), 1);
					}
					else
					{
						player.QuickSpawnItem(ItemType<Gambler.Weapons.Chips.Rusalka>(), 1);
					}

					if (Main.rand.Next(20) == 0)
					{
						player.QuickSpawnItem(ItemType<Gambler.Decks.DeckBone>(), 1);
					}
				}
			}
			if (context == "crate" && arg == 3203)
			{ // CORRUPT CRATE
				if (Main.rand.Next(2) == 0)
				{
					if (Main.rand.Next(2) == 0)
					{
						player.QuickSpawnItem(ItemType<Shaman.Weapons.ShadowWeaver>(), 1);
					}
					else
					{
						player.QuickSpawnItem(ItemType<Alchemist.Weapons.Catalysts.DemoniteCatalyst>(), 1);
					}
				}
			}
			if (context == "crate" && arg == 3208)
			{ // JUNGLE CRATE
				if (Main.rand.Next(5) < 2)
				{
					if (Main.rand.Next(2) == 0)
					{
						player.QuickSpawnItem(ItemType<Shaman.Accessories.DeepForestCharm>(), 1);
					}
					else
					{
						player.QuickSpawnItem(ItemType<Alchemist.Accessories.BloomingBud>(), 1);
					}
				}

				if (Main.rand.Next(2) == 0)
				{
					player.QuickSpawnItem(ModContent.ItemType<Content.Items.Materials.JungleLily>(), 1);
				}

				if (Main.rand.Next(20) == 0)
				{
					player.QuickSpawnItem(ItemType<Gambler.Decks.DeckJungle>(), 1);
				}

				if (Main.rand.Next(5) == 0)
				{
					player.QuickSpawnItem(ItemType<Gambler.Weapons.Cards.IvyChestCard>(), 1);
				}
			}
			if (context == "crate" && arg == 3204)
			{ // CRIMSON CRATE
				if (Main.rand.Next(2) == 0)
				{
					if (Main.rand.Next(2) == 0)
					{
						player.QuickSpawnItem(ItemType<Shaman.Weapons.BloodCaller>(), 1);
					}
					else
					{
						player.QuickSpawnItem(ItemType<Alchemist.Weapons.Catalysts.CrimtaneCatalyst>(), 1);
					}
				}
			}
			if (context == "crate" && arg == 3206)
			{ // SKY CRATE
				if (Main.rand.Next(4) == 0)
				{
					player.QuickSpawnItem(ItemType<Alchemist.Weapons.Air.SunplateFlask>(), 1);
				}
			}

			// THORIUM

			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				if (context == "bossBag" && arg == thoriumMod.Find<ModItem>("ThunderBirdBag").Type)
				{
					if (Main.rand.Next(4) == 0)
					{
						player.QuickSpawnItem(ItemType<Shaman.Weapons.Thorium.ThunderScepter>(), 1);
					}
				}

				if (context == "bossBag" && arg == thoriumMod.Find<ModItem>("JellyFishBag").Type)
				{
					if (Main.rand.Next(5) == 0)
					{
						player.QuickSpawnItem(ItemType<Shaman.Weapons.Thorium.QueenJellyfishScepter>(), 1);
					}
				}

				if (context == "bossBag" && arg == thoriumMod.Find<ModItem>("GraniteBag").Type)
				{
					if (Main.rand.Next(5) == 0)
					{
						player.QuickSpawnItem(ItemType<Shaman.Weapons.Thorium.GraniteEnergyScepter>(), 1);
					}
				}

				if (context == "bossBag" && arg == thoriumMod.Find<ModItem>("CountBag").Type)
				{
					if (Main.rand.Next(7) == 0)
					{
						player.QuickSpawnItem(ItemType<Shaman.Weapons.Thorium.ViscountScepter>(), 1);
					}
					player.QuickSpawnItem(ItemType<Shaman.Misc.Thorium.ViscountMaterial>(), 30);
				}

				if (context == "bossBag" && arg == thoriumMod.Find<ModItem>("ScouterBag").Type)
				{
					if (Main.rand.Next(6) == 0)
					{
						player.QuickSpawnItem(ItemType<Shaman.Weapons.Thorium.StarScouterScepter>(), 1);
					}
				}

				if (context == "bossBag" && arg == thoriumMod.Find<ModItem>("BeholderBag").Type)
				{
					if (Main.rand.Next(5) == 0)
					{
						player.QuickSpawnItem(ItemType<Shaman.Weapons.Thorium.Hardmode.CoznixScepter>(), 1);
					}
				}

				if (context == "bossBag" && arg == thoriumMod.Find<ModItem>("BoreanBag").Type)
				{
					if (Main.rand.Next(5) == 0)
					{
						player.QuickSpawnItem(ItemType<Shaman.Weapons.Thorium.Hardmode.BoreanStriderScepter>(), 1);
					}
				}

				if (context == "bossBag" && arg == thoriumMod.Find<ModItem>("LichBag").Type)
				{
					if (Main.rand.Next(7) == 0)
					{
						player.QuickSpawnItem(ItemType<Shaman.Weapons.Thorium.Hardmode.LichScepter>(), 1);
					}
				}

				if (context == "bossBag" && arg == thoriumMod.Find<ModItem>("AbyssionBag").Type)
				{
					if (Main.rand.Next(6) == 0)
					{
						player.QuickSpawnItem(ItemType<Shaman.Weapons.Thorium.Hardmode.AbyssionScepter>(), 1);
					}
				}
			}
		}
	}
}