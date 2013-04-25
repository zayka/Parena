using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PArena
{
    public class Boss:Enemy
    {
        protected float time = 0;
        public float bossTime = 0;
        //protected Type BulletType;
        

        public Boss(Texture2D tex)
            : base(tex)
        {
            isDistorted = true;
            isBoss = true;
        }

        protected virtual bool Shoot(Vector2 place, Vector2 targetPos)
        {
            if (canFire)
            {
                Bullet bullet = (Bullet)Activator.CreateInstance(BulletType, new object[] { place, targetPos - place, Cnt.game.bulletTex });
                Cnt.game.EnemyBullets.Add(bullet);                
                return true;
            }
            return false;
        }

        protected void MoveLine(Vector2 curWP, Vector2 nextWP, GameTime gt)
        {
            float elapsed = (float)gt.ElapsedGameTime.TotalSeconds;
            Vector2 dirToWP = nextWP - Pos;
            if (dirToWP!=Vector2.Zero)
                dirToWP.Normalize();
            Vector2 v = dirToWP - Dir;
            if (v!=Vector2.Zero) v.Normalize();

            Dir += v * turnspeed * elapsed;
            Pos += Dir * speed * elapsed;

        }

        protected void MoveSin(Vector2 curWP, Vector2 nextWP, GameTime gt)
        {
            Vector2 dirToWP = nextWP - curWP;
            float length = dirToWP.Length();
            dirToWP.Normalize();
            float angle = (float)Math.Acos(dirToWP.X);
            float Xc = time * speed;
            angle = angle * Math.Sign(dirToWP.Y + 0.00001f);
            Matrix m = Matrix.CreateFromAxisAngle(Vector3.UnitZ, angle);
            Vector2 P = new Vector2(Xc, 50 * (float)Math.Sin(4 * Xc * MathHelper.TwoPi / length));

            Pos = Vector2.Transform(P, m) + curWP;

            //if (Vector2.DistanceSquared(Pos, nextWP) <= epsilon) { currentIndex++; time = 0; }
            //if (currentIndex == phase1_w1.Count - 1) currentIndex = 0;     
        }
        /*
        protected void MoveCos(Vector2 curWP, Vector2 nextWP, GameTime gt)
        {
            Vector2 dirToWP = nextWP - curWP;
            float length = dirToWP.Length();
            dirToWP.Normalize();
            float angle = (float)Math.Acos(dirToWP.X);
            float Xc = time * speed;
            angle = angle * Math.Sign(dirToWP.Y + 0.00001f);
            Matrix m = Matrix.CreateFromAxisAngle(Vector3.UnitZ, angle);
            Vector2 P = new Vector2(Xc, 50 * (float)Math.Cos(4 * Xc * MathHelper.TwoPi / length));

            Pos = Vector2.Transform(P, m) + curWP;          
        }
        */
        public override void Update(GameTime gt)
        {
            base.Update(gt);
        }

    }
}
