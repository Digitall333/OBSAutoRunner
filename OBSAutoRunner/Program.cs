using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OBSAutoRunner
{
    class Program
    {
        public static Settings settings;
        static void Main(string[] args)
        {
            settings = ParseSettings("settings.json");

            Console.WriteLine("Write a command:");
            Console.WriteLine("/start NICK - open obs and start stream, where NICK is twitch nickname");
            Console.WriteLine("/stop - stop current stream and close obs");

            while (true)
            {
                string command = Console.ReadLine();
                if(command.Contains("/start"))
                {
                    string[] data = command.Split(' ');
                    if(ChangeProfile(data[1]))
                    {
                        StartOBS();
                        Console.WriteLine($"{data[1]} stream started");
                    }
                    else
                    {
                        Console.WriteLine($"{data[1]} profile hasn't been found");
                    }
                }
                else if(command.Contains("/stop"))
                {
                    StopOBS();
                    Console.WriteLine("Stream was stopped");
                }
            }
        }

        public static Settings ParseSettings(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                string text = sr.ReadToEnd();
                Settings setting = JsonConvert.DeserializeObject<Settings>(text);
                return setting;
            }
        }
        public static bool ChangeProfile(string profileName)
        {
            Profile profile = settings.profiles.ToList().Find(x => x.name.Equals(profileName));
            if (profile == null)
                return false;

            return ChangeProfile(profile);
        }
        public static bool ChangeProfile(Profile profile)
        {
            ChangeStreamKey(profile.streamKey);
            ChangeStreamVideo(profile.videoPath);

            return true;
        }
        public static void ChangeStreamKey(string streamKey)
        {
            string[] paths = Directory.GetDirectories($@"{settings.corePath}\profiles");
            dynamic stuff = null;

            using (StreamReader sr = new StreamReader($@"{paths[0]}\service.json"))
            {
                string text = sr.ReadToEnd();
                stuff = JObject.Parse(text);
                stuff.settings.key = streamKey;
            }

            using (StreamWriter sw = new StreamWriter($@"{paths[0]}\service.json"))
            {
                string text = JsonConvert.SerializeObject(stuff);
                sw.Write(text);
            }
        }
        public static void ChangeStreamVideo(string videoPath)
        {
            string file = Directory.GetFiles(settings.corePath + @"\scenes", "*.json").First();
            dynamic stuff = null;

            using (StreamReader sr = new StreamReader(file))
            {
                string text = sr.ReadToEnd();
                stuff = JObject.Parse(text);
            }

            stuff.sources[1].settings.local_file = videoPath;

            using (StreamWriter sw = new StreamWriter(file))
            {
                sw.Write(stuff);
            }
        }
        public static void StartOBS()
        {
            using (StreamWriter sw = new StreamWriter("obsstartup.bat"))
            {
                sw.Write(settings.startScript);
            }

            var proc = Process.Start(@"cmd.exe ", @"/c obsstartup.bat");
        }
        public static void StopOBS()
        {
            using (StreamWriter sw = new StreamWriter("obsclose.bat"))
            {
                sw.Write(settings.closeScript);
            }

            var proc = Process.Start(@"cmd.exe ", @"/c obsclose.bat");
        }
    }
}
