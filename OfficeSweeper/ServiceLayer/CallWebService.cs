using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Text;

namespace OfficeSweeper
{
	public class CallWebService
	{
		public static ServerResponse<ResponseModel> GetSweepingAreaDetails()
		{
			var response = new ServerResponse<ResponseModel> { isSuccess = false};
			try
			{
				//Call Http webrequest from our url
				var request = (HttpWebRequest)WebRequest.Create("http://lorensbergshost.azurewebsites.net/Robot/GetStartPresets");
				request.Method = "POST";
				request.ContentType = "application/json; charset=UTF-8";


				//Set Begin request stream for web request
				IAsyncResult resultRequest = request.BeginGetRequestStream(null, null);
				resultRequest.AsyncWaitHandle.WaitOne(30000); // 30 seconds for timeout

				// Receive data from server
				IAsyncResult resultResponse = request.BeginGetResponse(null, null);
				resultResponse.AsyncWaitHandle.WaitOne(30000); // 30 seconds for timeout

				//calll server response
				using (HttpWebResponse webResponse = request.EndGetResponse(resultResponse) as HttpWebResponse)
				{
					//validate the response is sucess or not
					if (webResponse.StatusCode == HttpStatusCode.OK)
					{
						
						response.isSuccess = true;

						//retriving data stream from response
						using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
						{
							//validate is retrieved stream is null or not
							if (reader != null)
							{
								//Convert stream into jsonstring format
								var jsonData = reader.ReadToEnd();
								if (jsonData != null)
									//Deserialize Json string
									response.Data = JsonConvert.DeserializeObject<ResponseModel>(jsonData);
							}
						}

					}
					else
					{
						response.isSuccess = false;
						response.Messasge = webResponse.StatusDescription;
					}
				}
			}
			catch (Exception ex)
			{
				response.isSuccess = false;
				response.Messasge = ex.Message;
			}
			return response;
		}
	}
}
