using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace PArena
{
    public class GameObject
    {

        public Vector2 Pos;
        public Vector2 Dir;
        public Texture2D sprite;        
        public int Height;
        public int Width;
        public float radius;
        public Rectangle rect { get { return new Rectangle((int)Math.Round(Pos.X - origin.X), (int)Math.Round(Pos.Y - origin.Y), sprite.Width, sprite.Height); } }

        protected float speed;
        protected Vector2 origin;
        public Vector2 Origin { get { return origin; } }
        public float Speed { get { return speed; } set { speed = value; } }
        public bool isOnScreen { get { return Pos == Vector2.Clamp(Pos, new Vector2(0 , 0 ), new Vector2(Game1.screenWidth , Game1.screenHeight )); } }
        public Color color;

        public GameObject(Vector2 pos, Vector2 dir, Texture2D tex, float speed = 100)
        {
            
            dir.Normalize();
            this.Pos = pos;
            this.Dir = dir;
            this.speed = speed;
            if (tex != null)
            {
                this.sprite = tex;
                Height = tex.Height;
                Width = tex.Width;
                origin = new Vector2(Width / 2, Height / 2);
            }
            radius = (float)Math.Sqrt(Height * Height + Width * Width);
            color = Color.White;
        }


        public virtual void Update(GameTime gt)
        {

            Pos += Dir * speed * (float)gt.ElapsedGameTime.TotalSeconds;
        }

        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, Pos - origin, color);
            // sprite.Draw(sb);
        }

        public virtual void Draw(SpriteBatch sb, Vector2 offset)
        {
            sb.Draw(sprite, Pos - origin + offset, color);
        }
    }
}
