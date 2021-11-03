using DataModel.Base;
using DataModel.Enums;
using System;
using System.Runtime.Serialization;

namespace DataModel.DTO
{
    [DataContract(IsReference = true)]
    public class AlbumDTO: BaseDTO<Guid>
    {
        [DataMember]
        public override Guid Id { get; set; }

        [DataMember]

        public string Title { get; set; }

        [DataMember]

        public string ArtistName { get; set; }

        [DataMember]

        public AlbumType Type { get; set; }

        [DataMember]

        public short Stock { get; set; }

    }
}
