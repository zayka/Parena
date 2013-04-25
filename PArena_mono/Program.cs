using System;
using System.IO;


namespace PArena
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            //try
            {
                using (Game1 game = new Game1())
                {
                    game.Run();
                }
            }
            /*
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter("errorlog.txt"))
                {
                    sw.WriteLine(ex.Message);
                }
            }*/
           
        }
    }
#endif
}

