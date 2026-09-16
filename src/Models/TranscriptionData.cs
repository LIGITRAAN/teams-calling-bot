using System;
using System.Collections.Generic;

namespace TeamsCallingBot.Models
{
    public class TranscriptionData
    {
        public string CallId { get; set; }
        public string UserId { get; set; }
        public string CampaignId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<TranscriptionSegment> Segments { get; set; } = new List<TranscriptionSegment>();
        public string FullTranscription { get; set; }
    }

    public class TranscriptionSegment
    {
        public string Speaker { get; set; } // "bot" o "user"
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public double Confidence { get; set; }
    }
}
