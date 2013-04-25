using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PArena
{
    class Boss2:Boss
    {
        List<Vector2> phase1_w1 = new List<Vector2>() { new Vector2(200, 400), new Vector2(500, 100), new Vector2(900, 400), new Vector2(500, 700), new Vector2(200, 400) };
        List<Vector2> phase2_w1 = new List<Vector2>() { new Vector2(100, 100), new Vector2(1000, 100), new Vector2(100, 100) };
        List<Vector2> phase3_w1 = new List<Vector2>() { new Vector2(100, 600), new Vector2(1000, 600), new Vector2(100, 600) };

        int currentIndex = 0;
        Player player;
        delegate void BehaviorDelegate(GameTime gt);
        BehaviorDelegate phase;
        bool phase1Over;
        bool phase2Over;
        //bool phase3Over;
        //bool phase4Over;

        float epsilon = 10;

        public Boss2(Texture2D tex):base(tex)
        {
            time = 0;
            player = Cnt.game.CurrentPlayer;
            phase = Phase1;
            maxHitpoints = 5000;
            hitpoints = maxHitpoints;
            turnspeed = 2 * MathHelper.Pi;
            Pos = phase1_w1[0];
            Dir = phase1_w1[0] - phase1_w1[1];
            Dir.Normalize();
            speed = 300;
            Width = 100;
            Height = 100;
            isBoss = true;
            deadAnimationTime = 2.5f;
            nSpot = 15;
            epsilon = 25;

            bloodColor = Color.BlueViolet;            
            BulletType = typeof(Bullet_Boss2);
        }

        public override void Update(GameTime gt)
        {
            float elapsed = (float)gt.ElapsedGameTime.TotalSeconds;

            killTime += elapsed;
            bossTime += elapsed;
            if (killTime >= deadAnimationTime && toDie) { toRemove = true; return; }

            if (toDie)
            {
                spotTime += elapsed;
                if (spotTime > deadAnimationTime / nSpot)
                {
                    spotTime = 0;
                    Vector2 randomPos = new Vector2(Pos.X + (float)Game1.rnd.NextDouble() * 150 - 75, Pos.Y + (float)Game1.rnd.NextDouble() * 150 - 75);
                    //Vector2 randomPos = Pos;
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
                if (hitpoints <= maxHitpoints / 2)  phase = Phase2;  
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

            rof = 1.5f;
            float elapsed = (float)gt.ElapsedGameTime.TotalSeconds;
            Vector2 curWP = phase1_w1[currentIndex];
            Vector2 nextWP = phase1_w1[currentIndex + 1];

            if (currentIndex == 0) MoveLine(curWP, nextWP, gt);
            if (currentIndex == 1) MoveLine(curWP, nextWP, gt);
            if (currentIndex == 2) MoveLine(curWP, nextWP, gt);
            if (currentIndex == 3) MoveLine(curWP, nextWP, gt);
            if (currentIndex == 4) MoveLine(curWP, nextWP, gt);
            if (currentIndex == 5) MoveLine(curWP, nextWP, gt);

            if (canFire)
            {
                int r = 100;
                for (int i = 0; i < 16; i++)
                {
                    Shoot(Pos,Pos+new Vector2(r*(float)Math.Sin(i*MathHelper.TwoPi/16),r*(float)Math.Cos(i*MathHelper.TwoPi/16)));
                   
                }
                timeToFire = rof;
            }
            if (Vector2.DistanceSquared(Pos, nextWP) <= epsilon) { currentIndex++; time = 0; }
            if (currentIndex == phase1_w1.Count - 1) currentIndex = 0;


        }
        void Phase2(GameTime gt)
        {
            if (!phase1Over)
            {
                time = 0;
                phase1Over = true;
                currentIndex = 0;
                rof = 0.4f;
            }
            phase2Over = false;

            //rof = 1.5f;
            float elapsed = (float)gt.ElapsedGameTime.TotalSeconds;
            Vector2 curWP = phase2_w1[currentIndex];
            Vector2 nextWP = phase2_w1[currentIndex + 1];

            if (currentIndex == 0) MoveSin(curWP, nextWP, gt);
            if (currentIndex == 1) MoveSin(curWP, nextWP, gt);


            if (canFire)
            {                
                Shoot(Pos, player.Pos);               
                timeToFire = rof;
            }


            if (Vector2.DistanceSquared(Pos, nextWP) <= epsilon) { currentIndex++; time = 0; }
            if (currentIndex == phase2_w1.Count - 1) currentIndex = 0;
        
        }

        void Phase3(GameTime gt)
        {
            if (!phase2Over)
            {
                time = 0;
                phase2Over = true;
                currentIndex = 0;
                rof = 0.3f;
            }
            //phase3Over = false;

           
            float elapsed = (float)gt.ElapsedGameTime.TotalSeconds;
            Vector2 curWP = phase3_w1[currentIndex];
            Vector2 nextWP = phase3_w1[currentIndex + 1];

            if (currentIndex == 0) MoveSin(curWP, nextWP, gt);
            if (currentIndex == 1) MoveSin(curWP, nextWP, gt);

            if (canFire)
            {
                if (Game1.rnd.Next(100) < 10)
                {
                    int r = 100;
                    for (int i = 0; i < 12; i++)
                    {
                        Shoot(Pos, Pos + new Vector2(r * (float)Math.Sin(i * MathHelper.TwoPi / 12), r * (float)Math.Cos(i * MathHelper.TwoPi / 12)));
                    }
                }
                Shoot(Pos, player.Pos);

                timeToFire = rof;
            }

            if (Vector2.DistanceSquared(Pos, nextWP) <= epsilon) { currentIndex++; time = 0; }
            if (currentIndex == phase2_w1.Count - 1) currentIndex = 0;

        }
    }
}
