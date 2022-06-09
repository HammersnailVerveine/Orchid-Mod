using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.Weapons.Air;
using OrchidMod.Alchemist.Weapons.Fire;
using OrchidMod.Alchemist.Weapons.Nature;
using OrchidMod.Alchemist.Weapons.Water;
using OrchidMod.Alchemist.Recipes;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist
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

		public static void triggerAlchemistReactionEffects(AlchemistHiddenReactionRecipe recipe, Mod mod, Player player, OrchidModPlayer modPlayer)
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
				player.AddBuff((BuffType<Alchemist.Buffs.Debuffs.ReactionCooldown>()), 60 * recipe.debuffDuration);
			}

			if (recipe.soundID != 0 && recipe.soundType != 0)
			{
				SoundEngine.PlaySound(recipe.soundType, (int)player.position.X, (int)player.position.Y, recipe.soundID);
			}
		}

		public static bool checkSubstitutes(int ingredientID, Mod mod, Player player, OrchidModPlayer modPlayer)
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
					return (OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<FartInAVial>(), player, modPlayer)
					|| OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<BlizzardInAVial>(), player, modPlayer)
					|| OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<TsunamiInAVial>(), player, modPlayer));
				}
				if (ingredientID == ItemType<AttractiteFlask>())
				{
					return OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<GlowingAttractiteFlask>(), player, modPlayer);
				}
				if (ingredientID == ItemType<GoblinArmyFlask>())
				{
					return OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<HellOil>(), player, modPlayer);
				}
				if (ingredientID == ItemType<BlinkrootFlask>())
				{
					return OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<FireblossomFlask>(), player, modPlayer);
				}
				if (ingredientID == ItemType<ShiverthornFlask>())
				{
					return OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<DeathweedFlask>(), player, modPlayer);
				}
				if (ingredientID == ItemType<CorruptionFlask>())
				{
					return OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<CrimsonFlask>(), player, modPlayer);
				}
			}
			return false;
		}

		public static void triggerAlchemistReaction(Mod mod, Player player, OrchidModPlayer modPlayer)
		{
			string floatingTextStr = "Failed reaction ...";
			AlchemistHiddenReactionRecipe hiddenReaction = new RecipeBlank();

			foreach (AlchemistHiddenReactionRecipe recipe in OrchidMod.alchemistReactionRecipes)
			{
				bool goodIngredients = true;
				if (modPlayer.alchemistNbElements == recipe.ingredients.Count)
				{
					foreach (int ingredientID in recipe.ingredients)
					{
						if (!(OrchidModAlchemistHelper.containsAlchemistFlask(ingredientID, player, modPlayer)))
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
							OrchidModGlobalItem globalItem = item.GetGlobalItem<OrchidModGlobalItem>();
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
				player.AddBuff((BuffType<Alchemist.Buffs.Debuffs.ReactionCooldown>()), 60 * 5);
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
				SoundEngine.PlaySound(2, (int)player.position.X, (int)player.position.Y, 16);
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
					floatingTextColor = new Color(255, 187, 0);
					floatingTextStr = "New Entry";
					Rectangle rect = player.Hitbox;
					rect.Y -= 50;
					CombatText.NewText(rect, floatingTextColor, floatingTextStr);
				}
			}

			int colorRed = modPlayer.alchemistColorR;
			int colorGreen = modPlayer.alchemistColorG;
			int colorBlue = modPlayer.alchemistColorB;

			int rand = 2 + Main.rand.Next(3);
			for (int i = 0; i < rand; i++)
			{
				int proj = ProjectileType<Alchemist.Projectiles.AlchemistSmoke1>();
				Vector2 vel = (new Vector2(0f, -((float)(2 + Main.rand.Next(5)))).RotatedByRandom(MathHelper.ToRadians(180)));
				int smokeProj = Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, proj, 0, 0f, player.whoAmI);
				Main.projectile[smokeProj].localAI[0] = colorRed;
				Main.projectile[smokeProj].localAI[1] = colorGreen;
				Main.projectile[smokeProj].ai[1] = colorBlue;
			}
			rand = 1 + Main.rand.Next(3);
			for (int i = 0; i < rand; i++)
			{
				int proj = ProjectileType<Alchemist.Projectiles.AlchemistSmoke2>();
				Vector2 vel = (new Vector2(0f, -((float)(2 + Main.rand.Next(5)))).RotatedByRandom(MathHelper.ToRadians(180)));
				int smokeProj = Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, proj, 0, 0f, player.whoAmI);
				Main.projectile[smokeProj].localAI[0] = colorRed;
				Main.projectile[smokeProj].localAI[1] = colorGreen;
				Main.projectile[smokeProj].ai[1] = colorBlue;
			}
			rand = Main.rand.Next(2);
			for (int i = 0; i < rand; i++)
			{
				int proj = ProjectileType<Alchemist.Projectiles.AlchemistSmoke3>();
				Vector2 vel = (new Vector2(0f, -((float)(2 + Main.rand.Next(5)))).RotatedByRandom(MathHelper.ToRadians(180)));
				int smokeProj = Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, proj, 0, 0f, player.whoAmI);
				Main.projectile[smokeProj].localAI[0] = colorRed;
				Main.projectile[smokeProj].localAI[1] = colorGreen;
				Main.projectile[smokeProj].ai[1] = colorBlue;
			}

			AlchemistHiddenReactionHelper.bonusReactionEffects(mod, player, modPlayer);

			modPlayer.alchemistFlaskDamage = 0;
			modPlayer.alchemistNbElements = 0;
			OrchidModAlchemistHelper.clearAlchemistElements(player, modPlayer, mod);
			OrchidModAlchemistHelper.clearAlchemistFlasks(player, modPlayer, mod);
			OrchidModAlchemistHelper.clearAlchemistColors(player, modPlayer, mod);
			modPlayer.alchemistSelectUIDisplay = false;
		}

		public static void bonusReactionEffects(Mod mod, Player player, OrchidModPlayer modPlayer)
		{
			if (modPlayer.alchemistReactiveVials)
			{
				player.AddBuff(BuffType<Alchemist.Buffs.ReactiveVialsBuff>(), 60 * 10);
			}
		}

		public static void addAlchemistHint(Player player, OrchidModPlayer modPlayer, int hintLevel, bool negativeMessage = true)
		{
			Color floatingTextColor = new Color(0, 0, 0);
			string floatingTextStr = "";
			List<AlchemistHiddenReactionRecipe> validHints = new List<AlchemistHiddenReactionRecipe>();

			foreach (AlchemistHiddenReactionRecipe recipe in OrchidMod.alchemistReactionRecipes)
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