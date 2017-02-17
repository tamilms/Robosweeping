using System;
using Newtonsoft.Json;
namespace OfficeSweeper
{
	public class ResponseModel
	{
		[JsonProperty("NumberOfCommands")]
		public int NumberOfCommands { get; set; }
		[JsonProperty("StartX")]
		public float Start_XPositiom { get; set; }
		[JsonProperty("StartY")]
		public float Start_YPositiom { get; set; }
	}
}
