using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PArena
{
    public struct Light2
    {      
        public Vector2 direction;
        public float radius;
        public float innerangle;
        public float outterangle;
    }

    public struct Spot
    {
        public Vector2 pos;
        //public Texture2D texture;
        public Rectangle rect;
        //public float angle;
        public Vector2 origin { get { return new Vector2(40, 40); } }
        public Color color;
    }

    public class Enemy : GameObject
    {
        public float hitpoints;
        public float maxHitpoints;
        protected float turnspeed = 3 * MathHelper.Pi;
        protected float rof = 1;
        protected float timeToFire = 0;
        public float killTime;
        public float deadAnimationTime = 0.15f;
        protected int nSpot = 2;
        protected float spotTime;
        protected Color bloodColor = Color.Red;
        public bool isDistorted = false;
        public bool isShooter = false;


        public bool canFire { get { return timeToFire == 0; } }
        protected Type BulletType;
        public bool control = false;
        public bool toDie;
        public bool toRemove;
        public bool isBoss;

        public Light2 light;
        public bool hasLight = false;
        
        public Enemy(Vector2 Pos)
            : base(Vector2.Zero, Vector2.Zero, null, 0)
        {            
            this.Pos = Pos;
            Dir = new Vector2((float)Game1.rnd.NextDouble()*2-1 , (float)Game1.rnd.NextDouble()*2-1);
            Dir.Normalize();            
            BulletType = typeof(Bullet_Gun);
            rof = 2;
            timeToFire = (float)Game1.rnd.NextDouble()*rof*0.75f;           
            spotTime = 0;// deadAnimationTime / nSpot;
            light.direction = new Vector2(0, 0);
            light.innerangle = MathHelper.TwoPi;
            light.outterangle = MathHelper.TwoPi;
            light.radius = Width;

        }        
        
         public Enemy(Texture2D tex)
            : base(Vector2.Zero, Vector2.Zero, tex)
         {

         }
         
        public override void Update(GameTime gt)
        {
            float elapsed = (float)gt.ElapsedGameTime.TotalSeconds;
            int deep = (int)(speed * elapsed*1.3f);
            Vector2 newPos = Pos + Dir * speed * elapsed;
            if ((newPos.X > Game1.screenWidth + deep && Pos.X < Game1.screenWidth + deep) || (newPos.X < -deep && Pos.X > -deep)) Dir.X = -Dir.X;
            if ((newPos.Y > Game1.screenHeight + deep && Pos.Y < Game1.screenHeight + deep) || (newPos.Y < -deep && Pos.Y > -deep)) Dir.Y = -Dir.Y;

            if (Dir != Vector2.Zero)
                Dir.Normalize();
           
            timeToFire = MathHelper.Clamp(timeToFire - elapsed, 0, rof);
            killTime += elapsed;
            if (killTime >= deadAnimationTime&&toDie) toRemove = true;
            if (toDie)
            {
                spotTime += elapsed;
                if (spotTime > deadAnimationTime / nSpot)
                {
                    spotTime = 0;
                    Vector2 randomPos = new Vector2(Pos.X + (float)Game1.rnd.NextDouble() * 20-10, Pos.Y + (float)Game1.rnd.NextDouble() * 20-10);
                    //Vector2 randomPos = Pos;
                    int randomTex = Game1.rnd.Next(Cnt.game.spotCount);
                    Spot sp = new Spot { rect = new Rectangle(randomTex*80,0,80,80), pos = randomPos, color = bloodColor };
                    Cnt.game.spots.Add(sp);
                }
            }
            base.Update(gt);
        }

        public override void Draw(SpriteBatch sb)
        {
            //sb.DrawString(Cnt.game.font, hitpoints.ToString(), Pos-origin- new Vector2(5, 10), Color.White);
           // sb.DrawString(Cnt.game.font, Pos.ToString(), Pos - origin - new Vector2(5, 14), Color.White);
            base.Draw(sb);
        }

        protected Vector2 AI_1(GameTime gt)
        {
            Player pl = Cnt.game.CurrentPlayer;
            Vector2 dirToPl = pl.Pos - Pos;
            if (dirToPl!=Vector2.Zero)
                dirToPl.Normalize();
            Vector2 v = dirToPl - Dir;
            if (v != Vector2.Zero)
                    v.Normalize();
            Dir += v * turnspeed * (float)gt.ElapsedGameTime.TotalSeconds;
            return Vector2.Zero;
        }

        public void AI_2(GameTime gt)
        {
            if (!isShooter) return;
            //Dir = Vector2.Zero;
            Shoot(Cnt.game.CurrentPlayer.Pos);
        }

        bool Shoot(Vector2 targetPos)
        {
            if (canFire)
            {

                if (BulletType == typeof(Bullet_EnemyStableT2))
                {
                    for (int i = 0; i < 8; i++)
                    {
                        Bullet bullet = (Bullet)Activator.CreateInstance(BulletType, new object[] { Pos, targetPos - Pos, Cnt.game.bulletTex, 0.5f });
                        Cnt.game.EnemyBullets.Add(bullet);
                    }
                }
                else
                {
                    Bullet bullet = (Bullet)Activator.CreateInstance(BulletType, new object[] { Pos, targetPos - Pos, Cnt.game.bulletTex, 0.5f });
                    Cnt.game.EnemyBullets.Add(bullet);
                }
               

                timeToFire = rof;
                return true;
            }
            return false;
        }





        public void Die()
        {
            if (toDie == false)
            {
                control = false;
                speed = 0;
                killTime = 0;
                toDie = true;
            }
        }
    }
}
