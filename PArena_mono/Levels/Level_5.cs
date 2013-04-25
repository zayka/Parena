using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;


namespace PArena
{
    class Level_5:Level
    {
        public Level_5()
            : base()
        {
            mainBackground = Cnt.game.Content.Load<Texture2D>("LevelBG\\mainBG");
            levelBackground = Cnt.game.Content.Load<Texture2D>("LevelBG\\bg_l5");
            levelTitle = Cnt.game.Content.Load<Texture2D>("LevelBG\\level5");

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

            if (true)
            {
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


                if (time > 12 && time - elapsed < 12)
                {
                    w.numEnemy = 40;
                    w.duration = 12;
                    w.waveDir = new Vector2(-1, 0);
                    w.eType = typeof(Enemy_Fast);
                    w.HelixP = 15;
                    w.N = 5;
                    w.R = 150;
                    w.speed = 100;
                    w.center = new Vector2(768 + 400, Game1.screenHeight / 2 - 300);
                    waves.Add(new Wave(WaveType.Helix, w));
                    w.center = new Vector2(-400, Game1.screenHeight / 2 + 300);
                    w.waveDir = new Vector2(1, 0);
                    waves.Add(new Wave(WaveType.Helix, w));

                }

                if (time > 25 && time - elapsed < 25)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 randomPos = new Vector2(Game1.rnd.Next(Game1.screenWidth - 100) + 50, Game1.rnd.Next(Game1.screenHeight - 100) + 50);
                        Enemy e = new Enemy_StableT1(randomPos);
                        e.control = false;
                        enemyList.Add(e);
                    }

                }

                if (time > 33 && (time - elapsed < 33))
                {
                    w.numEnemy = 30;
                    waves.Add(new Wave(WaveType.Random, w));
                }
                if (time > 40 && (time - elapsed < 40))
                {
                    w.eType = typeof(Enemy_Fast);
                    w.numEnemy = 15;
                    w.speed = 200;
                    w.duration = 15;
                    w.waveDir = new Vector2(0, 1);
                    waves.Add(new Wave(WaveType.Line, w));
                    w.waveDir = new Vector2(0, -1);
                    waves.Add(new Wave(WaveType.Line, w));
                    w.waveDir = new Vector2(-1, 0);
                    waves.Add(new Wave(WaveType.Line, w));
                    w.waveDir = new Vector2(1, 0);
                    waves.Add(new Wave(WaveType.Line, w));
                }

                if (time > 60 && (time - elapsed < 60))
                {
                    w.numEnemy = 30;
                    waves.Add(new Wave(WaveType.Random, w));
                }

                if (time > 70 && time - elapsed < 70)
                {

                    w.eType = typeof(Enemy_Fast);
                    w.numEnemy = 60;
                    w.N = 5;
                    w.R = 400;
                    w.speed = 200;
                    w.duration = 15;
                    waves.Add(new Wave(WaveType.Ngon, w));
                }

                // boss
                if (time > 75 && time - elapsed < 75)
                {
                    if (enemyList.Count == 0)
                    {
                        boss = new Boss5(Cnt.game.Content.Load<Texture2D>("boss5"));
                        enemyList.Add(boss);
                        //MediaPlayer.Stop();
                        //MediaPlayer.Play(Cnt.game.bossTheme);
                        Cnt.game.waveOutDevice.Stop();
                        Cnt.game.mainOutputStream = Cnt.game.CreateInputStream(Cnt.game.bossTheme);
                        Cnt.game.waveOutDevice.Init(Cnt.game.mainOutputStream);
                        Cnt.game.waveOutDevice.Play();
                    }
                    else time = 75 - 10 * elapsed;
                }
            }

            ///// end
            if (boss != null && boss.toDie)
            {
                Player pl = new Player(player.Pos, Vector2.Zero, player.pony);
                Cnt.game.CurrentPlayer = pl;
                nextLevel = new Level_1();
            }
            base.Update(gt);
        }




    }
}
