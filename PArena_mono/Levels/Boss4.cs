using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace PArena
{
    class Boss4 : Boss
    {
        List<Vector2> phase1_w1 = new List<Vector2>() { new Vector2(490, 100), new Vector2(560, 100), new Vector2(850, 100), new Vector2(850, 600), new Vector2(100, 600),new Vector2(150, 150), new Vector2(490, 100) };
        List<Vector2> phase2_w1 = new List<Vector2>() { new Vector2(490, 100), new Vector2(560, 100), new Vector2(850, 100), new Vector2(850, 600), new Vector2(100, 600), new Vector2(150, 150), new Vector2(490, 100) };
        List<Vector2> phase3_w1 = new List<Vector2>() { new Vector2(490, 100), new Vector2(560, 100), new Vector2(850, 100), new Vector2(850, 600), new Vector2(100, 600), new Vector2(150, 150), new Vector2(490, 100), new Vector2(850, 100), new Vector2(850, 600), new Vector2(100, 600), new Vector2(150, 150), new Vector2(490, 100)};

        int currentIndex = 0;
        Player player;
        delegate void BehaviorDelegate(GameTime gt);
        BehaviorDelegate phase;
        bool phase1Over;
        bool phase2Over;
        //bool phase3Over;

        float angle;


        float epsilon = 10;

        public Boss4(Texture2D tex)
            : base(tex)
        {
            time = 0;
            player = Cnt.game.CurrentPlayer;
            phase = Phase1;
            maxHitpoints = 10000;
            hitpoints = maxHitpoints;
            //hitpoints = 1667;
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
            BulletType = typeof(Bullet_Boss4);
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
                if (hitpoints <= 2*maxHitpoints / 3) phase = Phase2;
                if (hitpoints <= maxHitpoints / 4) phase = Phase3;
                phase(gt);
                //speed = MathHelper.Lerp(600, 300, hitpoints / maxHitpoints);
               
                hitpoints = MathHelper.Clamp(hitpoints, 0, maxHitpoints);
            }
        }

        void Phase1(GameTime gt)
        {
            phase1Over = false;

            rof = 10;//MathHelper.Lerp(0.1f, 1.0f, 3 * hitpoints / maxHitpoints - 2);
            float elapsed = (float)gt.ElapsedGameTime.TotalSeconds;
            Vector2 curWP = phase1_w1[currentIndex];
            Vector2 nextWP = phase1_w1[currentIndex + 1];


            if (currentIndex == 0) { MoveLine(curWP, nextWP, gt); speed = 20; epsilon = 10; }
            if (currentIndex == 1) { MoveLine(curWP, nextWP, gt); speed = 1700; epsilon = 100; }
            if (currentIndex == 2) { MoveLine(curWP, nextWP, gt); speed = 1700; epsilon = 100; }
            if (currentIndex == 3) { MoveLine(curWP, nextWP, gt); speed = 1700; epsilon = 100; }
            if (currentIndex == 4) { MoveLine(curWP, nextWP, gt); speed = 1700; epsilon = 100; }
            if (currentIndex == 5) { MoveLine(curWP, nextWP, gt); speed = 700; epsilon = 50; }
           // if (currentIndex == 6) { MoveLine(curWP, nextWP, gt); speed = 1700; }
           // if (currentIndex == 7) { MoveLine(curWP, nextWP, gt); speed = 300; }



            if (canFire)
            {
                if (currentIndex != 0)
                {
                    Shoot(Pos, player.Pos);
                    //Shoot(Pos, new Vector2(512,384));
                    timeToFire = 0.05f;
                }               
            }
            if (currentIndex == 0) timeToFire = MathHelper.Lerp(0.2f, 1.8f, 3 * hitpoints / maxHitpoints - 2);

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

            if (currentIndex == 0) { MoveLine(curWP, nextWP, gt); speed = 20; epsilon = 10; }
            if (currentIndex == 1) { MoveLine(curWP, nextWP, gt); speed = 1700; epsilon = 100; }
            if (currentIndex == 2) { MoveLine(curWP, nextWP, gt); speed = 1700; epsilon = 100; }
            if (currentIndex == 3) { MoveLine(curWP, nextWP, gt); speed = 1700; epsilon = 100; }
            if (currentIndex == 4) { MoveLine(curWP, nextWP, gt); speed = 1700; epsilon = 100; }
            if (currentIndex == 5) { MoveLine(curWP, nextWP, gt); speed = 700; epsilon = 50; }
            // if (currentIndex == 6) { MoveLine(curWP, nextWP, gt); speed = 1700; }
            // if (currentIndex == 7) { MoveLine(curWP, nextWP, gt); speed = 300; }



            if (canFire)
            {
                if (currentIndex != 0)
                {
                    Shoot(Pos, player.Pos);
                    //Shoot(Pos, new Vector2(512,384));
                    timeToFire = 0.05f;
                }
            }
            if (currentIndex == 0) timeToFire = 0.2f;

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
                rof = 0.2f;
            }
            //phase3Over = false;

            float elapsed = (float)gt.ElapsedGameTime.TotalSeconds;
            Vector2 curWP = phase3_w1[currentIndex];
            Vector2 nextWP = phase3_w1[currentIndex + 1];

            if (currentIndex == 0) { MoveLine(curWP, nextWP, gt); speed = 20; epsilon = 10; }
            if (currentIndex == 1) { MoveLine(curWP, nextWP, gt); speed = 1700; epsilon = 100; }
            if (currentIndex == 2) { MoveLine(curWP, nextWP, gt); speed = 1700; epsilon = 100; }
            if (currentIndex == 3) { MoveLine(curWP, nextWP, gt); speed = 1700; epsilon = 100; }
            if (currentIndex == 4) { MoveLine(curWP, nextWP, gt); speed = 1700; epsilon = 100; }
            if (currentIndex == 5) { MoveLine(curWP, nextWP, gt); speed = 1700; epsilon = 100; }
            if (currentIndex == 6) { MoveLine(curWP, nextWP, gt); speed = 1700; epsilon = 100; }
            if (currentIndex == 7) { MoveLine(curWP, nextWP, gt); speed = 1700; epsilon = 100; }
            if (currentIndex == 8) { MoveLine(curWP, nextWP, gt); speed = 1700; epsilon = 100; }
            if (currentIndex == 9) { MoveLine(curWP, nextWP, gt); speed = 1700; epsilon = 100; }
            if (currentIndex == 10) { MoveLine(curWP, nextWP, gt); speed = 700; epsilon = 50; }          


            if (canFire)
            {
                if (currentIndex != 0)
                {
                    Shoot(Pos, player.Pos);                    
                    timeToFire = 0.1f;
                }
            }
            if (currentIndex == 0) timeToFire = 0.2f;          

            if (Vector2.DistanceSquared(Pos, nextWP) <= epsilon) { currentIndex++; time = 0; }
            if (currentIndex == phase3_w1.Count - 1) currentIndex = 0;

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
         
        /*
        protected override bool Shoot(Vector2 place, Vector2 targetPos)
        {
            if (canFire)
            {
                Bullet bullet = (Bullet)Activator.CreateInstance(BulletType, new object[] { place, targetPos - place, Cnt.game.Content.Load<Texture2D>("boss3") });
                Cnt.game.EnemyBullets.Add(bullet);
                return true;
            }

            return false;
        }
        */

    }
}
