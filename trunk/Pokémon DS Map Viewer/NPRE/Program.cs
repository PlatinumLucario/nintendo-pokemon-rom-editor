namespace PG4Map
{
    using PG4Map.Formats;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;
    using Tao.OpenGl;

    public class Program
    {

        public static DirectoryInfo _textureDirectory;
        public static readonly Dictionary<string, IEnumerable<string>> _textureFileMaterials = new Dictionary<string, IEnumerable<string>>();
        public static string _usedTextureFile;
        public static int Actual_Map;
        public static Nsbtx actualTex;
        public static string Tex_File;
        public static FileInfo usedTextureFile;


        [STAThread]
        public static void Main(string[] args)
        {
            _textureDirectory = new DirectoryInfo(string.Format(@"{0}\{1}", Environment.CurrentDirectory, "Textures"));
            if (!_textureDirectory.Exists)
            {
                Directory.CreateDirectory(_textureDirectory.FullName);
            }
            GenerateMaterialListsForTextureFiles(_textureDirectory);
            var mainForm = new PG4Map.Main();
            Application.Run(mainForm);
        }

        public static bool DoesTextureFileContainAllTextures(string key, IEnumerable<string> modelUsedMaterialList)
        {
            Stack<string> stack = new Stack<string>(modelUsedMaterialList);
            while (stack.Count > 0)
            {
                string str = stack.Pop().Split(new char[1])[0];
                if (!_textureFileMaterials[key].Contains<string>(str))
                {
                    return false;
                }
            }
            return true;
        }

        public static IEnumerable<string> GenerateMaterialListForNsbtx(FileInfo fileInfo)
        {
            Tex_File = fileInfo.Name;
            List<NsbmdModel.MatTexPalStruct> list = actualTex.LoadBTX0(fileInfo);
            List<string> list2 = new List<string>();
            foreach (NsbmdModel.MatTexPalStruct _c in list)
            {
                list2.Add(_c.texName);
            }
            list2.Sort();
            return list2;
        }

        public static void GenerateMaterialListsForTextureFiles(DirectoryInfo textureDirectory)
        {
            FileInfo mappingFile = new FileInfo(textureDirectory.FullName + @"\materials.txt");
            if (mappingFile.Exists)
            {
                LoadTextureFilesMaterialListsFromFile(mappingFile);
            }
            else
            {
                MessageBox.Show("Textures folder does not exist. Creating directory...");
                FileInfo[] files = textureDirectory.GetFiles();
                foreach (FileInfo info2 in files)
                {
                    if (info2.Extension.ToLowerInvariant().EndsWith("btx"))
                    {
                        LoadMaterialListOfTextureFile(info2);
                    }
                }
                WriteMappingFile(mappingFile);
            }
        }

        public static string GetTextureFileToUseForModel(IEnumerable<string> modelUsedMaterialList, int mapType)
        {
            foreach (KeyValuePair<string, IEnumerable<string>> pair in _textureFileMaterials)
            {
                if (DoesTextureFileContainAllTextures(pair.Key, modelUsedMaterialList))
                    if (NameRightRom(pair.Key, mapType))
                        return pair.Key;
            }
            return string.Empty;
        }

        private static bool NameRightRom(string p, int mapType)
        {
            string romExtension = p.Split('_')[0];
            if (mapType == 4)
            {
                if (romExtension == "Pl")
                    return true;
            }
            else if (mapType != 4)
                return true;
            return false;
        }

        public static void LoadMaterialListOfTextureFile(FileInfo texFile)
        {
            _textureFileMaterials[texFile.Name] = GenerateMaterialListForNsbtx(texFile);
            Console.WriteLine(_textureFileMaterials[texFile.Name]);
        }

        public static void LoadTextureFilesMaterialListsFromFile(FileInfo mappingFile)
        {
            StreamReader reader = new StreamReader(mappingFile.FullName);
            string str = null;
            do
            {
                str = null;
                try
                {
                    str = reader.ReadLine();
                }
                catch
                {
                }
                if (str == null)
                {
                    break;
                }
                if (str.Contains("=") || str.Contains(";"))
                {
                    string[] strArray = str.Split(new char[] { '=' });
                    string str2 = strArray[0];
                    Tex_File = str2;
                    string[] strArray2 = strArray[1].Split(new char[] { ';' });
                    _textureFileMaterials[str2] = strArray2;
                }
            }
            while (str != null);
        }



        public static void OnLoadMapClicked()
        {
            ShowOpenMapDialog();
        }

        public static bool ShowOpenMapDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.Cancel)
            {
                return false;
            }
            FileInfo file = new FileInfo(dialog.FileName);
            MapEditor.file = dialog.OpenFile();
            //LoadMap(file, 0, 1, dialog.OpenFile());
            //_glForm.Text = file.Name;
            return true;
        }

        public static void WriteMappingFile(FileInfo mappingFile)
        {
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, IEnumerable<string>> pair in _textureFileMaterials)
            {
                builder.AppendFormat("{0}=", pair.Key);
                foreach (string str in pair.Value)
                {
                    builder.AppendFormat("{0};", str);
                }
                builder.AppendLine();
            }
            using (StreamWriter writer = new StreamWriter(mappingFile.FullName))
            {
                writer.Write(builder.ToString());
                writer.Flush();
                writer.Close();
            }
        }
    }
}

