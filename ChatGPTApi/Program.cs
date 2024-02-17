using System.IO;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ChatGPTApi.Classes;


namespace ChatGPTApi
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Run();
            Thread.Sleep(Int32.MaxValue);
        }

        static async void Run()
        {
            // токен из личного кабинета
            //
            //string apiKey = "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";
            string apiKey = "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";
            // адрес api для взаимодействия с чат-ботом
            string endpoint = "https://api.openai.com/v1/chat/completions";
            // набор соообщений диалога с чат-ботом
            List<Message> messages = new List<Message>();
            // HttpClient для отправки сообщений
            var httpClient = new HttpClient();
            httpClient.Timeout = new TimeSpan(0, 5, 0); // 5 минут
            // устанавливаем отправляемый в запросе токен
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            //messages.Add(new Message() { Role = "system", Content = "Ты являешься персоонажем игры. Вот твое описание: Имя персонажа: Элизабет \"Эли\" Старблейд\r\n\r\nОписание внешности: Элизабет высокая и стройная женщина с длинными, темно-русскими волосами, которые она всегда носит заплетенными в косу. У нее зеленые глаза, которые всегда кажутся готовыми к новым открытиям. На ее правой руке есть таинственная татуировка в форме магической руны.\r\n\r\nХарактер и личность: Элизабет обладает страстью к исследованиям и знанием, и она всегда готова рисковать, чтобы раскрывать тайны мира. Она обладает сильной волей и уверенностью, но также является сострадательной и готова помогать тем, кто в беде. Ей свойственна острая интуиция и любопытство.\r\n\r\nПрошлое персонажа: Элизабет родилась в семье известных археологов и исследователей древних артефактов. Она потеряла родителей в молодом возрасте в результате таинственного инцидента в старинном подземелье. Этот инцидент стал главным стимулом для ее приключений и поиска правды.\r\n\r\nОсновная цель в приключении: Элизабет стремится раскроить тайны древних артефактов и магии. Ее основная цель - найти ответы о прошлом своей семьи и разгадать магические загадки, которые преследуют ее с детства.\r\n\r\nСпособности и навыки: Элизабет владеет знаниями о древних языках, археологии и магии. Она отличается выдающейся ловкостью и навыками владения мечом. Ее магические способности включают в себя управление элементами и общение с духами.\r\n\r\nЧто мотивирует персонажа и его цели: Мотивацией Элизабет служит жажда знаний и желание найти истину о судьбе своей семьи. Она также стремится использовать свои способности, чтобы помочь другим, борясь с тьмой и злом в мире Эльдриса." });
            //messages.Add(new Message()s


            //{
            //    Role = "system",
            //    Content = ""
            //})

            while (true)
            {
                // ввод сообщения пользователя
                Console.Write("User: ");
                var content = Console.ReadLine();

                // если введенное сообщение имеет длину меньше 1 символа
                // то выходим из цикла и завершаем программу
                if (content is null || content.Length == 0) break;
                // формируем отправляемое сообщение
                var message = new Message() { Role = "user", Content = content };
                // добавляем сообщение в список сообщений
                messages.Add(message);

                // формируем отправляемые данные
                var requestData = new Request()
                {
                    ModelId = "gpt-3.5-turbo",
                    Messages = messages,
                    Stream = false
                };
                // отправляем запрос
                using var response = await httpClient.PostAsJsonAsync(endpoint, requestData);

                // если произошла ошибка, выводим сообщение об ошибке на консоль
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"{(int)response.StatusCode} {response.StatusCode}");
                    break;
                }
                // получаем данные ответа


                //Console.Write("ChatGPT: ");
                //ReadMessage(response);
                //Console.WriteLine();

                string concatMessages = "";
                ResponseData? responseData = new();

                responseData = await response.Content.ReadFromJsonAsync<ResponseData>();

                var choices = responseData?.Choices ?? new List<Choice>();
                if (choices.Count == 0)
                {
                    Console.WriteLine("No choices were returned by the API");
                    continue;
                }
                var choice = choices[0];
                var responseMessage = choice.Message;
                // добавляем полученное сообщение в список сообщений
                messages.Add(responseMessage);
                var responseText = responseMessage.Content.Trim();
                Console.WriteLine($"ChatGPT: {responseText}");
            }

        }

        private static async void ReadMessage(HttpResponseMessage? response, Action<ResponseData>? callback = null)
        {
            var stream = await response.Content.ReadAsStreamAsync();
            await foreach (var data in StreamCompletion(stream))
            {
                var jsonString = data.Replace("data: ", "");
                if (string.IsNullOrWhiteSpace(jsonString)) continue;
                if (jsonString == "[DONE]") break;
                ResponseData? responseData = JsonSerializer.Deserialize<ResponseData>(jsonString);
                if (responseData is null) continue;
                callback?.Invoke(responseData);
                string deltaContent = responseData.Choices.FirstOrDefault()?.DeltaMessage.Content;
                Console.Write(deltaContent);
            }
        }
        private static async IAsyncEnumerable<string> StreamCompletion(Stream stream)
        {
            using var reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (line != null)
                {
                    yield return line;
                }
            }
        }
    }
}