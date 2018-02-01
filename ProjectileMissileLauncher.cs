using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InsaneWarfare
{
    class ProjectileMissileLauncher : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Missile");
        }

        public override void SetDefaults()
        {
            projectile.width = 5;
            projectile.height = 10;
            projectile.aiStyle = -1;
            projectile.penetrate = 5;
            projectile.scale = 1.3f;
            projectile.damage = 100;
            projectile.light = 1;

            ////projectile.ownerHitCheck = true;
            ////projectile.tileCollide = true;
            projectile.friendly = true;

            if (InsaneWarfare.rand.Next(0, 5) == 0)
            {
                projectile.ai[0] = 1;   // is huge bullet

                projectile.scale *= 2;
            }
            else
            {
                projectile.ai[0] = 0;
            }

        }

        public float MovementFactor // Change this value to alter how fast the spear moves
        {
            get { return projectile.ai[0]; }
            set { projectile.ai[0] = value; }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(projectile.height,projectile.width).SafeNormalize(new Vector2());
            //Dust.NewDustPerfect(projectile.position , 1, projectile.velocity * 0.9f, 0, new Color(100, 255, 100), 1);
            Dust.NewDust((projectile.position + new Vector2(projectile.height*projectile.scale, 0)).RotatedBy((-projectile.velocity).ToRotation(), projectile.position), 15, 15, DustID.Electric, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, newColor: new Color(0,255,100));
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.penetrate--;
            if (projectile.penetrate <= 0)
            {
                projectile.Kill();
            }
            else
            {
                if (projectile.velocity.X != oldVelocity.X)
                {
                    projectile.velocity.X = -oldVelocity.X;
                }
                if (projectile.velocity.Y != oldVelocity.Y)
                {
                    projectile.velocity.Y = -oldVelocity.Y;
                }
                Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
            }
            return false;
        }

        public override void AI()
        {
            Player projOwner = Main.player[projectile.owner];

            var npcs = Main.npc.Where(x => !x.friendly && x.active && x.lifeMax > 5);
            var target = npcs.OrderBy(x => (projectile.position - x.position).Length()).FirstOrDefault();
            if (target != null && (projectile.position - target.position).Length() < 200)
            {
                var deltaPos = (target.position - projectile.position);
                deltaPos.X *= 0.9f;
                deltaPos.Normalize();
                var speedFac = 0.5f;
                projectile.velocity *= 0.95f;
                projectile.velocity += deltaPos * speedFac;
            }

            projectile.rotation = (float)Math.Atan2(projectile.velocity.X, -projectile.velocity.Y);

            //projectile.velocity *= new Vector2(1.001f,1.001f);
        }

        public override bool PreKill(int timeLeft)
        {
            // explosion code
            Main.PlaySound(SoundID.DD2_ExplosiveTrapExplode, projectile.position);
            var dmg = InsaneWarfare.rand.Next(1, 4) * 50f;
            foreach (var item in Main.npc.Where(x => (x.position - projectile.position).Length() < 25 && !x.friendly).ToList())
            {
                item.HitEffect(0, dmg);
            }
            // Smoke Dust spawn
            for (int i = 0; i < 50; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= 1.4f;
            }
            // Fire Dust spawn
            for (int i = 0; i < 80; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 5f;
                dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= 3f;
            }
            // Large Smoke Gore spawn
            for (int g = 0; g < 2; g++)
            {
                int goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
                goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
                goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
                goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
            }
            // explosion code end
            return base.PreKill(timeLeft);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.life - damage <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    int a = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 16f, Main.rand.Next(-10, 11) * .25f, Main.rand.Next(-10, -5) * .25f, 207, (int)((projectile.damage + 1) * .5f), 0, projectile.owner);
                    Main.projectile[a].aiStyle = 1;
                    Main.projectile[a].tileCollide = true;
                }

            }
            else
            {
                target.AddBuff(BuffID.OnFire, 200, true);
            }
            projectile.Kill();
        }
    }
}
