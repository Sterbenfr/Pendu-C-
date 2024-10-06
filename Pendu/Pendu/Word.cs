using System;
using System.Globalization;
using System.Text;

namespace Pendu
{
    internal class Word
    {
        private string _text; // mot à deviner
        private char[] _guessedWord; // mot deviné jusqu'à présent

        public Word(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Le texte ne peut pas être nul ou vide.", nameof(text));
            }

            _text = text;
            _guessedWord = new char[text.Length];

            for (int i = 0; i < text.Length; i++)
            {
                _guessedWord[i] = char.IsLetter(text[i]) ? '_' : text[i]; // Remplacer les lettres par des underscores
            }
        }

        public string Text => _text; // getter texte du mot

        public char[] GuessedWord => _guessedWord; // getter mot deviné

        public bool GuessLetter(char letter)
        {
            bool correctGuess = false;
            string normalizedText = RemoveDiacritics(_text); // Normaliser le texte pour ignorer les accents etc, a l'air de fonctionner
            char normalizedLetter = RemoveDiacritics(letter.ToString())[0]; // Normaliser la lettre devinée

            for (int i = 0; i < _text.Length; i++)
            {
                if (normalizedText[i] == normalizedLetter)
                {
                    _guessedWord[i] = _text[i]; // Révéler la lettre correcte dans le mot deviné
                    correctGuess = true;
                }
            }
            return correctGuess; // Retourner vrai si la lettre est correcte
        }

        public bool IsGuessed()
        {
            return Array.IndexOf(_guessedWord, '_') == -1; // Vérifier si le mot est entièrement deviné
        }

        static string RemoveDiacritics(string text)
        {
            string formD = text.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            foreach (char ch in formD)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch); 
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC); // Retourner le texte normalisé
        }
    }
}
