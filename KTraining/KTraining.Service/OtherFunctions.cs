using System;
using System.Collections.Generic;
using System.Linq;

namespace KTraining.Service
{
    public class OtherFunctions
    {
        /// <summary>
        /// Shuffle list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IList<T> Shuffle<T>(IList<T> list)
        {
            Random random = new Random(DateTime.Now.Millisecond);

            for (int i = list.Count - 1; i >= 0; i--)
            {
                int r = random.Next(i + 1);
                T value = list[r];
                list[r] = list[i];
                list[i] = value;
            }
            return list;
        }

        /// <summary>
        /// Check whether string has Java Script words
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsHasJS(string word)
        {
            word = word.ToLower();
            if (word.Replace(" ", "").Replace("&nbsp;", "").Contains("lt;script&gt;")
                || word.Replace(" ", "").Contains("<script"))
            {
                return true;
            }
            return false;
        }
    }

    public class AlphabetFuncions
    {
        private readonly Dictionary<string, string> words = new Dictionary<string, string>();

        public AlphabetFuncions()
        {
            words.Add("а", "a");
            words.Add("б", "b");
            words.Add("в", "v");
            words.Add("г", "g");
            words.Add("д", "d");
            words.Add("е", "e");
            words.Add("ж", "zh");
            words.Add("з", "z");
            words.Add("и", "i");
            words.Add("й", "j");
            words.Add("ь", "y");
            words.Add("к", "k");
            words.Add("л", "l");
            words.Add("м", "m");
            words.Add("н", "n");
            words.Add("о", "o");
            words.Add("п", "p");
            words.Add("р", "r");
            words.Add("с", "s");
            words.Add("т", "t");
            words.Add("у", "u");
            words.Add("ф", "f");
            words.Add("х", "h");
            words.Add("ц", "c");
            words.Add("ч", "ch");
            words.Add("ш", "sh");
            words.Add("щ", "sht");
            words.Add("ъ", "a");
            words.Add("ю", "u");
            words.Add("я", "q");
            words.Add("А", "A");
            words.Add("Б", "B");
            words.Add("В", "V");
            words.Add("Г", "G");
            words.Add("Д", "D");
            words.Add("Е", "E");
            words.Add("Ж", "Zh");
            words.Add("З", "Z");
            words.Add("И", "I");
            words.Add("Й", "J");
            words.Add("К", "K");
            words.Add("Л", "L");
            words.Add("М", "M");
            words.Add("Н", "N");
            words.Add("О", "O");
            words.Add("П", "P");
            words.Add("Р", "R");
            words.Add("С", "S");
            words.Add("Т", "T");
            words.Add("У", "U");
            words.Add("Ф", "F");
            words.Add("Х", "H");
            words.Add("Ц", "C");
            words.Add("Ч", "Ch");
            words.Add("Ш", "Sh");
            words.Add("Щ", "Scht");
            words.Add("Ъ", "A");
            words.Add("Ю", "U");
            words.Add("Я", "Q");
        }

        /// <summary>
        /// Convert latin string to cyrilic string
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public string ConvertLatinToCyrylic(string source)
        {
            foreach (KeyValuePair<string, string> pair in words)
            {
                source = source.Replace(pair.Value, pair.Key);
            }
            return source;
        }

        /// <summary>
        /// Convert cyrilic string to latin
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public string ConvertCyrilicToLatin(string source)
        {
            foreach (KeyValuePair<string, string> pair in words)
            {
                source = source.Replace(pair.Key, pair.Value);
            }
            return source;
        }
    }
}
