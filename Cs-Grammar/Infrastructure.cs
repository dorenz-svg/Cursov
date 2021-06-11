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
        /// <summary>
        /// функция поиска циклов
        /// </summary>
        /// <param name="rules"></param>
        /// <returns></returns>
        public static bool SearchCycles(List<Rule> rules)
        {
            DeleteUnattainable(rules);
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
        /// основная функция поиска максимального пути
        /// </summary>
        /// <param name="rules"></param>
        /// <returns></returns>
        public static string FindMaxWord(List<Rule> rules)
        {
            List<string> result = new List<string>() { rules[0].Name };
            List<string> words = new List<string>();
            while (!result.All(x=>x.IsLower())) { 

                List<string> temp = new List<string>();
                for (int j = 0; j < result.Count; j++)
                {
                    for (int k = 0; k < result[j].Length; k++)
                    {
                        if (Char.IsUpper(result[j][k]))
                            AddChilder(rules, result[j], k, temp);
                    }
                }
                result = temp;
                foreach (var item in result)
                {
                    if (item.IsLower())
                        words.Add(item);
                }
            }
            var temp1 = words.Distinct().Where(x => x.IsNotEpsilon()).Select(x => x = "ε").ToList();
            var temp2 = words.Distinct().Select(x => x.Replace("ε", "")).ToList();
            words = temp1.Union(temp2).ToList();
            return words.Count==0?"слов нет": words.OrderBy(x=>x.Length).Last();
        }
        /// <summary>
        /// удаление не достижимых нетерминалов
        /// </summary>
        /// <param name="rules"></param>
        private static void DeleteUnattainable(List<Rule> rules)
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
                        if (Char.IsUpper(result[a][k]) && !visited.Any(x=>x== result[a][k].ToString()))
                        {
                            AddChilder(rules, result[a], k, temp);
                            visited.Add(result[a][k].ToString());
                        }
                    }
                }
                result = temp;             
            }
            rules = (from x in rules
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
        private static bool Check(List<string>temp,string neTerminal)
        {
            return temp.Any(x=>x.Contains(neTerminal) && x.CountTerminal());
        }
        /// <summary>
        /// вспомогательная функция заменяет нетерминао на все правила
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="word"></param>
        /// <param name="index"></param>
        /// <param name="res"></param>
        private static void AddChilder(List<Rule> rules, string word,int index,List<string> res)
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
