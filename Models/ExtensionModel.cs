using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FindExtension.Helpers;

namespace FindExtension.Models
{
    public class ExtensionModel : NotificationObject
    {
        public ExtensionModel()
        {
            _minsize = int.MaxValue;
        }

        public ExtensionModel(string str)
        {
            _minsize = int.MaxValue;

            var ss = str.Split('\t');
            Ext = ss[0];
            Count = int.Parse(ss[1]);
            Size = int.Parse(ss[2]);
            MaxSize = int.Parse(ss[3]);
            NameMaxSize = ss[4];
            MinSize = int.Parse(ss[5]);
            NameMinSize = ss[6];
        }

        private string _extension;
        public string Ext
        {
            get { return _extension; }
            set
            {
                if (_extension != value)
                {
                    _extension = value;
                    RaisePropertyChanged(() => Ext);
                }
            }
        }

        private long _count;
        public long Count
        {
            get { return _count; }
            set
            {
                if (_count != value)
                {
                    _count = value;
                    RaisePropertyChanged(() => Count);
                }
            }
        }

        private long _size;
        public long Size
        {
            get { return _size; }
            set
            {
                if (_size != value)
                {
                    _size = value;
                    RaisePropertyChanged(() => Size);
                }
            }
        }

        private long _maxsize;
        public long MaxSize
        {
            get { return _maxsize; }
            set
            {
                if (_maxsize != value)
                {
                    _maxsize = value;
                    RaisePropertyChanged(() => MaxSize);
                }
            }
        }

        private string _nameMaxSize;
        public string NameMaxSize
        {
            get { return _nameMaxSize; }
            set
            {
                if (_nameMaxSize != value)
                {
                    _nameMaxSize = value;
                    RaisePropertyChanged(() => NameMaxSize);
                }
            }
        }
        private long _minsize;
        public long MinSize
        {
            get { return _minsize; }
            set
            {
                if (_minsize != value)
                {
                    _minsize = value;
                    RaisePropertyChanged(() => MinSize);
                }
            }
        }

        private string _nameMinSize;
        public string NameMinSize
        {
            get { return _nameMinSize; }
            set
            {
                if (_nameMinSize != value)
                {
                    _nameMinSize = value;
                    RaisePropertyChanged(() => NameMinSize);
                }
            }
        }

        public void AddFile(FoundFile file)
        {
            var len = file.Length;

            if (MaxSize < len)
            {
                MaxSize = len;
                NameMaxSize = file.Path;
            }
            if (MinSize > len)
            {
                MinSize = len;
                NameMinSize = file.Path;
            }
            Size = Size + len;
            Count = Count + 1;
        }

        public override string ToString()
        {
            return Ext + "\t" + Count + "\t" + Size + "\t" + MaxSize + "\t" + NameMaxSize + "\t" + MinSize + "\t" + NameMinSize;
        }
    }
}
