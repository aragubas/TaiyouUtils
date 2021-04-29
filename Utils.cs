using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IWshRuntimeLibrary;

namespace TaiyouUtils
{
    /// <summary>
    /// General Utilities
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Create a shortcut on desktop
        /// 
        /// taken from:
        /// https://stackoverflow.com/a/4909475/13160275
        /// </summary>
        /// <param name="ExecutablePath">Path of Executable</param>
        /// <param name="WorkingDirectory">Working Directory of executable</param>
        /// <param name="Description">Shortcut description</param>
        /// <param name="ShortcutName">Title of Shortcut</param>
        /// <param name="IconPath">Icon path of executable</param>
        public static void CreateDesktopShortcut(string ExecutablePath, string WorkingDirectory, string Description, string ShortcutName, string IconPath)
        {
            object shDesktop = (object)"Desktop";
            WshShell shell = new WshShell();
            string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + "\\" + ShortcutName + ".lnk";
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.Description = Description;
            shortcut.TargetPath = ExecutablePath;
            shortcut.IconLocation = IconPath;
            shortcut.WorkingDirectory = WorkingDirectory;
            shortcut.Save();
        }
    }
}
