using GeolocationAds.Tools;
using GeolocationAdsAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeolocationAdController : ControllerBase
    {
        private readonly IAdvertisementRepository advertisementRepository;

        private readonly IGeolocationAdRepository geolocationAdRepository;

        public GeolocationAdController(IGeolocationAdRepository geolocationAdRepository, IAdvertisementRepository advertisementRepository)
        {
            this.geolocationAdRepository = geolocationAdRepository;

            this.advertisementRepository = advertisementRepository;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add(GeolocationAd newGeolocationAd)
        {
            ResponseTool<GeolocationAd> response;

            try
            {
                var _currentAdToPost = newGeolocationAd.Advertisement;

                newGeolocationAd.Advertisement = null;

                response = await this.geolocationAdRepository.CreateAsync(newGeolocationAd);

                if (response.IsSuccess)
                {
                    //_currentAdToPost.IsPosted = true;

                    //_currentAdToPost.GeolocationAdId = response.Data.ID;

                    //var _adPostedResponse = await this.advertisementRepository.UpdateAsync(_currentAdToPost.ID, _currentAdToPost);

                    //if (!_adPostedResponse.IsSuccess)
                    //{
                    //    newGeolocationAd.Advertisement = _currentAdToPost;

                    //    response = ResponseFactory<GeolocationAd>.BuildSusccess("Content Posted Correctly.", newGeolocationAd, ToolsLibrary.Tools.Type.Added);

                    //    return Ok(response);
                    //}

                    return Ok(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                response = ResponseFactory<GeolocationAd>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }

        [HttpPost("[action]/{distance}")]
        public async Task<IActionResult> FindAdNear(CurrentLocation currentLocation, int distance)
        {
            ResponseTool<IEnumerable<Advertisement>> response;

            IList<Advertisement> _adsNear = new List<Advertisement>();

            try
            {
                var _geoAd_Response = await this.geolocationAdRepository.GetAllWithNavigationPropertyAsync();

                if (_geoAd_Response.IsSuccess)
                {
                    foreach (var item in _geoAd_Response.Data)
                    {
                        double meterDistance = GeolocationTool.VincentyFormula4(currentLocation.Latitude, currentLocation.Longitude, item.Latitude, item.Longitude);

                        if (meterDistance <= distance)
                        {
                            item.Advertisement.GeolocationAds = item.Advertisement.GeolocationAds.Select(g => new GeolocationAd() { ID = g.ID, Latitude = g.Latitude, Longitude = g.Longitude }).ToList();

                            _adsNear.Add(item.Advertisement);
                        }
                    }

                    if (_adsNear.Count > 0)
                    {
                        _adsNear = _adsNear.OrderBy(o => o.CreateDate).Reverse().ToList();

                        response = ResponseFactory<IEnumerable<Advertisement>>.BuildSusccess("Content Found.", _adsNear, ToolsLibrary.Tools.Type.DataFound);
                    }
                    else
                    {
                        response = ResponseFactory<IEnumerable<Advertisement>>.BuildSusccess("Not Nearby Content.", null, ToolsLibrary.Tools.Type.NotFound);
                    }
                }
                else
                {
                    response = ResponseFactory<IEnumerable<Advertisement>>.BuildSusccess(_geoAd_Response.Message, null, ToolsLibrary.Tools.Type.Fail);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = ResponseFactory<IEnumerable<Advertisement>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }

        //[HttpPost("[action]/{distance}/{settinTypeId}")]
        //public async Task<IActionResult> FindAdNear2(CurrentLocation currentLocation, int distance, int settinTypeId)
        //{
        //    ResponseTool<IEnumerable<Advertisement>> response;

        //    IList<Advertisement> _adsNear = new List<Advertisement>();

        //    try
        //    {
        //        var _geoAd_Response = await this.geolocationAdRepository.GetAllWithNavigationPropertyAsyncAndSettingEqualTo(settinTypeId);

        //        if (_geoAd_Response.IsSuccess)
        //        {
        //            foreach (var item in _geoAd_Response.Data)
        //            {
        //                double meterDistance = GeolocationTool.VincentyFormula4(currentLocation.Latitude, currentLocation.Longitude, item.Latitude, item.Longitude);

        //                if (meterDistance <= distance)
        //                {
        //                    //item.Advertisement.GeolocationAds = item.Advertisement.GeolocationAds.Select(g => new GeolocationAd() { ID = g.ID, Latitude = g.Latitude, Longitude = g.Longitude }).ToList();

        //                    _adsNear.Add(item.Advertisement);
        //                }
        //            }

        //            if (_adsNear.Count > 0)
        //            {
        //                _adsNear = _adsNear.OrderBy(o => o.CreateDate).Reverse().ToList();

        //                response = ResponseFactory<IEnumerable<Advertisement>>.BuildSusccess("Content Found.", _adsNear, ToolsLibrary.Tools.Type.DataFound);
        //            }
        //            else
        //            {
        //                response = ResponseFactory<IEnumerable<Advertisement>>.BuildSusccess("Not Nearby Content.", null, ToolsLibrary.Tools.Type.NotFound);
        //            }
        //        }
        //        else
        //        {
        //            response = ResponseFactory<IEnumerable<Advertisement>>.BuildSusccess(_geoAd_Response.Message, null, ToolsLibrary.Tools.Type.Fail);
        //        }

        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        response = ResponseFactory<IEnumerable<Advertisement>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

        //        return Ok(response);
        //    }
        //}

        [HttpPost("[action]/{distance}/{settinTypeId}")]
        public async Task<IActionResult> FindAdNear2(CurrentLocation currentLocation, int distance, int settinTypeId)
        {
            ResponseTool<IEnumerable<Advertisement>> response;

            IList<Advertisement> _adsNear = new List<Advertisement>();

            try
            {
                var _geoAd_Response = await this.geolocationAdRepository.GetAllWithNavigationPropertyAsyncAndSettingEqualTo2(currentLocation, distance, settinTypeId);

                if (_geoAd_Response.IsSuccess)
                {

                    //foreach (var item in _geoAd_Response.Data)
                    //{
                    //    _adsNear.Add(item);
                    //}

                    //foreach (var item in _geoAd_Response.Data)
                    //{
                    //    foreach (var geo in item.GeolocationAds)
                    //    {
                    //        double meterDistance = GeolocationTool.VincentyFormula4(currentLocation.Latitude, currentLocation.Longitude, geo.Latitude, geo.Longitude);

                    //        if (meterDistance <= distance)
                    //        {
                    //            _adsNear.Add(item);
                    //        }
                    //    }
                    //}

                    //var adsNear = _geoAd_Response.Data
                    //    .SelectMany(item => item.GeolocationAds)
                    //    .Where(geo =>
                    //    {
                    //        double meterDistance = GeolocationTool.VincentyFormula4(currentLocation.Latitude, currentLocation.Longitude, geo.Latitude, geo.Longitude);

                    //        return meterDistance <= distance;
                    //    })
                    //    .Distinct()
                    //    .ToList();

                    //_adsNear.AddRange(_geoAd_Response.Data.Where(v => v.GeolocationAds.Any(geo => adsNear.Any(n => n.ID == geo.ID))).ToList());


                    //var adsNear = _geoAd_Response.Data
                    //    .Where(advertisement => advertisement.GeolocationAds.Any(geo =>
                    //    {
                    //        double meterDistance = GeolocationTool.VincentyFormula4(currentLocation.Latitude, currentLocation.Longitude, geo.Latitude, geo.Longitude);

                    //        return meterDistance <= distance;

                    //    })).ToList();

                    // Define a batch size (adjust as needed)
                    //int batchSize = 1000;

                    //foreach (var item in _geoAd_Response.Data)
                    //{
                    //    var geolocationAds = item.GeolocationAds.ToList(); // Convert to a list for indexing

                    //    for (int i = 0; i < geolocationAds.Count; i += batchSize)
                    //    {
                    //        var batch = geolocationAds.Skip(i).Take(batchSize);

                    //        // Calculate distances for the current batch in parallel
                    //        var distances = batch.AsParallel().Select(geo =>
                    //        {
                    //            return GeolocationTool.VincentyFormula4(currentLocation.Latitude, currentLocation.Longitude, geo.Latitude, geo.Longitude);
                    //        }).ToList();

                    //        // Check if any distance in the batch is less than or equal to the specified distance
                    //        if (distances.Any(distance => distance <= distance))
                    //        {
                    //            _adsNear.Add(item);
                    //            break; // Exit the inner loop once a match is found for this item
                    //        }
                    //    }
                    //}

                    if (_geoAd_Response.Data.Count() > 0)
                    {
                        _adsNear = _adsNear.OrderBy(o => o.CreateDate).Reverse().ToList();

                        _geoAd_Response.Data = _geoAd_Response.Data.OrderBy(o => o.CreateDate).Reverse().ToList();

                        response = ResponseFactory<IEnumerable<Advertisement>>.BuildSusccess("Content Found.", _geoAd_Response.Data, ToolsLibrary.Tools.Type.DataFound);
                    }
                    else
                    {
                        response = ResponseFactory<IEnumerable<Advertisement>>.BuildSusccess("Not Nearby Content.", null, ToolsLibrary.Tools.Type.NotFound);
                    }
                }
                else
                {
                    response = ResponseFactory<IEnumerable<Advertisement>>.BuildSusccess(_geoAd_Response.Message, null, ToolsLibrary.Tools.Type.Fail);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = ResponseFactory<IEnumerable<Advertisement>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }
    }
}