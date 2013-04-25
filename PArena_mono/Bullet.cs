using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
//using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using dr = System.Drawing;


namespace PArena
{
    /*
    public class VecColor
    {
        public Vector2 v;
        public Color color;
        public VecColor(Vector2 v, Color c)
        {
            this.v = v;
            this.color = c;
        }
    }
    */
    public class Bullet : GameObject
    {
        public bool toRemove = false;
        public bool prepareToRemove = false;

        protected float ttl = 20;
        protected ShakeParam shakeParam;

        public float bulletSpeed = 1;
        public float damage = 1;

        public ShakeParam getShakeParam { get { shakeParam.curtime = shakeParam.maxtime; return shakeParam; } }
        //public Cue soundfx = null;
        public string soundfxString = string.Empty;

        public Vector2 prevPos;
        protected ParticleEmitter pEmitter;


        public Bullet(Vector2 pos, Vector2 dir, Texture2D tex, float initSpeed, bool good = true)
            : base(pos, dir, tex, initSpeed)
        {
            bulletSpeed = initSpeed;
            shakeParam.ampl = 10;
            shakeParam.curtime = 0;
            shakeParam.maxtime = 0.15f;
            shakeParam.dir = new Vector2(0, 1);
            pEmitter = new ParticleEmitter(Cnt.game.pEngine, pos);
        }

        public override void Update(GameTime gt)
        {
            ttl -= (float)gt.ElapsedGameTime.TotalSeconds;
            prevPos = Pos;
            if (ttl < 0) prepareToRemove = true;
            base.Update(gt);     
   
            Vector2 screen = new Vector2(Game1.screenWidth, Game1.screenHeight);
            if (Vector2.Clamp(Pos, Vector2.Zero, screen) != Pos) prepareToRemove = true;
        }

        public void Emit(GameTime gt)
        {
            pEmitter.Pos = Pos;
            //емит частиц от прошлого до текущего
            pEmitter.Update(gt);
            if (prepareToRemove) toRemove = true;
        }

        public override void Draw(SpriteBatch sb)
        {
            /*
            foreach (VecColor v in positions)
            {
                sb.Draw(tailTex, v.v - origin, v.color);
            }
            if (!prepareToRemove) sb.Draw(sprite, Pos - origin, Color.White);
            */
            base.Draw(sb);
        }
        public override void Draw(SpriteBatch sb, Vector2 offset)
        {
            /*
            foreach (VecColor v in positions)
            {
                sb.Draw(tailTex, v.v - origin, v.color);
            }
            if (!prepareToRemove) sb.Draw(sprite, Pos - origin+offset, Color.White);
           */
            base.Draw(sb);
        }        
    }
}
