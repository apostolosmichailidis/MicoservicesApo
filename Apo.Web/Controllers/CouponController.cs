using System.Collections.Generic;
using System.Reflection;
using Apo.Web.Models;
using Apo.Web.Service;
using Apo.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Apo.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        public async Task<IActionResult?> CouponIndex()
        {
            List<CouponDto> list = new();
                
            ResponseDto? response = await _couponService.GetAllCouponsAsync();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result)!);
            }

            return View(list);
        }

        public async Task<IActionResult?> CouponCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult?> CouponCreate(CouponDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _couponService.CreateUpdateCouponAsync(model);

                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(CouponIndex));
                }
            }

            return View(model);
        }

        public async Task<IActionResult?> CouponDelete(int couponId)
        {
            ResponseDto? response = await _couponService.GetCoupnByIdAsync(couponId);
            if (response != null && response.IsSuccess && response.Result != null)
            {
                CouponDto? model = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result)!);
                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult?> CouponDelete(CouponDto couponDto)
        {
            ResponseDto? response = await _couponService.DeleteCouponAsync(couponDto.CouponId);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(CouponIndex));
            }

            return View(couponDto);
        }
    }
}
