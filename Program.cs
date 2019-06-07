using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Ekzamen01
{
    public enum Language
    {
        Unknown = 0,
        Russian = 1,
        English = 2
    }

    public class Dict
    {
        public Language LanguageFrom;
        public Language LanguageTo;
        public NameValueCollection NameValueColl;

        public Dict(Language from, Language to)
        {
            try
            {
                if (from == to)
                    throw new Exception("Язык перевода должен быть отличный от первоначального!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                Program.Menu(this);
            }
            LanguageFrom = from;
            LanguageTo = to;
            NameValueColl = new NameValueCollection();
            ReadDictionaryFromFile(LanguageFrom, LanguageTo);
        }

        private Language CheckSymbolLanguage(int symbol)
        {
            if ((symbol >= 1040 && symbol <= 1103) || symbol == 1025 || symbol == 1105)
                return Language.Russian;
            else if ((symbol >= 65 && symbol <= 90) || symbol >= 97 && symbol <= 122)
                return Language.English;
            return Language.Unknown;
        }

        private Language CheckWordLanguage(string word)
        {
            Language current = CheckSymbolLanguage(word[0]);
            try
            {
                if (current == Language.Unknown)
                    throw new Exception("В слове присутствует недопустимый символ или язык нераспознан!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                return Language.Unknown;
            }

            Language tmp;
            for (int i = 1; i < word.Length; i++)
            {
                tmp = CheckSymbolLanguage(word[i]);
                try
                {
                    if (current != tmp)
                        throw new Exception("В слове присутствует недопустимый символ или язык нераспознан!");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadKey();
                    return Language.Unknown;
                }
            }
            return current;
        }

        private bool IsCorrectLanguage(string word, string translation)
        {
            if ((CheckWordLanguage(word) == LanguageFrom) && (CheckWordLanguage(translation) == LanguageTo))
                return true;
            return false;
        }

        public void Add(string word, string translation)
        {
            bool isCorrectToAdd = true;
            try
            {
                if (word.Length == 0 || translation.Length == 0)
                {
                    isCorrectToAdd = false;
                    throw new Exception("В слове и переводе должны быть символы!");
                }
                if (IsCorrectLanguage(word, translation) == false)
                {
                    isCorrectToAdd = false;
                    throw new Exception("Несоответствие языка слова и его перевода текущему словарю!");
                }
                string[] arrValues = NameValueColl.GetValues(word);
                if (arrValues != null)
                {
                    for (int i = 0; i < arrValues.Length; i++)
                    {
                        if (arrValues[i].ToLower() == translation.ToLower())
                        {
                            isCorrectToAdd = false;
                            throw new Exception("Данный перевод слова уже существует!");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                return;
            }
            if (isCorrectToAdd == true)
                NameValueColl.Add(word, translation);
        }

        public void ChangeWord(string oldWord, string newWord)
        {
            string[] values = NameValueColl.GetValues(oldWord);
            try
            {
                if (values == null)
                    throw new Exception("Слово не найдено или переводов данного слова нет!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                return;
            }
            DeleteWord(oldWord);
            for (int i = 0; i < values.Length; i++)
                Add(newWord, values[i]);
        }

        public void ChangeTranslation(string word, string oldTranslation, string newTranslation)
        {
            DeleteTranslation(word, oldTranslation);
            Add(word, newTranslation);
        }

        public void DeleteWord(string word)
        {
            NameValueColl.Remove(word);
        }

        public void DeleteTranslation(string word, string delTranslation)
        {
            string[] values = NameValueColl.GetValues(word);
            try
            {
                if (values == null || values.Length == 1)
                    throw new Exception("Невозможно удалить последний вариант перевода или переводов(слова) вообще не существует!");
                if (values.Contains(delTranslation.ToLower()))
                    throw new Exception("Данного варианта перевода не существует!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                return;
            }
            DeleteWord(word);
            for (int i = 0; i < values.Length - 1; i++)
            {
                if (values[i] != delTranslation)
                    NameValueColl.Add(word, values[i]);
            }
        }

        public void SearchTranslations(string word)
        {
            string[] values = NameValueColl.GetValues(word);
            try
            {
                if (values == null)
                    throw new Exception("Не найдено переводов данного слова!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                return;
            }
            Console.WriteLine($"{word.ToUpper()} - {NameValueColl[word].ToLower()}");
            Console.ReadKey();
        }

        public void ReadDictionaryFromFile(Language from, Language to)
        {
            string path = default;
            if (from == Language.English && to == Language.Russian)
                path = "Eng-Rus.txt";
            else if (from == Language.Russian && to == Language.English)
                path = "Rus-Eng.txt";
            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (StreamReader reader = new StreamReader(fileStream, Encoding.Default))
                {
                    Regex regexFrom = new Regex(@"[A-z](\w*)");
                    Regex regexTo = new Regex(@"[А-я](\w*)");
                    if (from == Language.Russian && to == Language.English)
                    {
                        regexFrom = new Regex(@"[А-я](\w*)");
                        regexTo = new Regex(@"[A-z](\w*)");
                    }
                    while (!reader.EndOfStream)
                    {
                        string currentLine = reader.ReadLine();
                        Match matchFrom = regexFrom.Match(currentLine);
                        Match matchTo = regexTo.Match(currentLine);
                        Add(matchFrom.Value, matchTo.Value);
                    }
                }
            }
        }

        public void WriteDictionaryToFile()
        {
            string path = default;
            if (LanguageFrom == Language.English && LanguageTo == Language.Russian)
                path = "Eng-Rus.txt";
            else if (LanguageFrom == Language.Russian && LanguageTo == Language.English)
                path = "Rus-Eng.txt";
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.Default))
            {
                for (int i = 0; i < NameValueColl.Count; i++)
                {
                    string[] values = NameValueColl.GetValues(i);
                    foreach (string item in values)
                    {
                        writer.WriteLine($"{NameValueColl.GetKey(i)} - {item}");
                    }
                }
            }
            Console.WriteLine("Поток записи записал весь массив в файл");
        }

        public void ExportWordToFile(string word)
        {
            string[] values = NameValueColl.GetValues(word);
            try
            {
                if (values == null)
                    throw new Exception("Не найдено переводов данного слова!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                return;
            }
            string path = "Export.txt";
            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (StreamWriter writer = new StreamWriter(stream, Encoding.Default))
                {
                    foreach (string item in values)
                    {
                        writer.WriteLine($"{word} - {item}");
                    }
                }
            }
        }
    }

    class Program
    {
        public static void SearchDictionary(out bool engRus, out bool rusEng)
        {
            string path = "Eng-Rus.txt";
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
                engRus = true;
            else
                engRus = false;
            path = "Rus-Eng.txt";
            fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
                rusEng = true;
            else
                rusEng = false;
        }

        public static void Menu(Dict dict)
        {
            bool engRus, rusEng;
            SearchDictionary(out engRus, out rusEng);
            Console.Clear();
            Console.WriteLine("СЛОВАРИ v1.0");
            Console.WriteLine("\nДля продолжения нажмите любую кнопку...");
            Console.ReadKey();

        BEGIN:
            char choice;
            do
            {
                Console.Clear();
                Console.WriteLine("Выберите словарь для работы:");
                Console.WriteLine("1 - Англо-русский");
                Console.WriteLine("2 - Русско-английский");
                Console.WriteLine("\n3 - Выход из программы");
                choice = Console.ReadKey().KeyChar;
                switch (choice)
                {
                    case '1':
                        if (engRus == false)
                        {
                            char choiceCreate;
                            do
                            {
                                Console.Clear();
                                Console.WriteLine("Словарь не найден.");
                                Console.WriteLine("1 - Создать новый словарь");
                                Console.WriteLine("2 - Вернуться назад");
                                choiceCreate = Console.ReadKey().KeyChar;
                                switch (choiceCreate)
                                {
                                    case '1':
                                        dict = new Dict(Language.English, Language.Russian);
                                        break;
                                    case '2':
                                        goto BEGIN;
                                    default:
                                        continue;
                                }
                            }
                            while (choiceCreate != '1');
                        }
                        else
                        {
                            dict = new Dict(Language.English, Language.Russian);
                        }
                        break;
                    case '2':
                        if (rusEng == false)
                        {
                            char choiceCreate;
                            do
                            {
                                Console.Clear();
                                Console.WriteLine("Словарь не найден.");
                                Console.WriteLine("1 - Создать новый словарь");
                                Console.WriteLine("2 - Вернуться назад");
                                choiceCreate = Console.ReadKey().KeyChar;
                                switch (choiceCreate)
                                {
                                    case '1':
                                        dict = new Dict(Language.Russian, Language.English);
                                        break;
                                    case '2':
                                        goto BEGIN;
                                    default:
                                        continue;
                                }
                            }
                            while (choiceCreate != '1');
                        }
                        else
                        {
                            dict = new Dict(Language.Russian, Language.English);
                        }
                        break;
                    case '3':
                        break;
                    default:
                        continue;
                }

            }
            while (choice != '1' && choice != '2' && choice != '3');

            char choiceMenu;
            do
            {
                if (choice == '3')
                {
                    Console.Clear();
                    break;
                }                    
                Console.Clear();
                Console.WriteLine("1 - Добавить слово и перевод в словарь");
                Console.WriteLine("2 - Изменить слово с уже существующими переводами");
                Console.WriteLine("3 - Изменить один из вариантов перевода слова");
                Console.WriteLine("4 - Удалить слово со всеми переводами");
                Console.WriteLine("5 - Удалить один из переводов слова (последний перевод не удаляется)");
                Console.WriteLine("6 - Показать все переводы слова");
                Console.WriteLine("7 - Экспорт слова и его переводов в файл");
                Console.WriteLine("8 - Сохранение словаря и выход в главное меню");
                choiceMenu = Console.ReadKey().KeyChar;
                switch (choiceMenu)
                {
                    case '1':
                        Console.Clear();
                        Console.WriteLine("Введите слово:");
                        string word = Console.ReadLine();
                        Console.WriteLine("Введите перевод:");
                        string translation = Console.ReadLine();
                        dict.Add(word, translation);
                        break;
                    case '2':
                        Console.Clear();
                        Console.WriteLine("Введите существующее слово:");
                        string oldWord = Console.ReadLine();
                        Console.WriteLine("Введите новое слово:");
                        string newWord = Console.ReadLine();
                        dict.ChangeWord(oldWord, newWord);
                        break;
                    case '3':
                        Console.Clear();
                        Console.WriteLine("Введите существующее слово:");
                        word = Console.ReadLine();
                        Console.WriteLine("Введите старый вариант перевода:");
                        string oldTranslation = Console.ReadLine();
                        Console.WriteLine("Введите новый вариант перевода:");
                        string newTranslation = Console.ReadLine();
                        dict.ChangeTranslation(word, oldTranslation, newTranslation);
                        break;
                    case '4':
                        Console.Clear();
                        Console.WriteLine("Введите удаляемое слово:");
                        word = Console.ReadLine();
                        dict.DeleteWord(word);
                        break;
                    case '5':
                        Console.Clear();
                        Console.WriteLine("Введите существующее слово:");
                        word = Console.ReadLine();
                        Console.WriteLine("Введите удаляемый перевод слова:");
                        string delTranslation = Console.ReadLine();
                        dict.DeleteTranslation(word, delTranslation);
                        break;
                    case '6':
                        Console.Clear();
                        Console.WriteLine("Введите существующее слово:");
                        word = Console.ReadLine();
                        dict.SearchTranslations(word);
                        break;
                    case '7':
                        Console.Clear();
                        Console.WriteLine("Введите существующее слово для экспорта в файл:");
                        word = Console.ReadLine();
                        dict.ExportWordToFile(word);
                        break;
                    case '8':
                        dict.WriteDictionaryToFile();
                        goto BEGIN;
                    default:
                        continue;
                }
            }
            while (true);
        }

        static void Main(string[] args)
        {
            Dict dict = default;
            Menu(dict);
        }
    }
}
