using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class ShuffleCardProj : OrchidModGamblerProjectile
	{
		private Vector2 initialHeartsVelocity = new Vector2(0f ,0f);
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shuffle");
        } 
		
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
		public override void SafeSetDefaults()
		{
			projectile.width = 18;
            projectile.height = 20;
            projectile.friendly = false;
            projectile.aiStyle = 0;
			projectile.timeLeft = 900;
			projectile.penetrate = -1;
			this.gamblingChipChance = 10;
			//this.projectileTrail = true;
			Main.projFrames[projectile.type] = 4;
		}
		
		public override void SafeAI()
		{
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int cardType = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj ? modPlayer.gamblerCardDummy.type : modPlayer.gamblerCardCurrent.type;
			projectile.frame = (int)(projectile.ai[0]);
			
			if (projectile.ai[1] != 6f) {
				Vector2 pos = new Vector2(0f, 0f);
				switch (projectile.ai[1]) {
					case 1:
						pos.X = player.Center.X;
						pos.Y = player.Center.Y - 75;
						break;
					case 2:
						pos.X = player.Center.X - 20;
						pos.Y = player.Center.Y - 60;
						break;
					case 3:
						pos.X = player.Center.X + 20;
						pos.Y = player.Center.Y - 60;
						break;
					case 4:
						pos.X = player.Center.X - 12;
						pos.Y = player.Center.Y - 38;
						break;
					case 5:
						pos.X = player.Center.X + 12;
						pos.Y = player.Center.Y - 38;
						break;
					default:
						break;
				}
				projectile.position = pos - (new Vector2(projectile.width, projectile.height) / 2);
				if (Main.myPlayer == projectile.owner) {
					Vector2 target = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
					Vector2 heading = target - projectile.Center;
					heading.Normalize();
					heading *= 15f;
					projectile.velocity = heading;
					projectile.rotation = projectile.velocity.RotatedBy(MathHelper.ToRadians(90f)).ToRotation();
					projectile.direction = projectile.spriteDirection;
					if (!(Main.mouseLeft && cardType == ItemType<Gambler.Weapons.Cards.ShuffleCard>())) {
					int count = 0;
						if (projectile.frame > 0) {
							for (int l = 0; l < Main.projectile.Length; l++) {  
								Projectile proj = Main.projectile[l];
								if (player.whoAmI == proj.owner && proj.active && proj.ai[1] != 6f && proj.type == projectile.type)  {
									count ++;
								}
							}
							
							float spreadMult = 1f + (0.5f * (count - 1));
							float spread =  projectile.frame == 3 ? 10f : 5f;
							projectile.velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(spread * (projectile.ai[1] - spreadMult)));
						} else {
							projectile.velocity *= 0.01f;
						}
						projectile.knockBack = projectile.frame == 1 ? 0.5f * count : 0f;
						projectile.penetrate = projectile.frame % 3 == 0 ? 1 : projectile.frame == 2 ? 3 : -1;
						projectile.friendly = true;
						projectile.damage = projectile.frame == 2 ? (int)(projectile.damage * 2f) : projectile.damage;
						projectile.ai[1] = 6f;
						
						switch (projectile.frame) {
							case 0 :
								projectile.timeLeft = 75;
								break;
							case 1 :
								projectile.timeLeft = 100 + (20 * count) - (int)(projectile.ai[1] * 10);
								break;
							case 2 :
								projectile.timeLeft = 105;
								break;
							case 3 :
								projectile.timeLeft = 30;
								break;
							default :
								projectile.timeLeft = 0;
								break;
						}
						
						projectile.netUpdate = true;
					} else {
						projectile.velocity *= 0f;
					}
				}
			} else {
				if (projectile.timeLeft > 895) {
					projectile.active = false;
					projectile.netUpdate = true;
				}
				
				if (projectile.frame == 1) {
					projectile.rotation += 0.25f;
					projectile.velocity *= 0.925f;
				} else {
					projectile.rotation = projectile.velocity.RotatedBy(MathHelper.ToRadians(90f)).ToRotation();
					projectile.direction = projectile.spriteDirection;
				}
				
				if (projectile.frame == 0 && projectile.timeLeft > 20) {
					projectile.velocity *= 1.1f;
				}
				
				if (projectile.frame == 2) {
					if (initialHeartsVelocity == Vector2.Zero) {
						initialHeartsVelocity = projectile.velocity;
					}
					projectile.velocity -= initialHeartsVelocity * 0.02f;
				}
				
				if (Main.rand.Next(5) == 0) {
					int dustType = projectile.frame < 2 ? 63 : 60;
					Vector2 pos = new Vector2(projectile.position.X, projectile.position.Y);
					int index = Dust.NewDust(pos, projectile.width, projectile.height, dustType);
					Main.dust[index].velocity *= 0.25f;
					Main.dust[index].scale *= 1.5f;
					Main.dust[index].noGravity = true;
				}
			}
			
			
			if (!this.initialized) {
				this.initialized = true;
				int dustType = projectile.frame < 2 ? 63 : 60;
				OrchidModProjectile.spawnDustCircle(projectile.Center, dustType, 5, 5, true, 1.5f, 1f, 3f, true, true, false, 0, 0, true);
			}
		}
		
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
			if (projectile.frame == 1) {
				projectile.velocity *= 0f;
			} else {
				projectile.Kill();
			}
            return false;
        }
		
		public override void Kill(int timeLeft) {
			for (int i = 0 ; i < 3 ; i ++) {
				int dustType = projectile.frame < 2 ? 63 : 60;
				Vector2 pos = new Vector2(projectile.position.X, projectile.position.Y);
				int index = Dust.NewDust(pos, projectile.width, projectile.height, dustType);
				Main.dust[index].velocity *= 0.25f;
				Main.dust[index].scale *= 1.5f;
				Main.dust[index].noGravity = true;
			}
			
			if (projectile.frame == 3 ||projectile.frame == 1) {
				bool dummy = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
				int dustType = projectile.frame < 2 ? 63 : 60;
				OrchidModProjectile.spawnDustCircle(projectile.Center, dustType, 10, 15, true, 1.5f, 1f, 5f, true, true, false, 0, 0, true);
				OrchidModGamblerHelper.DummyProjectile(spawnGenericExplosion(projectile, projectile.damage, projectile.knockBack, 80, 3, false, 14), dummy);
			} else {
				Main.PlaySound(2, (int)projectile.Center.X ,(int)projectile.Center.Y, 1);
			}
		}
	}
}