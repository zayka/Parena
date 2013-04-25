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
    class Level_3 : Level
    {

        public Level_3()
            : base()
        {
            mainBackground = Cnt.game.Content.Load<Texture2D>("LevelBG\\mainBG");
            levelBackground = Cnt.game.Content.Load<Texture2D>("LevelBG\\bg_l3");
            levelTitle = Cnt.game.Content.Load<Texture2D>("LevelBG\\level3");

            backgroundRT = new RenderTarget2D(Cnt.game.GraphicsDevice, mainBackground.Width, mainBackground.Height, false, Cnt.game.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents);
            Cnt.game.GraphicsDevice.SetRenderTarget(backgroundRT);
            Cnt.game.GraphicsDevice.Clear(Color.Black);
            Game1.spriteBatch.Begin();
            Game1.spriteBatch.Draw(mainBackground, Vector2.Zero, Color.White);
            Game1.spriteBatch.Draw(levelBackground, Vector2.Zero, Color.White);

            Game1.spriteBatch.End();
            Cnt.game.GraphicsDevice.SetRenderTarget(null);


        }

        public override void Update(GameTime gt)
        {
            WaveParam w = new WaveParam();

            float elapsed = (float)gt.ElapsedGameTime.TotalSeconds;
            time += elapsed;
            levelTime += elapsed;
            
            if (time > 2 && (time - elapsed < 2))
            {
                w.numEnemy = 30;
                waves.Add(new Wave(WaveType.Random, w));
            }

            if (time > 10 && (time - elapsed < 10))
            {
                w.numEnemy = 30;
                waves.Add(new Wave(WaveType.Random, w));
                for (int i = 0; i < 5; i++)
                {
                    Vector2 randomPos = new Vector2(Game1.rnd.Next(Game1.screenWidth-100)+50, Game1.rnd.Next(Game1.screenHeight-100)+50);
                    Enemy e = new Enemy_StableT2(randomPos);
                    e.control = false;
                    enemyList.Add(e);                    
                }
            }

            if (time > 25 && (time - elapsed < 25))
            {
                w.numEnemy = 30;
                waves.Add(new Wave(WaveType.Random, w));
                for (int i = 0; i < 7; i++)
                {
                    Vector2 randomPos = new Vector2(Game1.rnd.Next(Game1.screenWidth-100)+50, Game1.rnd.Next(Game1.screenHeight-100)+50);
                    Enemy e = new Enemy_StableT1(randomPos);
                    e.control = false;
                    enemyList.Add(e);
                }
            }
            if (time > 40 && (time - elapsed < 40))
            {
                w.numEnemy = 10;
                waves.Add(new Wave(WaveType.Random, w));                
            }
            
            if (time > 55 && time - elapsed < 55)
            {
                if (enemyList.Count == 0)
                {
                    w.eType = typeof(Enemy_Fast);
                    w.numEnemy = 20;
                    w.N = 5;
                    w.R = 400;
                    w.speed = 300;
                    w.duration = 10;
                    waves.Add(new Wave(WaveType.Ngon, w));
                }
                else time = 55 - 10 * elapsed;
            }

            if (time > 60 && time - elapsed < 60)
            {
                
                    w.eType = typeof(Enemy_Fast);
                    w.numEnemy = 30;
                    w.N = 6;
                    w.R = 400;
                    w.speed = 320;
                    w.duration = 10;
                    waves.Add(new Wave(WaveType.Ngon, w));
            }
             
            if (time > 63 && time - elapsed < 63)
            {
                if (enemyList.Count == 0)
                {
                    boss = new Boss3(Cnt.game.Content.Load<Texture2D>("boss3"));
                    enemyList.Add(boss);
                    //MediaPlayer.Stop();
                    //MediaPlayer.Play(Cnt.game.bossTheme);
                    Cnt.game.waveOutDevice.Stop();
                    Cnt.game.mainOutputStream = Cnt.game.CreateInputStream(Cnt.game.bossTheme);
                    Cnt.game.waveOutDevice.Init(Cnt.game.mainOutputStream);
                    Cnt.game.waveOutDevice.Play();

                }
                else time = 63 - 10 * elapsed;
            }

            if (boss != null && boss.toDie)
            {
                Player pl = new Player(player.Pos, Vector2.Zero, player.pony);
                Cnt.game.CurrentPlayer = pl;
                nextLevel = new Level_4();
            }


            base.Update(gt);
        }
    }
}
