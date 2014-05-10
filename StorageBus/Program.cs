using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace ConsoleApplication1
{
	class Program
	{
		static void Main(string[] args)
		{
			var cloud = CloudStorageAccount.Parse(azure storage credentials);
			var queueClient = cloud.CreateCloudQueueClient();
			var queue = queueClient.GetQueueReference("measurements");
			queue.CreateIfNotExists();

			var start = DateTime.Now;
			for (int i = 0; i < 100; i++)
			{


				var measurement = JsonConvert.SerializeObject(new
				{
					userId = "AB",
					deviceId = "Rpi",
					time = DateTime.Now,
					sensorId = "proc",
					value = i.ToString()
				});

				queue.AddMessageAsync(new CloudQueueMessage(measurement));
			}
			var end = DateTime.Now;
			Console.WriteLine(end.Subtract(start).TotalSeconds);

			var message = queue.GetMessage();
			while (message != null)
			{
				var reading = JsonConvert.DeserializeObject<Measurement>(message.AsString);

				Console.WriteLine(reading.UserId);
				Console.WriteLine(reading.DeviceId);
				Console.WriteLine(reading.MyProperty);
				Console.WriteLine(reading.SensorId);
				Console.WriteLine(reading.Value);
				Console.WriteLine();

				queue.DeleteMessage(message);
				message = queue.GetMessage();
			}

			Console.WriteLine("Press any key");
			Console.ReadKey();
		}

		public class Measurement
		{
			public string UserId { get; set; }
			public string DeviceId { get; set; }
			public DateTime MyProperty { get; set; }
			public string SensorId { get; set; }
			public string Value { get; set; }

		}
	}
}