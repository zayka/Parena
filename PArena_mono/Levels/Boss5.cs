using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PArena
{
    class Boss5:Boss
    {
        List<Vector2> phase1_w1 = new List<Vector2>() { new Vector2(-1000, 100), new Vector2(1024 + 1000, 100), new Vector2(1024 + 1000, 600), new Vector2(-1000, 600), new Vector2(-1000, 100),
                                                        new Vector2(150, 150), new Vector2(160, 150), new Vector2(140, 150),
                                                        new Vector2(850, 150), new Vector2(860, 150), new Vector2(840, 150),
                                                        new Vector2(850, 600), new Vector2(860, 600), new Vector2(840, 600),
                                                        new Vector2(150, 600), new Vector2(160, 600), new Vector2(140, 600),

        };


        List<Vector2> phase2_w1 = new List<Vector2>() { new Vector2(500, 100), new Vector2(500, 700), new Vector2(500, 100) };
        List<Vector2> phase3_w1 = new List<Vector2>() { new Vector2(500, 100), new Vector2(500, 700), new Vector2(500, 100) };

        int currentIndex = 0;
        Player player;
        delegate void BehaviorDelegate(GameTime gt);
        BehaviorDelegate phase;
        bool phase1Over;
        bool phase2Over;
        //bool phase3Over;

        float angle;
        float gunAngle;
        float pausep2;
        float gunTurnSpeed = MathHelper.PiOver2;


        float epsilon = 10;

        public Boss5(Texture2D tex)
            : base(tex)
        {
            time = 0;
            player = Cnt.game.CurrentPlayer;
            phase = Phase1;
            maxHitpoints = 10000;
            hitpoints = maxHitpoints;
           // hitpoints = 6660;
            turnspeed = 4 * MathHelper.Pi;
            Pos = phase1_w1[0];
            Dir = phase1_w1[0] - phase1_w1[1];
            Dir.Normalize();
            speed = 150;
            Width = 100;
            Height = 100;
            isBoss = true;
            deadAnimationTime = 2.5f;
            nSpot = 15;
            bloodColor = Color.Yellow;
            epsilon = 25;
            BulletType = typeof(Bullet_Boss5);
            isDistorted = true;
            
        }

        public override void Update(GameTime gt)
        {
            float elapsed = (float)gt.ElapsedGameTime.TotalSeconds;

            killTime += elapsed;
            if (killTime >= deadAnimationTime && toDie) { toRemove = true; return; }
            angle += elapsed * MathHelper.TwoPi;


            if (toDie)
            {
                spotTime += elapsed;
                if (spotTime > deadAnimationTime / nSpot)
                {
                    spotTime = 0;
                    Vector2 randomPos = new Vector2(Pos.X + (float)Game1.rnd.NextDouble() * 150 - 75, Pos.Y + (float)Game1.rnd.NextDouble() * 150 - 75);
                    int randomTex = Game1.rnd.Next(Cnt.game.spotCount);
                    Spot sp = new Spot { rect = new Rectangle(randomTex * 80, 0, 80, 80), pos = randomPos, color = bloodColor };
                    Cnt.game.spots.Add(sp);
                }
            }
            else
            {
                time += elapsed;
                bossTime += elapsed;
                timeToFire = MathHelper.Clamp(timeToFire - elapsed, 0, rof);
                if (hitpoints <= 2 * maxHitpoints / 3) phase = Phase2;
                if (hitpoints <= maxHitpoints / 3) phase = Phase3;
                phase(gt);
                //speed = MathHelper.Lerp(600, 300, hitpoints / maxHitpoints);
                //epsilon = MathHelper.Lerp(15, 5, hitpoints / maxHitpoints);
                hitpoints = MathHelper.Clamp(hitpoints, 0, maxHitpoints);
            }
        }


        void Phase1(GameTime gt)
        {
            phase1Over = false;

            rof = MathHelper.Lerp(0.3f, 1.0f, 3 * hitpoints / maxHitpoints - 2);
            float elapsed = (float)gt.ElapsedGameTime.TotalSeconds;
            Vector2 curWP = phase1_w1[currentIndex];
            Vector2 nextWP = phase1_w1[currentIndex + 1];

            if (currentIndex == 0) { MoveLine(curWP, nextWP, gt); speed = 2000; epsilon = 60; }
            if (currentIndex == 1) { MoveLine(curWP, nextWP, gt); speed = 300; epsilon = 10; }
            if (currentIndex == 2) { MoveLine(curWP, nextWP, gt); speed = 2000; epsilon = 60; }
            if (currentIndex == 3) { MoveLine(curWP, nextWP, gt); speed = 300; epsilon = 10; }
            if (currentIndex == 4) { Pos = nextWP; }
            if (currentIndex == 5) { MoveLine(curWP, nextWP, gt); speed = 10; epsilon = 2; }
            if (currentIndex == 6) { MoveLine(curWP, nextWP, gt); speed = 10; epsilon = 2; }
            if (currentIndex == 7) { Pos = nextWP; }
            if (currentIndex == 8) { MoveLine(curWP, nextWP, gt); speed = 10; epsilon = 2; }
            if (currentIndex == 9) { MoveLine(curWP, nextWP, gt); speed = 10; epsilon = 2; }
            if (currentIndex == 10) { Pos = nextWP; }
            if (currentIndex == 11) { MoveLine(curWP, nextWP, gt); speed = 10; epsilon = 2; }
            if (currentIndex == 12) { MoveLine(curWP, nextWP, gt); speed = 10; epsilon = 2; }
            if (currentIndex == 13) { Pos = nextWP; }
            if (currentIndex == 14) { MoveLine(curWP, nextWP, gt); speed = 10; epsilon = 2; }
            if (currentIndex == 15) { MoveLine(curWP, nextWP, gt); speed = 10; epsilon = 2; }


            if (canFire)
            {
                if (isOnScreen && currentIndex<=4)
                {
                    Shoot(Pos, player.Pos);
                    timeToFire = 0.01f;
                }
                if (currentIndex > 4)
                {
                    Shoot(Pos, player.Pos);
                    timeToFire = rof;
                }
                //timeToFire = rof;
            }

            if (Vector2.DistanceSquared(Pos, nextWP) <= epsilon) { currentIndex++; time = 0; }
            if (currentIndex == phase1_w1.Count - 1) { Pos = phase1_w1[0]; currentIndex = 0; }
        }

        void Phase2(GameTime gt)
        {
            if (!phase1Over)
            {
                time = 0;
                phase1Over = true;
                currentIndex = 0;
                rof = 0.2f;
                Pos = phase2_w1[0];
                gunAngle = MathHelper.PiOver2 - MathHelper.PiOver4;
                pausep2 = 2;
                speed = 50;
               
            }
            phase2Over = false;

            float elapsed = (float)gt.ElapsedGameTime.TotalSeconds;
            Vector2 curWP = phase2_w1[currentIndex];
            Vector2 nextWP = phase2_w1[currentIndex + 1];
            pausep2 -= elapsed;
            speed = MathHelper.Lerp(50, 100, -3*hitpoints / maxHitpoints+2);
            
            if (currentIndex == 0) { MoveLine(curWP, nextWP, gt);  }
            if (currentIndex == 1) { MoveLine(curWP, nextWP, gt);  }

            if (canFire )
            {
                float r = 100;
                if (pausep2 < 0)
                {
                   
                    for (int i = 0; i < 15; i++)
                    {
                        Shoot(Pos, Pos + new Vector2(r * (float)Math.Sin(gunAngle + i * MathHelper.PiOver2 / 15), r * (float)Math.Cos(gunAngle + i * MathHelper.PiOver2 / 15)));
                        Shoot(Pos, Pos + new Vector2(r * (float)Math.Sin(gunAngle + MathHelper.PiOver2 + i * MathHelper.PiOver2 / 15), r * (float)Math.Cos(gunAngle + MathHelper.PiOver2 + i * MathHelper.PiOver2 / 15)));
                        Shoot(Pos, Pos + new Vector2(r * (float)Math.Sin(gunAngle + MathHelper.Pi + i * MathHelper.PiOver2 / 15), r * (float)Math.Cos(gunAngle + MathHelper.Pi + i * MathHelper.PiOver2 / 15)));
                    }
                    
                }
                else 
                {
                    Shoot(Pos, Pos + new Vector2(r * (float)Math.Sin(gunAngle - MathHelper.PiOver4), r * (float)Math.Cos(gunAngle-MathHelper.PiOver4)));
                }
                timeToFire = rof;
            }


            if (Vector2.DistanceSquared(Pos, nextWP) <= epsilon) { currentIndex++; time = 0; gunAngle += Game1.rnd.Next(3)*MathHelper.PiOver2+MathHelper.PiOver4; pausep2 = 2; }
            if (currentIndex == phase2_w1.Count - 1) currentIndex = 0;

        }

        void Phase3(GameTime gt)
        {
            if (!phase2Over)
            {
                time = 0;
                phase2Over = true;
                currentIndex = 0;
                rof = 0.2f;
                Pos = phase3_w1[0];
                gunAngle = MathHelper.PiOver2 - MathHelper.PiOver4;
                pausep2 = 2;
                speed = 70;
            }
            
            //phase3Over = false;

            float elapsed = (float)gt.ElapsedGameTime.TotalSeconds;
            Vector2 curWP = phase3_w1[currentIndex];
            Vector2 nextWP = phase3_w1[currentIndex + 1];
            pausep2 -= elapsed;

            if (currentIndex == 0) { MoveLine(curWP, nextWP, gt); }
            if (currentIndex == 1) { MoveLine(curWP, nextWP, gt); }

            if (canFire)
            {
                float r = 100;
                if (pausep2 < 0)
                {

                    for (int i = 0; i < 15; i++)
                    {
                        Shoot(Pos, Pos + new Vector2(r * (float)Math.Sin(gunAngle + i * MathHelper.PiOver2 / 15), r * (float)Math.Cos(gunAngle + i * MathHelper.PiOver2 / 15)));
                        Shoot(Pos, Pos + new Vector2(r * (float)Math.Sin(gunAngle + MathHelper.PiOver2 + i * MathHelper.PiOver2 / 15), r * (float)Math.Cos(gunAngle + MathHelper.PiOver2 + i * MathHelper.PiOver2 / 15)));
                        Shoot(Pos, Pos + new Vector2(r * (float)Math.Sin(gunAngle + MathHelper.Pi + i * MathHelper.PiOver2 / 15), r * (float)Math.Cos(gunAngle + MathHelper.Pi + i * MathHelper.PiOver2 / 15)));
                    }
                    gunAngle += elapsed * gunTurnSpeed;
                }
                else
                {
                    Shoot(Pos, Pos + new Vector2(r * (float)Math.Sin(gunAngle - MathHelper.PiOver4), r * (float)Math.Cos(gunAngle - MathHelper.PiOver4)));
                }
                timeToFire = rof;
            }


            if (Vector2.DistanceSquared(Pos, nextWP) <= epsilon) { currentIndex++; time = 0; gunAngle += Game1.rnd.Next(3) * MathHelper.PiOver2 + MathHelper.PiOver4; pausep2 = 2; }
            if (currentIndex == phase2_w1.Count - 1) currentIndex = 0;

        }      

    }
}
