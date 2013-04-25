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
    class Bullet_Rail:Bullet
    {
       

        static public float initSpeed = 7000;

        public Bullet_Rail(Vector2 pos, Vector2 dir, Texture2D tex, float mul)
            : base(pos, dir, tex, initSpeed)
        {
            pEmitter.Color = Color.Green;
            ttl = 0.5f;
            damage = 110*mul;//190
            shakeParam.ampl = 20;
            shakeParam.maxtime = 0.10f;
            //soundfx = Cnt.game.soundBank.GetCue("railgun");
            soundfxString = Cnt.game.railgunSound;

            //pps, nPart, pSpeed, 
            //posVar, alphaVel, 
            //minSize,maxSize, sizeVel,
            //ttl
            pEmitter.SetParam(3000, 0.001f,
                              0, 100,
                              0.3f, 0.3f, 0.0001f,
                              50);
            color = Color.Green;

        }
        /*
        public Bullet_Rail()
            : base(Vector2.Zero, Vector2.Zero, Cnt.game.bulletTex, 7000)
        {
            tailColor = Color.Green;
            positions = new List<VecColor>();
            positions.Add(new VecColor(Vector2.Zero, tailColor));
            ttl = 3;
            tailLength = 3000;
        }
        */
        public override void Update(GameTime gt)
        {
            base.Update(gt);
        }
        /*
        public override void Draw(SpriteBatch sb)
        {            
            base.Draw(sb);
        }
        
        public void DrawGlow(SpriteBatch sb)
        {
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, Cnt.game.glowEffect);
            Vector4 col = new Vector4(tailColor.R/255f,tailColor.G/255f,tailColor.B/255f,tailColor.A/255f);
            Cnt.game.glowEffect.Parameters["tailColor"].SetValue(col);
            foreach (VecColor v in positions)
            {
                sb.Draw(tailTex, v.v - origin, v.color);
            }
            sb.End();
        }
        */
    }
}
