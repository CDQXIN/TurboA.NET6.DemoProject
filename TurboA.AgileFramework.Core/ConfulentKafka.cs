using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.AgileFramework.Common.IOCOptions;

namespace Zhaoxi.AgileFramework.Core
{
	public class ConfulentKafka
	{
		private readonly KafkaOptions _kafkaOptions;
		public ConfulentKafka(IOptionsMonitor<KafkaOptions> optionsMonitor)
		{
			_kafkaOptions = optionsMonitor.CurrentValue;
		}
		public ConfulentKafka(KafkaOptions kafkaOptions)
		{
			_kafkaOptions = kafkaOptions;
		}

		/// <summary>
		/// 发送事件
		/// </summary>
		/// <param name="event"></param>
		public async Task Produce(string content)
		{
			string brokerList = _kafkaOptions.BrokerList;
			string topicName = _kafkaOptions.TopicName;
			var config = new ProducerConfig
			{
				BootstrapServers = brokerList,
				//EnableIdempotence = true,//幂等性 
				//Acks = Acks.All,//数据不丢失
				MessageSendMaxRetries = 3,
			};

			using (var producer = new ProducerBuilder<string, string>(config)
				.Build())
			{
				Console.WriteLine("\n-----------------------------------------------------------------------");
				Console.WriteLine($"Producer {producer.Name} producing on topic {topicName}.");
				Console.WriteLine("-----------------------------------------------------------------------");
				try
				{
					var deliveryReport = await producer.ProduceAsync(
					topicName, new Message<string, string> { Key = Guid.NewGuid().ToString(), Value = content });
					Console.WriteLine($"delivered to: {deliveryReport.TopicPartitionOffset}");
				}
				catch (ProduceException<string, string> e)
				{
					Console.WriteLine($"failed to deliver message: {e.Message} [{e.Error.Code}]");
				}
			}
		}
	}
}
