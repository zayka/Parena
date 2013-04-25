using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PArena
{
    class Boss3:Boss
    {
        List<Vector2> phase1_w1 = new List<Vector2>() { new Vector2(120, 100), new Vector2(850, 100), new Vector2(850, 600), new Vector2(120, 600), new Vector2(120, 100) };
        List<Vector2> phase2_w1 = new List<Vector2>() { new Vector2(500, 375), new Vector2(520, 395) };
        List<Vector2> phase3_w1 = new List<Vector2>() { new Vector2(500, 375), new Vector2(520, 395) };

        int currentIndex = 0;
        Player player;
        delegate void BehaviorDelegate(GameTime gt);
        BehaviorDelegate phase;
        bool phase1Over;
        bool phase2Over;
        //bool phase3Over;
        float angle;
        float angleGun;
        float gunTurnSpeed;
        float pauseph3;


        float epsilon = 10;

        public Boss3(Texture2D tex)
            : base(tex)
        {
            time = 0;
            player = Cnt.game.CurrentPlayer;
            phase = Phase1;
            maxHitpoints = 10000;
            hitpoints = maxHitpoints;
            //hitpoints = 700;
            turnspeed = 2 * MathHelper.Pi;
            Pos = phase1_w1[0];
            Dir = phase1_w1[0] - phase1_w1[1];
            Dir.Normalize();
            speed = 150;
            Width = 100;
            Height = 100;
            isBoss = true;
            deadAnimationTime = 2.5f;
            nSpot = 15;
            bloodColor = Color.BlueViolet;
            epsilon = 25;
            BulletType = typeof(Bullet_Boss3);
            isDistorted = true;
            angle = 0;
            angleGun = 0;
            gunTurnSpeed = MathHelper.PiOver4;
            pauseph3=2;
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
                if (hitpoints <= 2*maxHitpoints / 3) phase = Phase2;
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

            rof = MathHelper.Lerp(0.1f, 1.0f, 3*hitpoints/maxHitpoints-2);
            float elapsed = (float)gt.ElapsedGameTime.TotalSeconds;
            Vector2 curWP = phase1_w1[currentIndex];
            Vector2 nextWP = phase1_w1[currentIndex + 1];
           
           

            if (currentIndex == 0) { MoveLine(curWP, nextWP, gt); speed = 100; }
            if (currentIndex == 1) { MoveLine(curWP, nextWP, gt); speed = 300; }
            if (currentIndex == 2) { MoveLine(curWP, nextWP, gt); speed = 100; }
            if (currentIndex == 3) { MoveLine(curWP, nextWP, gt); speed = 300; }
            
            if (canFire)
            {
                if (currentIndex == 0 || currentIndex == 2)
                {
                    int r = 100;
                    for (int i = 0; i < 18; i++)
                    {
                        Shoot(Pos, Pos + new Vector2(r * (float)Math.Sin(0.025f+i * MathHelper.TwoPi / 18), r * (float)Math.Cos(0.025f+ i * MathHelper.TwoPi / 18)));
                    }
                    timeToFire = 0.2f;
                }
                
                //timeToFire = rof;
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
                rof = 0.2f;
            }
            phase2Over = false;
            
            float elapsed = (float)gt.ElapsedGameTime.TotalSeconds;
            Vector2 curWP = phase2_w1[currentIndex];
            Vector2 nextWP = phase2_w1[currentIndex + 1];
            gunTurnSpeed = MathHelper.Lerp(3*MathHelper.PiOver4, MathHelper.PiOver4, hitpoints / maxHitpoints);

            angleGun +=  elapsed * gunTurnSpeed ;
           
           // speed = 
            if (Vector2.DistanceSquared(Pos, nextWP) > 20) speed = 300;
            else speed = 10;

            if (currentIndex == 0) { MoveLine(curWP, nextWP, gt); speed = 300; }
            if (currentIndex == 1) { MoveLine(curWP, nextWP, gt); speed = 300; }

            if (canFire&&Vector2.DistanceSquared(Pos,nextWP)<20)
            {
                int r = 100;
                for (int i = 0; i < 10; i++)
                {
                    Shoot(Pos, Pos + new Vector2(r * (float)Math.Sin(angleGun + i * MathHelper.PiOver2 / 10), r * (float)Math.Cos(angleGun + i * MathHelper.PiOver2 / 10)));
                }
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
                phase1Over = true;
                currentIndex = 0;
                rof = 0.2f;
            }
            //phase3Over = false;

            float elapsed = (float)gt.ElapsedGameTime.TotalSeconds;
            Vector2 curWP = phase3_w1[currentIndex];
            Vector2 nextWP = phase3_w1[currentIndex + 1];
            gunTurnSpeed = MathHelper.Lerp(3 * MathHelper.PiOver4, MathHelper.PiOver4, hitpoints / maxHitpoints);

            pauseph3 -= elapsed;
            angleGun -= elapsed * gunTurnSpeed;
            // speed = 
            if (Vector2.DistanceSquared(Pos, nextWP) > 20) speed = 300;
            else speed = 10;

            if (currentIndex == 0) { MoveLine(curWP, nextWP, gt);  }
            if (currentIndex == 1) { MoveLine(curWP, nextWP, gt);  }

            if (canFire && Vector2.DistanceSquared(Pos, nextWP) < 20 && pauseph3 < 0)
            {
                int r = 100;
                for (int i = 0; i < 10; i++)
                {
                    Shoot(Pos, Pos + new Vector2(r * (float)Math.Sin(angleGun + i * MathHelper.PiOver2 / 10), r * (float)Math.Cos(angleGun + i * MathHelper.PiOver2 / 10)));
                }
                timeToFire = rof;
            }


            if (Vector2.DistanceSquared(Pos, nextWP) <= epsilon) { currentIndex++; time = 0; }
            if (currentIndex == phase2_w1.Count - 1) currentIndex = 0;

        }       



        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, Pos, null, Color.White, angle, origin, 1, SpriteEffects.None, 0);
           // base.Draw(sb);
        }

        public override void Draw(SpriteBatch sb, Vector2 offset)
        {
            //sb.Draw(sprite, Pos - origin + , Color.White);
            sb.Draw(sprite, Pos+offset, null, Color.White, angle, origin, 1, SpriteEffects.None, 0);
        }
    }
}
