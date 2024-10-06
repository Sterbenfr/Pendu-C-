using System;

namespace Pendu
{
    class Program
    {
        static void Main(string[] args)
        {
            Player player = new Player();

            // Utiliser les ticks de TimeOfDay comme seed
            int seed = (int)DateTime.Now.TimeOfDay.TotalMilliseconds;
            Game game = new Game(player, seed);
        }
    }
}
