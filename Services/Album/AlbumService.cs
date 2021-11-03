using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataModel.DTO;
using DataModel.Enums;
using DataModel.Model;
using Microsoft.EntityFrameworkCore;
using Services.ErrorCodesNS;

namespace Services.AlbumNS
{
    public class AlbumService : IAlbumService
    {
        public readonly Context context;
        public readonly IMapper mapper;

        public AlbumService(Context ctx)
        {
            context = ctx;
        }

        /// <summary>
        /// Gets all filtered albums by a non-mandatory filter
        /// </summary>
        /// <param name="filterValue">Value that will be used to filter</param>
        /// <returns>List of albums</returns>
        public async Task<(bool, object)> GetAllFilteredAsync(string filterValue = null)
        {
            if (!string.IsNullOrWhiteSpace(filterValue))
            {
                return (true, await context.Albums.AsNoTracking().Where(k => k.Title.Contains(filterValue) || k.ArtistName.Contains(filterValue)).ProjectTo<AlbumDTO>(mapper.ConfigurationProvider).ToListAsync());
            }
            else
            {
                return (true, await context.Albums.AsNoTracking().ProjectTo<AlbumDTO>(mapper.ConfigurationProvider).ToListAsync());
            }
        }

        public async Task<(bool, object)> GetByIdAsync(Guid id)
        {
            AlbumDTO album = await context.Albums.AsNoTracking().ProjectTo<AlbumDTO>(mapper.ConfigurationProvider).FirstOrDefaultAsync(k => k.Id == id);

            if (album != null)
            {
                return (true, album);
            }

            return (false, ErrorCodes.NotFound);
        }

        /// <summary>
        /// Updates an Album
        /// </summary>
        /// <param name="album">Album with updated values</param>
        /// <returns>False && Error if any, if not, true and null</returns>
        public async Task<(bool, object)> UpdateAsync(AlbumDTO album)
        {
            if (album == null)
            {
                return (false, ErrorCodes.Null);
            }

            Album albumToUpdate = await context.Albums.AsNoTracking().FirstOrDefaultAsync(k => k.Id == album.Id);
            
            if (albumToUpdate == null)
            {
                return (false, ErrorCodes.NotFound);
            }

            if (!IsAlbumCorrectlyStructured(album))
            {
                return (false, ErrorCodes.EmptyMandatoryFields);
            }

            if (!album.Title.Equals(albumToUpdate.Title) && await context.Albums.AnyAsync(k => k.Title.Equals(album.Title)))
            {
                return (false, ErrorCodes.TitleDuplicated);
            }

            try
            {
                context.Entry(albumToUpdate).CurrentValues.SetValues(mapper.Map<Album>(album));

                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return (false, ErrorCodes.ServerError);
            }

            return (true, null);

        }

        /// <summary>
        /// Creates a new album.
        /// </summary>
        /// <param name="album">Album to create</param>
        /// <returns>False && Error if any, if not, true and added album</returns>
        public async Task<(bool, object)> CreateAsync(AlbumDTO album)
        {
            if (album == null)
            {
                return (false, ErrorCodes.Null);
            }

            if (await context.Albums.AnyAsync(k => k.Title.Equals(album.Title)))
            {
                return (false, ErrorCodes.TitleDuplicated);
            }

            if (!IsAlbumCorrectlyStructured(album))
            {
                return (false, ErrorCodes.EmptyMandatoryFields);
            }

            Album albumToAdd = mapper.Map<Album>(album);

            try
            {
                context.Albums.Add(albumToAdd);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return (false, ErrorCodes.ServerError);
            }

            return (true, mapper.Map<AlbumDTO>(albumToAdd));
        }

        /// <summary>
        /// Deletes an Album if found. 
        /// </summary>
        /// <param name="id">Album's id</param>
        /// <returns>False && Error if any, if not, true and null</returns>
        public async Task<(bool, object)> DeleteAsync(Guid id)
        {     
            Album albumToDelete = await context.Albums.FirstOrDefaultAsync(k => k.Id == id);

            if(albumToDelete == null)
            {
                return (false, ErrorCodes.NotFound);
            }

            try
            {
                context.Albums.Remove(albumToDelete);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return (false, ErrorCodes.ServerError);
            }

            return (true, null);

        }

        /// <summary>
        /// Verifies if the album doesn't have empty fields
        /// If I had more free time, I would've added a validation function to the Base Model, so every entity could validate itself
        /// </summary>
        /// <param name="album"></param>
        /// <returns></returns>
        private bool IsAlbumCorrectlyStructured(AlbumDTO album)
        {
            return !string.IsNullOrWhiteSpace(album.ArtistName) && !string.IsNullOrWhiteSpace(album.Title) && (album.Type.HasFlag(AlbumType.CD) || album.Type.HasFlag(AlbumType.Vinil)) && album.Stock != 0;
        }

    }

}