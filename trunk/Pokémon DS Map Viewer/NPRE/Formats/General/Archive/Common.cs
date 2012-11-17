using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NPRE.Archive
{
	public class Common
	{
        public struct Folder_s
        {
            public string Name;
            public uint ID;
            public ushort Parent_ID;
            public List<FileInfo_s> Files;
            public List<Folder_s> Folders;
        }

        public struct FileInfo_s
        {
            public string Name;
            public uint ID;
            public ushort Parent_ID;
            public uint Offset;
            public uint Size;
            public ClosableMemoryStream FileData;
        }
	}
}
