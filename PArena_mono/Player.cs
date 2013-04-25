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
    public enum Pony
    {
        Twily,
        Pinkie,
        RD,
        Flutty,
        Rarity,
        AJ
    } 

    public class Player: GameObject
    {

        InputState input;
        static float timeToFire = 0;
        public bool canFire { get { return timeToFire == 0 || timeToRifle == 0; } }
        Type BulletType;
        //Pony enumpony;
        public float hitpoints;
        public float maxHitpoints = 100;
        public bool isDead;

        public float damageMul = 1;
        public float rof = 1;
        float deadAnimationTime = 2.0f;
        int nSpot = 20;
        float spotTime;
        public Pony pony;

        int NRifle = 0;        
        float rofRifle = 0.13f;
        float timeToRifle = 0.07f;

        delegate ShakeParam ShootDelegate(float mx, float my);
        ShootDelegate ShootMethod;

        List<ParticleEmitter> emitters=new List<ParticleEmitter>();

        public Player(Vector2 pos, Vector2 dir, Pony pony, float speed = 300)
            : base(pos, dir, null, speed)
        {
            input = InputState.GetInput();
            hitpoints = maxHitpoints;
            //enumpony = Pony.AJ;
            ShootMethod = CommonShoot;
            switch (pony)
            {
                case Pony.Pinkie:
                    break;
                case Pony.RD:
                    rof = 0.9f;
                    timeToFire = 0;
                    BulletType = typeof(Bullet_Rail);
                    sprite = Cnt.game.playerTextures[4];
                    speed = 350;
                    //pps, pSpeed, 
                    //posVar, alphaVel, 
                    //minSize,maxSize, sizeVel,
                    //ttl
                    ParticleEmitter p = new ParticleEmitter(Cnt.game.pEngine, pos);
                    p.Color = Color.DarkViolet;
                    emitters.Add(p);
                    p.SetParam(200, 1,
                                      1, 700,
                                      0.2f, 0.3f, 0.0005f,
                                      50);
                     p = new ParticleEmitter(Cnt.game.pEngine, pos);
                     p.SetParam(200, 1,
                                      1, 700,
                                      0.2f, 0.3f, 0.0005f,
                                      50);
                    p.Color = Color.DeepSkyBlue;
                    emitters.Add(p);
                    p = new ParticleEmitter(Cnt.game.pEngine, pos);
                    p.SetParam(200, 1,
                                      1, 700,
                                      0.2f, 0.3f, 0.0005f,
                                      50);
                     p.Color = Color.Green;
                    emitters.Add(p);
                     p = new ParticleEmitter(Cnt.game.pEngine, pos);
                    p.SetParam(200, 1,
                                      1, 700,
                                      0.2f, 0.3f, 0.0005f,
                                      50);
                     p.Color = Color.Gold;
                    emitters.Add(p);
                    p = new ParticleEmitter(Cnt.game.pEngine, pos);
                    p.SetParam(200, 1,
                                      1, 700,
                                      0.2f, 0.3f, 0.0005f,
                                      50);
                     p.Color = Color.Red;
                    emitters.Add(p);
                    break;
                case Pony.Flutty:
                    rof = 0.17f;
                    timeToFire = 0;
                    BulletType = typeof(Bullet_Gun);
                    sprite = Cnt.game.playerTextures[1];
                    break;
                case Pony.Rarity:
                    rof = 1.00f;
                    timeToFire = 0;
                    BulletType = typeof(Bullet_Rifle);
                    sprite = Cnt.game.playerTextures[3];
                    ShootMethod = Rarity_Shoot;
                    break;
                case Pony.AJ:
                    rof = 0.9f;
                    timeToFire = 0;
                    BulletType = typeof(Bullet_Shotgun);
                    sprite = Cnt.game.playerTextures[0];
                    ShootMethod = AJ_Shoot;
                    break;
                case Pony.Twily:
                    rof = 0.5f;
                    timeToFire = 0;
                    BulletType = typeof(Bullet_Guide);
                    sprite = Cnt.game.playerTextures[5];
                    break;
                default:
                    break;
            }
            this.pony = pony;
            Height = sprite.Height;
            Width = sprite.Width;
            origin = new Vector2(Width / 2, Height / 2);
            radius = (float)Math.Sqrt(Height * Height + Width * Width);
        }

        public void Update(GameTime gt, bool move = true)
        {
            Dir = Vector2.Zero;
            if (move)
            {
                if (input.IsKeyPressed(Keys.A)) Dir.X--;
                if (input.IsKeyPressed(Keys.D)) Dir.X++;
                if (input.IsKeyPressed(Keys.W)) Dir.Y--;
                if (input.IsKeyPressed(Keys.S)) Dir.Y++;
                if (isDead) Dir = Vector2.Zero;

                timeToFire = MathHelper.Clamp(timeToFire - (float)gt.ElapsedGameTime.TotalSeconds, 0, rof);
                if (BulletType == typeof(Bullet_Rifle)) timeToRifle = MathHelper.Clamp(timeToRifle - (float)gt.ElapsedGameTime.TotalSeconds, 0, rofRifle);
            }

            float elapsed = (float)gt.ElapsedGameTime.TotalSeconds;
            Vector2 newPos = Pos + Dir * speed * elapsed;
            if (newPos.X < 10 || newPos.X > Game1.screenWidth - 10) Dir.X = 0;
            if (newPos.Y < 10 || newPos.Y > Game1.screenHeight - 10) Dir.Y = 0;

            for (int i = 0; i < emitters.Count; i++)
            {
                Vector2 v = Pos; v.Y = Pos.Y + 2 * i - 6;
                emitters[i].Pos = v;
                emitters[i].Update(gt);
            }

            Pos += Dir * speed * (float)gt.ElapsedGameTime.TotalSeconds;
       
            if (isDead)
            {
                spotTime += (float)gt.ElapsedGameTime.TotalSeconds;
                if (spotTime > deadAnimationTime / nSpot)
                {
                    spotTime = 0;
                    Vector2 randomPos = new Vector2(Pos.X + (float)Game1.rnd.NextDouble() * 300 - 150, Pos.Y + (float)Game1.rnd.NextDouble() * 300 - 150);
                    //Vector2 randomPos = Pos;
                    int randomTex = Game1.rnd.Next(Cnt.game.spotCount);
                    Spot sp = new Spot { rect = new Rectangle(randomTex * 80, 0, 80, 80), pos = randomPos, color = Color.DarkRed };
                    Cnt.game.spots.Add(sp);
                }
            }

        }

        

        public ShakeParam Shoot(float mx, float my)
        {
            return ShootMethod(mx, my);
        }


        ShakeParam CommonShoot(float mx, float my)
        {
            ShakeParam sh = new ShakeParam();

            Bullet bullet = (Bullet)Activator.CreateInstance(BulletType, new object[] { Pos, new Vector2(mx, my) - Pos, Cnt.game.bulletTex, damageMul });
            Cnt.game.Bullets.Add(bullet);
            sh = bullet.getShakeParam;
            /*bullet.soundfx.Play();*/
            Cnt.game.soundPlayer.SoundLocation = bullet.soundfxString;
            Cnt.game.soundPlayer.Play();

            timeToFire = rof;
            return sh;
        }

        ShakeParam AJ_Shoot(float mx, float my)
        {
            ShakeParam sh = new ShakeParam();

            for (int i = 0; i < 13; i++)
            {
                Bullet bullet = (Bullet)Activator.CreateInstance(BulletType, new object[] { Pos, new Vector2(mx, my) - Pos, Cnt.game.bulletTex, damageMul });
                Cnt.game.Bullets.Add(bullet);
                sh = bullet.getShakeParam;

            }
            Bullet tmpb = (Bullet)Activator.CreateInstance(BulletType, new object[] { Pos, new Vector2(mx, my) - Pos, Cnt.game.bulletTex, damageMul });
            Cnt.game.soundPlayer.SoundLocation = tmpb.soundfxString;
            Cnt.game.soundPlayer.Play();

            timeToFire = rof;
            return sh;
        }

        ShakeParam Rarity_Shoot(float mx, float my)
        {
            ShakeParam sh = new ShakeParam();

            if (timeToRifle == 0 && NRifle < 2)
            {
                Bullet bullet = (Bullet)Activator.CreateInstance(BulletType, new object[] { Pos, new Vector2(mx, my) - Pos, Cnt.game.bulletTex, damageMul });
                Cnt.game.Bullets.Add(bullet);
                sh = bullet.getShakeParam;
                Cnt.game.soundPlayer.SoundLocation = bullet.soundfxString;
                Cnt.game.soundPlayer.Play();
                timeToRifle = rofRifle;
                NRifle++;
            }
            else
                if (timeToFire == 0)
                {
                    Bullet bullet = (Bullet)Activator.CreateInstance(BulletType, new object[] { Pos, new Vector2(mx, my) - Pos, Cnt.game.bulletTex, damageMul });
                    Cnt.game.Bullets.Add(bullet);
                    sh = bullet.getShakeParam;
                    Cnt.game.soundPlayer.SoundLocation = bullet.soundfxString;
                    Cnt.game.soundPlayer.Play();
                    NRifle = 0;
                    timeToFire = rof;
                    timeToRifle = rofRifle;
                }



            return sh;
        }
        

        public void Die()
        {
            isDead = true;
        }
    }
}
