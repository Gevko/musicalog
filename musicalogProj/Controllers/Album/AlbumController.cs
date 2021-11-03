using System;
using System.Threading.Tasks;
using DataModel.DTO;
using Microsoft.AspNetCore.Mvc;
using Services.AlbumNS;

namespace musicalogProj.Controllers.Album
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumService _albumSrv;
        public AlbumController(IAlbumService albumSrv)
        {
            _albumSrv = albumSrv;
        }

        [HttpPut("getAll")]
        public async Task<IActionResult> GetAllFilteredAsync([FromBody] string filterValue)
        {
            (bool success, object result) = await _albumSrv.GetAllFilteredAsync(filterValue);

            if(!success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            (bool success, object result) = await _albumSrv.GetByIdAsync(id);

            if (!success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] AlbumDTO album)
        {
            (bool success, object result) = await _albumSrv.UpdateAsync(album);

            if (!success)
            {
                return BadRequest(result);
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] AlbumDTO album)
        {
            (bool success, object result) = await _albumSrv.CreateAsync(album);

            if (!success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            (bool success, object result) = await _albumSrv.DeleteAsync(id);

            if (!success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}
