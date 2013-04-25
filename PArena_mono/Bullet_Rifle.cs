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
    class Bullet_Rifle:Bullet
    {
        static public float initSpeed = 300;

        public Bullet_Rifle(Vector2 pos, Vector2 dir, Texture2D tex, float mul)
            : base(pos, dir, tex, initSpeed)
        {
            pEmitter.Color = new Color(0, 150, 255, 255);// Color.LightBlue;
            ttl = 10;
            this.damage= 40*mul;
            soundfxString = Cnt.game.rifleSound;
            //pps, pSpeed, 
            //posVar, alphaVel, 
            //minSize,maxSize, sizeVel,
            //ttl
            pEmitter.SetParam(600, 40,
                              0, 400,
                              0.2f, 0.4f, 0.05f,
                              50);
            color = new Color(0, 150, 255, 255);
            
        }


        public override void Update(GameTime gt)
        {
            base.Update(gt);
           
        }

    }
}
