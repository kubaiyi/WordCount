using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WordCount
{
    public class Program1
    {
        public static void Main(string[] args)
        {
            Console.Write("请输入要读取文件的路径: ");
            String path = Console.ReadLine();
            String r = null, content = null;
            StreamReader reader = new StreamReader(path, Encoding.Default);
            try
            {
                while ((r = reader.ReadLine()) != null)
                {
                    content += (r + "\n");
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("读取文件失败");
            }
            finally
            {
                reader.Close();
            }
            if (content == null)
            {
                Console.WriteLine("该文件为空文件");
                return;
            }
            Sum(content); // 统计字符个数
            String[] temp = Words(content);// 统计单词个数，并返回单词的字符串数组
            Lines(content); // 统计行数
            DicSort(temp, path); // 按字典序排序并输出前n个频繁出现的单词,之后将单词全部写入文件
            Phrase(temp);
            Console.ReadKey();
        }

        public static int Sum(String content)
        {
            int sum = 0;
            for (int i = 0; i < content.Length; i++)
            {
                if ((int)content[i] > 127)
                    sum++;
            }
            Console.WriteLine("characters: " + (content.Length - sum - 1));
            return content.Length - sum - 1;
        }

        public static String[] Words(string content)
        {
            String[] temp = new string[content.Length];
            String[] str = new string[content.Length];
            int num = 0;
            content = content.ToLower();
            for (int i = 0; i < content.Length; i++)
            {
                temp[i] = "";
            }
            for (int i = 0; i < content.Length; i++)
            {
                if ((content[i] > 47 && content[i] < 58) || (content[i] > 96 && content[i] < 123))
                    temp[num] += content[i];
                else
                {
                    for (int j = 0; j < temp[num].Length; j++)
                    {
                        if ((temp[num][j] > 47 && temp[num][j] < 58) || temp[num].Length < 4)
                        {
                            temp[num] = "";
                            break;
                        }
                        if (j > 3)
                        {
                            num++;
                            break;
                        }
                    }
                }
            }
            for (int i = 0; i < content.Length; i++)
            {
                str[i] = temp[i];
            }
            for (int i = 0; str[i] != ""; i++)
            {
                for (int j = i + 1; str[j] != ""; j++)
                {
                    if (str[i] == str[j])
                    {
                        num--;
                        str[j] = j.ToString();
                    }
                }
            }
            Console.WriteLine("words: " + num);
            return temp;
        }

        public static int Lines(string content)
        {
            int num = 0;
            for (int i = 0; i < content.Length; i++)
            {
                if (content[i] == '\n')
                {
                    num++;
                }
            }
            Console.WriteLine("lines: " + num);
            return num;
        }

        public static IOrderedEnumerable<KeyValuePair<String, int>> DicSort(string[] temp, string path)
        {
            Console.Write("请输入需要输出单词的个数: ");
            int n = int.Parse(Console.ReadLine());
            Dictionary<string, int> dic = new Dictionary<string, int>();
            String[] str = new string[temp.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                str[i] = "";
                str[i] = temp[i].ToLower();
            }
            for (int i = 0; i < str.Length - 1; i++)
            {
                int t = 1;
                if (str[i] == "")
                    continue;
                for (int j = i + 1; j < str.Length; j++)
                {
                    if (str[i] == str[j])
                    {
                        t++;
                        str[j] = "";
                    }
                }
                dic.Add(str[i], t);
            }
            var dicSort = dic.OrderByDescending(objDic => objDic.Value).ThenBy(objDic => objDic.Key);
            int n1 = 1;
            StreamWriter writer = new StreamWriter(path, true);
            foreach (KeyValuePair<string, int> kvp in dicSort)
            {
                Console.WriteLine("<" + kvp.Key + ">: " + kvp.Value);
                try
                {
                    writer.WriteLine(kvp.Key);
                }
                catch (IOException e)
                {
                    Console.WriteLine("写入文件操作失败");
                }
                if (n1 == n)
                    break;
                n++;
            }
            foreach (KeyValuePair<string, int> kvp in dicSort)
            {
                try
                {
                    writer.WriteLine(kvp.Key);
                }
                catch (IOException e)
                {
                    Console.WriteLine("写入文件操作失败");
                }
            }
            writer.Close();
            return dicSort;
        }

        public static void Phrase(String[] temp)
        {
            Console.Write("请输入需要统计的词组的长度: ");
            int n = int.Parse(Console.ReadLine());
            Dictionary<string, int> dic = new Dictionary<string, int>();
            string[] str = new string[temp.Length];
            for(int i = 0; i < temp.Length; i++)
            {
                str[i] = "";
            }
            for(int i = 0; temp[i] != ""; i++)
            {
                for(int j = i; j < i+n; j++)
                    str[i] += (temp[j] + " ");
                if(temp[i+n] == "")
                    break;
            }
            for (int i = 0; i < str.Length - 1; i++)
            {
                int t = 1;
                if (str[i] == "")
                    continue;
                for (int j = i + 1; j < str.Length; j++)
                {
                    if (str[i] == str[j])
                    {
                        t++;
                        str[j] = "";
                    }
                }
                dic.Add(str[i], t);
            }
            var dicSort = dic.OrderByDescending(objDic => objDic.Value).ThenBy(objDic => objDic.Key);
            foreach(KeyValuePair<string, int> kvp in dicSort)
            {
                Console.WriteLine("<" + kvp.Key + ">: " + kvp.Value);
            }
        }
    }
}
