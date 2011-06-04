namespace LayerlessMusicStore.Models
{
    public class Album
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public decimal Price { get; set; }
        public string AlbumArtUrl { get; set; }
    }
}