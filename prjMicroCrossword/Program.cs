using System;
using System.Linq;

namespace prjCrossword //Совместно с Я. Суреном и И. Александром
{
    class Program
    {
        static void Main(string[] args)
        {
            //================================================================================
            //ДАНО
            string strVertWord = "ЛОПУХ";
            //string strVertWord = "ПОЛУХА";

            string[] strHorWords = { "БУЛКА", "ПОТОК", "ПЛЮЩ", "БУМ", "СМЕХ" };
            //string[] strHorWords = { "СМЕХ", "ЛЮЩ", "ПОТОК", "ЛЕЛУШ", "БУЛКО", "БУПХ" };
            //string[] strHorWords = { "СМЕХ", "ЛЮЩ", "ПОТОК", "БУЛКО", "БУПХ" };
            //string[] strHorWords = { "ПОТОК", "БУЛКО", "БУМ","ПЛЮЩУ", "СМЕХ" };

            //================================================================================

            string[] strUsedWords = ArrangeWords(strVertWord, strHorWords);


            //Вычисление отступа для выравнивания по горизонтальному слову
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

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //ВЫВОД В КОНСОЛЬ
            for (int i = 0; i < strUsedWords.Length; i++)
            {
                //Добавление пропуска перед словом
                string strSpace = new string(' ', (maxPosition - strUsedWords[i].IndexOf(strVertWord[i]))*2);
                Console.Write(strSpace);

                bool flag = true; //Нужно, чтобы одна буква не выделялась в слове цветом дважды
                foreach (char letter in strUsedWords[i])
                {

                    if (flag && letter == strVertWord[i])
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.Write(letter);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;
                        flag = false;
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write(letter + " ");
                    }
                }
                Console.WriteLine();
            }
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        }

        static string[] ArrangeWords(string strVertWord, string[] strHorWords)
        {
            //  Подпрограмма выбирает слова, подходящие для подстановки под ту или иную букву вертикального слова.
            //(1*) Сначала берутся те слова, которые содержат только одну букву из слова. После того, как слово выбирается
            //под определённую букву, для всех других слов, содержащих ту же букву, становится на один доступный вариант меньше. 
            //  Пример:
            //    Для буквы "У" в слове "ЛОПУХ" подходят "БУМ" и "БУЛКА". Выбирается первое слово, так как в нем только одна буква, 
            //    содержащаяся в слове "ЛОПУХ". Так как на букву "У" уже было выбрано слово, в слове "БУЛКА" остается только одна буква,
            //    содержащаяся в слове "ЛОПУХ", а именно "Л" и т.д.
            //
            //(2*) Дальше подпрограмма смотрит на оставшиеся позиции в вертикальном слове. Для каждой оставшейся буквы считается число
            // слов, подходящих на позицию этой буквы. Если такое слово только одно, оно добавляется в массив.
            //
            //(3*) Если подпрограмма не вошла ни в (1*) ни в (2*), bool flag останется false и войдёт в (3*). Такое, скорее всего
            //случится, если останутся слова, в которых будут одинаковые буквы. Чтобы выйти из тупика, достаточно выбрать любое из них
            //программа просто ищет оставшиеся слова, которые можно поставить на оставшиеся позиции, после чего цикл while повторится
            //
            //(4*) Наконец, подпрограмма проверяет, заполнен ли массив strUsedWords, чтобы не Оставалось пропусков

            string[] strUsedWords = new string[strVertWord.Length]; //Массив с числом элементов, равным длине вертикального слова

            bool flag;
            bool running = true;

            while (running)
            {
                flag = false; //Переменная, показывающая, убрал ли цикл в этой итерации слово
                int firstMetInd = 0; //Если слово единственное, содержит букву, индекс слова запишется сюда
                int count;

                //(1*) Подбор слов по буквам (Сколько букв гориз. слова совпадают с буквой вертикального слова)
                for (int i = 0; i < strHorWords.Length; i++)
                {
                    count = 0;
                    firstMetInd = 0;
                    for (int j = 0; j < strVertWord.Length; j++)
                    {
                        char letterVert = strVertWord[j];
                        if (strHorWords[i].Contains(letterVert))
                        {
                            //Программа считает, сколько букв из вертикального слова содержит горизонтальное слово.
                            count++;
                            firstMetInd = j;
                        }
                    }
                    //Если горизонтальное слово содержит несколько букв из вертикального слова, оно пропускается.
                    //Если горизонтальное слово содержит только одну букву из вертикального слова, оно встаёт в массив
                    //на нужную позицию
                    if (count == 1 && strUsedWords[firstMetInd] == null)
                    {
                        strUsedWords[firstMetInd] = strHorWords[i];
                        strHorWords[i] = "";
                        flag = true;
                    }
                }


                //(2*) Подбор слов по их количеству (Сколько гориз. слов подходит к каждой букве верт. слова)
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


                //(3*) Программа зашла в тупик (Не осталось слов, в которых была бы только одна подходящая буква)
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

                //(4*) Проверка, полностью ли запонен массив strUsedWords
                if (strUsedWords.Contains(null))
                {
                    running = true;
                }

            }

            return strUsedWords;
        }
    }
}
