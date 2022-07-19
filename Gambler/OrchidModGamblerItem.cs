using Microsoft.Xna.Framework;
using OrchidMod.Common;
using OrchidMod.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace OrchidMod.Gambler
{
	[ClassTag(ClassTags.Gambler)]
	public abstract class OrchidModGamblerItem : ModItem
	{
		public int cardRequirement = -1;
		public GamblerCardSets cardSets;

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

			this.SafeSetDefaults();

			OrchidModGlobalItem orchidItem = Item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.gamblerCardRequirement = this.cardRequirement;
			orchidItem.gamblerCardSets = cardSets;
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
								OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
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
				string damageWord = splitText.Last();
				tt.Text = damageValue + " gambling " + damageWord;
			}

			Player player = Main.player[Main.myPlayer];
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			Item[] cards = modPlayer.gamblerCardsItem;
			int count = modPlayer.GetNbGamblerCards();
			int diff = this.cardRequirement - count;

			AddCardSetsTooltipLine(tooltips);

			tooltips.Insert(1, new TooltipLine(Mod, "CardsNeeded", "Requires " + this.cardRequirement + " cards (Deck : " + count + ")")
			{
				OverrideColor = new Color(255, 200, 100)
			});

			if (modPlayer.ContainsGamblerCard(Item))
			{
				tooltips.Insert(1, new TooltipLine(Mod, "UseTag", "Currently in your deck")
				{
					OverrideColor = new Color(255, 100, 100)
				});
			}
			else if (count == 20)
			{
				tooltips.Insert(1, new TooltipLine(Mod, "UseTag", "Your deck is full")
				{
					OverrideColor = new Color(255, 100, 100)
				});
			}
			else if (count < this.cardRequirement)
			{
				tooltips.Insert(1, new TooltipLine(Mod, "UseTag", "Requires " + diff + " more cards")
				{
					OverrideColor = new Color(255, 100, 100)
				});
			}
			else
			{
				tooltips.Insert(1, new TooltipLine(Mod, "UseTag", "Use to add to your deck")
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
			if (cardSets.Equals(GamblerCardSets.Without)) return;

			int index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Tooltip0")); // Ok...

			if (index < 0) return;

			var hexColor = Colors.AlphaDarken(new Color(175, 255, 175)).Hex3();
			var strCardSets = cardSets.ToString();

			tooltips.Insert(index, new TooltipLine(Mod, "TagsTag", $"{(strCardSets.Split(", ").Length > 1 ? "Sets" : "Set")}: [c/{hexColor}:{strCardSets}]"));
		}

		public int DummyProjectile(int proj, bool dummy) => OrchidGambler.DummyProjectile(proj, dummy);
	}
}
