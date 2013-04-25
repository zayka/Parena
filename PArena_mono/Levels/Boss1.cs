using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PArena
{
    public class Boss1:Boss
    {

        List<Vector2> phase1_w1 = new List<Vector2>() { new Vector2(100, 150), new Vector2(900, 150), new Vector2(900, 680), new Vector2(100, 680), new Vector2(100, 150) };
        List<Vector2> phase2_w1 = new List<Vector2>() { new Vector2(100, 150), new Vector2(100, 680), new Vector2(900, 680), new Vector2(900, 150), new Vector2(100, 150) };                
        int currentIndex = 0;

        
        Player player;
        delegate void BehaviorDelegate(GameTime gt);
        BehaviorDelegate phase;
       
        float epsilon = 10;
       

        public Boss1(Texture2D tex):base(tex)
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
            deadAnimationTime = 2.5f;
            nSpot = 15;
            bloodColor = Color.RosyBrown;
            BulletType = typeof(Bullet_Boss1);           
            radius = (float)Math.Sqrt(Height * Height + Width * Width)/2;
        }

        public override void Update(GameTime gt)
        {
            float elapsed = (float)gt.ElapsedGameTime.TotalSeconds;

            killTime += elapsed;
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
                Vector2 curWP = phase1_w1[currentIndex];
                Vector2 nextWP = phase1_w1[currentIndex + 1];
                time += elapsed;
                bossTime += elapsed;
                timeToFire = MathHelper.Clamp(timeToFire - (float)gt.ElapsedGameTime.TotalSeconds, 0, rof);
                //if (hitpoints > 2 * hitpoints / 3) phase = Phase2;
                if (hitpoints <= 2 * maxHitpoints / 3) phase = Phase2;
                if (hitpoints <= maxHitpoints / 3) phase = Phase3;
                phase(gt);
                speed = MathHelper.Lerp(600, 300, hitpoints / maxHitpoints);
                epsilon = MathHelper.Lerp(15, 5, hitpoints / maxHitpoints);
                hitpoints = MathHelper.Clamp(hitpoints, 0, maxHitpoints);
            }
        }

        void Phase1(GameTime gt)
        {
            rof = 1.5f;
            float elapsed = (float)gt.ElapsedGameTime.TotalSeconds;
            Vector2 curWP = phase1_w1[currentIndex];
            Vector2 nextWP = phase1_w1[currentIndex + 1];

            if (currentIndex == 0) MoveLine(curWP, nextWP, gt);
            if (currentIndex == 1) MoveLine(curWP, nextWP, gt);
            if (currentIndex == 2) MoveLine(curWP, nextWP, gt);
            if (currentIndex == 3) MoveLine(curWP, nextWP, gt);


            if (canFire)
            {
                Shoot(Pos + new Vector2(-Width/2, -Height/2 + 1), Pos + new Vector2(-Width/2, -Height/2));
                Shoot(Pos + new Vector2(Width / 2, -Height / 2 + 1), Pos + new Vector2(Width / 2, -Height / 2));
                Shoot(Pos + new Vector2(Width / 2, Height / 2 - 1), Pos + new Vector2(Width / 2, Height / 2));
                Shoot(Pos + new Vector2(-Width / 2, Height / 2 - 1), Pos + new Vector2(-Width / 2, Height / 2));               

                Shoot(Pos, Cnt.game.CurrentPlayer.Pos);
                timeToFire = rof;
            }

            if (Vector2.DistanceSquared(Pos, nextWP) <= epsilon) { currentIndex++; time = 0; }
            if (currentIndex == phase1_w1.Count - 1) currentIndex = 0;  
        }

        void Phase2(GameTime gt)
        {
            rof = 1.5f;
            float elapsed = (float)gt.ElapsedGameTime.TotalSeconds;
            Vector2 curWP = phase2_w1[currentIndex];
            Vector2 nextWP = phase2_w1[currentIndex + 1];

            if (currentIndex == 0) MoveLine(curWP, nextWP, gt);
            if (currentIndex == 1) MoveLine(curWP, nextWP, gt);
            if (currentIndex == 2) MoveLine(curWP, nextWP, gt);
            if (currentIndex == 3) MoveLine(curWP, nextWP, gt);


            if (canFire)
            {
                Shoot(Pos + new Vector2(-Width / 2, -Height / 2 + 1), Pos + new Vector2(-Width / 2, -Height / 2));
                Shoot(Pos + new Vector2(Width / 2, -Height / 2 + 1), Pos + new Vector2(Width / 2, -Height / 2));
                Shoot(Pos + new Vector2(Width / 2, Height / 2 - 1), Pos + new Vector2(Width / 2, Height / 2));
                Shoot(Pos + new Vector2(-Width / 2, Height / 2 - 1), Pos + new Vector2(-Width / 2, Height / 2));

                Shoot(Pos + new Vector2(-Width / 2 - 1, -Height / 2), Pos + new Vector2(-Width / 2, -Height / 2));
                Shoot(Pos + new Vector2(Width / 2 + 1, -Height / 2), Pos + new Vector2(Width / 2, -Height / 2));
                Shoot(Pos + new Vector2(Width / 2 + 1, Height / 2), Pos + new Vector2(Width / 2, Height / 2));
                Shoot(Pos + new Vector2(-Width / 2 - 1, Height / 2), Pos + new Vector2(-Width / 2, Height / 2));

                Shoot(Pos, Cnt.game.CurrentPlayer.Pos);
                timeToFire = rof;
            }
            if (Vector2.DistanceSquared(Pos, nextWP) <= epsilon) { currentIndex++; time = 0; }
            if (currentIndex == phase2_w1.Count - 1) currentIndex = 0;  

        }


        void Phase3(GameTime gt)
        {
            rof = 1.5f;
            float elapsed = (float)gt.ElapsedGameTime.TotalSeconds;
            Vector2 curWP = phase1_w1[currentIndex];
            Vector2 nextWP = phase1_w1[currentIndex + 1];

            if (currentIndex == 0) MoveLine(curWP, nextWP, gt);
            if (currentIndex == 1) MoveLine(curWP, nextWP, gt);
            if (currentIndex == 2) MoveLine(curWP, nextWP, gt);
            if (currentIndex == 3) MoveLine(curWP, nextWP, gt);


            if (canFire)
            {
                Shoot(Pos + new Vector2(-Width / 2, -Height / 2 + 1), Pos + new Vector2(-Width / 2, -Height / 2));
                Shoot(Pos + new Vector2(Width / 2, -Height / 2 + 1), Pos + new Vector2(Width / 2, -Height / 2));
                Shoot(Pos + new Vector2(Width / 2, Height / 2 - 1), Pos + new Vector2(Width / 2, Height / 2));
                Shoot(Pos + new Vector2(-Width / 2, Height / 2 - 1), Pos + new Vector2(-Width / 2, Height / 2));

                Shoot(Pos + new Vector2(-Width / 2 - 1, -Height / 2), Pos + new Vector2(-Width / 2, -Height / 2));
                Shoot(Pos + new Vector2(Width / 2 + 1, -Height / 2), Pos + new Vector2(Width / 2, -Height / 2));
                Shoot(Pos + new Vector2(Width / 2 + 1, Height / 2), Pos + new Vector2(Width / 2, Height / 2));
                Shoot(Pos + new Vector2(-Width / 2 - 1, Height / 2), Pos + new Vector2(-Width / 2, Height / 2));

                Shoot(Pos + new Vector2(-Width / 2, -Height / 2 + 1), Cnt.game.CurrentPlayer.Pos);
                Shoot(Pos + new Vector2(Width / 2, -Height / 2 + 1), Cnt.game.CurrentPlayer.Pos);
                Shoot(Pos + new Vector2(Width / 2, Height / 2 - 1), Cnt.game.CurrentPlayer.Pos);
                Shoot(Pos + new Vector2(-Width / 2, Height / 2 - 1), Cnt.game.CurrentPlayer.Pos);

                Shoot(Pos + new Vector2(-Width / 2 - 1, -Height / 2), Cnt.game.CurrentPlayer.Pos);
                Shoot(Pos + new Vector2(Width / 2 + 1, -Height / 2), Cnt.game.CurrentPlayer.Pos);
                Shoot(Pos + new Vector2(Width / 2 + 1, Height / 2), Cnt.game.CurrentPlayer.Pos);
                Shoot(Pos + new Vector2(-Width / 2 - 1, Height / 2), Cnt.game.CurrentPlayer.Pos);

                Shoot(Pos, Cnt.game.CurrentPlayer.Pos);
                timeToFire = rof;
            }
            if (Vector2.DistanceSquared(Pos, nextWP) <= epsilon) { currentIndex++; time = 0; }
            if (currentIndex == phase1_w1.Count - 1) currentIndex = 0;

        }


      
    }
}
