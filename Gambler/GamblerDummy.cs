using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler
{
	public class GamblerDummy : OrchidModItem
	{
		public override void SetDefaults()
		{
			Item.DamageType = ModContent.GetInstance<GamblerDamageClass>();
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
			Item.rare = ItemRarityID.Blue;
			Item.shootSpeed = 1f;
			Item.shoot = 1;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 0, 2, 0);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			Item currentCard = modPlayer.gamblerCardDummy;
			if (modPlayer.GetNbGamblerCards() > 0)
			{
				if (player.altFunctionUse == 2 || modPlayer.gamblerCardDummy.type == 0)
				{
					SoundEngine.PlaySound(SoundID.Item64, player.position);
					modPlayer.DrawDummyCard();
					currentCard = modPlayer.gamblerCardDummy;
					CheckStats(currentCard);
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
			CheckStats(currentCard);
			currentCard.GetGlobalItem<OrchidModGlobalItem>().gamblerShootDelegate(player, source, position, velocity, damage, knockback, true);
			return false;
		}

		public override void HoldItem(Player player)
		{
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			modPlayer.GamblerDeckInHand = true;
			modPlayer.GamblerDummyInHand = true;
			if (Main.mouseLeft)
			{
				modPlayer.ShootBonusProjectiles(true);
			}
		}

		// public override void UpdateInventory(Player player) {
		// OrchidPlayer modPlayer = player.GetModPlayer<OrchidPlayer>();
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
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
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

		public void CheckStats(Item currentCard)
		{
			if (currentCard.type != ItemID.None)
			{
				Item.damage = currentCard.damage;
				//item.rare = currentCard.rare;
				Item.crit = currentCard.crit;
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
				//Item.rare = ItemRarityID.White;
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
