using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris.Controllers
{
    public static class RecordsController
    {
        public static string recordPath = @"C:\Users\sodrk\Desktop\recordsFile.txt";
        public static void SaveRecords(string playerName)
        {
            List<string> recordsArray = new List<string>(LoadRecords());
            bool isNameExistsInRecords = false;
            for (int i = 0; i < recordsArray.Count; i++)
            {
                if (playerName == recordsArray[i].Split('|')[0])
                {
                    isNameExistsInRecords = true;
                    recordsArray.RemoveAt(i);
                    recordsArray.Add(playerName + "|" + MapController.score);
                }
            }
            if (!isNameExistsInRecords)
                recordsArray.Add(playerName + "|" + MapController.score);
            File.WriteAllLines(recordPath, recordsArray);
        }

        public static IEnumerable<string> LoadRecords()
        {
            string[] recordsArray = File.ReadAllLines(recordPath);
            return recordsArray;
        }

        public static void ShowRecords(Label label3)
        {
            List<string> recordsArray = new List<string>(LoadRecords());

            for (int i = 0; i < recordsArray.Count - 1; ++i)//отсортируем рекорды по очкам
            {
                string str = recordsArray[i];
                string strNext = recordsArray[i + 1];
                string[] args = str.Split('|');
                string[] argsNext = strNext.Split('|');

                if (int.Parse(args[1]) < int.Parse(argsNext[1]))
                {
                    var temp = recordsArray[i];
                    recordsArray[i] = recordsArray[i + 1];
                    recordsArray[i + 1] = temp;
                    i = -1;
                }

            }

            label3.Text = "Таблица рекордов";
            foreach (var str in recordsArray)
            {
                string[] args = str.Split('|');

                label3.Text += "\n";
                label3.Text += "Игрок " + args[0] + " : " + args[1];

            }
        }
    }
}
