using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace OrchidMod.Alchemist.UI
{
	public class AlchemistSelectKeysUIState : UIState
	{
		public AlchemistSelectKeysUIFrame alchemistSelectKeysUIFrame = new AlchemistSelectKeysUIFrame();

		public override void OnInitialize()
		{
			alchemistSelectKeysUIFrame.Width.Set(0f, 0f);
			alchemistSelectKeysUIFrame.Height.Set(0f, 0f);
			alchemistSelectKeysUIFrame.Left.Set(Main.screenWidth / 2, 0f);
			alchemistSelectKeysUIFrame.Top.Set(Main.screenHeight / 2, 0f);

			base.Append(alchemistSelectKeysUIFrame);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Recalculate();
			base.Draw(spriteBatch);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}
	}

	public class AlchemistSelectKeysUIFrame : UIElement
	{
		public Color backgroundColor;
		public List<Item> displayItems;
		public AlchemistElement element;
		public CalculatedStyle dimensions;
		public Point point;
		public bool releasedKey;

		public static Texture2D emptyTexture;

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			Player player = Main.LocalPlayer;
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

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
							Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 106);
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
								Texture2D itemTexture = Main.itemTexture[item.type];
								bool oddWidth = (((itemTexture.Width / 2) % 2) == 0);
								Rectangle itemRectangle = new Rectangle(point.X + offSetX + (oddWidth ? 1 : 0), point.Y - 100, oddWidth ? 28 : 30, 30);
								spriteBatch.Draw(itemTexture, itemRectangle, backgroundColor);
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, count.ToString(), new Vector2((float)(point.X + offSetX + 8), (float)(point.Y - 65)), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
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

		public void brew(Item item, Player player, OrchidModPlayer modPlayer)
		{
			if (item.type != 0)
			{
				OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
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
						Main.PlaySound(19, (int)player.Center.X, (int)player.Center.Y, 1);
					}
					else
					{
						if (Main.rand.Next(2) == 0)
						{
							Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 112);
						}
						else
						{
							Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 111);
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
							rand = 86;
							break;
						case 2:
							rand = 87;
							break;
						default:
							rand = 85;
							break;
					}

					Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, rand);
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

		public void initUI(Player player, OrchidModPlayer modPlayer)
		{
			Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 7);
			modPlayer.alchemistSelectUIKeysInitialize = false;
			element = AlchemistElement.NULL;

			foreach (Item item in this.ConcatInventories(Main.LocalPlayer, modPlayer))
			{
				if (item.type != 0)
				{
					OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
					if (orchidItem.alchemistElement != AlchemistElement.NULL)
					{
						element = orchidItem.alchemistElement;
						break;
					}
				}
			}

			this.checkInventory(true, player, modPlayer);
		}

		public void checkInventory(bool up, Player player, OrchidModPlayer modPlayer)
		{
			if (modPlayer.alchemistNbElements < modPlayer.alchemistNbElementsMax)
			{
				int elementsChecked = 0;
				this.displayItems = new List<Item>();
				while (displayItems.Count() == 0)
				{
					foreach (Item item in this.ConcatInventories(Main.LocalPlayer, modPlayer))
					{
						if (item.type != 0)
						{
							OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
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

		public AlchemistSelectKeysUIFrame()
		{
			dimensions = GetDimensions();
			point = new Point((int)dimensions.X, (int)dimensions.Y);
			backgroundColor = Color.White;
			element = AlchemistElement.NULL;
			displayItems = new List<Item>();
			releasedKey = true;
			if (emptyTexture == null) emptyTexture = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistKeysUIEMpty");
		}
		
		public Item[] ConcatInventories(Player player, OrchidModPlayer modPlayer) {
            return modPlayer.alchemistPotionBag.Concat(player.inventory).ToArray();
		}
	}
}