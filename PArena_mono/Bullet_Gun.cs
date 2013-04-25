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
    class Bullet_Gun:Bullet
    {
        static float initSpeed = 700;

        public Bullet_Gun(Vector2 pos, Vector2 dir, Texture2D tex, float mul)
            : base(pos, dir, tex, initSpeed)
        {
            ttl = 2;
            this.damage = 20*mul;
            soundfxString = Cnt.game.chaingunSound;
          /*  soundfx = Cnt.game.soundBank.GetCue("chaingun");*/
            pEmitter.Color = Color.Gold;
            //pps, pSpeed, 
            //posVar, alphaVel, 
            //minSize,maxSize, sizeVel,
            //ttl
            pEmitter.SetParam(1000, 1, 
                              1, 3000, 
                              0.1f, 0.4f, 0.0005f, 
                              50);
        }

        
        

        public override void Update(GameTime gt)
        {
            base.Update(gt);           
        }
        


    }
}
