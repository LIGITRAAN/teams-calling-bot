using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using TeamsCallingBot.Models;
using Newtonsoft.Json;

namespace TeamsCallingBot.Services
{
    public class DataService
    {
        private readonly BlobContainerClient _containerClient;

        public DataService(string connectionString, string containerName)
        {
            var client = new BlobContainerClient(new Uri(connectionString), containerName);
            _containerClient = client;
        }

        /// <summary>
        /// Guarda la transcripción en Azure Blob Storage
        /// </summary>
        public async Task SaveTranscriptionAsync(TranscriptionData data)
        {
            try
            {
                var blobName = $"transcriptions/{data.CallId}/{DateTime.UtcNow:yyyy-MM-dd-HHmmss}.json";
                var blobClient = _containerClient.GetBlobClient(blobName);

                var json = JsonConvert.SerializeObject(data, Formatting.Indented);
                var bytes = System.Text.Encoding.UTF8.GetBytes(json);

                await blobClient.UploadAsync(
                    System.IO.BinaryData.FromBytes(bytes),
                    overwrite: true
                );
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar transcripción: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene una transcripción guardada
        /// </summary>
        public async Task<TranscriptionData> GetTranscriptionAsync(string callId)
        {
            try
            {
                var blobClient = _containerClient.GetBlobClient($"transcriptions/{callId}");
                var download = await blobClient.DownloadAsync();

                using (var stream = download.Value.Content)
                {
                    using (var reader = new System.IO.StreamReader(stream))
                    {
                        var json = await reader.ReadToEndAsync();
                        return JsonConvert.DeserializeObject<TranscriptionData>(json);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener transcripción: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Guarda datos de la llamada para auditoría
        /// </summary>
        public async Task SaveCallRecordAsync(CallResponse response)
        {
            try
            {
                var blobName = $"call-records/{response.CallId}.json";
                var blobClient = _containerClient.GetBlobClient(blobName);

                var json = JsonConvert.SerializeObject(response, Formatting.Indented);
                var bytes = System.Text.Encoding.UTF8.GetBytes(json);

                await blobClient.UploadAsync(
                    System.IO.BinaryData.FromBytes(bytes),
                    overwrite: true
                );
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar registro de llamada: {ex.Message}", ex);
            }
        }
    }
}
