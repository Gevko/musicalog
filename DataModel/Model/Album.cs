using DataModel.Base;
using DataModel.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataModel.Model
{
    public class Album: Base<Guid>
    {
        [Required()]
        [Key]
        public override Guid Id { get; set; }

        [Required()]
        public string Title { get; set; }

        [Required()]

        public string ArtistName { get; set; }

        [Required()]

        public AlbumType Type { get; set; }

        [Required()]
        // I'm not sure if the stock should be a number or a string data type
        public short Stock { get; set; }
    }
}


