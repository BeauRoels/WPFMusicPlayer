using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WMPLib;

namespace WPFMusicPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window

    {           
       //Windows media player
        public static WindowsMediaPlayer player = new WindowsMediaPlayer();
        public static string docsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string destination = System.IO.Path.Combine(docsFolder, "playlist.txt ");
        public string musicLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

        //inputs
        public static string input = "";
        public static string song;
        
        //lists and arrays
        public static string[] linesArray;
        public static List<string> linesList = new List<string>();
        public List<string> songPathList = new List<string>();
        public int sizeOfList;

        public MainWindow()
        {
            InitializeComponent();               
            //er is een bug waarbij enkel het geselecteerde liedje van de combobox wordt toegevoegd aan de playslist
        }
        private void WindowMusicPlayer_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSongsFromFolder();
        }
        private void btnShowExistingPlaylist_Click(object sender, RoutedEventArgs e)
        {
            ReadTextFile();
        }    
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {

            ReturnItemFromBox();
            lblPlayingSong.Content = song;          
            Play(song, musicLocation);
        }
        private void btnAddToPlaylist_Click(object sender, RoutedEventArgs e)
        {
            AddToListbox();
            AddSongToList();
        }
       
        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            Continue();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            Pause();
        }
        private void btnMute_Click(object sender, RoutedEventArgs e)
        {                       
            Mute(true);              
        }
        private void btnUnmute_Click(object sender, RoutedEventArgs e)
        {
            Mute(false);
        }
        private void sldVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ChangeSound(Convert.ToInt32(sldVolume.Value));
        }
        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            player.URL = System.IO.Path.Combine(musicLocation, GetPrevious());
            lblPlayingSong.Content = song.Substring(musicLocation.Length + 1); ;
        }
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            player.URL = System.IO.Path.Combine(musicLocation, GetNextSong());
            lblPlayingSong.Content = song.Substring(musicLocation.Length + 1); 
        }
        static public void ReadSong()
        {
            song = input;
        }
        static public void AddSongToList()
        {
            ReadLinesFromText();
            linesList.Add(song);            
            File.WriteAllLines(destination, linesList);
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
        public void Play(string song, string musicLocation)
        {
            player.URL = System.IO.Path.Combine(musicLocation, song);
        }
        public void Continue()
        {
            player.controls.play();
        }
        public void Pause()
        {
            player.controls.pause();
        }
        public string GetNextSong()
        {           
            int index = songPathList.FindIndex(a => a.Contains(song));
            string nextSong = songPathList[index+1];
            song = nextSong;          
            return song;
        }
        public string GetPrevious()
        {
            int index = songPathList.FindIndex(a => a.Contains(song));
            string previousSong = songPathList[index-1];
            song = previousSong;        
            return song;
        }
        static public void Close()
        {
            Environment.Exit(0);
        } 
        public string ReturnItemFromBox()
        {
            return song = (string)cmbAllSongs.SelectedItem;
        }         
        public void AddToListbox()
        {
            ListBoxItem item = new ListBoxItem();
            item.Content = ReturnItemFromBox();
            listBoxPlaylist.Items.Add(item);        
        }
        public void ReadTextFile()
        {
            try
            {
                using (var sr = new StreamReader(destination))
                {
                    while(!sr.EndOfStream)
                    {
                        ListBoxItem item = new ListBoxItem();
                        item.Content = sr.ReadLine();
                        listBoxPlaylist.Items.Add(item);
                    }
                

                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }                
        public void LoadSongsFromFolder()
        {
            string[] music = Directory.GetFiles(@musicLocation);
            foreach (string currentSong in music)
            {
                //string liedjesNaam;
               song = currentSong.Substring(musicLocation.Length + 1);   // e.g. song.mp3

                if (song.Contains(".mp3") || song.Contains(".flac") )
                {
                    //fill list with song path  e.g. c:\test\music\song.mp3
                    songPathList.Add(currentSong);
                    cmbAllSongs.Items.Add(song);
                    sizeOfList = songPathList.Count;
                }
            }

        }

       
    }
}
