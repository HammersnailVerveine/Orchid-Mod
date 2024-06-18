using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.ID;
using OrchidMod.Common.UIs;
using OrchidMod.Content.Alchemist.Bag;
using OrchidMod.Common.Global.Items;

namespace OrchidMod.Content.Alchemist.UI
{

	public class AlchemistSelectKeysUIFrame : OrchidUIState
	{
		public Color backgroundColor;
		public List<Item> displayItems;
		public AlchemistElement element;
		public CalculatedStyle dimensions;
		public Point point;
		public bool releasedKey;

		public static Texture2D emptyTexture;

		public override int InsertionIndex(List<GameInterfaceLayer> layers)
			=> layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

		public override void OnInitialize()
		{
			emptyTexture = ModContent.Request<Texture2D>("OrchidMod/Content/Alchemist/UI/Textures/AlchemistKeysUIEMpty").Value;

			dimensions = GetDimensions();
			point = new Point((int)dimensions.X, (int)dimensions.Y);
			backgroundColor = Color.White;
			element = AlchemistElement.NULL;
			displayItems = new List<Item>();
			releasedKey = true;

			Width.Set(0f, 0f);
			Height.Set(0f, 0f);
			Left.Set(Main.screenWidth / 2, 0f);
			Top.Set(Main.screenHeight / 2, 0f);
			Recalculate();
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Recalculate();
			Player player = Main.LocalPlayer;
			OrchidAlchemist modPlayer = player.GetModPlayer<OrchidAlchemist>();

			Vector2 vector = (player.position + new Vector2(player.width * 0.5f, player.gravDir > 0 ? player.height - 10 + player.gfxOffY : 10 + player.gfxOffY)).Floor();
			vector = Vector2.Transform(vector - Main.screenPosition, Main.GameViewMatrix.EffectMatrix * Main.GameViewMatrix.ZoomMatrix) / Main.UIScale;

			this.Left.Set(vector.X, 0f);
			this.Top.Set(vector.Y, 0f);

			CalculatedStyle dimensions = GetDimensions();

			if (!player.dead)
			{
				if (modPlayer.alchemistSelectUIKeysDisplay && modPlayer.alchemistSelectUIKeysItem)
				{
					if (modPlayer.alchemistSelectUIKeysInitialize)
					{
						this.initUI(player, modPlayer);
						return;
					}

					if (Main.mouseLeft && Main.mouseLeftRelease)
					{
						modPlayer.alchemistSelectUIKeysDisplay = false;
						if (modPlayer.alchemistNbElements > 0)
						{
							modPlayer.alchemistShootProjectile = true;
							SoundEngine.PlaySound(SoundID.Item106);
						}
						return;
					}

					if (element == AlchemistElement.NULL)
					{
						Point point = new Point((int)dimensions.X, (int)dimensions.Y);
						Rectangle displayRectangle = new Rectangle(point.X - (emptyTexture.Width / 2), point.Y - 70, emptyTexture.Width, emptyTexture.Height);
						spriteBatch.Draw(emptyTexture, displayRectangle, backgroundColor);
					}
					else
					{
						Point point = new Point((int)dimensions.X, (int)dimensions.Y);
						int nbItems = this.displayItems.Count();
						int offSetX = -(nbItems * 16);
						int count = 0;
						foreach (Item item in this.displayItems)
						{
							if (item.type != 0)
							{
								count++;
								Texture2D itemTexture = TextureAssets.Item[item.type].Value;
								bool oddWidth = (((itemTexture.Width / 2) % 2) == 0);
								Rectangle itemRectangle = new Rectangle(point.X + offSetX + (oddWidth ? 1 : 0), point.Y - 100, oddWidth ? 28 : 30, 30);
								spriteBatch.Draw(itemTexture, itemRectangle, backgroundColor);
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, count.ToString(), new Vector2((float)(point.X + offSetX + 8), (float)(point.Y - 65)), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								offSetX += 32;
							}
						}

						int wheelValue = PlayerInput.ScrollWheelDeltaForUI;
						if (wheelValue != 0)
						{
							this.scroll(wheelValue > 0);
							this.checkInventory(wheelValue > 0, player, modPlayer);
							return;
						}

						if (releasedKey)
						{
							if (PlayerInput.Triggers.Current.Hotbar1 && count > 0)
							{
								this.brew(displayItems[0], player, modPlayer);
								return;
							}

							if (PlayerInput.Triggers.Current.Hotbar2 && count > 1)
							{
								this.brew(displayItems[1], player, modPlayer);
								return;
							}

							if (PlayerInput.Triggers.Current.Hotbar3 && count > 2)
							{
								this.brew(displayItems[2], player, modPlayer);
								return;
							}

							if (PlayerInput.Triggers.Current.Hotbar4 && count > 3)
							{
								this.brew(displayItems[3], player, modPlayer);
								return;
							}

							if (PlayerInput.Triggers.Current.Hotbar5 && count > 4)
							{
								this.brew(displayItems[4], player, modPlayer);
								return;
							}

							if (PlayerInput.Triggers.Current.Hotbar6 && count > 5)
							{
								this.brew(displayItems[5], player, modPlayer);
								return;
							}

							if (PlayerInput.Triggers.Current.Hotbar7 && count > 6)
							{
								this.brew(displayItems[6], player, modPlayer);
								return;
							}

							if (PlayerInput.Triggers.Current.Hotbar8 && count > 7)
							{
								this.brew(displayItems[7], player, modPlayer);
								return;
							}

							if (PlayerInput.Triggers.Current.Hotbar9 && count > 8)
							{
								this.brew(displayItems[8], player, modPlayer);
								return;
							}

							if (PlayerInput.Triggers.Current.Hotbar10 && count > 9)
							{
								this.brew(displayItems[9], player, modPlayer);
								return;
							}
						}
						else
						{
							if (!(PlayerInput.Triggers.Current.Hotbar1 || PlayerInput.Triggers.Current.Hotbar2 || PlayerInput.Triggers.Current.Hotbar3 ||
							PlayerInput.Triggers.Current.Hotbar4 || PlayerInput.Triggers.Current.Hotbar5 || PlayerInput.Triggers.Current.Hotbar6 ||
							PlayerInput.Triggers.Current.Hotbar7 || PlayerInput.Triggers.Current.Hotbar8 || PlayerInput.Triggers.Current.Hotbar9 ||
							PlayerInput.Triggers.Current.Hotbar10))
							{
								releasedKey = true;
							}
						}

						// if (PlayerInput.Triggers.Current.Up) {
						// this.scroll(true);
						// return;
						// }

						// if (PlayerInput.Triggers.Current.Down) {
						// this.scroll(false);
						// return;
						// }
					}
				}
			}

		}

		public void brew(Item item, Player player, OrchidAlchemist modPlayer)
		{
			if (item.type != ItemID.None)
			{
				OrchidGlobalItemPerEntity orchidItem = item.GetGlobalItem<OrchidGlobalItemPerEntity>();
				AlchemistElement element = orchidItem.alchemistElement;
				int damage = item.damage;
				int flaskType = item.type;
				int potencyCost = orchidItem.alchemistPotencyCost;
				int rightClickDust = orchidItem.alchemistRightClickDust;
				int colorR = orchidItem.alchemistColorR;
				int colorG = orchidItem.alchemistColorG;
				int colorB = orchidItem.alchemistColorB;
				bool noPotency = modPlayer.alchemistPotency < potencyCost + 1;
				bool alreadyContains = false;
				if ((int)element > 0 && (int)element < 7)
				{
					alreadyContains = modPlayer.alchemistElements[(int)element - 1];
				}

				if (alreadyContains || noPotency
				|| modPlayer.alchemistNbElements >= modPlayer.alchemistNbElementsMax
				|| element == AlchemistElement.NULL || flaskType == 0)
				{
					if (noPotency && !alreadyContains)
					{
						SoundEngine.PlaySound(SoundID.SplashWeak);
					}
					else
					{
						if (Main.rand.NextBool(2))
						{
							SoundEngine.PlaySound(SoundID.Item112);
						}
						else
						{
							SoundEngine.PlaySound(SoundID.Item111);
						}
					}
				}
				else
				{
					OrchidModAlchemistItem.playerAddFlask(player, element, flaskType, damage, potencyCost, rightClickDust, colorR, colorG, colorB);
					int rand = Main.rand.Next(3);
					switch (rand)
					{
						case 1:
							SoundEngine.PlaySound(SoundID.Item86);
							break;
						case 2:
							SoundEngine.PlaySound(SoundID.Item87);
							break;
						default:
							SoundEngine.PlaySound(SoundID.Item85);
							break;
					}

					this.scroll(true);
					this.checkInventory(true, player, modPlayer);

					for (int k = 0; k < 5; k++)
					{
						int dust = Dust.NewDust(player.Center, 10, 10, rightClickDust);
					}
				}
			}

			releasedKey = false;
		}

		public void initUI(Player player, OrchidAlchemist modPlayer)
		{
			SoundEngine.PlaySound(SoundID.Item7);
			modPlayer.alchemistSelectUIKeysInitialize = false;
			element = AlchemistElement.NULL;

			foreach (Item item in Main.LocalPlayer.GetModPlayer<PotionBagPlayer>().GetPotionsFromInventoryAndBags())
			{
				if (item.type != 0)
				{
					OrchidGlobalItemPerEntity orchidItem = item.GetGlobalItem<OrchidGlobalItemPerEntity>();
					if (orchidItem.alchemistElement != AlchemistElement.NULL)
					{
						element = orchidItem.alchemistElement;
						break;
					}
				}
			}

			this.checkInventory(true, player, modPlayer);
		}

		public void checkInventory(bool up, Player player, OrchidAlchemist modPlayer)
		{
			if (modPlayer.alchemistNbElements < modPlayer.alchemistNbElementsMax)
			{
				int elementsChecked = 0;
				this.displayItems = new List<Item>();
				while (displayItems.Count() == 0)
				{
					foreach (Item item in Main.LocalPlayer.GetModPlayer<PotionBagPlayer>().GetPotionsFromInventoryAndBags())
					{
						if (item.type != ItemID.None)
						{
							OrchidGlobalItemPerEntity orchidItem = item.GetGlobalItem<OrchidGlobalItemPerEntity>();
							if ((int)element > 0 && (int)element < 7)
							{
								if (orchidItem.alchemistElement == this.element && !modPlayer.alchemistElements[(int)element - 1])
								{
									this.displayItems.Add(item);
									if (this.displayItems.Count() > 9)
									{
										break;
									}
								}
							}
						}
					}

					if (displayItems.Count() == 0)
					{
						elementsChecked++;
						if (elementsChecked == 6)
						{
							if (modPlayer.alchemistNbElements == 0)
							{
								modPlayer.alchemistSelectUIKeysDisplay = false;
								CombatText.NewText(player.Hitbox, new Color(255, 0, 0), "No ingredients");
							}
							else
							{
								element = AlchemistElement.NULL;
							}
							return;
						}
						else
						{
							this.scroll(up);
						}
					}
				}
			}
			else
			{
				element = AlchemistElement.NULL;
			}
		}

		public void scroll(bool up)
		{
			if (up)
			{
				switch (this.element)
				{
					case AlchemistElement.WATER:
						this.element = AlchemistElement.FIRE;
						break;
					case AlchemistElement.FIRE:
						this.element = AlchemistElement.NATURE;
						break;
					case AlchemistElement.NATURE:
						this.element = AlchemistElement.AIR;
						break;
					case AlchemistElement.AIR:
						this.element = AlchemistElement.LIGHT;
						break;
					case AlchemistElement.LIGHT:
						this.element = AlchemistElement.DARK;
						break;
					case AlchemistElement.DARK:
						this.element = AlchemistElement.WATER;
						break;
				}
			}
			else
			{
				switch (this.element)
				{
					case AlchemistElement.WATER:
						this.element = AlchemistElement.DARK;
						break;
					case AlchemistElement.FIRE:
						this.element = AlchemistElement.WATER;
						break;
					case AlchemistElement.NATURE:
						this.element = AlchemistElement.FIRE;
						break;
					case AlchemistElement.AIR:
						this.element = AlchemistElement.NATURE;
						break;
					case AlchemistElement.LIGHT:
						this.element = AlchemistElement.AIR;
						break;
					case AlchemistElement.DARK:
						this.element = AlchemistElement.LIGHT;
						break;
				}
			}
		}
	}
}