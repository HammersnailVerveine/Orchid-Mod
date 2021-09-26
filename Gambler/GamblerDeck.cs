using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler
{
	public abstract class GamblerDeck : OrchidModItem
	{
		public virtual void SafeSetDefaults() { }

		public sealed override void SetDefaults()
		{
			SafeSetDefaults();
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
			item.useAnimation = 30;
			item.useTime = 30;
			item.knockBack = 1f;
			item.damage = 1;
			item.rare = 1;
			item.shootSpeed = 1f;
			item.shoot = 1;
			item.autoReuse = true;
			OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.gamblerDeck = true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			Item currentCard = modPlayer.gamblerCardCurrent;
			currentCard.GetGlobalItem<OrchidModGlobalItem>().gamblerShootDelegate(player, position, speedX, speedY, type, item.damage, item.knockBack);
			return false;
		}

		public override void HoldItem(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.GamblerDeckInHand = true;
			modPlayer.gamblerUIFightDisplay = true;
			if (Main.mouseLeft)
			{
				this.checkStats(modPlayer.gamblerCardCurrent, modPlayer);
				OrchidModGamblerHelper.ShootBonusProjectiles(player, player.Center, false);
			}
		}

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			mult *= player.GetModPlayer<OrchidModPlayer>().gamblerDamage;
		}

		public override void GetWeaponCrit(Player player, ref int crit)
		{
			crit += player.GetModPlayer<OrchidModPlayer>().gamblerCrit;
		}

		// public override void UpdateInventory(Player player) {
		// OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
		// Item currentCard = modPlayer.gamblerCardCurrent;
		// this.checkStats(currentCard, modPlayer);
		// }

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player == Main.LocalPlayer)
			{
				OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
				Item currentCard = modPlayer.gamblerCardCurrent;
				if (OrchidModGamblerHelper.getNbGamblerCards(player, modPlayer) > 0)
				{
					if (player.altFunctionUse == 2)
					{
						if (modPlayer.gamblerRedraws > 0 && modPlayer.gamblerRedrawCooldownUse <= 0)
						{
							modPlayer.gamblerRedraws--;
							modPlayer.gamblerRedrawCooldownUse = 30;
							Main.PlaySound(SoundID.Item64, player.position);
							OrchidModGamblerHelper.drawGamblerCard(player, modPlayer);
							currentCard = modPlayer.gamblerCardCurrent;
							this.checkStats(currentCard, modPlayer);
						}
						return false;
					}
					else
					{
						if (modPlayer.gamblerShuffleCooldown <= 0)
						{
							OrchidModGamblerHelper.drawGamblerCard(player, modPlayer);
							Main.PlaySound(SoundID.Item64, player.position);
							currentCard = modPlayer.gamblerCardCurrent;
							this.checkStats(currentCard, modPlayer);
						}
					}
				}
				else
				{
					return false;
				}
			}
			return base.CanUseItem(player);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				tooltips.Insert(1, new TooltipLine(mod, "ClassTag", "-Gambler Class-")
				{
					overrideColor = new Color(255, 200, 0)
				});
			}

			Player player = Main.player[Main.myPlayer];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			Item currentCard = modPlayer.gamblerCardCurrent;

			if (currentCard.type != ItemID.None)
			{
				int index = tooltips.FindIndex(ttip => ttip.mod.Equals("Terraria") && ttip.Name.Equals("Tooltip0"));
				if (index != -1)
				{
					Color textColor = new Color(255, 200, 0); // Rarity Color ???
					string text = $"Current card: [c/{Terraria.ID.Colors.AlphaDarken(textColor).Hex3()}:{currentCard.HoverName.Replace("Playing Card : ", "")}]";

					tooltips.Insert(index, new TooltipLine(mod, "CardType", text));
				}
			}
		}

		public void checkStats(Item currentCard, OrchidModPlayer modPlayer)
		{
			if (currentCard.type != ItemID.None)
			{
				item.damage = (int)(currentCard.damage * modPlayer.gamblerDamage);
				//item.rare = currentCard.rare; //
				item.crit = currentCard.crit + modPlayer.gamblerCrit;
				item.useAnimation = currentCard.useAnimation;
				item.useTime = currentCard.useTime;
				item.reuseDelay = currentCard.reuseDelay;
				item.knockBack = currentCard.knockBack;
				item.shootSpeed = currentCard.shootSpeed;
				item.channel = currentCard.channel;
			}
			else
			{
				item.damage = 0;
				//item.rare = ItemRarityID.White;
				item.crit = 0;
				item.useAnimation = 1;
				item.useTime = 1;
				item.reuseDelay = 1;
				item.knockBack = 1f;
				item.shootSpeed = 1f;
				item.channel = false;
			}
		}
	}
}
