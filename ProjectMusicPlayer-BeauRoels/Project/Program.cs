using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMPLib;

namespace ConsoleMusicPlayerBR
{
    class Program

    {
        //controls
        public const string PAUSE = "pauze";
        public const string ADD = "toevoegen";
        public const string STOP = "stop";
        public const string MUTE = "mute";
        public const string UNMUTE = "unmute";
        public const string PLAY = "play";
        public const string NEXT = "next";
        public const string PREVIOUS = "previous";
        public const string BEWERK = "bewerk";
        public const string VERWIJDER = "verwijder";
        public const string VOEGTOE = "voegtoe";
        public const string TERUG = "terug";
        public const string VOLUME = "volume";
        public const string CONTINUE = "continue";
        public const string REQUIREMENT = ".mp3";
        //Windows media player
        public static WindowsMediaPlayer player = new WindowsMediaPlayer();
        public static string docsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string destination = System.IO.Path.Combine(docsFolder, "playlist.txt ");
        public static string musicLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        //inputs
        public static string input;
        public static string song;
        public static int sizeOfList;
        //lists and arrays
        public static string[] linesArray;
        public static List<string> songList = new List<string>();
        public static List<string> songPathList = new List<string>();
        static void Main(string[] args)
        {

            // player.URL = System.IO.Path.Combine(getLocation(), song);
           
            while(input != STOP)
            {
                geefStartmenu();
                switch (input)
                {
                    case PLAY:
                        Console.WriteLine("geef het nummer dat je wil spelen 'song.mp3'");
                        ReadSong();
                        Play(song, musicLocation);
                        break;
                    case PAUSE:
                        Console.WriteLine("je liedje is gepauzeerd");
                        Pause();
                        break;

                    case BEWERK:
                        PlaylistMenu();
                        break;
                    case MUTE:
                        Mute(true);
                        break;
                    case NEXT:
                        player.URL = System.IO.Path.Combine(musicLocation, GetPrevious());
                        Console.WriteLine($"huidig liedje {song.Substring(musicLocation.Length + 1)}");
                        break;
                    case PREVIOUS:
                        player.URL = System.IO.Path.Combine(musicLocation, GetNextSong());
                        Console.WriteLine($"huidig liedje {song.Substring(musicLocation.Length + 1)}");
                        break;
                        
                    case UNMUTE:
                        Mute(false);
                        break;
                    case VOLUME:
                        Console.WriteLine("geef het volume dat het wil op zetten");
                        int volume = Convert.ToInt32(Console.ReadLine());
                        ChangeSound(volume);
                        Console.WriteLine($"je volume is nu {volume}");
                        break;

                        
                    case CONTINUE:
                        Continue();
                        break;

                }
            }
      

            Console.ReadKey();
        }
        static public void ReadSong()
        {
            song = Console.ReadLine();
        }
        static public void PlaylistMenu()
        {
            Console.WriteLine("wat wil je doen?");
            Console.WriteLine("type 'voegtoe' om een liedje to te voegen");
            Console.WriteLine("type 'verwijder' om een liedje te verwijdere");
            Console.WriteLine("type 'terug om terug te gaan");
            string keuze = Console.ReadLine();        
                switch (keuze)
                {
                    case VOEGTOE:
                        Console.WriteLine("geef het liedje dat je wil toevoegen");
                        song = Console.ReadLine();
                        AddSongToList(song);
                        break;

                    case VERWIJDER:
                        Console.WriteLine("geef het liedje dat je wil verwijderen");
                        song = Console.ReadLine();
                        RemoveSongFromList(song);
                        break;

                    case TERUG:
                        geefStartmenu();
                        break;
                }           
        }
        static public void geefStartmenu()
        {
            Console.WriteLine(" .------------------------.");
            Console.WriteLine(" |\\\\////////       Beau R |");
            Console.WriteLine(" | \\/  __  ______  __     |");
            Console.WriteLine(" |    /  \\|\\.....|/  \\    |");
            Console.WriteLine(" |    \\__/|/_____|\\__/    |    ");
            Console.WriteLine(" | play     pause    stop |   ");
            Console.WriteLine(" |    ________________    |   ");
            Console.WriteLine(" |___/_._o________o_._\\___|  ");

            Console.WriteLine("");
            Console.WriteLine("* type 'play' om een liedje te spelen  ");
            Console.WriteLine("* type 'pause' om te pauzeren");
            Console.WriteLine("* type 'stop' om te stoppen");
            Console.WriteLine("* type 'volume' met je gewenste volume om het volume aan te passen");
            Console.WriteLine("");
            Console.WriteLine("* type 'bewerk' om playlist aanpassen" +
                "");
            Console.WriteLine("");
            Console.WriteLine("Geef je Commando");
            input = Console.ReadLine();
        }
        static public void AddSongToList(string song)
        {
            songList.Add(song);
            File.WriteAllLines(destination, songList);
        }
        static public void RemoveSongFromList(string song)
        {
            songList.Remove(song);
            File.WriteAllLines(destination, songList);
        }
        static public string[] ReadLinesFromText()
        {
            return linesArray = File.ReadAllLines(destination);
        }
        static public void ChangeSound(int volume)
        {
            player.settings.volume = volume;
        }
        static public void Mute(bool mute)
        {
            player.settings.mute = mute;
        }
        static public void Play(string song, string musicLocation)
        {
            player.URL = System.IO.Path.Combine(musicLocation, song);
        }
        static public void Continue()
        {
            player.controls.play();
        }
        static public void Pause()
        {
            player.controls.pause();
        }
        static public void Close()
        {
            Environment.Exit(0);
        }
        static public void LoadSongsFromFolder()
        {
            string[] music = Directory.GetFiles(@musicLocation);
            foreach (string currentSong in music)
            {
                //string liedjesNaam;
                song = currentSong.Substring(musicLocation.Length + 1);   // e.g. song.mp3

                if (song.Contains(".mp3") || song.Contains(".flac"))
                {
                    //fill list with song path  e.g. c:\test\music\song.mp3
                    songPathList.Add(currentSong);
                  
                    sizeOfList = songPathList.Count;
                }
            }

        }
        static public string GetNextSong()
        {
            int index = songPathList.FindIndex(a => a.Contains(song));
            string nextSong = songPathList[index + 1];
            song = nextSong;
            return song;
        }
       static public string GetPrevious()
        {
            int index = songPathList.FindIndex(a => a.Contains(song));
            string previousSong = songPathList[index - 1];
            song = previousSong;
            return song;
        }
    }
}
