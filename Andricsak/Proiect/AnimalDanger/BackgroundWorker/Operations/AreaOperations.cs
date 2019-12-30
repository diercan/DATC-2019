using BackgroundWorker.Models;
using BackgroundWorker.Repositories;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundWorker.Operations
{
    class AreaOperations
    {
        // the coordinates that define the safe area
        GeoCoordinate topLeft = new GeoCoordinate(45.751714, 21.219859);
        GeoCoordinate topRight = new GeoCoordinate(45.750864, 21.233672);
        GeoCoordinate bottomLeft = new GeoCoordinate(45.742850, 21.219516);
        GeoCoordinate bottomRight = new GeoCoordinate(45.743676, 21.236074);


		public async Task CheckAnimalsInZoneAsync(IEnumerable<Animal> animals, AlertRepo repo)
		{
			int counter = 0;

			Alert newAlert = new Alert();
			List<string> animalsNames = new List<string>();
			foreach (var animal in animals)
			{
				GeoCoordinate animalLocation = new GeoCoordinate(animal.Lat, animal.Long);
				if (isWithin(animalLocation, bottomLeft, topRight))
				{
					counter++;
					animalsNames.Add(animal.Name);
				}

			}
			if (counter > 0)
			{
				newAlert.Description = $"{counter} animals are inside the area!";
				newAlert.Id = 1;
				newAlert.City = "Timisoara";
				newAlert.PartitionKey = "Timisoara";
				newAlert.RowKey = "1";

				await repo.InsertOrUpdate(newAlert);
			}
			else
			{
				await repo.DeleteEnetityAsync("Timisoara", "1");
			}

			foreach (var elem in animalsNames)
			{
				Console.WriteLine(elem);
			}
		}

		public Boolean isWithin(GeoCoordinate pt, GeoCoordinate sw, GeoCoordinate ne)
		{
			return pt.Latitude >= sw.Latitude &&
				   pt.Latitude <= ne.Latitude &&
				   pt.Longitude >= sw.Longitude &&
				   pt.Longitude <= ne.Longitude;
		}
	}
}
