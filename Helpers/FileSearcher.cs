using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FindExtension.Helpers
{
    class FileSearcher
    {
        string path;
        Action<FoundFile> onFoundFileFunction;

        public FileSearcher(string path, Action<FoundFile> onFoundFileFunction)
        {
            this.path = path;
            this.onFoundFileFunction = onFoundFileFunction;
        }

        public void Start()
        {
            var dirStack = new Stack<string>();
            var currentProgressStack = new Stack<decimal>();
            dirStack.Push(path);
            currentProgressStack.Push(1);
            
            while(dirStack.Any())
            {
                var currentDir = dirStack.Pop();

                IList<string> subDirs;
                IList<string> files;

                try
                {
                    subDirs = Directory.EnumerateDirectories(currentDir).ToList();
                    files = Directory.EnumerateFiles(currentDir).ToList();
                }
                catch (Exception)
                {
                    subDirs = new List<string>();
                    files = new List<string>();
                }

                var val = currentProgressStack.Peek() / Math.Max(1, subDirs.Count + files.Count);
                currentProgressStack.Pop();
                foreach (var subDir in subDirs)
                {
                    currentProgressStack.Push(val);
                }


                foreach (var file in files)
                {
                    onFoundFileFunction(new FoundFile { Path = file, ProgressSize = val, Length = new FileInfo(file).Length });
                }

                if (files.Count == 0)
                {
                    onFoundFileFunction(new FoundFile { Path = null, ProgressSize = val });
                }

                foreach (var dir in subDirs)
                {
                    dirStack.Push(dir);
                }
            }

        }
    }

    public class FoundFile
	{
		public string Path {get;set;}
        public decimal ProgressSize { get; set; }
        public long Length { get; set; }
	}
}
