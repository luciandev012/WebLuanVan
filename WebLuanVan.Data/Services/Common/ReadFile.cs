using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Word = Microsoft.Office.Interop.Word;
namespace WebLuanVan.Data.Services.Common
{
    public class ReadFile
    {
        public int Index { get; set; }
        public ReadFile()
        {
            Index = 0;
        }
        public string GetString(string key, Word.Document myDoc)
        {
            string value = "";
            for (int i = Index; i < myDoc.Paragraphs.Count; i++)
            {
                var text = myDoc.Paragraphs[i + 1].Range.Text.ToString();
                if (text == "\r" || text == "/\r")
                {
                    continue;
                }
                if (text.ToLower().Contains(key))
                {
                    value = text;
                    Index = i;
                    break;
                }
            }
            return value;
        }
        public string GetStringBefore(int ind, Word.Document myDoc)
        {
            string value = "";
            do
            {
                ind--;
                value = myDoc.Paragraphs[ind + 1].Range.Text.ToString();

            } while (value == "\r" || value == "/\r");
            return value;
        }
        public string[] GetRange(string start, string end, Word.Document myDoc)
        {
            string[] ret = new string[] { };
            for (int i = Index; i < myDoc.Paragraphs.Count; i++)
            {
                var text = myDoc.Paragraphs[i + 1].Range.Text.ToString();
                if (text.ToLower().Contains(start))
                {
                    //ret = ret.Concat(new string[] { text }).ToArray();
                    int id = i;
                    do
                    {
                        text = myDoc.Paragraphs[id + 1].Range.Text.ToString();
                        ret = ret.Concat(new string[] { text }).ToArray();
                        id++;
                    } while (!myDoc.Paragraphs[id + 1].Range.Text.ToString().ToLower().Contains(end));
                    Index = id;
                    break;
                }
            }
            return ret;
        }
        public string[] GetArrayByString(string key, Word.Document myDoc)
        {
            string[] ret = new string[] { };
            for (int i = Index; i < myDoc.Paragraphs.Count; i++)
            {
                var text = myDoc.Paragraphs[i + 1].Range.Text.ToString();
                if (text.ToLower().Contains(key))
                {
                    int id = i;
                    do
                    {
                        text = myDoc.Paragraphs[id + 1].Range.Text.ToString();
                        ret = ret.Concat(new string[] { text }).ToArray();
                        id++;
                    } while (myDoc.Paragraphs[id + 1].Range.Text.ToString().ToLower().Contains(key));
                    Index = id;
                    break;
                }

            }
            return ret;
        }

        public string GetStringAfter(string[] key, Word.Document myDoc)
        {
            var ret = "";
            for (int i = Index; i < myDoc.Paragraphs.Count; i++)
            {
                var text = myDoc.Paragraphs[i].Range.Text.ToString();
                if (text.ToLower().Contains("và"))
                {
                    continue;
                }
                if (text.ToLower().Contains(key[0]) || text.ToLower().Contains(key[1]))
                {
                    int id = i;


                    text = myDoc.Paragraphs[id + 1].Range.Text.ToString();
                    ret = text;
                    id++;

                    Index = id;
                    break;
                }

            }
            return ret;
        }
        public string[] GetChapter(Word.Document myDoc)
        {
            string[] ret = new string[] { };
            for (int i = Index; i < myDoc.Paragraphs.Count; i++)
            {
                var text = myDoc.Paragraphs[i + 1].Range.Text.ToString();
                if (text.ToLower().Contains("chương"))
                {

                    ret = ret.Concat(new string[] { text }).ToArray();
                    Index = i;
                }
                if (text.ToLower().Contains("tài liệu tham khảo"))
                {
                    break;
                }
            }
            return ret;
        }
        public string[] GetReferDoc(Word.Document myDoc)
        {
            string[] ret = new string[] { };
            string text = "";
            do
            {
                text = myDoc.Paragraphs[Index].Range.Text.ToLower().ToString();
                Index++;
            }
            while (Index < myDoc.Paragraphs.Count && !text.Contains("tài liệu tham khảo"));
            while (Index < myDoc.Paragraphs.Count && (!text.Contains("phụ lục") && !text.Contains(@"\")))
            {

                text = myDoc.Paragraphs[Index].Range.Text.ToLower().ToString();
                ret = ret.Concat(new string[] { text }).ToArray();
                Index++;
            }
            return ret;
        }
        public string[] GetTable(Word.Document myDoc)
        {
            string[] ret = new string[] { };
            string text = "";
            for (int i = Index; i < myDoc.Paragraphs.Count; i++)
            {
                text = myDoc.Paragraphs[i].Range.Text.ToLower().ToString();
                if (text.Contains("mục lục"))
                {
                    int id = i;
                    do
                    {
                        text = myDoc.Paragraphs[++id].Range.Text.ToLower().ToString();
                        ret = ret.Concat(new string[] { text }).ToArray();
                    } while (id < i + 40);
                    Index = id;
                    break;
                }
            }
            return ret;
        }
        public string GetSummay(string[] key, Word.Document myDoc)
        {
            var ret = "";
            for (int i = Index; i < myDoc.Paragraphs.Count; i++)
            {
                var text = myDoc.Paragraphs[i].Range.Text.ToString();

                if (text.ToLower().Contains(key[0]) || text.ToLower().Contains(key[1]))
                {
                    int id = i;

                    do
                    {
                        id++;
                        text = myDoc.Paragraphs[id].Range.Text.ToString();
                        ret += text;

                    } while (!myDoc.Paragraphs[id + 1].Range.Text.ToLower().Contains("mục lục"));


                    Index = id--;
                    break;
                }

            }
            return ret;
        }
    }
}
