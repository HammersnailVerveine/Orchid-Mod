using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler
{
	public class GamblerDummy : OrchidModItem
	{
		public override void SetDefaults()
		{
			Item.melee = false;
			Item.ranged = false;
			Item.magic = false;
			Item.thrown = false;
			Item.summon = false;
			Item.noMelee = true;
			Item.maxStack = 1;
			Item.width = 34;
			Item.height = 34;
			Item.useStyle = 1;
			Item.noUseGraphic = true;
			//item.UseSound = SoundID.Item7;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.knockBack = 1f;
			Item.damage = 1;
			Item.rare = 1;
			Item.shootSpeed = 1f;
			Item.shoot = 1;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 0, 2, 0);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			Item currentCard = modPlayer.gamblerCardDummy;
			if (OrchidModGamblerHelper.getNbGamblerCards(player, modPlayer) > 0)
			{
				if (player.altFunctionUse == 2 || modPlayer.gamblerCardDummy.type == 0)
				{
					SoundEngine.PlaySound(SoundID.Item64, player.position);
					OrchidModGamblerHelper.drawDummyCard(player, modPlayer);
					currentCard = modPlayer.gamblerCardDummy;
					this.checkStats(currentCard, player, modPlayer);
					Color floatingTextColor = new Color(255, 200, 0);
					CombatText.NewText(player.Hitbox, floatingTextColor, modPlayer.gamblerCardDummy.Name);
					return false;
				}
			}
			else
			{
				return false;
			}

			currentCard = modPlayer.gamblerCardDummy;
			this.checkStats(currentCard, player, modPlayer);
			currentCard.GetGlobalItem<OrchidModGlobalItem>().gamblerShootDelegate(player, position, speedX, speedY, type, Item.damage, Item.knockBack, true);
			return false;
		}

		public override void HoldItem(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.GamblerDeckInHand = true;
			if (Main.mouseLeft)
			{
				OrchidModGamblerHelper.ShootBonusProjectiles(player, player.Center, true);
			}
		}

		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			mult *= player.GetModPlayer<OrchidModPlayer>().gamblerDamage;
		}

		public override void ModifyWeaponCrit(Player player, ref float crit)
		{
			crit += player.GetModPlayer<OrchidModPlayer>().gamblerCrit;
		}

		// public override void UpdateInventory(Player player) {
		// OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
		// Item currentCard = modPlayer.gamblerCardCurrent;
		// this.checkStats(currentCard);
		// }

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player == Main.LocalPlayer)
			{
				if (player.altFunctionUse == 2)
				{
					Item.useAnimation = 20;
					Item.useTime = 20;
					Item.reuseDelay = 0;
				}
			}
			return base.CanUseItem(player);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				tooltips.Insert(1, new TooltipLine(Mod, "ClassTag", "-Gambler Class-")
				{
					OverrideColor = new Color(255, 200, 0)
				});
			}

			Player player = Main.player[Main.myPlayer];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			Item currentCard = modPlayer.gamblerCardCurrent;

			if (currentCard.type != ItemID.None)
			{
				int index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Tooltip0"));
				if (index != -1)
				{
					Color textColor = new Color(255, 200, 0); // Rarity Color ???
					string text = $"Current card: [c/{Terraria.ID.Colors.AlphaDarken(textColor).Hex3()}:{currentCard.HoverName.Replace("Playing Card : ", "")}]";

					tooltips.Insert(index, new TooltipLine(Mod, "CardType", text));
				}
			}
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gambler Dummy Card");
			Tooltip.SetDefault("Allows the use of specific gambler cards"
							+ "\nRight click to cycle through your deck"
							+ "\nCan only deal damage to dummies"
							+ "\nUtility cards may have no effect");
		}

		public void checkStats(Item currentCard, Player player, OrchidModPlayer modPlayer)
		{
			if (currentCard.type != ItemID.None)
			{
				Item.damage = (int)(currentCard.damage * (modPlayer.gamblerDamage + player.allDamage - 1f));
				//item.rare = currentCard.rare;
				Item.crit = currentCard.crit + modPlayer.gamblerCrit;
				Item.useAnimation = currentCard.useAnimation;
				Item.useTime = currentCard.useTime;
				Item.reuseDelay = currentCard.reuseDelay;
				Item.knockBack = currentCard.knockBack;
				Item.shootSpeed = currentCard.shootSpeed;
				Item.channel = currentCard.channel;
			}
			else
			{
				Item.damage = 0;
				//item.rare = 0;
				Item.crit = 0;
				Item.useAnimation = 1;
				Item.useTime = 1;
				Item.reuseDelay = 1;
				Item.knockBack = 1f;
				Item.shootSpeed = 1f;
				Item.channel = false;
			}
		}
	}
}
