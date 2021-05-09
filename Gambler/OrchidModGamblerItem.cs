using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using static Terraria.ModLoader.ModContent;
using Terraria.Utilities;

namespace OrchidMod.Gambler
{
	public abstract class OrchidModGamblerItem : OrchidModItem
	{
		public int cardRequirement = -1;
		public List<string> gamblerCardSets = new List<string>();
		
		public virtual void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false) {}
		
		public virtual void SafeSetDefaults() {}
		
		public sealed override void SetDefaults() {
			SafeSetDefaults();
			item.width = 20;
			item.height = 26;
			item.melee = false;
			item.ranged = false;
			item.magic = false;
			item.thrown = false;
			item.summon = false;
			item.noMelee = true;
			item.useStyle = 4;
			item.UseSound = SoundID.Item64;
			item.consumable = true;
			item.autoReuse = false;
			
			OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.gamblerCardRequirement = this.cardRequirement;
			orchidItem.gamblerCardSets = this.gamblerCardSets;
			orchidItem.gamblerShootDelegate = GamblerShoot;
		}

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat) {
			mult *= player.GetModPlayer<OrchidModPlayer>().gamblerDamage;
		}
		
		public override void GetWeaponCrit(Player player, ref int crit) {
			crit += player.GetModPlayer<OrchidModPlayer>().gamblerCrit;
		}
		
		public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
		{
			if (Main.rand.Next(101) <= ((OrchidModPlayer)player.GetModPlayer(mod, "OrchidModPlayer")).gamblerCrit)
                crit = true;
			else crit = false;
		}
		
		public override bool CloneNewInstances {
			get
			{
				return true;
			}
		}
		
		public override bool AltFunctionUse(Player player) {
			return true;
		}
		
		public override bool CanUseItem(Player player) {
			if (player == Main.player[Main.myPlayer]) {
				OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
				Item[] cards = modPlayer.gamblerCardsItem;
				int count = OrchidModGamblerHelper.getNbGamblerCards(player, modPlayer);
				if (OrchidModGamblerHelper.containsGamblerCard(item, player, modPlayer) || player.altFunctionUse == 2 || count < this.cardRequirement || count >= 20) {
					return false;
				} else {
					if (OrchidModGamblerHelper.getNbGamblerCards(player, modPlayer) <= 0) {
						bool found = false;
						int gamblerDeck = ItemType<Gambler.GamblerAttack>();
						for (int i = 0; i < Main.maxInventory; i++) {
							Item item = Main.LocalPlayer.inventory[i];
							if (item.type == gamblerDeck) {
								found = true;
								break;
							}
						}
						if (!found) {
							player.QuickSpawnItem(gamblerDeck, 1);
						}
					}
					item.useAnimation = 20;
					item.useTime = 20;
					for (int i = 0 ; i < 20 ; i ++) {
						if (cards[i].type == 0) {
							cards[i] = new Item();
							cards[i].SetDefaults(item.type, true);
							OrchidModGamblerHelper.clearGamblerCardCurrent(player, modPlayer);
							OrchidModGamblerHelper.clearGamblerCardsNext(player, modPlayer);
							modPlayer.gamblerShuffleCooldown = 0;
							modPlayer.gamblerRedraws = 0;
							OrchidModGamblerHelper.drawGamblerCard(player, modPlayer);
							return true;
						}
					}
					//item.TurnToAir();
				}
			}
			return base.CanUseItem(player);
		}
		
		public override bool UseItem(Player player) {
			return true;
		}

		// Since cards should not have prefixes, we do so to avoid errors with rarity
		public override bool? PrefixChance(int pre, UnifiedRandom rand) => false;

		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
			if (tt != null) {
				string[] splitText = tt.text.Split(' ');
				string damageValue = splitText.First();
				string damageWord = splitText.Last();
				tt.text = damageValue + " gambling " + damageWord;
			}
			
			Player player = Main.player[Main.myPlayer];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			Item[] cards = modPlayer.gamblerCardsItem;
			int count = OrchidModGamblerHelper.getNbGamblerCards(player, modPlayer);
			int diff = this.cardRequirement - count;
			
			int index = tooltips.FindIndex(ttip => ttip.mod.Equals("Terraria") && ttip.Name.Equals("Tooltip0"));
			if (index != -1)
			{
				int tagCount = this.gamblerCardSets.Count - 1;
				if (tagCount > -1) {
					List<string> alreadyDone = new List<string>();
					string tags = "";
					foreach(string tag in this.gamblerCardSets)
					{
						if (!alreadyDone.Contains(tag)) {
							tags += alreadyDone.Count > 0 ? ", " : "";
							tags += tag;
							tagCount --;
							alreadyDone.Add(tag);
						}
					}
					tags += alreadyDone.Count > 1 ? " sets" : " set";
					
					tooltips.Insert(index, new TooltipLine(mod, "TagsTag", tags)
					{
						overrideColor = new Color(175, 255, 175)
					});
				}
			}
			
			tooltips.Insert(1, new TooltipLine(mod, "CardsNeeded", "Requires " + this.cardRequirement + " cards (Deck : " + count + ")")
			{
				overrideColor = new Color(255, 200, 100)
			});
			
			if (OrchidModGamblerHelper.containsGamblerCard(item, player, modPlayer)) {
				tooltips.Insert(1, new TooltipLine(mod, "UseTag", "Currently in your deck")
				{
					overrideColor = new Color(255, 100, 100)
				});
			} else if (count == 20) {
				tooltips.Insert(1, new TooltipLine(mod, "UseTag", "Your deck is full")
				{
					overrideColor = new Color(255, 100, 100)
				});
			} else if (count < this.cardRequirement) {
				tooltips.Insert(1, new TooltipLine(mod, "UseTag", "Requires " + diff + " more cards")
				{
					overrideColor = new Color(255, 100, 100)
				});
			} else {
				tooltips.Insert(1, new TooltipLine(mod, "UseTag", "Use to add to your deck")
				{
					overrideColor = new Color(255, 200, 100)
				});
			}
			
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod != null) {
				tooltips.Insert(1, new TooltipLine(mod, "ClassTag", "-Gambler Class-")
				{
					overrideColor = new Color(255, 200, 0)
				});
			}
			
			tt = tooltips.FirstOrDefault(x => x.Name == "Speed" && x.mod == "Terraria");
			if (tt != null) tooltips.Remove(tt);
			
			tt = tooltips.FirstOrDefault(x => x.Name == "Consumable" && x.mod == "Terraria");
			if (tt != null) tooltips.Remove(tt);
		}
	}
}
