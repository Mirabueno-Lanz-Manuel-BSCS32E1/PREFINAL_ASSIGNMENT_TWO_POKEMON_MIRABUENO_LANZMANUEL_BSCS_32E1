using Microsoft.AspNetCore.Mvc;
using PokemonApp.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class PokemonController : Controller
{
    private readonly HttpClient _httpClient;

    public PokemonController()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new System.Uri("https://pokeapi.co/api/v2/")
        };
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        var response = await _httpClient.GetStringAsync($"pokemon?limit=20&offset={(page - 1) * 20}");
        var data = JObject.Parse(response);
        var pokemonList = new List<Pokemon>();

        foreach (var result in data["results"])
        {
            var pokemon = new Pokemon
            {
                Name = result["name"].ToString(),
                Id = int.Parse(result["url"].ToString().Split('/')[6])
            };
            pokemonList.Add(pokemon);
        }

        ViewBag.Page = page;
        ViewBag.TotalCount = data["count"].ToObject<int>();

        return View(pokemonList);
    }

    public async Task<IActionResult> Details(int id)
    {
        var response = await _httpClient.GetStringAsync($"pokemon/{id}");
        var data = JObject.Parse(response);

        var pokemon = new Pokemon
        {
            Id = data["id"].ToObject<int>(),
            Name = data["name"].ToString(),
            Moves = data["moves"].Select(m => m["move"]["name"].ToString()).ToList(),
            Abilities = data["abilities"].Select(a => a["ability"]["name"].ToString()).ToList()
        };

        return View(pokemon);
    }
}