using BackgroundWorker.Models;
using BackgroundWorker.Operations;
using BackgroundWorker.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace BackgroundWorker
{
    class Program
    {
        static private HttpClient _client;
        static private AreaOperations _ops;
        static private AlertRepo _repo;
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            Console.WriteLine("Initializing background worker...");
            Initialize();
            Console.WriteLine("Calculating if animals are inside safe zone...");
            await CheckAnimalsSafeAreaAsync();
            Console.WriteLine("\nJob done!\n");
        }

        static public void Initialize()
        {
            _client = new HttpClient();
            _ops = new AreaOperations();
            _repo = new AlertRepo();
        }

        static public async System.Threading.Tasks.Task CheckAnimalsSafeAreaAsync()
        {
            var request = _client.GetAsync($"https://animaldangerapi.azurewebsites.net/api/Animal/");
            var initialAnimal = JsonConvert.DeserializeObject<IEnumerable<Animal>>(await request.Result.Content.ReadAsStringAsync());

            await _ops.CheckAnimalsInZoneAsync(initialAnimal, _repo);
        }
    }
}
