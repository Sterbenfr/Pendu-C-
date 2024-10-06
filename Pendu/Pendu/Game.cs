using System;
using System.Collections.Generic;
using System.IO;

namespace Pendu
{
    internal class Game
    {
        private Player player; // Référence au joueur
        private Word word; // Mot à deviner
        private List<string> wordList = new List<string>(); // Liste des mots disponibles récupréré du fichier
        private Random random; // Générateur de nombres aléatoire
        private List<char> wrongGuesses = new List<char>(); // Lettre incorrecte devinées

        public Game(Player player, int seed)
        {
            this.player = player;
            this.random = new Random(seed);
            LoadWords(); // Charger les mots depuis le fichier
            StartGame(); // Démarrer le jeu
        }

        private void LoadWords()
        {
            try
            {
                string[] words = File.ReadAllLines("WordList.txt");
                wordList = new List<string>(words); // Charger les mots dans la liste
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Fichier de mots introuvable.");
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur lors de la lecture du fichier : " + e.Message);
                throw;
            }
        }

        private void StartGame()
        {
            bool playAgain = true;

            while (playAgain)
            {
                if (wordList.Count == 0)
                {
                    Console.WriteLine("La liste de mots est vide. Remplissez la.");
                    return;
                }

                string wordToGuess = wordList[random.Next(wordList.Count)];
                word = new Word(wordToGuess); // Sélectionner un mot aléatoire

                // Réinitialiser pour pouvoir rejouer
                player.Life = 10;
                wrongGuesses.Clear();

                while (player.Life > 0 && !word.IsGuessed())
                {
                    Console.WriteLine("Mot à deviner: " + new string(word.GuessedWord));
                    Console.WriteLine("Tentatives restantes: " + player.Life);
                    Console.WriteLine("Lettres incorrectes: " + string.Join(", ", wrongGuesses));
                    Console.Write("Entrez une lettre: ");

                    try
                    {
                        char guess = GetValidInput(); // input vérifier pour non null par ex
                        if (!word.GuessLetter(guess))
                        {
                            player.LoseLife(); // Perdre une vie si la lettre est incorrecte
                            wrongGuesses.Add(guess);
                            DrawHangman(player.Life); // Dessiner le pendu
                        }
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                if (word.IsGuessed())
                {
                    Console.WriteLine("GG ! Vous avez deviné le mot : " + word.Text);
                }
                else
                {
                    Console.WriteLine("Perdu! C'était : " + word.Text);
                }

                playAgain = AskToPlayAgain(); // Demander si le joueur veut rejouer
            }
        }

        private bool AskToPlayAgain()
        {
            Console.WriteLine("Voulez-vous rejouer ? (o/n)");
            string? input = Console.ReadLine();
            return input != null && input.ToLower() == "o"; // Vérifier si le joueur veut rejouer
        }

        private char GetValidInput()
        {
            string? input = Console.ReadLine();

            if (string.IsNullOrEmpty(input) || input.Length != 1)
            {
                throw new ArgumentException("Une seule lettre! ");
            }

            return char.ToLower(input[0]); // Retourner la lettre en minuscule
        }

        private void DrawHangman(int life)
        {
            string[] hangmanStages = new string[]
            {
                // Différentes étapes du dessin du pendu
                @"
              
                  
                   
                   
                   
                   
            =========",
                @"
              
                  |
                  |
                  |
                  |
                  |
            =========",
                @"
              +---+
                  |
                  |
                  |
                  |
                  |
            =========",
                @"
              +---+
              |   |
                  |
                  |
                  |
                  |
            =========",
                @"
              +---+
              |   |
              O   |
                  |
                  |
                  |
            =========",
                @"
              +---+
              |   |
              O   |
              |   |
                  |
                  |
            =========",
                @"
              +---+
              |   |
              O   |
             /|   |
                  |
                  |
            =========",
                @"
              +---+
              |   |
              O   |
             /|\  |
                  |
                  |
            =========",
                @"
              +---+
              |   |
              O   |
             /|\  |
             /    |
                  |
            =========",
                @"
              +---+
              |   |
              O   |
             /|\  |
             / \  |
                  |
            ========="
            };

            int index = 9 - life;
            if (index >= 0 && index < hangmanStages.Length)
            {
                Console.WriteLine(hangmanStages[index]); // Afficher l'étape correspondante
            }
            else
            {
                Console.WriteLine("Erreur: Index de vie invalide.");
            }
        }
    }
}
