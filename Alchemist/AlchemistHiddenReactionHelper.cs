using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using OrchidMod;
using OrchidMod.Alchemist.Weapons.Nature;
using OrchidMod.Alchemist.Weapons.Fire;
using OrchidMod.Alchemist.Weapons.Water;
using OrchidMod.Alchemist.Weapons.Air;
using OrchidMod.Alchemist.Weapons.Light;
using OrchidMod.Alchemist.Weapons.Dark;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist
{
	public class AlchemistHiddenReactionHelper
	{
		
		public static List<AlchemistHiddenReactionRecipe> ListReactions() {
			/* Hint levels :
			<1 : Special
			1 : Pre EoC Vanilla
			2 : Pre Skeletron Vanilla
			3 : Pre WoF Vanilla
			4 : Pre Mechs Vanilla
			5 : Pre Plantera Vanilla
			6 : Endgame Vanilla
			*/
			
			List<AlchemistHiddenReactionRecipe> recipes = new List<AlchemistHiddenReactionRecipe>();
			
			recipes.Add(new AlchemistHiddenReactionRecipe(AlchemistHiddenReaction.SUNFLOWERSEEDS, -1, "Sunflower Seeds",
			"Releases damaging sunflower seeds around the player",
			ItemType<SunflowerFlask>(), ItemType<SlimeFlask>()));
			recipes.Add(new AlchemistHiddenReactionRecipe(AlchemistHiddenReaction.GLOWSHROOMHEALING, 1, "Glowshroom Healing", 
			"Heals the player for 25 health",
			ItemType<GlowingMushroomVial>(), ItemType<KingSlimeFlask>()));
			recipes.Add(new AlchemistHiddenReactionRecipe(AlchemistHiddenReaction.MUSHROOMTHREAD, -2, "Mushroom Thread", 
			"Creates a sample of Mushroom Thread, which can be used to create armor",
			ItemType<GlowingMushroomVial>(), ItemType<BlinkrootFlask>()));
			
			recipes.Add(new AlchemistHiddenReactionRecipe(AlchemistHiddenReaction.FIRESPORES, -3, "Fire Spores", 
			"Releases a sizeable amount of fire spores, which doesn't destroy existing ones",
			ItemType<AttractiteFlask>(), ItemType<BlinkrootFlask>()));
			recipes.Add(new AlchemistHiddenReactionRecipe(AlchemistHiddenReaction.WATERSPORES, -3, "Water Spores", 
			"Releases a sizeable amount of water spores, which doesn't destroy existing ones",
			ItemType<AttractiteFlask>(), ItemType<WaterleafFlask>()));
			recipes.Add(new AlchemistHiddenReactionRecipe(AlchemistHiddenReaction.AIRSPORES, -3, "Air Spores", 
			"Releases a sizeable amount of air spores, which doesn't destroy existing ones",
			ItemType<AttractiteFlask>(), ItemType<ShiverthornFlask>()));
			
			recipes.Add(new AlchemistHiddenReactionRecipe(AlchemistHiddenReaction.PROPULSION, 1, "Propulsion", 
			"Vertically propels the player",
			ItemType<GunpowderFlask>(), ItemType<CouldInAVial>()));
			
			recipes.Add(new AlchemistHiddenReactionRecipe(AlchemistHiddenReaction.BUBBLESLIME, 1, "Slime Bubble", 
			"Creates a catalytic slime bubble",
			ItemType<CouldInAVial>(), ItemType<KingSlimeFlask>()));
			recipes.Add(new AlchemistHiddenReactionRecipe(AlchemistHiddenReaction.BUBBLESAP, 1, "Sap Bubble", 
			"Creates a catalytic sap bubble",
			ItemType<CouldInAVial>(), ItemType<LivingSapVial>()));
			recipes.Add(new AlchemistHiddenReactionRecipe(AlchemistHiddenReaction.BUBBLEOIL, 1, "Oil Bubble", 
			"Creates a catalytic oil bubble",
			ItemType<CouldInAVial>(), ItemType<GoblinArmyFlask>()));
			recipes.Add(new AlchemistHiddenReactionRecipe(AlchemistHiddenReaction.BUBBLESPIRITED, 2, "Spirited Bubble", 
			"Creates a catalytic spirit bubble",
			ItemType<CouldInAVial>(), ItemType<DungeonFlask>()));
			recipes.Add(new AlchemistHiddenReactionRecipe(AlchemistHiddenReaction.BUBBLESEAFOAM, 1, "Seafoam Bubble", 
			"Creates a catalytic seafoam bubble",
			ItemType<CouldInAVial>(), ItemType<SeafoamVial>()));
			recipes.Add(new AlchemistHiddenReactionRecipe(AlchemistHiddenReaction.BUBBLEPOISON, 2, "Poison Bubble", 
			"Creates a catalytic poison bubble",
			ItemType<CouldInAVial>(), ItemType<PoisonVial>()));
			
			recipes.Add(new AlchemistHiddenReactionRecipe(AlchemistHiddenReaction.BEESWARM, 2, "Bee Swarm", 
			"Releases a swarm of harmful bees around the player",
			ItemType<QueenBeeFlask>(), ItemType<PoisonVial>()));
			
			recipes.Add(new AlchemistHiddenReactionRecipe(AlchemistHiddenReaction.POTIONFLIPPER, 1, "Flipper Potion", 
			"Gives 30 seconds of Flipper Potion effect",
			ItemType<ShiverthornFlask>(), ItemType<WaterleafFlask>()));
			recipes.Add(new AlchemistHiddenReactionRecipe(AlchemistHiddenReaction.POTIONNIGHTOWL, 1, "Night Owl Potion", 
			"Gives 30 seconds of Night Owl Potion effect",
			ItemType<DaybloomFlask>(), ItemType<BlinkrootFlask>()));
			recipes.Add(new AlchemistHiddenReactionRecipe(AlchemistHiddenReaction.POTIONINVISIBILITY, 1, "Invisibility Potion", 
			"Gives 30 seconds of Invisibility Potion effect",
			ItemType<BlinkrootFlask>(), ItemType<MoonglowFlask>()));
			recipes.Add(new AlchemistHiddenReactionRecipe(AlchemistHiddenReaction.POTIONBUILDER, 1, "Builder Potion", 
			"Gives 30 seconds of Builder Potion effect",
			ItemType<BlinkrootFlask>(), ItemType<ShiverthornFlask>(), ItemType<MoonglowFlask>()));
			recipes.Add(new AlchemistHiddenReactionRecipe(AlchemistHiddenReaction.POTIONFEATHERFALL, 1, "Featherfall Potion", 
			"Gives 30 seconds of Featherfall Potion effect",
			ItemType<DaybloomFlask>(), ItemType<BlinkrootFlask>(), ItemType<CouldInAVial>()));
			
			return recipes;
		}
		
		public static void triggerAlchemistReactionEffects(AlchemistHiddenReaction hiddenReaction, Mod mod, Player player, OrchidModPlayer modPlayer) {
			int debuffDuration = 0;
			int soundID = 0;
			int soundType = 0;
			
			int dmg = 0;
			int nb = 0;
			
			int spawnProj = 0;
			int spawnProj2 = 0;
			//int spawnProj3 = 0;
			
			int itemType = 0;
			int itemType2 = 0;
			
			int dust = 0;
			int alpha = 0;
			
			Vector2 vel = new Vector2(0f, 0f);
			
			switch (hiddenReaction) {
				case AlchemistHiddenReaction.MUSHROOMTHREAD :
					debuffDuration = 5;
					soundType = 2;
					soundID = 25;
					for(int i=0; i < 10; i++) {
						dust = Dust.NewDust(player.Center, 10, 10, 56);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity *= 2f;
						Main.dust[dust].scale *= 1.5f;
					}
					player.QuickSpawnItem(ItemType<Alchemist.Misc.MushroomThread>(), 1);
					break;
				case AlchemistHiddenReaction.BEESWARM :
					debuffDuration = 25;
					soundType = 2;
					soundID = 97;
					for (int i=0; i < 10; i++) {
						dust = Dust.NewDust(player.Center, 10, 10, 16);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity *= 2f;
						Main.dust[dust].scale *= 1.5f;
					}
					
					itemType = ItemType<QueenBeeFlask>();
					dmg = OrchidModAlchemistHelper.getSecondaryDamage(modPlayer, itemType, 4, true);
					for (int i = 0 ; i < 10 ; i ++) {
						vel = ( new Vector2(0f, -(float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(80)));
						if (player.strongBees && Main.rand.Next(2) == 0) 
							Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, 566, (int)(dmg * 1.15f), 0f, player.whoAmI);
						else {
							Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, 181, dmg, 0f, player.whoAmI);
						}
					}
					for (int i = 0 ; i < 10 ; i ++) {
						vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(80)));
						spawnProj = ProjectileType<Alchemist.Projectiles.Air.QueenBeeFlaskProj>();
						Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, player.whoAmI);
					}
					break;
				case AlchemistHiddenReaction.BUBBLESPIRITED :
					debuffDuration = 20;
					soundType = 2;
					soundID = 85;
					for (int i=0; i < 10; i++) {
						alpha = 175;
						Color newColor = new Color(0, 80, (int) byte.MaxValue, 100);
						dust = Dust.NewDust(player.Center, 10, 10, 4, 0.0f, 0.0f, alpha, newColor, 1.2f);
						Main.dust[dust].velocity *= 1.5f;
						Main.dust[dust].scale *= 1f;
					}
					
					itemType = ItemType<DungeonFlask>();
					dmg = OrchidModAlchemistHelper.getSecondaryDamage(modPlayer, itemType, 4, true);
					spawnProj = ProjectileType<Alchemist.Projectiles.Reactive.SpiritedBubble>();
					Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -5f, spawnProj, dmg, 0f, player.whoAmI);
					break;
				case AlchemistHiddenReaction.BUBBLEPOISON :
					debuffDuration = 20;
					soundType = 2;
					soundID = 85;
					for (int i=0; i < 10; i++) {
						alpha = 175;
						Color newColor = new Color(0, 80, (int) byte.MaxValue, 100);
						dust = Dust.NewDust(player.Center, 10, 10, 4, 0.0f, 0.0f, alpha, newColor, 1.2f);
						Main.dust[dust].velocity *= 1.5f;
						Main.dust[dust].scale *= 1f;
					}
					
					itemType = ItemType<PoisonVial>();
					dmg = OrchidModAlchemistHelper.getSecondaryDamage(modPlayer, itemType, 4, true);
					spawnProj = ProjectileType<Alchemist.Projectiles.Reactive.PoisonBubble>();
					Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -5f, spawnProj, dmg, 0f, player.whoAmI);
					break;
				case AlchemistHiddenReaction.BUBBLEOIL :
					debuffDuration = 20;
					soundType = 2;
					soundID = 85;
					for (int i=0; i < 10; i++) {
						alpha = 175;
						Color newColor = new Color(0, 80, (int) byte.MaxValue, 100);
						dust = Dust.NewDust(player.Center, 10, 10, 4, 0.0f, 0.0f, alpha, newColor, 1.2f);
						Main.dust[dust].velocity *= 1.5f;
						Main.dust[dust].scale *= 1f;
					}
					
					dmg = 0;
					spawnProj = ProjectileType<Alchemist.Projectiles.Reactive.OilBubble>();
					Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -5f, spawnProj, dmg, 0f, player.whoAmI);
					break;
				case AlchemistHiddenReaction.BUBBLESEAFOAM :
					debuffDuration = 20;
					soundType = 2;
					soundID = 85;
					for(int i=0; i < 10; i++) {
						int Alpha = 175;
						Color newColor = new Color(0, 80, (int) byte.MaxValue, 100);
						dust = Dust.NewDust(player.Center, 10, 10, 4, 0.0f, 0.0f, Alpha, newColor, 1.2f);
						Main.dust[dust].velocity *= 1.5f;
						Main.dust[dust].scale *= 1f;
					}
					
					itemType = ItemType<SeafoamVial>();
					dmg = OrchidModAlchemistHelper.getSecondaryDamage(modPlayer, itemType, 4, true);
					spawnProj = ProjectileType<Alchemist.Projectiles.Reactive.SeafoamBubble>();
					Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -5f, spawnProj, dmg, 0f, player.whoAmI);
					break;
				case AlchemistHiddenReaction.BUBBLESAP :
					debuffDuration = 20;
					soundType = 2;
					soundID = 85;
					for (int i=0; i < 10; i++) {
						alpha = 175;
						Color newColor = new Color(0, 80, (int) byte.MaxValue, 100);
						dust = Dust.NewDust(player.Center, 10, 10, 4, 0.0f, 0.0f, alpha, newColor, 1.2f);
						Main.dust[dust].velocity *= 1.5f;
						Main.dust[dust].scale *= 1f;
					}
					
					itemType = ItemType<LivingSapVial>();
					dmg = OrchidModAlchemistHelper.getSecondaryDamage(modPlayer, itemType, 25, false);
					spawnProj = ProjectileType<Alchemist.Projectiles.Reactive.LivingSapBubble>();
					Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -5f, spawnProj, dmg, 0f, player.whoAmI);
					Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 85);
					break;
				case AlchemistHiddenReaction.BUBBLESLIME :
					debuffDuration = 20;
					soundType = 2;
					soundID = 85;
					for (int i=0; i < 10; i++) {
						alpha = 175;
						Color newColor = new Color(0, 80, (int) byte.MaxValue, 100);
						dust = Dust.NewDust(player.Center, 10, 10, 4, 0.0f, 0.0f, alpha, newColor, 1.2f);
						Main.dust[dust].velocity *= 1.5f;
						Main.dust[dust].scale *= 1f;
					}
					
					itemType = ItemType<KingSlimeFlask>();
					dmg = OrchidModAlchemistHelper.getSecondaryDamage(modPlayer, itemType, 4, true);
					spawnProj = ProjectileType<Alchemist.Projectiles.Reactive.SlimeBubble>();
					Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -5f, spawnProj, dmg, 0f, player.whoAmI);
					break;
				case AlchemistHiddenReaction.PROPULSION :
					debuffDuration = 10;
					soundType = 2;
					soundID = 14;
					player.jump = 1;
					player.velocity.Y = -15f;
					for (int i=0; i < 10; i++) {
						dust = Dust.NewDust(player.Center, 10, 10, 15);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity *= 2f;
						Main.dust[dust].scale *= 1.5f;
					}
					for (int i=0; i < 15; i++) {
						dust = Dust.NewDust(player.Center, 10, 10, 37);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity *= 2f;
						Main.dust[dust].scale *= 1.2f;
					}
					break;
				case AlchemistHiddenReaction.POTIONBUILDER :
					debuffDuration = 30;
					soundType = 2;
					soundID = 25;
					player.AddBuff(107, 60 * 30); // Builder
					for (int i=0; i < 10; i++) {
						dust = Dust.NewDust(player.Center, 10, 10, 15);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity *= 2f;
						Main.dust[dust].scale *= 1.5f;
					}
					break;
				case AlchemistHiddenReaction.POTIONNIGHTOWL :
					debuffDuration = 30;
					soundType = 2;
					soundID = 25;
					player.AddBuff(12, 60 * 30); // Night Owl
					for (int i=0; i < 10; i++) {
						dust = Dust.NewDust(player.Center, 10, 10, 15);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity *= 2f;
						Main.dust[dust].scale *= 1.5f;
					}
					break;
				case AlchemistHiddenReaction.POTIONINVISIBILITY :
					debuffDuration = 30;
					soundType = 2;
					soundID = 25;
					player.AddBuff(10, 60 * 30); // Invisibility
					for(int i=0; i < 10; i++) {
						dust = Dust.NewDust(player.Center, 10, 10, 15);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity *= 2f;
						Main.dust[dust].scale *= 1.5f;
					}
					break;
				case AlchemistHiddenReaction.POTIONFEATHERFALL :
					debuffDuration = 30;
					soundType = 2;
					soundID = 25;
					player.AddBuff(8, 60 * 30); // Featherfall
					for(int i=0; i < 10; i++)
					{
						dust = Dust.NewDust(player.Center, 10, 10, 16);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity *= 2f;
						Main.dust[dust].scale *= 1.5f;
					}
					break;
				case AlchemistHiddenReaction.POTIONFLIPPER :
					debuffDuration = 30;
					soundType = 2;
					soundID = 25;
					player.AddBuff(109, 60 * 30); // Flipper
					for(int i=0; i < 10; i++)
					{
						dust = Dust.NewDust(player.Center, 10, 10, 6);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity *= 2f;
						Main.dust[dust].scale *= 1.5f;
					}
					break;
				case AlchemistHiddenReaction.AIRSPORES :
					debuffDuration = 20;
					soundType = 2;
					soundID = 45;
					for(int i=0; i < 10; i++) {
						dust = Dust.NewDust(player.Center, 10, 10, 16);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity *= 2f;
						Main.dust[dust].scale *= 1.5f;
					}
					itemType = ItemType<DeathweedFlask>();
					itemType2 = ItemType<ShiverthornFlask>();
					itemType = OrchidModAlchemistHelper.containsAlchemistFlask(itemType, player, modPlayer, mod) ? itemType : itemType2;
					dmg = OrchidModAlchemistHelper.getSecondaryDamage(modPlayer, itemType, 4, true);
					for (int i = 0 ; i < 10 ; i ++) {
						vel = ( new Vector2(0f, -5f).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))));
						spawnProj = ProjectileType<Alchemist.Projectiles.Air.AirSporeProj>();
						spawnProj2 = Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, dmg, 0f, player.whoAmI);
						Main.projectile[spawnProj2].localAI[1] = 1f;
					}
					nb = 4 + Main.rand.Next(3);
					for (int i = 0 ; i < nb ; i ++) {
						vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
						spawnProj = ProjectileType<Alchemist.Projectiles.Air.AirSporeProjAlt>();
						Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, player.whoAmI);
					}
					if (OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<GlowingAttractiteFlask>(), player, modPlayer, mod)) {
						for (int i = 0 ; i < 5 ; i ++) {
							vel = ( new Vector2(0f, -5f).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))));
							spawnProj = ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProj>();
							spawnProj2 = Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, dmg, 0f, player.whoAmI);
							Main.projectile[spawnProj2].localAI[1] = 1f;
						}
						for (int i = 0 ; i < 2 ; i ++) {
							vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
							spawnProj = ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProjAlt>();
							Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, player.whoAmI);
						}
					}
					break;
				case AlchemistHiddenReaction.WATERSPORES :
					debuffDuration = 20;
					soundType = 2;
					soundID = 45;
					for(int i=0; i < 10; i++) {
						dust = Dust.NewDust(player.Center, 10, 10, 33);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity *= 2f;
						Main.dust[dust].scale *= 1.5f;
					}
					itemType = ItemType<WaterleafFlask>();
					dmg = OrchidModAlchemistHelper.getSecondaryDamage(modPlayer, itemType, 4, true);
					for (int i = 0 ; i < 10 ; i ++) {
						vel = ( new Vector2(0f, -5f).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))));
						spawnProj = ProjectileType<Alchemist.Projectiles.Water.WaterSporeProj>();
						spawnProj2 = Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, dmg, 0f, player.whoAmI);
						Main.projectile[spawnProj2].localAI[1] = 1f;
					}
					nb = 4 + Main.rand.Next(3);
					for (int i = 0 ; i < nb ; i ++) {
						vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
						spawnProj = ProjectileType<Alchemist.Projectiles.Water.WaterSporeProjAlt>();
						Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, player.whoAmI);
					}
					if (OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<GlowingAttractiteFlask>(), player, modPlayer, mod)) {
						for (int i = 0 ; i < 5 ; i ++) {
							vel = ( new Vector2(0f, -5f).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))));
							spawnProj = ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProj>();
							spawnProj2 = Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, dmg, 0f, player.whoAmI);
							Main.projectile[spawnProj2].localAI[1] = 1f;
						}
						for (int i = 0 ; i < 2 ; i ++) {
							vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
							spawnProj = ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProjAlt>();
							Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, player.whoAmI);
						}
					}
					break;
				case AlchemistHiddenReaction.FIRESPORES :
					debuffDuration = 20;
					soundType = 2;
					soundID = 45;
					for (int i=0; i < 10; i++) {
						dust = Dust.NewDust(player.Center, 10, 10, 6);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity *= 2f;
						Main.dust[dust].scale *= 1.5f;
					}
					itemType = ItemType<BlinkrootFlask>();
					itemType2 = ItemType<FireblossomFlask>();
					itemType = OrchidModAlchemistHelper.containsAlchemistFlask(itemType, player, modPlayer, mod) ? itemType : itemType2;
					dmg = OrchidModAlchemistHelper.getSecondaryDamage(modPlayer, itemType, 4, true);
					for (int i = 0 ; i < 10 ; i ++) {
						vel = ( new Vector2(0f, -5f).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))));
						spawnProj = ProjectileType<Alchemist.Projectiles.Fire.FireSporeProj>();
						spawnProj2 = Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, dmg, 0f, player.whoAmI);
						Main.projectile[spawnProj2].localAI[1] = 1f;
					}
					nb = 4 + Main.rand.Next(3);
					for (int i = 0 ; i < nb ; i ++) {
						vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
						spawnProj = ProjectileType<Alchemist.Projectiles.Fire.FireSporeProjAlt>();
						Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, player.whoAmI);
					}
					if (OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<GlowingAttractiteFlask>(), player, modPlayer, mod)) {
						for (int i = 0 ; i < 5 ; i ++) {
							vel = ( new Vector2(0f, -5f).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))));
							spawnProj = ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProj>();
							spawnProj2 = Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, dmg, 0f, player.whoAmI);
							Main.projectile[spawnProj2].localAI[1] = 1f;
						}
						for (int i = 0 ; i < 2 ; i ++) {
							vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
							spawnProj = ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProjAlt>();
							Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, player.whoAmI);
						}
					}
					break;
				case AlchemistHiddenReaction.GLOWSHROOMHEALING :
					debuffDuration = 30;
					soundType = 2;
					soundID = 25;
				
					if (Main.myPlayer == player.whoAmI)
						player.HealEffect(25, true);
					player.statLife += 25;
					
					for (int i=0; i < 10; i++) {
						dust = Dust.NewDust(player.Center, 10, 10, 56);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity *= 2f;
						Main.dust[dust].scale *= 1.5f;
					}
					break;
				case AlchemistHiddenReaction.BUBBLES :
					debuffDuration = 15;
					soundType = 2;
					soundID = 85;
					itemType = ItemType<PoisonVial>();
					dmg = OrchidModAlchemistHelper.getSecondaryDamage(modPlayer, itemType, 4, true);
					
					for (int i = 0 ; i < 7 ; i ++) {
						spawnProj = Main.rand.Next(2) == 0 ? ProjectileType<Alchemist.Projectiles.Water.SeafoamVialProj>() : ProjectileType<Alchemist.Projectiles.Nature.PoisonVialProj>();
						Projectile.NewProjectile(player.Center.X - 120 + i * 40, player.Center.Y, 0f, -(float)(3 + Main.rand.Next(4)) * 0.5f, spawnProj, dmg, 0f, player.whoAmI);
					}
					
					for (int i = 0 ; i < 11 ; i ++) {
						vel = (new Vector2(0f, -(float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(10)));
						spawnProj = Main.rand.Next(2) == 0 ? ProjectileType<Alchemist.Projectiles.Water.SeafoamVialProjAlt>() : ProjectileType<Alchemist.Projectiles.Nature.PoisonVialProjAlt>();
						Projectile.NewProjectile(player.Center.X - 150 + i * 30, player.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, player.whoAmI);
					}
					break;
				case AlchemistHiddenReaction.SUNFLOWERSEEDS :
					debuffDuration = 15;
					soundType = 2;
					soundID = 85;
					
					itemType = ItemType<SunflowerFlask>();
					dmg = OrchidModAlchemistHelper.getSecondaryDamage(modPlayer, itemType, 4, true);
					nb = 5 + Main.rand.Next(4);
					
					for (int i = 0 ; i < 5 ; i ++) {
						vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(10)).RotatedBy(MathHelper.ToRadians(- 40 + (20 * i))));
						Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, ProjectileType<Alchemist.Projectiles.Nature.SunflowerFlaskProj1>(), dmg, 0f, player.whoAmI);
					}
					
					for (int i = 0 ; i < nb ; i ++) {
						vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
						spawnProj = ProjectileType<Alchemist.Projectiles.Nature.SunflowerFlaskProj4>();
						Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, player.whoAmI);
					}
					break;
				default :
					CombatText.NewText(player.Hitbox, new Color(255, 0, 0), "No Effect ?");
					break;
			}
			
			if (debuffDuration != 0) {
				player.AddBuff((BuffType<Alchemist.Buffs.Debuffs.ReactionCooldown>()), 60 * debuffDuration);
			}
			
			if (soundID != 0 && soundType != 0) {
				Main.PlaySound(soundType, (int)player.position.X, (int)player.position.Y, soundID);
			}
		}
		
		public static bool checkSubstitutes(int ingredientID, Mod mod, Player player, OrchidModPlayer modPlayer) {
			List<int> ingredientToCompare = new List<int>();
			ingredientToCompare.Add(ItemType<FartInAVial>());
			
			foreach (int ingredient in ingredientToCompare) {
				if (ingredientID == ItemType<CouldInAVial>()) {
					return OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<FartInAVial>(), player, modPlayer, mod);
				}
				if (ingredientID == ItemType<AttractiteFlask>()) {
					return OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<GlowingAttractiteFlask>(), player, modPlayer, mod);
				}
				if (ingredientID == ItemType<BlinkrootFlask>()) {
					return OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<FireblossomFlask>(), player, modPlayer, mod);
				}
				if (ingredientID == ItemType<ShiverthornFlask>()) {
					return OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<DeathweedFlask>(), player, modPlayer, mod);
				}
			}
			return false;
		}
		
		public static void triggerAlchemistReaction(Mod mod, Player player, OrchidModPlayer modPlayer) {
			bool reaction = false;
			string floatingTextStr = "Failed reaction ...";
			AlchemistHiddenReaction hiddenReaction = AlchemistHiddenReaction.NULL;
			
			foreach (AlchemistHiddenReactionRecipe recipe in OrchidMod.alchemistReactionRecipes) {
				bool goodIngredients = true;
				if (modPlayer.alchemistNbElements == recipe.reactionIngredients.Count) {
					foreach(int ingredientID in recipe.reactionIngredients) {
						if (!(OrchidModAlchemistHelper.containsAlchemistFlask(ingredientID, player, modPlayer, mod))) {
							if (!AlchemistHiddenReactionHelper.checkSubstitutes(ingredientID, mod, player, modPlayer)) {
								goodIngredients = false;
								break;	
							}
						}
					}
					if (goodIngredients) {
						hiddenReaction = recipe.reactionType;
						floatingTextStr = recipe.reactionText;
						reaction = true;
						
						int val = 0;
						Item item = new Item();
						foreach(int ingredientID in recipe.reactionIngredients) {
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
			
			Color floatingTextColor = reaction ? new Color(128, 255, 0) : new Color(255, 0, 0);
			CombatText.NewText(player.Hitbox, floatingTextColor, floatingTextStr);
			
			if (hiddenReaction != AlchemistHiddenReaction.NULL) {
				triggerAlchemistReactionEffects(hiddenReaction, mod, player, modPlayer);
			}
			
			if (reaction == false) {
				player.AddBuff((BuffType<Alchemist.Buffs.Debuffs.ReactionCooldown>()), 60 * 5);
				for(int i=0; i < 15; i++)
				{
					int dust = Dust.NewDust(player.Center, 10, 10, 37);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 2f;
					Main.dust[dust].scale *= 1.2f;
				}
				for(int i=0; i < 10; i++)
				{
					int dust = Dust.NewDust(player.Center, 10, 10, 6);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 2f;
					Main.dust[dust].scale *= 1.5f;
				}
				Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 16);
			} else {
				if (!(modPlayer.alchemistKnownReactions.Contains((int)hiddenReaction))) {
					if (modPlayer.alchemistKnownHints.Contains((int)hiddenReaction)) {
						modPlayer.alchemistKnownHints.Remove((int)hiddenReaction);
					}
					modPlayer.alchemistKnownReactions.Add((int)hiddenReaction);
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
			for (int i = 0 ; i < rand ; i ++) {
				int proj = ProjectileType<Alchemist.Projectiles.AlchemistSmoke1>();
				Vector2 vel = (new Vector2(0f, -((float)(2 + Main.rand.Next(5)))).RotatedByRandom(MathHelper.ToRadians(180)));
				int smokeProj = Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, proj, 0, 0f, player.whoAmI);
				Main.projectile[smokeProj].localAI[0] = colorRed;
				Main.projectile[smokeProj].localAI[1] = colorGreen;
				Main.projectile[smokeProj].ai[1] = colorBlue;
			}
			rand = 1 + Main.rand.Next(3);
			for (int i = 0 ; i < rand ; i ++) {
				int proj = ProjectileType<Alchemist.Projectiles.AlchemistSmoke2>();
				Vector2 vel = (new Vector2(0f, -((float)(2 + Main.rand.Next(5)))).RotatedByRandom(MathHelper.ToRadians(180)));
				int smokeProj = Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, proj, 0, 0f, player.whoAmI);
				Main.projectile[smokeProj].localAI[0] = colorRed;
				Main.projectile[smokeProj].localAI[1] = colorGreen;
				Main.projectile[smokeProj].ai[1] = colorBlue;
			}
			rand = Main.rand.Next(2);
			for (int i = 0 ; i < rand ; i ++) {
				int proj = ProjectileType<Alchemist.Projectiles.AlchemistSmoke3>();
				Vector2 vel = (new Vector2(0f, -((float)(2 + Main.rand.Next(5)))).RotatedByRandom(MathHelper.ToRadians(180)));
				int smokeProj = Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, proj, 0, 0f, player.whoAmI);
				Main.projectile[smokeProj].localAI[0] = colorRed;
				Main.projectile[smokeProj].localAI[1] = colorGreen;
				Main.projectile[smokeProj].ai[1] = colorBlue;
			}
			
			AlchemistHiddenReactionHelper.bonusReactionEffects(mod, player, modPlayer);
			
			OrchidModAlchemistHelper.clearAlchemistDusts(player, modPlayer, mod);
			modPlayer.alchemistFlaskDamage = 0;
			modPlayer.alchemistNbElements = 0;
			OrchidModAlchemistHelper.clearAlchemistElements(player, modPlayer, mod);
			OrchidModAlchemistHelper.clearAlchemistFlasks(player, modPlayer, mod);
			OrchidModAlchemistHelper.clearAlchemistColors(player, modPlayer, mod);
			modPlayer.alchemistSelectUIDisplay = false;
		}
		
		public static void bonusReactionEffects(Mod mod, Player player, OrchidModPlayer modPlayer) {
			if (modPlayer.alchemistReactiveVials) {
				player.AddBuff(BuffType<Alchemist.Buffs.ReactiveVialsBuff>(), 60 * 10);
			}
		}
		
		public static void addAlchemistHint(Player player, OrchidModPlayer modPlayer, int hintLevel) {
			Color floatingTextColor = new Color(0,0,0);
			string floatingTextStr = "";
			List<AlchemistHiddenReactionRecipe> validHints = new List<AlchemistHiddenReactionRecipe>();
			
			foreach (AlchemistHiddenReactionRecipe recipe in OrchidMod.alchemistReactionRecipes) {
				if (recipe.reactionLevel == hintLevel) {
					if (!(modPlayer.alchemistKnownReactions.Contains((int)recipe.reactionType) || modPlayer.alchemistKnownHints.Contains((int)recipe.reactionType))) {
						validHints.Add(recipe);
					}
				}
			}
			
			if (validHints.Count == 0) {
				floatingTextColor = new Color(255, 92, 0);
				floatingTextStr = "No hints left for this tier";
				CombatText.NewText(player.Hitbox, floatingTextColor, floatingTextStr);
			} else {
				int rand = Main.rand.Next(validHints.Count);
				AlchemistHiddenReactionRecipe hint = validHints[rand];
				modPlayer.alchemistKnownHints.Add((int)hint.reactionType);
				floatingTextColor = new Color(255, 187, 0);
				floatingTextStr = "New Hidden Reaction Hint";
				CombatText.NewText(player.Hitbox, floatingTextColor, floatingTextStr);
			}
		}
	}
}