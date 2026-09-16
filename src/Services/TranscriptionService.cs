using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using TeamsCallingBot.Models;

namespace TeamsCallingBot.Services
{
    public class TranscriptionService
    {
        private readonly string _speechKey;
        private readonly string _speechRegion;

        public TranscriptionService(string speechKey, string speechRegion)
        {
            _speechKey = speechKey;
            _speechRegion = speechRegion;
        }

        /// <summary>
        /// Convierte audio a texto usando Azure Speech Services
        /// </summary>
        public async Task<string> TranscribeAudioAsync(string audioFilePath)
        {
            try
            {
                var config = SpeechConfig.FromSubscription(_speechKey, _speechRegion);
                config.SpeechRecognitionLanguage = "es-ES";

                using (var audioConfig = AudioConfig.FromWavFileInput(audioFilePath))
                {
                    using (var recognizer = new SpeechRecognizer(config, audioConfig))
                    {
                        var result = await recognizer.RecognizeOnceAsync();

                        if (result.Reason == ResultReason.RecognizedSpeech)
                        {
                            return result.Text;
                        }
                        else if (result.Reason == ResultReason.NoMatch)
                        {
                            return "No se pudo reconocer el audio";
                        }
                        else if (result.Reason == ResultReason.Canceled)
                        {
                            var cancellation = CancellationDetails.FromResult(result);
                            return $"Error: {cancellation.Reason}";
                        }
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en transcripción: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Convierte texto a voz para reproducir durante la llamada
        /// </summary>
        public async Task<byte[]> SynthesizeSpeechAsync(string text, string language = "es-ES")
        {
            try
            {
                var config = SpeechConfig.FromSubscription(_speechKey, _speechRegion);
                config.SpeechSynthesisLanguage = language;

                using (var synthesizer = new SpeechSynthesizer(config, null))
                {
                    var result = await synthesizer.SpeakTextAsync(text);

                    if (result.Reason == ResultReason.SynthesizingAudioCompleted)
                    {
                        return result.AudioData;
                    }
                    else
                    {
                        throw new Exception($"Error en síntesis: {result.Reason}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en síntesis de voz: {ex.Message}", ex);
            }
        }
    }
}
