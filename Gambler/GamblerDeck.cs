using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using OrchidMod.Common.Attributes;
using OrchidMod.Common;

namespace OrchidMod.Gambler
{
	[ClassTag(ClassTags.Gambler)]
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
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();

			if (!initialized)
			{
				CheckStats(modPlayer.gamblerCardCurrent);
				initialized = true;
			}
			else
			{
				Item currentCard = modPlayer.gamblerCardCurrent;
				currentCard.GetGlobalItem<OrchidModGlobalItem>().gamblerShootDelegate(player, source, position, velocity, damage, knockback);
			}
			return false;
		}

		public override void HoldItem(Player player)
		{
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			modPlayer.GamblerDeckInHand = true;
			modPlayer.gamblerUIFightDisplay = true;
			if (Main.mouseLeft)
			{
				modPlayer.ShootBonusProjectiles(false);
			}
		}

		// public override void UpdateInventory(Player player) {
		// OrchidPlayer modPlayer = player.GetModPlayer<OrchidPlayer>();
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
				OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
				Item currentCard = modPlayer.gamblerCardCurrent;
				if (modPlayer.GetNbGamblerCards() > 0)
				{
					if (player.altFunctionUse == 2)
					{
						if (modPlayer.gamblerRedraws > 0 && modPlayer.gamblerRedrawCooldownUse <= 0)
						{
							modPlayer.gamblerRedraws--;
							modPlayer.gamblerRedrawCooldownUse = 30;
							SoundEngine.PlaySound(SoundID.Item64, player.position);
							modPlayer.DrawGamblerCard();
							currentCard = modPlayer.gamblerCardCurrent;
							CheckStats(currentCard);
						}
						return false;
					}
					else
					{
						if (modPlayer.gamblerShuffleCooldown <= 0)
						{
							modPlayer.DrawGamblerCard();
							SoundEngine.PlaySound(SoundID.Item64, player.position);
							currentCard = modPlayer.gamblerCardCurrent;
							CheckStats(currentCard);
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

		public void CheckStats(Item currentCard)
		{
			if (currentCard.type != ItemID.None)
			{
				//Item.damage = (int)(currentCard.damage * (modPlayer.gamblerDamage + player.GetDamage(DamageClass.Generic).Multiplicative - 1f));
				Item.damage = currentCard.damage;
				//item.rare = currentCard.rare; //
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
