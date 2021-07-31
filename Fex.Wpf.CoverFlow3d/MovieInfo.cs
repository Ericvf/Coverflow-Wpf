using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Media.Imaging;

namespace iTunaFish.Library
{
    [XmlType("MovieInfo")]
    public class MovieInfo : IComparable
    {
        public string ImdbId { get; set; }
        public int TmdbId { get; set; }

        public string Title { get; set; }
        public int Runtime { get; set; }
        public decimal Rating { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Language { get; set; }
        public string Overview { get; set; }
        
        public string TrailerUrl { get; set; }
        public string HomepageUrl { get; set; }
        public string MediaFile { get; set; }

        [XmlArrayAttribute("CastMembers")]
        public List<CastMember> CastMembers { get; set; }

        [XmlArrayAttribute("Genres")]
        public List<Genre> Genres { get; set; }

        [XmlIgnore]
        public string filePath;

        [XmlIgnore]
        public BitmapImage ThumbnailImage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        public MovieInfo(string filePath)
            : this()
        {
            this.filePath = filePath;

            var fileInfo = new FileInfo(filePath);
            this.MediaFile = fileInfo.Name;
        }

        public MovieInfo()
        {
            this.CastMembers = new List<CastMember>();
            this.Genres = new List<Genre>();
        }

        public bool IsValid()
        {
            bool fileExists = System.IO.File.Exists(this.filePath);
            return fileExists;
        }

        public bool HasMediaFileInfo()
        {
            var mediaFileInfoFile = Path.Combine(this.filePath, ".movieinfo");
            return File.Exists(mediaFileInfoFile);
        }

        public void DeleteMediaFileInfo()
        {
            var mediaFileInfoFile = Path.Combine(this.filePath, ".movieinfo");
            File.Delete(mediaFileInfoFile);
        }

        public string GetBaseName()
        {
            var fileInfo = new FileInfo(this.filePath);
            return fileInfo.Name;
        }

        public void SaveMediaInfo()
        {
            var fileInfo = new FileInfo(this.filePath);
            var directory = fileInfo.DirectoryName;
            var newFileName = Path.Combine(directory, this.TmdbId.ToString() + ".movieinfo");

            XmlSerializationHelper.Serialize<MovieInfo>(this, newFileName);
        }

        [XmlIgnore]
        public string ThumbnailPath
        {
            get
            {
                return this.GetThumbnailPath();
            }
        }

        public string GetThumbnailPath()
        {
            var mediaFileInfoFile = Path.Combine(this.filePath, this.TmdbId + "_thumb.jpg");
            return mediaFileInfoFile;
        }


        public string[] GetSubtitles()
        {
            var files = Directory.GetFiles(this.filePath, "*.srt");
            return files;
        }

        private List<string> backgrounds;
        private int currentBackdrop = 0;
        
        public string GetNextBackdropPath(out int backdropId, out int backdropCount)
        {
            if (this.backgrounds == null)
                this.GetBackgrounds();

            backdropId = this.currentBackdrop;
            backdropCount = this.backgrounds.Count;

            var mediaFileInfoFile = Path.Combine(this.filePath, this.TmdbId + string.Format("_bg{0:00}.jpg", this.currentBackdrop));
            if (!File.Exists(mediaFileInfoFile))
            {
                currentBackdrop = 0;
            }
            else
            {
                currentBackdrop++;
            }

            return mediaFileInfoFile;
        }

        private void GetBackgrounds()
        {
            var i = 0;
            var bgFile = Path.Combine(this.filePath, this.TmdbId + string.Format("_bg{0:00}.jpg", i));

            this.backgrounds = new List<string>();
            while (File.Exists(bgFile))
            {
                this.backgrounds.Add(bgFile);
                bgFile = Path.Combine(this.filePath, this.TmdbId + string.Format("_bg{0:00}.jpg", ++i));
            }
        }



        public string GetMoviePath()
        {
            return Path.Combine(this.filePath, this.MediaFile);
        }

        public int CompareTo(object obj)
        {
            var movieInfo = obj as MovieInfo;
            return this.Title.CompareTo(movieInfo.Title);
        }
    }
       
}
