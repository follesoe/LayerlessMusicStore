using System.Linq;
using System.Collections.Generic;

namespace LayerlessMusicStore.Models
{
    public class ShoppingCart
    {
        public string Id { get; set; }
        public List<ShoppingCartLine> Lines { get; set; }

        public decimal Total
        {
            get { return Lines.Sum(l => l.Price*l.Quantity); }
        }

        public ShoppingCart()
        {
            Lines = new List<ShoppingCartLine>();
        }
        
        public void AddToChart(Album album)
        {
            var albumLine = Lines.Where(l => l.Album.Id == album.Id).SingleOrDefault();
            if (albumLine != null)
            {
                albumLine.Quantity++;
                return;
            }

            Lines.Add(new ShoppingCartLine
            {
                Album = new ShoppingCartLine.ShoppingCartLineAlbum
                            {
                                Id = album.Id,
                                Title = album.Title
                            },
                Price = album.Price,
                Quantity = 1
            });
        }
        
        public void RemoveFromChart(string albumId)
        {
            var albumLine = Lines.Where(l => l.Album.Id == albumId).SingleOrDefault();
            if (albumLine == null) return;

            albumLine.Quantity--;
            if (albumLine.Quantity == 0)
                Lines.Remove(albumLine);
        }

        public class ShoppingCartLine
        {
            public int Quantity { get; set; }
            public decimal Price { get; set; }

            public ShoppingCartLineAlbum Album { get; set; }

            public class ShoppingCartLineAlbum
            {
                public string Id { get; set; }
                public string Title { get; set; }
            }            
        }
    }
}