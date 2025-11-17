using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.API.Controllers
{
    [Route("api/banners")]
    [ApiController]
    public class BannerController : ControllerBase
    {
        private readonly IBannerService _bannerService;

        public BannerController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _bannerService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBannerRequest request)
        {
            return Ok(await _bannerService.CreateAsync(request));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateBannerRequest request)
        {
            return Ok(await _bannerService.UpdateAsync(id, request));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _bannerService.DeleteAsync(id));
        }

        [HttpPut("{id}/reorder/{newOrder}")]
        public async Task<IActionResult> Reorder(int id, int newOrder)
        {
            return Ok(await _bannerService.ReorderAsync(id, newOrder));
        }
    }
}
