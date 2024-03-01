using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ
{
    public class AssetUtil
    {
        public static void GetFilesFromDir(List<string> files, string path)
        {
            if (files.Count == 0)
            {
                using (DirAccess dir = DirAccess.Open(path))
                {
                    dir.ListDirBegin();
                    string filename;
                    while (!string.IsNullOrEmpty(filename = dir.GetNext()))
                    {
                        filename = filename.Replace("png.import", "png");
                        string file = System.IO.Path.Combine(dir.GetCurrentDir(), filename);
                        files.Add(file);
                        filename = dir.GetNext();
                    }

                    dir.ListDirEnd();
                }
            }
        }
    }
}
