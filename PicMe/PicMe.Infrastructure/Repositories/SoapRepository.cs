using Microsoft.Extensions.Configuration;
using PicMe.Core.Interfaces.Repositories;
using PicMe.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace PicMe.Infrastructure.Repositories
{
	public class SoapRepository : ISoapRepository
	{
		private readonly HttpClient _httpClient;
		private readonly ISecureStorageService _secureStorageService;

		public SoapRepository(ISecureStorageService secureStorageService, HttpClient httpClient)
		{
			_secureStorageService = secureStorageService;
			_httpClient = httpClient;
		}

		public async Task<string> SendPhotoAsync(string userIdentifier, string title, string body, string fileName, string fileData)
		{
			var school = (await _secureStorageService.GetAsync("SchoolName"))?.Trim();
			var soapApiKey = await _secureStorageService.GetAsync("SoapApiKey");
			var sender = await _secureStorageService.GetAsync("Sender");

			var soapEnvelope = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
            <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                               xmlns:mns=""https://{school}.smartschool.be/Webservices/V3"">
              <SOAP-ENV:Body>
                <mns:sendMsg>
                  <accesscode xsi:type=""xsd:string"">{soapApiKey}</accesscode>
                  <userIdentifier xsi:type=""xsd:string"">{userIdentifier}</userIdentifier>
                  <title xsi:type=""xsd:string"">{title}</title>
                  <body xsi:type=""xsd:string"">{body}</body>
                  <senderIdentifier xsi:type=""xsd:string"">{sender}</senderIdentifier>
                  <attachments xsi:type=""xsd:string"">
                    [
                      {{ ""filename"": ""{fileName}"", ""filedata"": ""{fileData}"" }}
                    ]
                  </attachments>
                  <coaccount xsi:type=""xsd:int"">0</coaccount>
                  <copyToLVS xsi:type=""xsd:boolean"">false</copyToLVS>
                </mns:sendMsg>
              </SOAP-ENV:Body>
            </SOAP-ENV:Envelope>";

			var content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");
			var response = await _httpClient.PostAsync($"https://{school}.smartschool.be/Webservices/V3", content);

			if (!response.IsSuccessStatusCode)
			{
				await Application.Current.MainPage.DisplayAlert("Error", $"(FOUT:{response.StatusCode}) Contacteer uw IT dienst", "OK");
				return null;
			}
			var responseBody = await response.Content.ReadAsStringAsync();

			return responseBody;
		}

		public async Task<string> GetBase64ProfilePictureAsync(string identifier)
		{
			var school = (await _secureStorageService.GetAsync("SchoolName"))?.Trim();
			var soapApiKey = await _secureStorageService.GetAsync("SoapApiKey");
			var identification = await _secureStorageService.GetAsync("Identification");

			if (string.IsNullOrWhiteSpace(school) || string.IsNullOrWhiteSpace(soapApiKey) ||string.IsNullOrWhiteSpace(identification))
			{
				await Application.Current.MainPage.DisplayAlert("Error", $"1 van de gegevens is leeg, gelieve de IT dienst te contacteren", "OK");
				return null;
			}
			//if (identification == "true")
			//{
			//	identification = "userIdentifier";
			//}
			//else
			//{
			//	identification = "internNumber";
			//}
			try
			{
				var soapEnvelope = $@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
                <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" 
                    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
                    xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" 
                    xmlns:mns=""https://{school}.smartschool.be/Webservices/V3"" 
                    SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
                  <SOAP-ENV:Body>
                    <mns:getAccountPhoto>
                      <accesscode xsi:type=""xsd:string"">{soapApiKey}</accesscode>
                      <userIdentifier xsi:type=""xsd:string"">{identifier}</userIdentifier>
                    </mns:getAccountPhoto>
                  </SOAP-ENV:Body>
                </SOAP-ENV:Envelope>";

				var requestContent = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");
				var response = await _httpClient.PostAsync($"https://{school}.smartschool.be/Webservices/V3", requestContent);

				if (!response.IsSuccessStatusCode)
				{
					await Application.Current.MainPage.DisplayAlert("Error", $"(FOUT:{response.StatusCode}) Contacteer uw IT dienst", "OK");
					return null;
				}

				var responseContent = await response.Content.ReadAsStringAsync();
				var doc = XDocument.Parse(responseContent);
				var imageData = doc.Descendants("return").FirstOrDefault()?.Value;

				return imageData;
			}
			catch (HttpRequestException ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", $"Een onverwachte fout opgetreden bij de HTTP Request", "OK");
				return null;
			}
			catch (XmlException ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", $"Er is en onverwachte fout opgetreden bij het omzetten van de Soap reponse", "OK");
				return null;
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", $"Er is en onverwachte fout opgetreden", "OK");
				return null;
			}
		}

		public async Task<string> SetAccountPhotoAsync(string base64String, string userIdentifier)
		{
			var school = (await _secureStorageService.GetAsync("SchoolName"))?.Trim();
			var soapApiKey = await _secureStorageService.GetAsync("SoapApiKey");

			if (string.IsNullOrWhiteSpace(school) || string.IsNullOrWhiteSpace(soapApiKey))
			{
				await Application.Current.MainPage.DisplayAlert("Error", $"1 van de gegevens is leeg, gelieve de IT dienst te contacteren", "OK");
				return null;
			}

			try
			{
				string xmlString = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
                <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" 
                                   xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
                                   xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" 
                                   xmlns:mns=""https://{school}.smartschool.be/Webservices/V3"">
                  <SOAP-ENV:Body>
                    <mns:setAccountPhoto>
                      <accesscode xsi:type=""xsd:string"">{soapApiKey}</accesscode>
                      <userIdentifier xsi:type=""xsd:string"">{userIdentifier}</userIdentifier>
                      <photo xsi:type=""xsd:string"">{base64String}</photo>
                    </mns:setAccountPhoto>
                  </SOAP-ENV:Body>
                </SOAP-ENV:Envelope>";

				var requestContent = new StringContent(xmlString, Encoding.UTF8, "text/xml");
				var response = await _httpClient.PostAsync($"https://{school}.smartschool.be/Webservices/V3", requestContent);

				if (!response.IsSuccessStatusCode)
				{
					await Application.Current.MainPage.DisplayAlert("Error", $"Er liep iets mis bij het versturen naar de Soap Api ", "OK");
					return null;
				}
				
				var responseContent = await response.Content.ReadAsStringAsync();
				return responseContent;
			}
			catch (HttpRequestException ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", $"Een onverwachte fout opgetreden bij de HTTP Request, FOUT: {ex}", "OK");
				return null;
			}
			catch (XmlException ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", $"Er is en onverwachte fout opgetreden bij het omzetten van de Soap reponse, FOUT: {ex}", "OK");
				return null;
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", $"Er is en onverwachte fout opgetreden, FOUT: {ex}", "OK");
				return null;
			}
		}
	}
}

