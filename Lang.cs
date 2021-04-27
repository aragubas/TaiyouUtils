using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaiyouUtils
{
    public class Lang
    {
        static Dictionary<string, string> LangData = new Dictionary<string, string>();
        static string CurrentLangFile = "";

        public static string GetLangData(string Key)
        {
            if (CurrentLangFile == "") { throw new Exception("No language file is currently loaded."); }
            if (!LangData.ContainsKey(Key))
            {
                throw new KeyNotFoundException("Cannot find key '" + Key + "' at currently loaded language.");
            }

            return LangData[Key];
        }

        public static void UnloadLangData()
        {
            LangData.Clear();
            CurrentLangFile = "";

        }

        public static void LoadDictData(string LangFilePath, BackgroundWorker bgWorker = null)
        {
            LangData.Clear();
            CurrentLangFile = LangFilePath;

            //DicData should be located at (./lang_bank/en-us)
            if (!File.Exists(Lang.CurrentLangFile))
            {
                Console.WriteLine("Cannot locate language file.");
                Console.WriteLine(Lang.CurrentLangFile);
                throw new FileNotFoundException("Cannot locate language file.");
            }

            string[] FileRead = File.ReadAllLines(CurrentLangFile);


            int Wax = -1;
            foreach (string Line in FileRead)
            {
                Wax++;
                if (Line.StartsWith("#"))
                {
                    continue;
                }
                if (Line.Length < 10) { continue; }
                string[] LineSplit = Line.Split(';');
                string Key = "";
                string Data = "";

                foreach (string LineSplitData in LineSplit)
                {
                    if (Key == "")
                    {
                        Key = LineSplitData;
                        continue;
                    }
                    Data = LineSplitData.Replace("%n", "\n");
                }


                try
                {
                    LangData.Add(Key, Data);

                }
                catch (ArgumentException) { Console.WriteLine("Duplicate entry found, ignoring..."); continue; }
                Console.WriteLine("LangLoader: Added '" + Key + "' with value '" + Data + "'");

                if (bgWorker != null)
                {
                    var progress = (int)Math.Ceiling(((float)Wax / FileRead.Length) * 100);
                    bgWorker.ReportProgress(progress);
                }

            }

        }

    }
}
