using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaiyouUtils
{
    /// <summary>
    /// Taiyou Language System
    /// 
    /// == Language Files ==
    /// A Taiyou Language File is a regular text file but specially formatted
    /// In a language file, you have {key};{value} on the same line
    /// You can use '#' on the start of a line to comment it
    /// You can also use '%n' to add a new line into a value
    /// 
    /// == How to set up ==
    /// First, you will need to load a language file
    /// To load a language file, call 'LoadDictData' function with a FilePath as first parameter
    /// The bgWorker parameter can be used for progress report when loading a Language File
    /// 
    /// To get a language key, just call 'Get' with name of key as first parameter
    /// </summary>
    public class Lang
    {
        static Dictionary<string, string> LangData = new Dictionary<string, string>();
        static string CurrentLangFile = "";

        /// <summary>
        /// Get a value from key name
        /// </summary>
        /// <param name="Key">Key name</param>
        /// <returns>Lang Data</returns>
        public static string Get(string Key)
        {
            if (CurrentLangFile == "") { throw new Exception("No language file is currently loaded."); }
            if (!LangData.ContainsKey(Key))
            {
                throw new KeyNotFoundException("Cannot find key '" + Key + "' at currently loaded language.");
            }

            return LangData[Key];
        }

        /// <summary>
        /// Unload all loaded keys
        /// </summary>
        public static void UnloadLangData()
        {
            LangData.Clear();
            CurrentLangFile = "";

        }

        /// <summary>
        /// Load a language file into language system
        /// </summary>
        /// <param name="LangFilePath">Path of language file</param>
        /// <param name="bgWorker">Used for progress report</param>
        public static void LoadDictData(string LangFilePath, BackgroundWorker bgWorker = null)
        {
            // Set current lang file
            CurrentLangFile = LangFilePath;
            
            // Check if language file exists
            if (!File.Exists(Lang.CurrentLangFile))
            {
                Console.WriteLine("Cannot locate language file.");
                Console.WriteLine(Lang.CurrentLangFile);
                throw new FileNotFoundException("Cannot locate language file.");
            }
            
            // Read language file
            string[] FileRead = File.ReadAllLines(CurrentLangFile);
            
            int Wax = -1;
            foreach (string Line in FileRead)
            {
                Wax++;
                // Ignore lines that starts with '#'
                if (Line.StartsWith("#"))
                {
                    continue;
                }

                // Ignore lines that is less than 10 chars
                if (Line.Length < 10) { continue; }

                // Split {key};{value}
                string[] LineSplit = Line.Split(';');
                string Key = "";
                string Data = "";

                // Run though every key and value
                foreach (string LineSplitData in LineSplit)
                {
                    // Set Key
                    if (Key == "")
                    {
                        Key = LineSplitData;
                        continue;
                    }
                    // Set data
                    Data = LineSplitData.Replace("%n", "\n");
                }

                // Handle Duplicate Entry
                try
                {
                    // Add key
                    LangData.Add(Key, Data);

                }
                catch (ArgumentException) { Console.WriteLine("Duplicate entry found, ignoring..."); continue; }
                Console.WriteLine("LangLoader: Added '" + Key + "' with value '" + Data + "'");

                // Report Progress
                if (bgWorker != null)
                {
                    var progress = (int)Math.Ceiling(((float)Wax / FileRead.Length) * 100);
                    bgWorker.ReportProgress(progress);
                }

            }

        }

    }
}
