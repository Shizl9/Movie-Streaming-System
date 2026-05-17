using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie_Streaming.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public List<Review> Reviews { get; set; } = new List<Review>();
        public List<Watchlist> Watchlists { get; set; } = new List<Watchlist>();
    }
}
