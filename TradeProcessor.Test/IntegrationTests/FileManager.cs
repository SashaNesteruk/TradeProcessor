using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeProcessor.Test.IntegrationTests
{
    public  class FileManager
    {
        public void MoveFiles(string sourcePath, string destPath) {
            if (Directory.Exists(sourcePath))
            {
                foreach (var file in new DirectoryInfo(sourcePath).GetFiles())
                {
                    if (!Directory.Exists(destPath))
                    {
                        Directory.CreateDirectory(destPath);
                    }

                    if (File.Exists(destPath + "/" + file.Name))
                    {
                        File.Delete(destPath + "/" + file.Name);
                    }
                    File.Move(sourcePath + "/" + file.Name, destPath + "/" + file.Name);
                }
            }
        }
    }
}
