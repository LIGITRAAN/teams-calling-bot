using System;
using System.Collections.Generic;

namespace TeamsCallingBot.Models
{
    public class CallResponse
    {
        /// <summary>
        /// ID único de la llamada
        /// </summary>
        public string CallId { get; set; }

        /// <summary>
        /// Estado de la llamada: initiated, ringing, connected, completed, failed
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Duración de la llamada en segundos
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Transcripción completa de la llamada
        /// </summary>
        public string Transcription { get; set; }

        /// <summary>
        /// Datos recopilados durante la llamada
        /// </summary>
        public Dictionary<string, string> CollectedData { get; set; }

        /// <summary>
        /// Razón del error si la llamada falló
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Timestamp de inicio de la llamada
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Timestamp de fin de la llamada
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// URL de la grabación de audio
        /// </summary>
        public string RecordingUrl { get; set; }
    }
}
