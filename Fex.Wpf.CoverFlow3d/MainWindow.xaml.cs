using iTunaFish.App.UserControls;
using iTunaFish.Library;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Fex.Wpf.CoverFlow3d
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            this.PreviewKeyUp += new KeyEventHandler(MainWindow_PreviewKeyUp);
        }

        void MainWindow_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    this.coverFlow.Prev();
                    break;

                case Key.Right:
                    this.coverFlow.Next();
                    break;

                case Key.PageUp:
                    this.coverFlow.First();
                    break;

                case Key.PageDown:
                    this.coverFlow.Last();
                    break;
            }
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var movies = new List<MovieInfo>();

            var files = Directory.GetFiles(@"../../localmirror", "*.movieinfo", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var movie = XmlSerializationHelper.DeserializeFile<MovieInfo>(file);
                movie.filePath = new FileInfo(file).DirectoryName;

                var thumbnailPath = movie.GetThumbnailPath();
                if (!File.Exists(thumbnailPath))
                    continue;

                byte[] buffer = File.ReadAllBytes(thumbnailPath);
                MemoryStream memoryStream = new MemoryStream(buffer);

                BitmapImage bitmap = new BitmapImage();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.BeginInit();
                bitmap.DecodePixelWidth = 80;
                bitmap.DecodePixelHeight = 60;
                bitmap.StreamSource = memoryStream;
                bitmap.EndInit();
                bitmap.Freeze();

                movie.ThumbnailImage = bitmap;

                movies.Add(movie);
            }

            movies.Sort();

            // Update the UI
            for (int i = 0; i < movies.Count; i++)
            {
                var movie = movies[i];
                var child = new MovieThumbnail()
                {
                    DataContext = movie,
                    Width = 180,
                    Height = 260
                };

                this.coverFlow.AddChild(child);
            }
        }
    }
}
