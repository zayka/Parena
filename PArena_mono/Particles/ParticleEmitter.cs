using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PArena
{
    public class ParticleEmitter
    {
        ParticleEngine engine;
        Vector2 pos;
        public Vector2 Pos { get { return pos; } set { pos = value; } }
        Vector2 prevPos;
        Color color;
        public Color Color { get { return color; } set { color = value; } }

        // param
        //Texture2D p1;
        //Texture2D p2;
        //Texture2D p3;
        Texture2D p4;
        float pps=300.0f; // particles per second
        float pSpeed=1.0f;
        float posVar = 1.0f;
        float alphaVel = 2000;
        float minSize=0.2f;
        float maxSize=0.7f;
        float sizeVel=0.5f;
        float ttl = 50;
        

        Vector2 startDir=new Vector2(1,0);
        
       
       

        public ParticleEmitter(ParticleEngine engine,Vector2 Pos)
        {
            this.engine = engine;
            this.pos = Pos;
            this.prevPos = Pos;
            //p1 = Cnt.game.Content.Load<Texture2D>("p1");
            //p2 = Cnt.game.Content.Load<Texture2D>("p2");
            //p3 = Cnt.game.Content.Load<Texture2D>("p3");
            p4 = Cnt.game.Content.Load<Texture2D>("p4");
            color = Color.Green;
        }

        public void Update(GameTime gt)
        {
            float elapsed = (float)gt.ElapsedGameTime.TotalSeconds;            
            //param check
            
            Vector2 dist = prevPos - pos;
            float length = dist.Length();             
            float curTime = 0;
            float pTime = 1 / pps;
            
            while (curTime < elapsed)
            {
                curTime += pTime;
                Vector2 curPos = Vector2.Lerp(prevPos, pos, curTime / elapsed);
                /*
                float angle = (float)Game1.rnd.NextDouble() * MathHelper.TwoPi;
                float x = (float)Math.Cos(angle);
                float y = (float)Math.Sin(angle);
                Vector2 randomDir = new Vector2(x, y);
                */
                Vector2 randomDir = new Vector2((float)Game1.rnd.NextDouble() * 1.0f - 0.5f, (float)Game1.rnd.NextDouble() * 1.0f - 0.5f);
                Vector2 randomPos = new Vector2((float)Game1.rnd.NextDouble() * 1.0f - 0.5f, (float)Game1.rnd.NextDouble() * 1.0f - 0.5f);                
                engine.Add(posVar * randomPos + curPos, pSpeed * randomDir , 0, 1, new Vector4(color.R, color.G, color.B, color.A), alphaVel, (float)Game1.rnd.NextDouble() * (maxSize - minSize) + minSize, sizeVel, ttl);
            }         
            
             



            /*
            for (int i = 0; i < elapsed * pps; i++)
            {
                Vector2 randomDir = new Vector2((float)Game1.rnd.NextDouble() * 1.0f - 0.5f, (float)Game1.rnd.NextDouble() * 1.0f - 0.5f);
                Vector2 posVar = new Vector2((float)Game1.rnd.NextDouble() * 10.0f - 5.5f, (float)Game1.rnd.NextDouble() * 10.0f - 5.5f);
                float randomSpeed = (float)Game1.rnd.NextDouble() * 100;
                Particle p = new Particle(p4, pos + posVar, 60.1f * randomDir, 0, 1, new Vector4(0, 255, 0, 255), 50, (float)Game1.rnd.NextDouble() * 0.3f + 0.6f, 0.1f, 5);
                engine.Add(p);
            }*/
            //generate particle/s
            //Particle p = new Particle()
            //engine.Add(p);
            prevPos = pos;
        }

        /// <summary>
        /// Параметры эмиттера
        /// </summary>
        /// <param name="pps">Частиц в секудну</param>
        /// <param name="nPart">интервалы для однородности, при высохик скоростях больше</param>
        /// <param name="pSpeed">скорость</param>
        /// <param name="posVar">максимум вариации скорости</param>
        /// <param name="alphaVel">скорость гашения цвета</param>
        /// <param name="minSize">минимальный размер</param>
        /// <param name="maxSize">максимальный размер</param>
        /// <param name="sizeVel">скорость уменьшения</param>
        /// <param name="ttl">время жизни</param>
        public void SetParam(float pps=10.0f, float pSpeed=1.0f,
                             float posVar = 1.0f,float alphaVel = 3000, 
                             float minSize=0.2f, float maxSize=0.7f, float sizeVel=0.5f,
                             float ttl = 50)
        {
            this.pps = pps; // particles per second
            this.pSpeed = pSpeed;
            this.posVar = posVar;
            this.alphaVel = alphaVel;
            this.minSize = minSize;
            this.maxSize = maxSize;
            this.sizeVel = sizeVel;
            this.ttl = ttl;
        }
    }
}
