using AutoMapper;
using DataModel.DTO;
using DataModel.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.Mapping
{
    public partial class AlbumMapping: Profile
    {
        public AlbumMapping()
        {
            CreateMap<Album, AlbumDTO>().ReverseMap();
        }
    }
}
