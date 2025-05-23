using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MagicVilla_Web.Models;
using AutoMapper;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Utility;

namespace MagicVilla_Web.Controllers;

public class HomeController : Controller
{
    private readonly IVillaService _villaService;
    private readonly IMapper _mapper;

    public HomeController(IVillaService villaService, IMapper mapper)
    {
        _villaService = villaService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        List<VillaDTO> list = new();

        var response = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

        if (response != null && response.IsSuccess)
        {
            //Convierte el JSON a un objeto. En este caso a una lista
            list = JsonConvert.DeserializeObject
                <List<VillaDTO>>(Convert.ToString(response.Result));
        }
        return View(list);
    }
}
