using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using OrchidMod.Gambler;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler
{
	public class GamblerAttack : ModItem
	{
		public	override void SetDefaults() {
			item.melee = false;
			item.ranged = false;
			item.magic = false;
			item.thrown = false;
			item.summon = false;
			item.noMelee = true;
			item.maxStack = 1;
			item.width = 34;
			item.height = 34;
			item.useStyle = 1;
			item.noUseGraphic = true;
			//item.UseSound = SoundID.Item7;
			item.useAnimation = 1;
			item.useTime = 1;
			item.knockBack = 1f;
			item.damage = 1;
			item.rare = 1;
			item.shootSpeed = 1f;
			item.shoot = 1;
			item.autoReuse = true;
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			Item currentCard = modPlayer.gamblerCardCurrent;
			bool firstUse = item.useAnimation == 1 && item.useTime == 1;
			if (OrchidModGamblerHelper.getNbGamblerCards(player, modPlayer) > 0) {
				if (player.altFunctionUse == 2) {
					if (modPlayer.gamblerRedraws > 0 && modPlayer.gamblerRedrawCooldownUse <= 0) {
						modPlayer.gamblerRedraws --;
						modPlayer.gamblerRedrawCooldownUse = 30;
						Main.PlaySound(SoundID.Item64, player.position);
						OrchidModGamblerHelper.drawGamblerCard(player, modPlayer);
						currentCard = modPlayer.gamblerCardCurrent;
						this.checkStats(currentCard, modPlayer);
					}
					return false;
				} else {
					if (modPlayer.gamblerShuffleCooldown <= 0) {
						OrchidModGamblerHelper.drawGamblerCard(player, modPlayer);
						Main.PlaySound(SoundID.Item64, player.position);
						currentCard = modPlayer.gamblerCardCurrent;
						this.checkStats(currentCard, modPlayer);
					}
				}
			} else {
				return false;
			}
			
			currentCard = modPlayer.gamblerCardCurrent;
			
			if (firstUse) {
				return false;
			} else {
				this.checkStats(currentCard, modPlayer);
				currentCard.GetGlobalItem<OrchidModGlobalItem>().gamblerShootDelegate(player, position, speedX, speedY, type, item.damage, item.knockBack);
			}
			return false;
		}
		
		public override void HoldItem(Player player) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.gamblerAttackInHand = true;
			if (Main.mouseLeft) {
				OrchidModGamblerHelper.ShootBonusProjectiles(player, player.Center, false);
			}
		}
		
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat) {
			mult *= player.GetModPlayer<OrchidModPlayer>().gamblerDamage;
		}
		
		public override void GetWeaponCrit(Player player, ref int crit) {
			crit += player.GetModPlayer<OrchidModPlayer>().gamblerCrit;
		}
		
		// public override void UpdateInventory(Player player) {
			// OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			// Item currentCard = modPlayer.gamblerCardCurrent;
			// this.checkStats(currentCard);
		// }
		
		public override bool AltFunctionUse(Player player) {
			return true;
		}
		
		public override bool CanUseItem(Player player) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			return base.CanUseItem(player);
		}
		
		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod != null) {
				tooltips.Insert(1, new TooltipLine(mod, "ClassTag", "-Gambler Class-")
				{
					overrideColor = new Color(255, 200, 0)
				});
			}
			Player player = Main.player[Main.myPlayer]; 
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			Item currentCard = modPlayer.gamblerCardCurrent;
			if (currentCard.type != 0) {
				int index = tooltips.FindIndex(ttip => ttip.mod.Equals("Terraria") && ttip.Name.Equals("Tooltip0"));
				if (index != -1)
				{
					tooltips.Insert(index, new TooltipLine(mod, "CardType", currentCard.Name)
					{
						overrideColor = new Color(255, 200, 0)
					});
				}
			}
		}
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gambler Deck");
		    Tooltip.SetDefault("Allows you to use your gambler abilities");
		}
		
		public void checkStats(Item currentCard, OrchidModPlayer modPlayer) {
			if (currentCard.type != 0) {
				item.damage = (int)(currentCard.damage * modPlayer.gamblerDamage);
				item.rare = currentCard.rare;
				item.crit = currentCard.crit + modPlayer.gamblerCrit;
				item.useAnimation = currentCard.useAnimation;
				item.useTime = currentCard.useTime;
				item.knockBack = currentCard.knockBack;
				item.shootSpeed = currentCard.shootSpeed;
			} else {
				item.damage = 0;
				item.rare = 0;
				item.crit = 0;
				item.useAnimation = 1;
				item.useTime = 1;
				item.knockBack = 1f;
				item.shootSpeed = 1f;
			}
		}
	}
}
