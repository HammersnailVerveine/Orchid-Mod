using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler
{
	public abstract class GamblerDeck : OrchidModItem
	{
		private bool initialized = false;

		public virtual void SafeSetDefaults() { }

		public sealed override void SetDefaults()
		{
			Item.DamageType = ModContent.GetInstance<GamblerDamageClass>();
			Item.width = 34;
			Item.height = 34;
			SafeSetDefaults();
			Item.noMelee = true;
			Item.maxStack = 1;
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
			OrchidModGlobalItem orchidItem = Item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.gamblerDeck = true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			if (!initialized)
			{
				this.checkStats(modPlayer.gamblerCardCurrent, player, modPlayer);
				initialized = true;
			}
			else
			{
				Item currentCard = modPlayer.gamblerCardCurrent;
				currentCard.GetGlobalItem<OrchidModGlobalItem>().gamblerShootDelegate(player, source, position, velocity, type, damage, knockback);
			}
			return false;
		}

		public override void HoldItem(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.GamblerDeckInHand = true;
			modPlayer.gamblerUIFightDisplay = true;
			if (Main.mouseLeft)
			{
				OrchidModGamblerHelper.ShootBonusProjectiles(player, player.Center, false);
			}
		}

		// public override void UpdateInventory(Player player) {
		// OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
		// Item currentCard = modPlayer.gamblerCardCurrent;
		// this.checkStats(currentCard, player, modPlayer);
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
							SoundEngine.PlaySound(SoundID.Item64, player.position);
							OrchidModGamblerHelper.drawGamblerCard(player, modPlayer);
							currentCard = modPlayer.gamblerCardCurrent;
							this.checkStats(currentCard, player, modPlayer);
						}
						return false;
					}
					else
					{
						if (modPlayer.gamblerShuffleCooldown <= 0)
						{
							OrchidModGamblerHelper.drawGamblerCard(player, modPlayer);
							SoundEngine.PlaySound(SoundID.Item64, player.position);
							currentCard = modPlayer.gamblerCardCurrent;
							this.checkStats(currentCard, player, modPlayer);
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

		public void checkStats(Item currentCard, Player player, OrchidModPlayer modPlayer)
		{
			if (currentCard.type != ItemID.None)
			{
				//Item.damage = (int)(currentCard.damage * (modPlayer.gamblerDamage + player.GetDamage(DamageClass.Generic).Multiplicative - 1f));
				Item.damage = currentCard.damage;
				//item.rare = currentCard.rare; //
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
				//item.rare = ItemRarityID.White;
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
