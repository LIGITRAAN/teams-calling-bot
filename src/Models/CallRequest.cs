using System;
using System.Collections.Generic;

namespace TeamsCallingBot.Models
{
    public class CallRequest
    {
        /// <summary>
        /// ID del usuario de Teams a llamar (email o UPN)
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// ID de la campaña o tipo de llamada
        /// </summary>
        public string CampaignId { get; set; }

        /// <summary>
        /// Mensaje de bienvenida del bot
        /// </summary>
        public string WelcomeMessage { get; set; }

        /// <summary>
        /// Preguntas a hacer durante la llamada
        /// </summary>
        public List<string> Questions { get; set; }

        /// <summary>
        /// URL de callback para notificaciones
        /// </summary>
        public string CallbackUrl { get; set; }

        /// <summary>
        /// Datos adicionales de contexto
        /// </summary>
        public Dictionary<string, string> ContextData { get; set; }

        /// <summary>
        /// Idioma de la llamada (default: es-ES)
        /// </summary>
        public string Language { get; set; } = "es-ES";

        /// <summary>
        /// Tiempo máximo de la llamada en segundos
        /// </summary>
        public int MaxDurationSeconds { get; set; } = 600; // 10 minutos
    }
}
