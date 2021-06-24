using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class Infrastructure
    {
        /// <summary>
        /// функция считывания данных с формы
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static List<Rule> FillList(string[] arr)
        {
            List<Rule> res = new List<Rule>();
            for (int i = 0; i < arr.Length; i++)
            {
                res.Add(new Rule { Name = arr[i].Split(' ').First(), Rules = AddRules(arr[i].Split(' ')) });
            }
            return res;
        }
        /// <summary>
        /// вспомогательная функция добавления правил в массив
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static List<string> AddRules(string[] str)
        {
            List<string> res = new List<string>();
            for (int i = 1; i < str.Length; i++)
            {
                res.Add(str[i]);
            }
            return res;
        }
        public static List<Rule> Normalize(List<Rule> rules)
        {
            rules = DeleteEmptyTerminal(rules);
            if (rules.Count == 0)
                return rules;
            rules = DeleteUnattainable(rules);
            return rules;
        }
        /// <summary>
        /// функция поиска циклов
        /// </summary>
        /// <param name="rules"></param>
        /// <returns></returns>
        public static bool SearchCycles(List<Rule> rules)
        {
            for (int i = 0; i < rules.Count; i++)
            {
                List<string> result = new List<string>() { rules[i].Name };
                string neTerminal = rules[i].Name;
                for (int j = 0; j < rules.Count; j++)
                {
                    List<string> temp = new List<string>();
                    for (int a = 0; a < result.Count; a++)
                    {
                        for (int k = 0; k < result[a].Length; k++)
                        {
                            if (Char.IsUpper(result[a][k]))
                                AddChilder(rules, result[a], k, temp);
                        }
                    }
                    result = temp;
                    if (Check(temp, neTerminal))
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// удаление пусых нетерминалов
        /// </summary>
        /// <param name="rules"></param>
        /// <returns></returns>
        private static List<Rule> DeleteEmptyTerminal(List<Rule> rules)
        {
            List<string> temp = (from x in rules
                                 from y in x.Rules
                                 where y.IsLower()
                                 select x.Name).ToList(); //находим нетерминалы порождающие терминалы
            int count;
            do
            {
                List<string> iter = new List<string>();
                count = temp.Count();
                for (int i = 0; i < rules.Count; i++)//итерируемся по правилам
                {
                    if (temp.Any(x => x == rules[i].Name))//пропускаем правила которые уже есть в множестве N
                        continue;
                    for (int j = 0; j < rules[i].Rules.Count; j++)//итерируемся по кол-ву правил для определенного нетерминала
                    {
                        if (CheckEmpty(rules[i].Rules[j], temp))//если нетерминал выводим только из нетерминалов принадлежащих множеству N или терминалов
                        {
                            iter.Add(rules[i].Name); //то добавляем его в множество N
                            break;
                        }
                    }
                }
                temp = temp.Union(iter).Distinct().ToList();
            } while (count != temp.Count);//итерируемся пока множество N не будет меняться
            bool f = true;
            List<string> temp2 = new List<string>();//массив нетерминалов которые необходимо удалить
            for (int i = 0; i < rules.Count; i++)
            {
                f = false;
                for (int j = 0; j < temp.Count; j++)
                {
                    if (rules[i].Name == temp[j])
                        f = true;
                }
                if (!f)
                    temp2.Add(rules[i].Name);
            }
            rules = (from x in rules
                     from y in temp
                     where x.Name == y
                     select x).ToList();//удаление правил нетерминалов которые нужно удалить
            for (int i = 0; i < rules.Count; i++)
            {
                for (int j = 0; j < rules[i].Rules.Count(); j++)
                {
                    for (int k = 0; k < temp2.Count && j < rules[i].Rules.Count(); k++)
                    {
                        if (rules[i].Rules[j].Contains(temp2[k]))//если правило для нетерминала содержит нетерминал который нужно удалить то удаляем это правило
                        {
                            rules[i].Rules.RemoveAt(j);
                            j--;
                        }
                    }
                }
            }
            return rules;
        }
        /// <summary>
        /// проверка на выводимость 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="temp"></param>
        /// <returns></returns>
        private static bool CheckEmpty(string str, List<string> temp)
        {
            for (int i = 0; i < str.Length; i++)
            {
                bool f = false;
                if (Char.IsLower(str[i]))
                    continue;
                else
                {
                    for (int j = 0; j < temp.Count; j++)
                    {
                        if (str[i] == char.Parse(temp[j]))
                        {
                            f = true;
                        }
                    }
                    if (!f)
                        return false;
                }
            }
            return true;
        }
        /// <summary>
        /// основная функция поиска максимального пути
        /// </summary>
        /// <param name="rules"></param>
        /// <returns></returns>
        public static string FindMaxWord(List<Rule> rules)
        {
            List<string> result = new List<string>() { rules[0].Name };
            List<string> temp3 = (from x in rules
                                  from y in x.Rules
                                  where y.IsLower()
                                  select x.Name).ToList();
            do
            {
                for (int j = 0; j < rules.Count; j++)
                {
                    if (temp3.Contains(rules[j].Name) && rules[j].Rules.All(x=>x.IsLower()))
                        continue;
                    for (int k = 0; k < rules[j].Rules.Count(); k++)
                    {
                        for (int i = 0; i < rules[j].Rules[k].Length; i++)
                        {
                            for (int c = 0; c < temp3.Count(); c++)
                            {
                                if (i < rules[j].Rules[k].Length && rules[j].Rules[k][i] == char.Parse(temp3[c]))
                                {
                                    var tempString = rules[j].Rules[k].Replace(temp3[c], Word(rules, temp3[c]));
                                    rules[j].Rules.RemoveAt(k);
                                    rules[j].Rules.Add(tempString);
                                    if (!temp3.Contains(rules[j].Name) && rules[j].Rules.All(x => x.IsLower()))
                                        temp3.Add(rules[j].Name);
                                }
                            }
                        }

                    }
                }
            } while (rules.Count != temp3.Count);
            return rules.Count == 0 ? "слов нет" : rules[0].Rules.OrderBy(x => x.Length).Last();
        }

        private static string Word(List<Rule> rules, string v)
        {
            var str = (from x in rules
                       from y in x.Rules
                       where x.Name == v && y.IsLower()
                       select y).OrderBy(x => x.Length).Last();
            return str;
        }

        /// <summary>
        /// удаление не достижимых нетерминалов
        /// </summary>
        /// <param name="rules"></param>
        private static List<Rule> DeleteUnattainable(List<Rule> rules)
        {
            List<string> result = new List<string>() { rules[0].Name };
            List<string> visited = new List<string>();
            for (int j = 0; j < rules.Count; j++)
            {
                List<string> temp = new List<string>();
                for (int a = 0; a < result.Count; a++)
                {
                    for (int k = 0; k < result[a].Length; k++)
                    {
                        if (Char.IsUpper(result[a][k]) && !visited.Any(x => x == result[a][k].ToString()))
                        {
                            AddChilder(rules, result[a], k, temp);
                            visited.Add(result[a][k].ToString());
                        }
                    }
                }
                result = temp;
            }
            return (from x in rules
                    from y in visited
                    where x.Name == y
                    select x).ToList();
        }
        /// <summary>
        /// проверка на цикл
        /// </summary>
        /// <param name="temp"></param>
        /// <param name="neTerminal"></param>
        /// <returns></returns>
        private static bool Check(List<string> temp, string neTerminal)
        {
            return temp.Any(x => x.Contains(neTerminal) && x.CountTerminal());
        }
        /// <summary>
        /// вспомогательная функция заменяет нетерминал на все правила
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="word"></param>
        /// <param name="index"></param>
        /// <param name="res"></param>
        private static void AddChilder(List<Rule> rules, string word, int index, List<string> res)
        {
            char letter = word[index];
            word = word.Remove(index, 1);
            var temp = rules.First(x => x.Name == letter.ToString());
            for (int i = 0; i < temp.Rules.Count; i++)
            {
                res.Add(word.Insert(index, temp.Rules[i]));
            }
        }
    }
}
