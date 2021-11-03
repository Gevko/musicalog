using System;
using System.Threading.Tasks;
using DataModel.DTO;

namespace Services.AlbumNS
{

    public interface IAlbumService 
    {
        Task<(bool, object)> GetAllFilteredAsync(string filterValue = null);
        Task<(bool, object)> GetByIdAsync(Guid id);
        Task<(bool, object)> UpdateAsync(AlbumDTO album);
        Task<(bool, object)> CreateAsync(AlbumDTO album);
        Task<(bool, object)> DeleteAsync(Guid id);

    }

}