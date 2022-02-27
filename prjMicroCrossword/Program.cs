using System;
using System.Linq;

namespace prjCrossword
{
    class Program
    {
        static void Main(string[] args)
        {
            //================================================================================
            //ДАНО
            string strVertWord = "ЛОПУХ";
            //string strVertWord = "ПОЛУХА";

            //string[] strHorWords = { "СМЕХ", "ЛЮЩ", "ПОТОК", "ЛЕЛУШ", "БУЛКО", "БУПХ" };
            //string[] strHorWords = { "БУЛКА", "ПОТОК", "ПЛЮЩ", "БУМ", "СМЕХ" };
            //string[] strHorWords = { "СМЕХ", "ЛЮЩ", "ПОТОК", "БУЛКО", "БУПХ" };
            string[] strHorWords = { "ПОТОК", "БУЛКО", "БУМ","ПЛЮЩУ", "СМЕХ" };

            //================================================================================

            string[] strUsedWords = ArrangeWords(strVertWord, strHorWords);


            //Вычисление отступа для всех слов
            int maxPosition = 0;
            for (int i = 0; i < strUsedWords.Length; i++)
            {
                //Для каждого слова вычисляется позиция соответствующей ему буквы вертикального слова
                int lastPosition = strUsedWords[i].IndexOf(strVertWord[i]);
                if (lastPosition > maxPosition)
                {
                    maxPosition = lastPosition;
                }
            }


            //Вывод
            Console.ForegroundColor = ConsoleColor.Yellow;
            for (int i = 0; i < strUsedWords.Length; i++)
            {
                //Добавление пропуска перед словом
                string strSpace = new string(' ', maxPosition - strUsedWords[i].IndexOf(strVertWord[i]));
                Console.Write(strSpace);

                bool flag = true; //Нужно, чтобы одна буква не выделялась в слове цветом дважды
                foreach (char letter in strUsedWords[i])
                {

                    if (letter == strVertWord[i] && flag)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(letter);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        flag = false;
                    }
                    else
                    {
                        Console.Write(letter);
                    }
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.White;

        }

        static string[] ArrangeWords(string strVertWord, string[] strHorWords)
        {
            //Подпрограмма выбирает слова, подходящие для подстановки под ту или иную букву вертикального слова. Сначала берутся
            //те слова, которые содержат только одну букву из слова. После того, как слово выбирается под определённую букву, для 
            //всех других слов, содержащих ту же букву, становится на один доступный вариант меньше. Пример:
            //    Для буквы "У" в слове "ЛОПУХ" подходят "БУМ" и "БУЛКА". Выбирается первое слово, так как в нем только одна буква, 
            //    содержащаяся в слове "ЛОПУХ". Так как на букву "У" уже было выбрано слово, в слове "БУЛКА" остается только одна буква,
            //    содержащаяся в слове "ЛОПУХ", а именно "Л" и т.д.

            string[] strUsedWords = new string[strVertWord.Length]; //Массив с числом элементов, равным длине вертикального слова

            bool flag = false;
            bool running = true;

            while (running)
            {
                flag = false; //Переменная, показывающая, убрал ли цикл в этой итерации слово
                int firstMetInd = 0; //Если слово единственное, содержит букву, индекс слова запишется сюда
                int count;

                //Подбор слов под буквы
                for (int i = 0; i < strHorWords.Length; i++)
                {
                    count = 0;
                    firstMetInd = 0;
                    for (int j = 0; j < strVertWord.Length; j++)
                    {
                        char letterVert = strVertWord[j];
                        if (strHorWords[i].Contains(letterVert))
                        {
                            count++;
                            firstMetInd = j;
                        }
                    }
                    if (count == 1 && strUsedWords[firstMetInd] == null)
                    {
                        strUsedWords[firstMetInd] = strHorWords[i];
                        strHorWords[i] = "";
                        flag = true;
                    }
                }


                //Подбор слов по количеству слов на букву
                for (int i = 0; i < strVertWord.Length; i++)
                {
                    char letter = strVertWord[i];
                    count = 0;
                    for (int j = 0; j < strHorWords.Length; j++)
                    {
                        if (strHorWords[j].Contains(letter))
                        {
                            count++; //Сколько слов из списка strHorWords подходит для той или иной буквы
                            firstMetInd = j;
                        }
                    }

                    if (count == 1 && strUsedWords[strVertWord.IndexOf(letter)] == null)
                    {
                        //Если для буквы возможно только одно слово, оно ищется убирается из исходного массива и добавляется в итоговый массив
                        strUsedWords[strVertWord.IndexOf(letter)] = strHorWords[firstMetInd];
                        strHorWords[firstMetInd] = "";
                        flag = true;
                    }
                    else if (count == 0)
                    {
                        running = false; //Для буквы вертикального слова не осталось возможных слов
                    }
                }


                //Программа зашла в тупик (Не осталось слов, в которых была бы только одна подходящая буква)
                if (!flag)
                {
                    for (int i = 0; i < strUsedWords.Length; i++)
                    {
                        if (strUsedWords[i] == null)
                        {
                            running = true;
                            for (int j = 0; j < strHorWords.Length; j++)
                            {
                                if (strHorWords[j] != "" && strHorWords[j].Contains(strVertWord[i]))
                                {
                                    strUsedWords[i] = strHorWords[j];
                                    strHorWords[j] = "";
                                    flag = true;
                                    break;
                                }
                            }
                        }
                        if (flag) break;
                    }
                }

                //Проверка, полностью ли запонен массив strUsedWords
                if (strUsedWords.Contains(null))
                {
                    running = true;
                }

            }

            return strUsedWords;
        }

        static int GetLetterPosition(string strWord, char letter)
        {
            //Функция вычисляет позицию в слове, на которой стоит буква
            for (int i = 0; i < strWord.Length; i++)
            {
                char currLetter = strWord[i];
                if (currLetter == letter)
                {
                    return i;
                }
            }

            return 0;
        }
    }
}
