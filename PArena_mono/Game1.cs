using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using NAudio;
using NAudio.Wave;
using NAudio.FileFormats;
//using NAudio.WindowsMediaFormat;

//using System.Runtime.InteropServices;
using System.Diagnostics;


namespace PArena
{
    public struct ShakeParam
    {
        public Vector2 dir;
        public float ampl;
        public float maxtime;
        public float curtime;
    };

    public enum GameStates
    {
        GS_Menu,
        GS_Level,
        GS_ChangeLevel,
        GS_GameOver
    }
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        

        public GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;        
        public Camera camera;
        public Camera Camera { get { return camera; } }
        InputState input;


        public SpriteFont font;
        public SpriteFont fontVerdana;
        public SpriteFont fontVerdanaLarge;
      
        //public static Random rnd = new Random(222111);
        public static Random rnd = new Random((int)DateTime.Now.Millisecond);
        public GameStates gameState = GameStates.GS_Menu;

        public List<Spot> spots=new List<Spot>();
        Texture2D spotTextures;
        public Texture2D tooltipTex;
        List<ToolTip> tooltips;
        public int spotCount { get { return spotTextures.Width/80; } }

        public static int screenWidth = 1024;
        public static int screenHeight = 768;

        Player player;
        Level currentLevel;
        public Player CurrentPlayer { get { return player; } set { player = value; } }        
        public Level CurrentLevel { get { return currentLevel; } set { currentLevel = value; } }
      
        
        List<Enemy> enemyList = new List<Enemy>();
        List<Bullet> bullets = new List<Bullet>();
        List<Bullet> enemyBullets = new List<Bullet>();
        List<Bonus> bonusList = new List<Bonus>();
        public List<Enemy> EnemyList { get { return enemyList; } }
        public List<Bullet> Bullets { get { return bullets; } }
        public List<Bullet> EnemyBullets { get { return enemyBullets; } }
        public List<Bonus> BonusList { get { return bonusList; } }

        public Dictionary<string, Texture2D> EnemyTextures;
        public List<Texture2D> playerTextures = new List<Texture2D>();

        public Texture2D bulletTex;
        public Texture2D tailTexCommon;
        public Texture2D bonusesTex;
        Texture2D menuTex;
        Texture2D deadScreenTex;
        Texture2D cursorTex;
        
        HUD hud;
        int currentLevelSelect = 0;

        RenderTarget2D mainScene;
        RenderTarget2D shakeScene;
        RenderTarget2D currentTarget;
        RenderTarget2D distortRT;
        RenderTarget2D distortedRT;

        RenderTarget2D maskdRT;
        RenderTarget2D lightdRT;

        Texture2D distort;      
        Texture2D lightMask;
        Texture2D lightMaskDirect;


        Effect shakeEffect;
        Effect distortEffect;
        Effect lightEffect;

        delegate Vector2 ShakeDelegate(ShakeParam sh);
        ShakeDelegate ShakeFunction;
        ShakeParam shakeParam;


        
        //Sounds
        /*
        Song titleTheme;
        Song levelTheme;
        public Song bossTheme;
        AudioEngine audioEngine;
        WaveBank waveBank;
        public SoundBank soundBank;
        */
        public System.Media.SoundPlayer soundPlayer;
        public IWavePlayer waveOutDevice;
        public WaveStream mainOutputStream;
        public WaveChannel32 volumeStream;
        string titleTheme = "Content\\Audio\\Song\\rest.mp3";
        string levelTheme = "Content\\Audio\\Song\\L.S.D.mp3";
        public string bossTheme = "Content\\Audio\\Song\\toxic.mp3";

        public string clickSound = "Content\\Audio\\click.wav";
        public string CYDSound = "Content\\Audio\\CYD.wav";
        public string deathSound = "Content\\Audio\\death.wav";
        public string chaingunSound = "Content\\Audio\\chaingun.wav";
        public string victorySound = "Content\\Audio\\FF-Victory.wav";
        public string guidedSound = "Content\\Audio\\guided.wav";
        public string railgunSound = "Content\\Audio\\railgun.wav";
        public string rifleSound = "Content\\Audio\\rifle.wav";
        public string shotgunSound = "Content\\Audio\\shotgun.wav";

        public ParticleEngine pEngine;

        //gameplay
        float LoadBonus=1;

        // TMP             
        ParticleEmitter testEmitter;
        long elapsed1;
        long elapsed2;
        float elapsed3;
        /*
        [DllImport("user32.dll")]
        public static extern void SetWindowPos(IntPtr Hwnd, uint Level, int X, int Y, int W, int H, uint Flags);
        */
        
        public Game1()
        {            
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            
            //IsMouseVisible = false;
            //IsFixedTimeStep = false;
            //graphics.SynchronizeWithVerticalRetrace = false;
            //TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 5);
            //this.Window.ClientBounds.Location = new Point(100, 100);    
            //SetWindowPos(Window.Handle, 0, 100, 100, screenWidth, screenHeight, 0);            
            
        }
    


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Cnt.game = this;
            ShakeFunction = RandomShake;
            hud = new HUD();
            camera = new Camera();
            input = InputState.GetInput();

            base.Initialize();
            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            mainScene = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, graphics.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
            shakeScene = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, graphics.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
            currentTarget = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, graphics.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);            
            distortRT = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, graphics.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
            distortedRT = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, graphics.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);

            maskdRT = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, graphics.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
            lightdRT = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, graphics.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);

            //audioEngine = new AudioEngine("Content\\Audio\\audio.xgs");
            //waveBank = new WaveBank(audioEngine, "Content\\Audio\\Wave Bank.xwb");
            //soundBank = new SoundBank(audioEngine, "Content\\Audio\\Sound Bank.xsb");
            //Cue cyd = soundBank.GetCue("CYD");
            //cyd.Play();
          
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
           
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Font1");
            fontVerdana = Content.Load<SpriteFont>("Font2");
            fontVerdanaLarge = Content.Load<SpriteFont>("Font3");
           // Components.Add(new FPSCounter(this, font, spriteBatch));            

            bulletTex = Content.Load<Texture2D>("b1");
      
            /*
            shakeEffect = Content.Load<Effect>("transparent");            
            distortEffect = Content.Load<Effect>("distort");
            lightEffect = Content.Load<Effect>("lights");
            */
            //load mono effect
            byte[] effectfile = File.ReadAllBytes("Content\\transparent.monofx");
            shakeEffect = new Effect(GraphicsDevice, effectfile);
            effectfile = File.ReadAllBytes("Content\\distort.monofx");
            distortEffect = new Effect(GraphicsDevice, effectfile);
            effectfile = File.ReadAllBytes("Content\\lights.monofx");
            lightEffect = new Effect(GraphicsDevice, effectfile);


            EnemyTextures = new Dictionary<string, Texture2D>();
            EnemyTextures.Add("Fast", Content.Load<Texture2D>("enemy_fast"));
            EnemyTextures.Add("Normal", Content.Load<Texture2D>("enemy_normal"));
            EnemyTextures.Add("Slow", Content.Load<Texture2D>("enemy_slow"));
            EnemyTextures.Add("Stable", Content.Load<Texture2D>("enemy_stable"));
            playerTextures.Add(Content.Load<Texture2D>("pony\\aj"));//0
            playerTextures.Add(Content.Load<Texture2D>("pony\\flutty"));//1
            playerTextures.Add(Content.Load<Texture2D>("pony\\p"));//2
            playerTextures.Add(Content.Load<Texture2D>("pony\\rar"));//3
            playerTextures.Add(Content.Load<Texture2D>("pony\\rd"));//4
            playerTextures.Add(Content.Load<Texture2D>("pony\\tw"));//5

            spotTextures=Content.Load<Texture2D>("spotsTex");
            cursorTex = Content.Load<Texture2D>("cursor");
            menuTex = Content.Load<Texture2D>("menu");
            tooltipTex = Content.Load<Texture2D>("toolTip");
            tooltips = new List<ToolTip>();
            tooltips.Add(new ToolTip(new Rectangle(117, 137, 114, 105), new Rectangle(37, 380, 270, 90), "Twilight Sparkle: Element of Magic\n\nWeapon: Guided Shot", Color.MediumOrchid, Pony.Twily));
            tooltips.Add(new ToolTip(new Rectangle(248, 137, 114, 105), new Rectangle(167, 380, 270, 90), "Rarity: Element of Generosity\n\nWeapon: Rifle Shot", Color.MediumPurple, Pony.Rarity));
            tooltips.Add(new ToolTip(new Rectangle(379, 137, 114, 105), new Rectangle(297, 380, 270, 90), "Rainbow Dash: Element of Loyalty\n\nWeapon: RailGun\n+20% coolness", Color.CornflowerBlue, Pony.RD));
            tooltips.Add(new ToolTip(new Rectangle(510, 137, 114, 105), new Rectangle(427, 380, 270, 90), "Fluttershy: Element of Kindness\n\nWeapon: MachineGun", Color.Khaki, Pony.Flutty));
            tooltips.Add(new ToolTip(new Rectangle(641, 137, 114, 105), new Rectangle(557, 380, 270, 90), "AppleJack: Element of Honesty\n\nWeapon: ShotGun", Color.DarkOrange, Pony.AJ));
            tooltips.Add(new ToolTip(new Rectangle(772, 137, 114, 105), new Rectangle(687, 380, 270, 90), "Pinie Pie:Element of Laughter\n\nWeapon: FlameThrower\nin DLC for $19.99", Color.Magenta, Pony.Pinkie));


            distort = Content.Load<Texture2D>("distortMap");
            deadScreenTex = Content.Load<Texture2D>("deadscreen");
            tailTexCommon = Cnt.game.Content.Load<Texture2D>("t2");
            bonusesTex = Cnt.game.Content.Load<Texture2D>("bonusesTex");
            
            lightMask = Cnt.game.Content.Load<Texture2D>("lightmask");
            lightMaskDirect = Cnt.game.Content.Load<Texture2D>("lightmaskdirect");

            pEngine = new ParticleEngine(this);
            testEmitter = new ParticleEmitter(pEngine, Vector2.Zero);
            //pps, pSpeed, 
            //posVar, alphaVel, 
            //minSize,maxSize, sizeVel,
            //ttl
            testEmitter.SetParam(6000, 70, 
                              1, 100, 
                              0.2f, 0.7f, 0.000005f, 
                              50);
            /*
            titleTheme = Content.Load<Song>("Audio\\Song\\rest");
            levelTheme = Content.Load<Song>("Audio\\Song\\L.S.D");
            bossTheme = Content.Load<Song>("Audio\\Song\\toxic");
            */

            waveOutDevice = new WaveOut(WaveCallbackInfo.FunctionCallback());
            mainOutputStream = CreateInputStream(titleTheme);
            waveOutDevice.Init(mainOutputStream);
            //waveOutDevice.Volume = 0.6f;
            waveOutDevice.Play();
            

            soundPlayer = new System.Media.SoundPlayer();           
            soundPlayer.SoundLocation = CYDSound;
            soundPlayer.Play();            
        }

        public WaveStream CreateInputStream(string fileName)
        {
            WaveChannel32 inputStream;
            if (fileName.EndsWith(".mp3"))
            {
                WaveStream mp3Reader = new Mp3FileReader(fileName);
                inputStream = new WaveChannel32(mp3Reader);
                //inputStream.Volume = 0.5f;
            }
            else
            {
                throw new InvalidOperationException("Unsupported extension");
            }
            if (volumeStream != null) volumeStream.Dispose();
            volumeStream = inputStream;
            return volumeStream;
        }

        private void CloseWaveOut()
        {
            if (waveOutDevice != null)
            {
                waveOutDevice.Stop();
            }
            if (mainOutputStream != null)
            {
                // this one really closes the file and ACM conversion
                volumeStream.Close();
                //volumeStream.Dispose();
                volumeStream = null;
                // this one does the metering stream
                mainOutputStream.Close();
                //mainOutputStream.Dispose();
                mainOutputStream = null;
            }
            if (waveOutDevice != null)
            {
                waveOutDevice.Dispose();
                waveOutDevice = null;
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {                      
            CloseWaveOut();
        }

        protected override void Update(GameTime gameTime)
        {
            input.Update();
            switch (gameState)
            {
                case GameStates.GS_Menu:
                    Update_Menu(gameTime);
                    break;
                case GameStates.GS_Level:
                    Update_Level(gameTime);
                    break;
                case GameStates.GS_ChangeLevel:
                    Update_ChangeLevel(gameTime);
                    break;
                case GameStates.GS_GameOver:
                    Update_GameOver(gameTime);
                    break;
                default:
                    break;
            }
            base.Update(gameTime);   
        }

        private void Update_GameOver(GameTime gameTime)
        {
            if (input.IsNewKeyPressed(Keys.Space))            
            {
                gameState = GameStates.GS_Menu;
                //MediaPlayer.Stop();
               // Cue cyd = soundBank.GetCue("CYD");
              //  cyd.Play();
                waveOutDevice.Stop();
                soundPlayer.SoundLocation = CYDSound;
                soundPlayer.Play();
                
            }

            if (input.IsNewKeyPressed(Keys.Escape))
            {
                gameState = GameStates.GS_Menu;
                MediaPlayer.Stop();
                /*
                Cue cyd = soundBank.GetCue("CYD");
                cyd.Play();  */
                soundPlayer.SoundLocation = CYDSound;
                soundPlayer.Play();
            }

            player.Update(gameTime,false);            

        }

        void Update_Menu(GameTime gt)
        {
            
            KeyboardState kstate = Keyboard.GetState();
            MouseState mstate = Mouse.GetState();

            if (waveOutDevice.PlaybackState == PlaybackState.Stopped || mainOutputStream.CurrentTime> mainOutputStream.TotalTime)
            {
                waveOutDevice.Stop();                
                mainOutputStream = CreateInputStream(titleTheme);
                waveOutDevice.Init(mainOutputStream);
                waveOutDevice.Play();
            }                                     
            /*
            if (MediaPlayer.State == MediaState.Stopped)
            {
                
                MediaPlayer.Volume = 0.2f;
                MediaPlayer.Play(titleTheme);
            }
            */
            if (input.IsNewKeyPressed(Keys.D1)) { }

            if (input.IsNewKeyPressed(Keys.Escape)) this.Exit();
           
            foreach (ToolTip tt in tooltips)
            {
                tt.Update(mstate, gt);
            }

            if (input.IsLeftButtonClick())            
            {
                foreach (ToolTip tt in tooltips)
                {
                    if (tt.Active && tt.playerType != Pony.Pinkie)
                    {
                        enemyList.Clear();
                        enemyBullets.Clear();
                        bullets.Clear();
                        player = new Player(new Vector2(screenWidth / 2, screenHeight / 2), Vector2.Zero, tt.playerType);
                        CurrentLevel = (Level)Activator.CreateInstance(Type.GetType("PArena.Level_" + (currentLevelSelect + 1).ToString()));
                        //player = new Player(new Vector2(220,200), Vector2.Zero, Pony.Twily);
                        //CurrentLevel = new Level_5();                       
                        
                        gameState = GameStates.GS_Level;
                        //MediaPlayer.Stop();
                        waveOutDevice.Stop();                        
                    }
                }
                
                for (int i = 0; i < 5; i++)
                {
                    Rectangle rect = new Rectangle(155 + i * 146, 580, 120, 70);
                    if (rect.Contains(new Point(mstate.X, mstate.Y)))
                    {
                        currentLevelSelect = i; 
                        soundPlayer.SoundLocation = clickSound;
                        soundPlayer.Play();
                    }
                }
            }
            if (input.IsNewKeyPressed(Keys.A))
            {
                currentLevelSelect--;                
                /*Cue c = soundBank.GetCue("click"); c.Play();*/
                soundPlayer.SoundLocation = clickSound;                
                soundPlayer.Play();
            }
            if (input.IsNewKeyPressed(Keys.D))
            {
                currentLevelSelect++;
                /*Cue c = soundBank.GetCue("click"); c.Play();*/
                soundPlayer.SoundLocation = clickSound;
                soundPlayer.Play();
            }
            currentLevelSelect = (int)MathHelper.Clamp(currentLevelSelect, 0, 4);
        }

        void Update_ChangeLevel(GameTime gt)
        {           
            KeyboardState kstate = Keyboard.GetState();
            MouseState mstate = Mouse.GetState();
            if (input.IsNewKeyPressed(Keys.Escape)) 
            {
                gameState = GameStates.GS_Menu;                
                soundPlayer.SoundLocation = CYDSound;
                soundPlayer.Play();
                waveOutDevice.Stop();
                /*
                MediaPlayer.Stop();
                Cue cyd = soundBank.GetCue("CYD");
                cyd.Play();*/
            }

            player.Update(gt, false);
            CurrentLevel.LevelBoss.Update(gt);
            CurrentLevel.UpdateTime(gt);

            if (input.IsNewKeyPressed(Keys.Space)) 
            {
                gameState = GameStates.GS_Level;
                CurrentLevel = CurrentLevel.nextLevel;
                enemyList.Clear();
                bullets.Clear();
                EnemyBullets.Clear();
                pEngine.Clear();
                player.hitpoints = player.maxHitpoints;
                shakeParam.curtime = 0;
                //MediaPlayer.Stop();
                //MediaPlayer.Play(levelTheme);
                PresentationParameters pp = GraphicsDevice.PresentationParameters;
                //mainScene = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, graphics.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
                //shakeScene = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, graphics.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
                //currentTarget = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, graphics.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
                //distortRT = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, graphics.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
                //distortedRT = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, graphics.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);

                maskdRT = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, graphics.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
                //lightdRT = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, graphics.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);

                waveOutDevice.Stop();
                mainOutputStream = CreateInputStream(levelTheme);
                waveOutDevice.Init(mainOutputStream);
                waveOutDevice.Play();
            }           
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        void Update_Level(GameTime gameTime)
        {
            testEmitter.Pos = input.MouseVector();
            //testEmitter.Update(gameTime);

            if (rnd.NextDouble()  < 0.01&&LoadBonus<0) { Bonus.LoadRandomBonus(this); LoadBonus = 20; }
            LoadBonus -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            HandleInput();
            
            if (waveOutDevice.PlaybackState == PlaybackState.Stopped|| mainOutputStream.CurrentTime> mainOutputStream.TotalTime)
            {
                if (CurrentLevel.LevelBoss != null)
                {
                    waveOutDevice.Stop();
                    mainOutputStream = CreateInputStream(bossTheme);
                    waveOutDevice.Init(mainOutputStream);
                    waveOutDevice.Play();
                }
                else
                {
                    waveOutDevice.Stop();
                    mainOutputStream = CreateInputStream(levelTheme);
                    waveOutDevice.Init(mainOutputStream);
                    waveOutDevice.Play();
                }
            }
            /*
            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Volume = 0.3f;
                if (CurrentLevel.LevelBoss != null)
                    MediaPlayer.Play(bossTheme);
                else
                    MediaPlayer.Play(levelTheme);
            }                      
            */
            var sw = Stopwatch.StartNew();
            pEngine.Update(gameTime);
            elapsed1 = sw.ElapsedTicks;

            sw.Restart();

            player.Update(gameTime);

            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].Update(gameTime);
                if (enemyList[i].toRemove) enemyList.RemoveAt(i--); 
            }

            for (int i = 0; i < bonusList.Count; i++)
            {
                bonusList[i].Update(gameTime);
                if (bonusList[i].toRemove) bonusList.RemoveAt(i--);
            }
            
            for (int i = 0; i < Bullets.Count; i++)
            {
                Bullets[i].Update(gameTime);
                //if (Bullets[i].toRemove) Bullets.RemoveAt(i);                
            }
            for (int i = 0; i < EnemyBullets.Count; i++)
            {
                EnemyBullets[i].Update(gameTime);
                //if (EnemyBullets[i].toRemove) EnemyBullets.RemoveAt(i);
            }

            //if (boss1 != null) boss1.Update(gameTime);
            if (currentLevel != null) currentLevel.Update(gameTime);

            CheckCollision(gameTime);
            if (player.isDead)
            {                                
                //MediaPlayer.Stop();
                //soundBank.GetCue("death").Play();

                waveOutDevice.Stop();
                soundPlayer.SoundLocation = deathSound;
                soundPlayer.Play();
                gameState = GameStates.GS_GameOver;
                
            }
            elapsed3 = sw.ElapsedTicks;
            sw.Restart();
            for (int i = 0; i < Bullets.Count; i++)
            {
                Bullets[i].Emit(gameTime);
                if (Bullets[i].toRemove) Bullets.RemoveAt(i--);                
            }
            for (int i = 0; i < EnemyBullets.Count; i++)
            {
                EnemyBullets[i].Emit(gameTime);
                if (EnemyBullets[i].toRemove) EnemyBullets.RemoveAt(i--);
            }
            elapsed2 = sw.ElapsedTicks;
            
            shakeParam.curtime = MathHelper.Clamp(shakeParam.curtime - (float)gameTime.ElapsedGameTime.TotalSeconds, 0, shakeParam.maxtime);                                
        }

        void HandleInput()
        {
            KeyboardState kstate = Keyboard.GetState();
            MouseState mstate = Mouse.GetState();
            /*
            if (input.IsNewKeyPressed(Keys.D1)) 
            {
                Enemy e = new Enemy_StableT2(new Vector2(200, 200));
                enemyList.Add(e);
                e = new Enemy_StableT1(new Vector2(240, 200));
                //enemyList.Add(e);
                e = new Enemy_StableT1(new Vector2(280, 200));
                //enemyList.Add(e);
            }
            */
            if (input.IsNewKeyPressed(Keys.Escape)) 
            {
                //this.Exit();
                gameState = GameStates.GS_Menu;
                //MediaPlayer.Stop();                
                /*Cue cyd = soundBank.GetCue("CYD");
                cyd.Play();*/
                soundPlayer.SoundLocation = CYDSound;
                soundPlayer.Play();
                waveOutDevice.Stop();
                mainOutputStream = CreateInputStream(titleTheme);
                waveOutDevice.Init(mainOutputStream);
                waveOutDevice.Play();
            }
            
            if (input.IsLeftButtonPress() && CurrentLevel.levelTime > 0.1f /*&& oldmstate.LeftButton != ButtonState.Pressed*/)
            {
                if (player.canFire)
                {
                    shakeParam = player.Shoot(mstate.X, mstate.Y);                    
                    Vector2 dir = player.Pos - new Vector2(mstate.X, mstate.Y);
                    dir.Normalize();
                    shakeParam.dir = dir;                   
                    ShakeFunction = VectorShake;
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            switch (gameState)
            {
                case GameStates.GS_Menu:
                    Draw_Menu(gameTime);
                    break;
                case GameStates.GS_Level:
                    Draw_Level(gameTime);
                    break;
                case GameStates.GS_ChangeLevel:
                    Draw_ChangeLevel(gameTime);
                    break;
                case GameStates.GS_GameOver:
                    Draw_GameOver(gameTime);
                    break;
                default:
                    break;
            }
            MouseState mstate = Mouse.GetState();
            spriteBatch.Begin();
            spriteBatch.Draw(cursorTex,new Vector2(mstate.X,mstate.Y)-new Vector2(10,10),Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void Draw_GameOver(GameTime gameTime)
        {
            
            Draw_Level(gameTime);
            spriteBatch.Begin();
            spriteBatch.Draw(deadScreenTex, Vector2.Zero, Color.White);
            spriteBatch.End();
        }

        void Draw_Menu(GameTime gt)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(menuTex, new Rectangle(0,0,screenWidth,screenHeight), Color.White);
            spriteBatch.Draw(tooltipTex, new Vector2(109, 129), new Rectangle(0, 0, 786, 122), Color.White);
            foreach (ToolTip tt in tooltips)
            {
                if (tt.Visible) tt.Draw(spriteBatch);
            }
            
            spriteBatch.DrawString(fontVerdanaLarge, "Select Level", new Vector2(450, 500), Color.White);

            spriteBatch.Draw(tooltipTex, new Rectangle(155+currentLevelSelect*146, 580, 120, 70), new Rectangle(0, 122, 270, 90), Color.White);
            spriteBatch.DrawString(fontVerdanaLarge, "Level 1           Level 2           Level 3           Level 4           Level 5", new Vector2(180, 600), Color.White);
           

            spriteBatch.End();
        }

        void Draw_ChangeLevel(GameTime gt)
        {
            Rectangle rect = new Rectangle(-50, -50, Game1.screenWidth + 100, Game1.screenHeight + 100);
            //Rectangle rect2 = new Rectangle(0, 0, CurrentLevel.mainBackground.Width, CurrentLevel.mainBackground.Height);

            if (spots.Count > 0)
            {
                GraphicsDevice.SetRenderTarget(CurrentLevel.backgroundRT);
                spriteBatch.Begin();
                foreach (var spot in spots)
                {
                    spriteBatch.Draw(spotTextures, spot.pos - spot.origin + new Vector2(25, 25), spot.rect, spot.color);
                }
                spriteBatch.End();
                spots.Clear();
            }
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin();
            CurrentLevel.Draw(spriteBatch, rect);
            CurrentLevel.LevelBoss.Draw(spriteBatch);
            player.Draw(spriteBatch);
            spriteBatch.End();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        void Draw_Level(GameTime gameTime)
        {
            Vector2 shakeVector = ShakeFunction(shakeParam);
            Rectangle rect = new Rectangle(-50, -50, Game1.screenWidth + 100, Game1.screenHeight + 100);
            //Rectangle rect2 = new Rectangle(0, 0, CurrentLevel.mainBackground.Width, CurrentLevel.mainBackground.Height);

            if (spots.Count > 0)
            {
                GraphicsDevice.SetRenderTarget(CurrentLevel.backgroundRT);
                spriteBatch.Begin();
                foreach (var spot in spots)
                {
                    spriteBatch.Draw(spotTextures, spot.pos - spot.origin + new Vector2(50, 50), spot.rect, spot.color);
                }
                spriteBatch.End();
                spots.Clear();

            }
           
            GraphicsDevice.SetRenderTarget(mainScene);
            GraphicsDevice.Clear(new Color(0, 0, 0, 0));
            spriteBatch.Begin();

            rect.Offset((int)shakeVector.X, (int)shakeVector.Y);
            CurrentLevel.Draw(spriteBatch, rect);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            pEngine.Draw();
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            foreach (var bullet in Bullets)
            {
                bullet.Draw(spriteBatch);
            }
            foreach (var bullet in EnemyBullets)
            {
                bullet.Draw(spriteBatch);
            }

            for (int i = 0; i < bonusList.Count; i++)
            {
                bonusList[i].Draw(spriteBatch);
            }


            //if (boss1 != null) boss1.Draw(spriteBatch);

            player.Draw(spriteBatch);
            foreach (var e in enemyList)
            {
                if (!e.isDistorted) { e.Draw(spriteBatch); }

            }

            spriteBatch.End();
            currentTarget = mainScene;

            if (CurrentLevel.LevelBoss != null && CurrentLevel.LevelBoss.isDistorted )
            {
                GraphicsDevice.SetRenderTarget(distortRT);
                GraphicsDevice.Clear(new Color(127, 127, 127, 127));
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                spriteBatch.Draw(distort, CurrentLevel.LevelBoss.Pos - CurrentLevel.LevelBoss.Origin, Color.White);
                spriteBatch.End();
                GraphicsDevice.SetRenderTarget(distortedRT);
                GraphicsDevice.Textures[1] = (Texture2D)currentTarget;
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, distortEffect);
                spriteBatch.Draw((Texture2D)distortRT, Vector2.Zero, Color.White);
                spriteBatch.End();
                currentTarget = distortedRT;
            }

            //
            /*
            bool flag = false;
            GraphicsDevice.SetRenderTarget(distortRT);
            GraphicsDevice.Clear(new Color(128, 128, 128, 128));
           
            foreach (Enemy e in EnemyList)
            {
                if (!e.toDie) continue;
                flag = true;
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque);
                spriteBatch.Draw(distort, e.Pos - e.Origin-new Vector2(100,100), Color.White);
                //spriteBatch.Draw(distort, e.rect, null, Color.White);
                spriteBatch.End();
            }
            
            foreach (Enemy e in EnemyList)
            {
                
                //GraphicsDevice.SetRenderTarget(rt);
                float strength = (float)Math.Sin(MathHelper.Pi* e.killTime / e.deadAnimationTime);//MathHelper.Lerp(0.05, 0.15, e.killTime / e.deadAnimationTime);
                if (!e.toDie) continue;
                RenderTarget2D rt = new RenderTarget2D(GraphicsDevice, 1024, 768, false, graphics.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
                strength = MathHelper.Lerp(0.05f, 0.15f, strength);
                GraphicsDevice.SetRenderTarget(rt);
                GraphicsDevice.Textures[1] = (Texture2D)currentTarget;
                distortEffect.Parameters["strength"].SetValue(strength);
                distortEffect.Parameters["pos"].SetValue(e.Pos);
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, distortEffect);
                spriteBatch.Draw((Texture2D)distortRT, Vector2.Zero, Color.White);
                spriteBatch.End();
                //currentTarget.Dispose();  
                currentTarget = rt;
            }
           //if (flag) currentTarget = distortedRT;
            //
            //currentTarget = distortRT;
            */

            // lights      
            GraphicsDevice.SetRenderTarget(maskdRT);
            GraphicsDevice.Clear(new Color(0, 0, 0, 255));

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            Rectangle currecr = Rectangle.Empty;
            foreach (Enemy e in enemyList)
            {
                if (e.hasLight)
                {
                    currecr.Width = (int)e.light.radius;
                    currecr.Height = (int)e.light.radius;
                    currecr.X = (int)e.Pos.X - currecr.Width / 2;
                    currecr.Y = (int)e.Pos.Y - currecr.Height / 2;
                    spriteBatch.Draw(lightMask,  currecr, Color.White);
                }
                
            }
            foreach (Bullet b in bullets)
            {               
                    currecr.Width = 80;
                    currecr.Height = 80;
                    currecr.X = (int)b.Pos.X - currecr.Width / 2;
                    currecr.Y = (int)b.Pos.Y - currecr.Height / 2;
                   spriteBatch.Draw(lightMask, currecr, Color.White);

            }
            foreach (Bullet b in enemyBullets)
            {
                currecr.Width = 80;
                currecr.Height = 80;
                currecr.X = (int)b.Pos.X - currecr.Width / 2;
                currecr.Y = (int)b.Pos.Y - currecr.Height / 2;
                spriteBatch.Draw(lightMask, currecr, Color.White);

            }
            if (CurrentLevel.LevelBoss != null)
            {
                currecr.Width = 500;
                currecr.Height = 500;
                currecr.X = (int)CurrentLevel.LevelBoss.Pos.X - currecr.Width / 2;
                currecr.Y = (int)CurrentLevel.LevelBoss.Pos.Y - currecr.Height / 2;
                spriteBatch.Draw(lightMask, currecr, Color.White);
            }

            if (player!= null)
            {                
                Vector2 v =  input.MouseVector()-player.Pos;
                float angle = (float)Math.Atan2(v.Y, v.X);
                Vector2 origin = new Vector2(0, 100);
                currecr.Width = 500;
                currecr.Height = 500;
                currecr.X = (int)player.Pos.X;
                currecr.Y = (int)player.Pos.Y ;
                spriteBatch.Draw(lightMaskDirect, currecr, null, Color.White, angle, origin, SpriteEffects.None, 0);
            }
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(lightdRT);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, lightEffect);
            GraphicsDevice.Textures[1] = (Texture2D)maskdRT;
            spriteBatch.Draw((Texture2D)currentTarget,Vector2.Zero,Color.White);

            spriteBatch.End();

            currentTarget = lightdRT;
            // 

            
            if (shakeParam.curtime > 0)
            {
                GraphicsDevice.SetRenderTarget(shakeScene);
                //GraphicsDevice.Clear(new Color(0, 0, 0, 0));                
                spriteBatch.Begin();
                spriteBatch.Draw((Texture2D)currentTarget, Vector2.Zero, Color.White);
                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, shakeEffect);

                foreach (var bullet in Bullets)
                {
                    bullet.Draw(spriteBatch, shakeVector);
                }

                player.Draw(spriteBatch, shakeVector);
                foreach (var e in enemyList)
                {
                    e.Draw(spriteBatch, shakeVector);
                }

                for (int i = 0; i < bonusList.Count; i++)
                {
                    bonusList[i].Draw(spriteBatch, shakeVector);
                }

                spriteBatch.End();

                currentTarget = shakeScene;
            }

            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin();
            spriteBatch.Draw((Texture2D)currentTarget, Vector2.Zero, Color.White);
            foreach (var e in enemyList)
            {
                if (e.isDistorted) e.Draw(spriteBatch, shakeVector);
            }
            hud.Draw(spriteBatch);


            // DEBUG
            /*
            spriteBatch.DrawString(font, "particles: " + pEngine.Count, new Vector2(5, 0 * 12), Color.White);
            spriteBatch.DrawString(font, "pEngine: " + elapsed1, new Vector2(5, 6 * 12), Color.White);
            spriteBatch.DrawString(font, "emit: " + elapsed2, new Vector2(5, 7 * 12), Color.White);
            spriteBatch.DrawString(font, "update: " + elapsed3, new Vector2(5, 8 * 12), Color.White);

            //spriteBatch.DrawString(font, "lb: " + LoadBonus, new Vector2(10, 5 * 12), Color.White);
            //spriteBatch.DrawString(font, "HP: " + player.hitpoints, new Vector2(10, 6 * 12), Color.White);
            // spriteBatch.DrawString(font, "time: " + CurrentLevel.Time, new Vector2(10, 7 * 12), Color.White);
            //spriteBatch.DrawString(font, "BossHP: " + boss1.Speed, new Vector2(10, 4 * 12), Color.White);
            
            */
            spriteBatch.End();
        }    

        public static bool Collision(GameObject first, GameObject second)
        {
            Rectangle firstRect = first.rect;
            Rectangle secondRect = second.rect;
            Color[] firstData = new Color[first.sprite.Width * first.sprite.Height];
            Color[] secondData = new Color[second.sprite.Width * second.sprite.Height];

            first.sprite.GetData<Color>(firstData);
            second.sprite.GetData<Color>(secondData);

            int top = Math.Max(firstRect.Top, secondRect.Top);
            int bottom = Math.Min(firstRect.Bottom, secondRect.Bottom);
            int left = Math.Max(firstRect.Left, secondRect.Left);
            int right = Math.Min(firstRect.Right, secondRect.Right);

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Color color1 = firstData[(x - firstRect.Left) + (y - firstRect.Top) * firstRect.Width];
                    Color color2 = secondData[(x - secondRect.Left) + (y - secondRect.Top) * secondRect.Width];
                    int sum1 = color1.R + color1.G + color1.B + color1.A;
                    int sum2 = color2.R + color2.G + color2.B + color2.A; 
                    if (sum1 > 300 && sum2 > 300) return true;                    
                }
            }
            return false;

        }
        
        void CheckCollision(GameTime gt)
        {           
            List<Enemy> enemyToInteract = new List<Enemy>();
            foreach (Bullet b in Bullets)
            {
                foreach (Enemy e in EnemyList)
                {
                    if (e.toDie) continue;
                    if (CheckCross(b.prevPos, b.Pos, e))
                    {                        
                        Vector2 path = b.Pos - b.prevPos;
                        Vector2 dist = e.Pos - b.prevPos;
                        float pathlength = path.Length();                      
                        Vector2 oldPos = b.Pos;
                        if (pathlength > dist.Length())
                        {
                            float xCross = Vector2.Dot(path, dist) / pathlength;
                            path.X /= pathlength;
                            path.Y /= pathlength;
                            b.Pos = b.prevPos + xCross * path;
                        }

                        if (Collision(b, e))
                        {
                            enemyToInteract.Add(e);                           
                        }                        
                        b.Pos = oldPos;
                    }                 
                }
                List<Enemy> sortedList = enemyToInteract.OrderBy(x => (x.Pos - b.prevPos).LengthSquared()).ToList();

                for (int i = 0; i < sortedList.Count && b.damage >= 0; i++)
                {
                    float hp = sortedList[i].hitpoints;
                    sortedList[i].hitpoints -= b.damage;
                    b.damage -= hp;
                    if (sortedList[i].hitpoints <= 0) sortedList[i].Die();
                    if (b.damage <= 0) {

                        Vector2 path = b.Pos - b.prevPos;
                        Vector2 dist = sortedList[i].Pos - b.prevPos;
                        float pathlength = path.Length();
                        Vector2 oldPos = b.Pos;
                        if (pathlength > dist.Length())
                        {
                            float xCross = Vector2.Dot(path, dist) / pathlength;
                            path.X /= pathlength;
                            path.Y /= pathlength;                           
                            b.Pos = b.prevPos + xCross * path;
                        }                     
                        b.prepareToRemove = true; 
                    }
                }
                sortedList.Clear();
                enemyToInteract.Clear();
            }

            

            foreach (Bullet b in EnemyBullets)
            {
                if (CheckCross(b.prevPos, b.Pos, player))
                {


                    Vector2 path = b.Pos - b.prevPos;
                    Vector2 dist = player.Pos - b.prevPos;
                    float pathlength = path.Length();
                    Vector2 oldPos = b.Pos;
                    if (pathlength > dist.Length())
                    {
                        float xCross = Vector2.Dot(path, dist) / pathlength;
                        path.X /= pathlength;
                        path.Y /= pathlength;
                        b.Pos = b.prevPos + xCross * path;
                    }

                    if (Collision(b, player))
                    {
                        player.hitpoints -= b.damage;
                        if (player.hitpoints <= 0) player.Die();
                        b.prepareToRemove = true;
                    }
                    b.Pos = oldPos;
                }                 
            }

            foreach (Enemy e in enemyList)
            {
                if (player.rect.Intersects(e.rect))
                {
                    if (Collision(player, e))
                    {
                        player.hitpoints -= 20 * (float)gt.ElapsedGameTime.TotalSeconds;
                        e.hitpoints -= 20 * (float)gt.ElapsedGameTime.TotalSeconds;
                        if (e.hitpoints <= 0) e.Die();
                        if (player.hitpoints <= 0) player.Die();
                    }
                }
            }

            foreach (Bonus bonus in bonusList)
            {
                if (bonus.rect.Intersects(player.rect))
                {
                    bonus.Interact(player);
                }
            }

        }
       
        bool CheckCross(Vector2 p1, Vector2 p2, GameObject go)
        {          
            Vector2 path = p2 - p1;
            Vector2 dist = go.Pos - p1;                        
            float sinus = path.X * dist.Y - path.Y * dist.X;
            float length = path.Length();
            sinus /= length;

            if (Math.Abs(sinus) < go.radius) {                
                if (Vector2.Dot(dist, path) > 0)
                {
                    path.X /= length; path.Y /= length;
                    Vector2 p2g = go.Pos - p2 - path * go.radius;
                    if (Vector2.Dot(p2g, path) < 0) return true;
                }                
            }
            return false;
        }
       

        Vector2 VectorShake(ShakeParam sh)
        {
            if (sh.curtime == 0) return Vector2.Zero;
            float time = sh.curtime / sh.maxtime;
            return sh.dir*sh.ampl*(float)Math.Sin(Math.PI*time);            
        }

        Vector2 RandomShake(ShakeParam sh)
        {
            if (sh.curtime == 0) return Vector2.Zero;
            return new Vector2(rnd.Next(2*(int)sh.ampl) - sh.ampl, rnd.Next(2*(int)sh.ampl) - sh.ampl);
        }
      
    }
}
