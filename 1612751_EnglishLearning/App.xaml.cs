﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Collections;
using System.Diagnostics;
using System.IO;

namespace _1612751_EnglishLearning
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
      
        public IniFile settingFile;

      public  class CollectionTopic
        {
            public static String NUMBER_COLLECTION = "Number_Collection";
            public static String PATH ="Path";
            public static String NAME ="Name";
            public static String QUESTION_NUMBER ="Question_Number";
            public static String COLLECTION = "Collection";
            public static String HIGH_SCORE ="Hight_Score";
            public String Path { get; set; }
            public String Name { get; set; }
            public int HightScore { get; set; }
            public int QuestionNumber { get; set; }
            public CollectionTopic(String path, String name, String hightScore,String questionNumber )
            {
                Path = path;
                Name = name;
                HightScore = int.Parse(hightScore);
                QuestionNumber = int.Parse(questionNumber);
            }
        }
       public List<CollectionTopic> topics = new List<CollectionTopic>();
        private void AutoGeneratedBaseCollection()
        {
            settingFile = new IniFile("setting.ini");
            String dir = System.AppDomain.CurrentDomain.BaseDirectory;
            if ((!settingFile.KeyExists(CollectionTopic.NAME,"Collection1"))&&Directory.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "Animal"))
            {
                
                String collection1 = CollectionTopic.COLLECTION + "1";
                settingFile.Write(CollectionTopic.NUMBER_COLLECTION,"1");
                settingFile.Write(CollectionTopic.PATH, dir+"Animal", collection1);
                settingFile.Write(CollectionTopic.NAME, "Animal", collection1);
                settingFile.Write(CollectionTopic.HIGH_SCORE, "0", collection1);
                DirectoryInfo dirInfo = new DirectoryInfo(dir+"Animal");
                FileInfo[] info = dirInfo.GetFiles("*.*", SearchOption.AllDirectories);
                settingFile.Write(CollectionTopic.QUESTION_NUMBER,info.Length+"",collection1);
            }
        
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AutoGeneratedBaseCollection();
            GetAllTopicFromSetting();
        }
        private void GetAllTopicFromSetting()
        {
            string n = settingFile.Read(CollectionTopic.NUMBER_COLLECTION);
            int size = int.Parse(n);
            for(int i=1;i<=size;i++)
            {
                topics.Add(new CollectionTopic(
                    settingFile.Read(CollectionTopic.PATH, CollectionTopic.COLLECTION + i),
                    settingFile.Read(CollectionTopic.NAME, CollectionTopic.COLLECTION + i),
                    settingFile.Read(CollectionTopic.HIGH_SCORE, CollectionTopic.COLLECTION + i),
                    settingFile.Read(CollectionTopic.QUESTION_NUMBER, CollectionTopic.COLLECTION + i)

                    ));
            }
        }
        public bool AddAnTopicFolder(String folder)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(folder);
                
                String collection = CollectionTopic.COLLECTION + (topics.Count+1);

                FileInfo[] info = dir.GetFiles("*.*", SearchOption.AllDirectories);
                if (info.Length == 0) return false;
                settingFile.Write(CollectionTopic.NUMBER_COLLECTION, topics.Count+1+"");
                settingFile.Write(CollectionTopic.PATH, folder, collection);
                settingFile.Write(CollectionTopic.NAME, dir.Name, collection);
                settingFile.Write(CollectionTopic.HIGH_SCORE, "0", collection);
                settingFile.Write(CollectionTopic.QUESTION_NUMBER, info.Length + "", collection);

            }
            catch (Exception)
            {
                return false;

            }
            GetAllTopicFromSetting();
            return true;
        }
    }
}
