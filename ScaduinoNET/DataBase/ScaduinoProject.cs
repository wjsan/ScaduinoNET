﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;
using ScreenEditor.DataBase;

namespace ScaduinoNET.DataBase
{
    class ScaduinoProject
    {
        public ScaduinoProjectProperties Properties { get; set; }
        public ScaduinoProjectDirectories Directories { get; set; }

        public ScaduinoProject(string name, string path, Size size)
        {
            Properties = new ScaduinoProjectProperties(name, path, size);

            string root = string.Format("{0}\\{1}", path, name);
            Directories = new ScaduinoProjectDirectories(root);
        }

        public void Save()
        {
            Properties.SaveProperties();
        }

        public void Load()
        {
            Properties.LoadProperties(string.Format("{0}\\Properties.cfg", Directories.Root));
        }

    }

    class ScaduinoProjectDirectories
    {
        public string Root { get; set; }
        public string Screens { get; set; }

        public ScaduinoProjectDirectories(string root)
        {
            Root = root;
            Screens = string.Format("{0}\\Screens", root);

            Directory.CreateDirectory(Screens);
        }
    }

    class ScaduinoProjectProperties
    {
        internal string FilePath { get; set; }
        internal string Path { get; set; }

        public string Name { get; set; }
        public Size ScreenSize { get; set; }

        public ScaduinoProjectProperties()
        {

        }

        public ScaduinoProjectProperties(string name, string path, Size size)
        {
            Name = name;
            Path = string.Format("{0}\\{1}", path, name);
            ScreenSize = size;

            FilePath = string.Format("{0}\\Properties.cfg", Path);
            
            SaveProperties();
        }

        public void SaveProperties()
        {
            Directory.CreateDirectory(Path);

            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        public void LoadProperties(string filePath)
        {
            //Directory.CreateDirectory(Path);
            FileInfo fileInfo = new FileInfo(filePath);
            FilePath = filePath;
            Path = fileInfo.DirectoryName;

            string json = File.ReadAllText(FilePath);

            ScaduinoProjectProperties newProperties = JsonConvert.DeserializeObject<ScaduinoProjectProperties>(json);

            Name = newProperties.Name;
            ScreenSize = newProperties.ScreenSize;
        }

        public bool FileChanged()
        {
            string thisJson = JsonConvert.SerializeObject(this, Formatting.Indented);
            string fileJson = File.ReadAllText(FilePath);
            return (thisJson != fileJson);
        }
    }
}