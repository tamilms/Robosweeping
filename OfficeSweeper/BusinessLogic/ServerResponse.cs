using System;
namespace OfficeSweeper
{
	public class ServerResponse<T> : ResponseBase
	{
		public T Data { get; set; }
	}
}
