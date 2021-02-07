using System;

namespace GigHub.Core.Dtos
{
    public class GigDto
    {
        public int Id { get; set; }
        public bool IsCanceled { get; set; }
        public UserDto Artist { get; set; }
        public DateTime Date { get; set; }
        public string DateString { get { return this.Date.ToString("dd MM HH:mm"); } }
        public string Venue { get; set; }
        public GenreDto Genre { get; set; }
    }
}