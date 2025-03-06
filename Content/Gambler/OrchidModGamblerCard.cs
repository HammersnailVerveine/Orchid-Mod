using Microsoft.Xna.Framework;
using OrchidMod.Common;
using OrchidMod.Common.Attributes;
using OrchidMod.Common.Global.Items;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace OrchidMod.Content.Gambler
{
	[ClassTag(ClassTags.Gambler)]
	public abstract class OrchidModGamblerCard : ModItem
	{
		public int cardRequirement = -1;
		public List<GamblerCardSet> cardSets;

		public virtual void GamblerShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockBack, bool dummy = false) { }

		public virtual void SafeSetDefaults() { }

		public sealed override void SetDefaults()
		{
			Item.DamageType = ModContent.GetInstance<GamblerDamageClass>();
			Item.width = 20;
			Item.height = 26;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.UseSound = SoundID.Item64;
			Item.consumable = true;
			Item.autoReuse = false;
			cardSets = new List<GamblerCardSet>();

			this.SafeSetDefaults();

			OrchidGlobalItemPerEntity orchidItem = Item.GetGlobalItem<OrchidGlobalItemPerEntity>();
			orchidItem.gamblerCardRequirement = this.cardRequirement;
			orchidItem.gamblerShootDelegate = this.GamblerShoot;
		}
		/*
		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			damage *= player.GetModPlayer<OrchidPlayer>().gamblerDamage;
		}

		public override void ModifyWeaponCrit(Player player, ref float crit)
		{
			crit += player.GetModPlayer<OrchidPlayer>().gamblerCrit;
		}
		*/

		/*
		public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
		{
			if (Main.rand.Next(101) <= player.GetModPlayer<OrchidPlayer>().gamblerCrit)
				crit = true;
			else crit = false;
		}
		*/

		protected override bool CloneNewInstances => true;
		public override bool AltFunctionUse(Player player) => true;

		public override bool CanUseItem(Player player)
		{
			if (player == Main.player[Main.myPlayer])
			{
				OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
				Item[] cards = modPlayer.gamblerCardsItem;
				int count = modPlayer.GetNbGamblerCards();
				if (modPlayer.ContainsGamblerCard(Item) || player.altFunctionUse == 2 || count < this.cardRequirement || count >= 20)
				{
					return false;
				}
				else
				{
					if (modPlayer.GetNbGamblerCards() <= 0)
					{
						bool found = false;
						for (int i = 0; i < Main.InventorySlotsTotal; i++)
						{
							Item item = Main.LocalPlayer.inventory[i];
							if (item.type != 0)
							{
								OrchidGlobalItemPerEntity orchidItem = item.GetGlobalItem<OrchidGlobalItemPerEntity>();
								if (orchidItem.gamblerDeck)
								{
									found = true;
									break;
								}
							}
						}
						if (!found)
						{
							int gamblerDeck = ModContent.ItemType<Decks.GamblerAttack>();
							player.QuickSpawnItem(player.GetSource_GiftOrReward("Gambler Deck"), gamblerDeck, 1);
						}
					}
					Item.useAnimation = 20;
					Item.useTime = 20;
					for (int i = 0; i < 20; i++)
					{
						if (cards[i].type == 0)
						{
							cards[i] = new Item();
							cards[i].SetDefaults(Item.type, true);
							modPlayer.ClearGamblerCardCurrent();
							modPlayer.ClearGamblerCardsNext();
							modPlayer.gamblerShuffleCooldown = 0;
							modPlayer.gamblerRedraws = 0;
							modPlayer.DrawGamblerCard();
							return true;
						}
					}
					//item.TurnToAir();
				}
			}
			return base.CanUseItem(player);
		}

		public override bool? UseItem(Player player)/* Suggestion: Return null instead of false */
		{
			return true;
		}

		// Since cards should not have prefixes, we do so to avoid errors with rarity
		public override bool? PrefixChance(int pre, UnifiedRandom rand) => false;

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
			if (tt != null)
			{
				string[] splitText = tt.Text.Split(' ');
				string damageValue = splitText.First();
				tt.Text = damageValue + " " + Language.GetTextValue(ModContent.GetInstance<OrchidMod>().GetLocalizationKey("DamageClasses.GamblerDamageClass.DisplayName"));
			}

			Player player = Main.player[Main.myPlayer];
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			Item[] cards = modPlayer.gamblerCardsItem;
			int count = modPlayer.GetNbGamblerCards();
			int diff = this.cardRequirement - count;

			AddCardSetsTooltipLine(tooltips);

			tooltips.Insert(1, new TooltipLine(Mod, "CardsNeeded", Language.GetTextValue("Mods.OrchidMod.UI.GamblerItem.Requires", this.cardRequirement, count))
			{
				OverrideColor = new Color(255, 200, 100)
			});

			if (modPlayer.ContainsGamblerCard(Item))
			{
				tooltips.Insert(1, new TooltipLine(Mod, "UseTag", Language.GetTextValue("Mods.OrchidMod.UI.GamblerItem.InDeck"))
				{
					OverrideColor = new Color(255, 100, 100)
				});
			}
			else if (count == 20)
			{
				tooltips.Insert(1, new TooltipLine(Mod, "UseTag", Language.GetTextValue("Mods.OrchidMod.UI.GamblerItem.DeckIsFull"))
				{
					OverrideColor = new Color(255, 100, 100)
				});
			}
			else if (count < this.cardRequirement)
			{
				tooltips.Insert(1, new TooltipLine(Mod, "UseTag", Language.GetTextValue("Mods.OrchidMod.UI.GamblerItem.RequiresMore", diff))
				{
					OverrideColor = new Color(255, 100, 100)
				});
			}
			else
			{
				tooltips.Insert(1, new TooltipLine(Mod, "UseTag", Language.GetTextValue("Mods.OrchidMod.UI.GamblerItem.AddToDeck"))
				{
					OverrideColor = new Color(255, 200, 100)
				});
			}

			tt = tooltips.FirstOrDefault(x => x.Name == "Speed" && x.Mod == "Terraria");
			if (tt != null) tooltips.Remove(tt);

			tt = tooltips.FirstOrDefault(x => x.Name == "Consumable" && x.Mod == "Terraria");
			if (tt != null) tooltips.Remove(tt);
		}

		private void AddCardSetsTooltipLine(List<TooltipLine> tooltips)
		{
			if (cardSets.Count == 0) return;

			int index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Tooltip0")); // Ok...

			if (index < 0) return;

			var hexColor = Colors.AlphaDarken(new Color(175, 255, 175)).Hex3();
			var strCardSets = "";

			foreach (GamblerCardSet cardSet in cardSets) strCardSets += ", " + Language.GetTextValue("Mods.OrchidMod.UI.GamblerItem.Set" + cardSet.ToString());
			strCardSets = strCardSets.Remove(0, 2);

			tooltips.Insert(index, new TooltipLine(Mod, "TagsTag", $"{(strCardSets.Split(", ").Length > 1 ? Language.GetTextValue("Mods.OrchidMod.UI.GamblerItem.Sets") : Language.GetTextValue("Mods.OrchidMod.UI.GamblerItem.Set"))}: [c/{hexColor}:{strCardSets}]"));
		}

		public int DummyProjectile(int proj, bool dummy) => OrchidGambler.DummyProjectile(proj, dummy);
	}
}
