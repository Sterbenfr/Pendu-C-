using System;

namespace Pendu
{
    internal class Player
    {
        private int _life; 

        public Player()
        {
            _life = 10; // Initialiser la vie à 10
        }

        public int Life
        {
            get => _life; // vie actuelle
            set
            {
                if (value >= 0)
                {
                    _life = value; // Mettre à jour la vie si la valeur est positive
                }
                else
                {
                    throw new ArgumentException("La vie ne peut pas être négative.");
                }
            }
        }

        public void LoseLife()
        {
            if (_life > 0)
            {
                _life--; // Réduire la vie de 1 si on as pas déja perdu
            }
        }
    }
}

