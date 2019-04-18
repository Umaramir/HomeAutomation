using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMC.Sarah.ChatBot.Helpers
{
    public class MusicList
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Text { get; set; }
        public string Img { get; set; }
        public string Url { get; set; }
    
        public static List<MusicList> GetAllMusicRelax()
        {
            return new List<MusicList>()
            {
                new MusicList () { Title="Relaxing Music", SubTitle="Flute Japanese", Text="Just Listen and relax", Img="https://padidata.blob.core.windows.net/files/flute-min.jpg", Url="https://padidata.blob.core.windows.net/files/BeautifulBambooFlute.mp3" }, //Url="https://padidata.blob.core.windows.net/files/RelaxingFlute.mp3" },
                new MusicList () { Title="Ballad Rock Music", SubTitle="Guitar Rock", Text="Just Listen and relax", Img="https://padidata.blob.core.windows.net/files/RockGuitar.PNG", Url="https://padidata.blob.core.windows.net/files/SHORTRockBallad.mp3" }, //Url="https://padidata.blob.core.windows.net/files/InstrumentalRockBallads.mp3" }
            };
        }public static List<MusicList> GetAllMusicSpirit()
        {
            return new List<MusicList>()
            {
                new MusicList () { Title="Lets Rock n Roll", SubTitle="Guitar Rock", Text="Just Listen and Bang", Img="https://padidata.blob.core.windows.net/files/spiritUp.jpg", Url="https://padidata.blob.core.windows.net/files/Rebellion.mp4" }
            };
        }
    }
}