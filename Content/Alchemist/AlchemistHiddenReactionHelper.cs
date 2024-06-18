using Microsoft.Xna.Framework;
using OrchidMod.Content.Alchemist.Weapons.Air;
using OrchidMod.Content.Alchemist.Weapons.Fire;
using OrchidMod.Content.Alchemist.Weapons.Nature;
using OrchidMod.Content.Alchemist.Weapons.Water;
using OrchidMod.Content.Alchemist.Recipes;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using OrchidMod.Common.Global.Items;

namespace OrchidMod.Content.Alchemist
{
	public class AlchemistHiddenReactionHelper
	{
		public static List<AlchemistHiddenReactionRecipe> ListReactions()
		{
			/* Hint levels :
			<1 : Special
			1 : Pre EoC Vanilla
			2 : Pre Skeletron Vanilla
			3 : Pre WoF Vanilla
			4 : Pre Mechs Vanilla
			5 : Pre Golem Vanilla
			6 : Endgame Vanilla
			*/

			List<AlchemistHiddenReactionRecipe> recipes = new List<AlchemistHiddenReactionRecipe>();
			
			recipes.Add(new RecipeSunflowerSeeds());
			recipes.Add(new RecipeStardustConfusion());
			recipes.Add(new RecipeBurningSamples());
			recipes.Add(new RecipeGlowshroomHealing());
			recipes.Add(new RecipeMushroomThread());
			recipes.Add(new RecipeFireSpores());
			recipes.Add(new RecipeWaterSpores());
			recipes.Add(new RecipeAirSpores());
			recipes.Add(new RecipePropulsion());
			recipes.Add(new RecipeAttractiteShurikens());
			recipes.Add(new RecipeMistySteps());
			recipes.Add(new RecipePermanentFreeze());
			recipes.Add(new RecipeStellarTalcOrbit());
			recipes.Add(new RecipeAttractiteAuraLesser());
			recipes.Add(new RecipeBubbleSlime());
			recipes.Add(new RecipeBubbleSap());
			recipes.Add(new RecipeBubbleOil());
			recipes.Add(new RecipeBubbleSpirited());
			recipes.Add(new RecipeBubbleSeafoam());
			recipes.Add(new RecipeBubblePoison());
			recipes.Add(new RecipeBubbleSlimeLava());
			recipes.Add(new RecipeJungleLilyPurification());
			recipes.Add(new RecipePurifyingLilies());
			recipes.Add(new RecipePoisonousSlime());
			recipes.Add(new RecipeBeeSwarm());
			recipes.Add(new RecipeLivingBeehive());
			recipes.Add(new RecipeBubbles());
			recipes.Add(new RecipeSpiritedDroplets());
			recipes.Add(new RecipeDemonReeks());
			recipes.Add(new RecipePotionFlipper());
			recipes.Add(new RecipePotionObsidianSkin());
			recipes.Add(new RecipePotionNightOwl());
			recipes.Add(new RecipePotionInvisibility());
			recipes.Add(new RecipePotionBuilder());
			recipes.Add(new RecipePotionFeatherfall());

			return recipes;
		}

		public static bool playerKnowsRecipe(AlchemistHiddenReactionRecipe recipe, Player player)
			=> player.GetModPlayer<OrchidAlchemist>().alchemistKnownReactions.Contains(recipe.typeName);

		public static void triggerAlchemistReactionEffects(AlchemistHiddenReactionRecipe recipe, Mod mod, Player player, OrchidAlchemist modPlayer)
		{
			recipe.recipeEffect(player, modPlayer);
			
			if (recipe.dust != 0) {
				for (int i = 0; i < 10; i++)
				{
					int dust = Dust.NewDust(player.Center, 10, 10, recipe.dust);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 2f;
					Main.dust[dust].scale *= 1.5f;
				}
			}
			
			if (recipe.buff != 0 && recipe.buffDuration != 0) {
				player.AddBuff(recipe.buff, 60 * recipe.buffDuration);
			}

			if (recipe.debuffDuration != 0)
			{
				player.AddBuff((BuffType<Alchemist.Debuffs.ReactionCooldown>()), 60 * recipe.debuffDuration);
			}

			SoundEngine.PlaySound(recipe.sound);
		}

		public static bool checkSubstitutes(int ingredientID, Mod mod, Player player, OrchidAlchemist modPlayer)
		{
			List<int> ingredientToCompare = new List<int>();
			ingredientToCompare.Add(ItemType<CloudInAVial>());
			ingredientToCompare.Add(ItemType<AttractiteFlask>());
			ingredientToCompare.Add(ItemType<BlinkrootFlask>());
			ingredientToCompare.Add(ItemType<ShiverthornFlask>());
			ingredientToCompare.Add(ItemType<CorruptionFlask>());
			ingredientToCompare.Add(ItemType<GoblinArmyFlask>());

			foreach (int ingredient in ingredientToCompare)
			{
				if (ingredientID == ItemType<CloudInAVial>())
				{
					return (modPlayer.ContainsAlchemistFlask(ItemType<FartInAVial>())
					|| modPlayer.ContainsAlchemistFlask(ItemType<BlizzardInAVial>())
					|| modPlayer.ContainsAlchemistFlask(ItemType<TsunamiInAVial>()));
				}
				if (ingredientID == ItemType<AttractiteFlask>())
				{
					return modPlayer.ContainsAlchemistFlask(ItemType<GlowingAttractiteFlask>());
				}
				if (ingredientID == ItemType<GoblinArmyFlask>())
				{
					return modPlayer.ContainsAlchemistFlask(ItemType<HellOil>());
				}
				if (ingredientID == ItemType<BlinkrootFlask>())
				{
					return modPlayer.ContainsAlchemistFlask(ItemType<FireblossomFlask>());
				}
				if (ingredientID == ItemType<ShiverthornFlask>())
				{
					return modPlayer.ContainsAlchemistFlask(ItemType<DeathweedFlask>());
				}
				if (ingredientID == ItemType<CorruptionFlask>())
				{
					return modPlayer.ContainsAlchemistFlask(ItemType<CrimsonFlask>());
				}
			}
			return false;
		}

		public static void triggerAlchemistReaction(Mod mod, Player player, OrchidAlchemist modPlayer)
		{
			string floatingTextStr = "Failed reaction ...";
			AlchemistHiddenReactionRecipe hiddenReaction = new RecipeBlank();

			foreach (AlchemistHiddenReactionRecipe recipe in OrchidMod.AlchemistReactionRecipes)
			{
				bool goodIngredients = true;
				if (modPlayer.alchemistNbElements == recipe.ingredients.Count)
				{
					foreach (int ingredientID in recipe.ingredients)
					{
						if (!(modPlayer.ContainsAlchemistFlask(ingredientID)))
						{
							if (!AlchemistHiddenReactionHelper.checkSubstitutes(ingredientID, mod, player, modPlayer))
							{
								goodIngredients = false;
								break;
							}
						}
					}
					if (goodIngredients)
					{
						hiddenReaction = recipe;
						floatingTextStr = recipe.name;

						int val = 0;
						Item item = new Item();
						foreach (int ingredientID in recipe.ingredients)
						{
							item.SetDefaults(ingredientID);
							OrchidGlobalItemPerEntity globalItem = item.GetGlobalItem<OrchidGlobalItemPerEntity>();
							val += globalItem.alchemistPotencyCost;
						}
						val = (int)(val / 2);
						CombatText.NewText(player.Hitbox, new Color(128, 255, 0), val);
						modPlayer.alchemistPotency += modPlayer.alchemistPotency + val > modPlayer.alchemistPotencyMax ? modPlayer.alchemistPotencyMax : modPlayer.alchemistPotency;
						break;
					}
				}
			}

			Color floatingTextColor = hiddenReaction.typeName != "RecipeBlank" ? new Color(128, 255, 0) : new Color(255, 0, 0);
			CombatText.NewText(player.Hitbox, floatingTextColor, floatingTextStr);

			if (hiddenReaction.typeName == "RecipeBlank")
			{
				player.AddBuff((BuffType<Debuffs.ReactionCooldown>()), 60 * 5);
				for (int i = 0; i < 15; i++)
				{
					int dust = Dust.NewDust(player.Center, 10, 10, 37);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 2f;
					Main.dust[dust].scale *= 1.2f;
				}
				for (int i = 0; i < 10; i++)
				{
					int dust = Dust.NewDust(player.Center, 10, 10, 6);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 2f;
					Main.dust[dust].scale *= 1.5f;
				}
				SoundEngine.PlaySound(SoundID.Item16);
			}
			else
			{
				triggerAlchemistReactionEffects(hiddenReaction, mod, player, modPlayer);

				if (!(modPlayer.alchemistKnownReactions.Contains(hiddenReaction.typeName)))
				{
					if (modPlayer.alchemistKnownHints.Contains(hiddenReaction.typeName))
					{
						modPlayer.alchemistKnownHints.Remove(hiddenReaction.typeName);
					}

					modPlayer.alchemistKnownReactions.Add(hiddenReaction.typeName);
					Rectangle rect = player.Hitbox;
					rect.Y -= 50;

					Main.NewText($"Entry {HiddenReactionTagHandler.GenerateTag(hiddenReaction)} added to the hidden reaction codex!");
				}
			}

			int colorRed = modPlayer.alchemistColorR;
			int colorGreen = modPlayer.alchemistColorG;
			int colorBlue = modPlayer.alchemistColorB;

			int rand = 2 + Main.rand.Next(3);
			for (int i = 0; i < rand; i++)
			{
				int proj = ProjectileType<Projectiles.AlchemistSmoke1>();
				Vector2 vel = (new Vector2(0f, -((float)(2 + Main.rand.Next(5)))).RotatedByRandom(MathHelper.ToRadians(180)));
				int smokeProj = Projectile.NewProjectile(null, player.Center, vel, proj, 0, 0f, player.whoAmI);
				Main.projectile[smokeProj].localAI[0] = colorRed;
				Main.projectile[smokeProj].localAI[1] = colorGreen;
				Main.projectile[smokeProj].ai[1] = colorBlue;
			}
			rand = 1 + Main.rand.Next(3);
			for (int i = 0; i < rand; i++)
			{
				int proj = ProjectileType<Projectiles.AlchemistSmoke2>();
				Vector2 vel = (new Vector2(0f, -((float)(2 + Main.rand.Next(5)))).RotatedByRandom(MathHelper.ToRadians(180)));
				int smokeProj = Projectile.NewProjectile(null, player.Center, vel, proj, 0, 0f, player.whoAmI);
				Main.projectile[smokeProj].localAI[0] = colorRed;
				Main.projectile[smokeProj].localAI[1] = colorGreen;
				Main.projectile[smokeProj].ai[1] = colorBlue;
			}
			rand = Main.rand.Next(2);
			for (int i = 0; i < rand; i++)
			{
				int proj = ProjectileType<Projectiles.AlchemistSmoke3>();
				Vector2 vel = (new Vector2(0f, -((float)(2 + Main.rand.Next(5)))).RotatedByRandom(MathHelper.ToRadians(180)));
				int smokeProj = Projectile.NewProjectile(null, player.Center, vel, proj, 0, 0f, player.whoAmI);
				Main.projectile[smokeProj].localAI[0] = colorRed;
				Main.projectile[smokeProj].localAI[1] = colorGreen;
				Main.projectile[smokeProj].ai[1] = colorBlue;
			}

			AlchemistHiddenReactionHelper.bonusReactionEffects(mod, player, modPlayer);

			modPlayer.alchemistFlaskDamage = 0;
			modPlayer.alchemistNbElements = 0;
			modPlayer.ClearAlchemistElements();
			modPlayer.ClearAlchemistFlasks();
			modPlayer.ClearAlchemistColors();
			modPlayer.alchemistSelectUIDisplay = false;
		}

		public static void bonusReactionEffects(Mod mod, Player player, OrchidAlchemist modPlayer)
		{
			if (modPlayer.alchemistReactiveVials)
			{
				player.AddBuff(BuffType<Alchemist.Buffs.ReactiveVialsBuff>(), 60 * 10);
			}
		}

		public static void addAlchemistHint(Player player, OrchidAlchemist modPlayer, int hintLevel, bool negativeMessage = true)
		{
			Color floatingTextColor = new Color(0, 0, 0);
			string floatingTextStr = "";
			List<AlchemistHiddenReactionRecipe> validHints = new List<AlchemistHiddenReactionRecipe>();

			foreach (AlchemistHiddenReactionRecipe recipe in OrchidMod.AlchemistReactionRecipes)
			{
				if (recipe.level == hintLevel)
				{
					if (!(modPlayer.alchemistKnownReactions.Contains(recipe.typeName) || modPlayer.alchemistKnownHints.Contains(recipe.typeName)))
					{
						validHints.Add(recipe);
					}
				}
			}

			if (validHints.Count == 0)
			{
				if (negativeMessage)
				{
					floatingTextColor = new Color(255, 92, 0);
					floatingTextStr = "No hints left for this tier";
					CombatText.NewText(player.Hitbox, floatingTextColor, floatingTextStr);
				}
			}
			else
			{
				int rand = Main.rand.Next(validHints.Count);
				AlchemistHiddenReactionRecipe hint = validHints[rand];
				modPlayer.alchemistKnownHints.Add(hint.typeName);
				if (!modPlayer.alchemistEntryTextCooldown)
				{
					floatingTextColor = new Color(255, 187, 0);
					floatingTextStr = "New Hidden Reaction Hint";
					CombatText.NewText(player.Hitbox, floatingTextColor, floatingTextStr);
					modPlayer.alchemistEntryTextCooldown = true;
				}
			}
		}
	}
}