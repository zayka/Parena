using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace PArena
{
    class Level_4:Level
    {
        public Level_4()
            : base()
        {
            mainBackground = Cnt.game.Content.Load<Texture2D>("LevelBG\\mainBG");
            levelBackground = Cnt.game.Content.Load<Texture2D>("LevelBG\\bg_l4");
            levelTitle = Cnt.game.Content.Load<Texture2D>("LevelBG\\level4");

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
            /*
            //test wave
            if (time > 1 && (time - elapsed < 1))
            {

                w.eType = typeof(Enemy_Normal);
                w.numEnemy = 10;
                w.speed = 10;
                w.duration = 1500;
                w.waveDir = new Vector2(0, 1);
                waves.Add(new Wave(WaveType.Line, w));
            }
            */
            
            if (time > 2 && (time - elapsed < 2))
            {
                w.numEnemy = 30;
                waves.Add(new Wave(WaveType.Random, w));
            }
            if (time > 5 && (time - elapsed < 5))
            {
                w.numEnemy = 30;
                waves.Add(new Wave(WaveType.Random, w));
            }

            if (time > 10 && (time - elapsed < 10))
            {               

                w.eType = typeof(Enemy_StableT1);
                w.numEnemy = 10;
                w.speed = 100;
                w.duration = 1500;
                w.waveDir = new Vector2(0, 1);
                waves.Add(new Wave(WaveType.Line, w));
            }

            if (time > 20 && time - elapsed < 20)
            {

                w.eType = typeof(Enemy_Fast);
                w.numEnemy = 30;
                w.N = 6;
                w.R = 400;
                w.speed = 300;
                w.duration = 12;
                waves.Add(new Wave(WaveType.Ngon, w));
            }

            if (time > 25 && time - elapsed < 25)
            {

                w.eType = typeof(Enemy_Fast);
                w.numEnemy = 30;
                w.N = 5;
                w.R = 400;
                w.speed = 300;
                w.duration = 12;
                waves.Add(new Wave(WaveType.Ngon, w));
            }

            if (time > 30 && time - elapsed < 30)
            {

                w.eType = typeof(Enemy_Fast);
                w.numEnemy = 25;
                w.N = 4;
                w.R = 400;
                w.speed = 300;
                w.duration = 12;
                waves.Add(new Wave(WaveType.Ngon, w));
            }

            if (time > 40 && time - elapsed < 40)
            {

                w.eType = typeof(Enemy_Fast);
                w.numEnemy = 40;
                w.N = 3;
                w.R = 400;
                w.speed = 300;
                w.duration = 12;
                waves.Add(new Wave(WaveType.Ngon, w));
            }

            if (time > 62 && time - elapsed < 62)
            {

                w.numEnemy = 10;
                waves.Add(new Wave(WaveType.Random, w));
            }
            
            if (time > 65 && time - elapsed < 65)
            {
                w.numEnemy = 25;
                w.duration = 10;
                w.waveDir = new Vector2(-1, 0);
                w.eType = typeof(Enemy_Fast);
                w.HelixP = 10;
                w.N = 19;
                w.R = 50;
                w.speed = 150;
                w.center = new Vector2(768+500, Game1.screenHeight / 2);
                waves.Add(new Wave(WaveType.Helix, w));
                w.center = new Vector2(768 + 500, Game1.screenHeight / 2-300);
                waves.Add(new Wave(WaveType.Helix, w));
                w.center = new Vector2(768 + 500, Game1.screenHeight / 2+300);
                waves.Add(new Wave(WaveType.Helix, w));
            }
            
            if (time > 70 && time - elapsed < 70)
            {
                if (enemyList.Count == 0)
                {
                    boss = new Boss4(Cnt.game.Content.Load<Texture2D>("boss4"));
                    enemyList.Add(boss);
                    MediaPlayer.Stop();
                    //MediaPlayer.Play(Cnt.game.bossTheme);
                    Cnt.game.waveOutDevice.Stop();
                    Cnt.game.mainOutputStream = Cnt.game.CreateInputStream(Cnt.game.bossTheme);
                    Cnt.game.waveOutDevice.Init(Cnt.game.mainOutputStream);
                    Cnt.game.waveOutDevice.Play();

                }
                else time = 70 - 10 * elapsed;
            }
            

            ///// end
            if (boss != null && boss.toDie)
            {
                Player pl = new Player(player.Pos, Vector2.Zero, player.pony);
                Cnt.game.CurrentPlayer = pl;
                nextLevel = new Level_5();
            }
            base.Update(gt);
        }

    }
}
