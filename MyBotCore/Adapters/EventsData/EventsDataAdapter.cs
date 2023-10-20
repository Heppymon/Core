using Microsoft.Extensions.Options;
using MyBotCore.Shared.Interfaces.Adapters;
using MyBotCore.Shared.Settings;
using Newtonsoft.Json;

namespace MyBotCore.Adapters.EventsData
{
    public class EventsDataAdapter : IEventsDataAdapter
    {
        private readonly EDAdapterSettings adapterSettings;
        private readonly string testString = "{\"current_page\":1,\"data\":[{\"id\":2186,\"name\":\"ЖЕНСКИЙ УЖИН «ПРОЯВЛЕННОСТЬ»\",\"excerpt\":\"Возможность найти подруг, партнеров, получить ответ на волнующий вопрос\",\"post\":\"ЖЕНСКИЙ УЖИН «ПРОЯВЛЕННОСТЬ»\\r\\n\\r\\nВозможность найти подруг, партнеров, получить ответ на волнующий вопрос и зарядиться женской атмосферой \\r\\n\\r\\n▫️Программа: \\r\\n\\r\\n- знакомство, где ты можешь рассказать о себе, своих услугах и прям продать их, я в этом помогаю с помощью наводящих вопросов \\r\\n\\r\\n- практика на сближение, которая поможет сделать атмосферу более теплой, узнать ценности друг друга\\r\\n\\r\\n- разберем тему проявленности, по желанию разберу индивидуально твою ситуацию, выясним что мешает тебе проявляться, больше зарабатывать и как из этого выйти \\r\\n\\r\\n📍ужин пройдёт 19 октября с 17:00 до 19:00 \\r\\nресторан Плакучая Ива\\r\\n\\r\\n💸стоимость 1.111р, всего 8 мест \\r\\n\\r\\nЗабронировать можно написав мне в тг @Ollyhaappy\\r\\n\\r\\nОтзывы, атмосферу можно посмотреть в инст you.magic.sochi\",\"price\":\"1111\",\"place\":\"Ресторан Плакучая Ива\",\"address\":\"Сочи, ул. Войкова, 3\",\"photo\":\"events/xnGgZ3PgWW9bHM7H1jRYiOHZ6xOia2szExNdJKxi.jpg\",\"start_at\":\"2023-10-19T14:00:00.000000Z\",\"end_at\":\"2023-10-19T16:00:00.000000Z\",\"user_id\":67,\"created_at\":\"2023-10-17T18:03:51.000000Z\",\"updated_at\":\"2023-10-17T18:22:20.000000Z\",\"link\":\"https://t.me/ollyhaappy\",\"photo_width\":720,\"photo_height\":1280,\"photo_color\":\"#3d3227\",\"user\":{\"id\":67,\"name\":\"Пиксель\",\"logo\":\"user_logos/6ORE8SQEdXeXRCHve2mJq1S7WQkl4Py56tJpInqg.jpg\"}},{\"id\":244,\"name\":\"ДОМ-МУЗЕЙ А. Х. ТАММСААРЕ.\",\"excerpt\":\"ВЫСТАВОЧНАЯ ЭКСПОЗИЦИЯ\",\"post\":\"Прием посетителей с 10:00 до 17:00\\r\\nВыходной — среда\\r\\n\\r\\nПоследний вторник каждого месяца музей закрыт на санитарно-профилактический день\",\"price\":\"100\",\"place\":\"ЛИТЕРАТУРНО-МЕМОРИАЛЬНЫЙ МУЗЕЙ ИМ. Н. ОСТРОВСКОГО - ДОМ - МУЗЕЙ А.Х. ТАММСААРЕ\",\"address\":\"Эстонская ул., 35, Эстосадок, Краснодарский край, 354392\",\"photo\":\"events/sxcKuRwrmbb5mFB9jCZt7OAh4gLfqpKzBQy1clyU.png\",\"start_at\":\"2023-04-01T04:00:00.000000Z\",\"end_at\":\"2024-11-30T11:00:00.000000Z\",\"user_id\":59,\"created_at\":\"2023-03-19T15:51:14.000000Z\",\"updated_at\":\"2023-10-01T12:53:29.000000Z\",\"link\":\"https://sochi.kassir.ru/muzey/literaturno-memorialnyiy-muzey-im-n-ostrovskogo/dom-muzey-a-h-tammsaare-vyistavochnaya-ekspozitsiya_2023-04-01\",\"photo_width\":216,\"photo_height\":314,\"photo_color\":\"#cad0d3\",\"user\":{\"id\":59,\"name\":\"Pixel - Афиша\",\"logo\":\"user_logos/2pfmtnPz4BvKjr5wtzWxxvbLYHVDwHS4YWR28oRO.png\"}},{\"id\":228,\"name\":\"Русское искусство ХIХ-ХХ вв.\",\"excerpt\":\"Постоянная экспозиция\",\"post\":\"Масштабную и яркую картину развития русского изобразительного искусства представляет постоянно действующая выставка «Русское искусство ХIХ-ХХ вв.», которая знакомит с произведениями, представляющими разнообразие тенденций в русском изобразительном искусстве этого периода (классицизм, романтизм, реализм и зарождающийся в первой половине XX века социалистический реализм)\",\"price\":\"300\",\"place\":\"Сочинский художественный музей\",\"address\":\"Адрес: 354000, Краснодарский край,  город Сочи,  Курортный проспект 51\",\"photo\":\"events/V9fgIZAL3HDKoPvZEeSWTaLiwReIrjbJjb8VGAbB.jpg\",\"start_at\":\"2023-03-18T18:00:00.000000Z\",\"end_at\":\"2025-02-27T18:00:00.000000Z\",\"user_id\":59,\"created_at\":\"2023-03-18T16:08:05.000000Z\",\"updated_at\":\"2023-10-01T12:53:30.000000Z\",\"link\":\"http://sochiartmuseum.com/direction/1/\",\"photo_width\":1232,\"photo_height\":562,\"photo_color\":\"#c5d0b5\",\"user\":{\"id\":59,\"name\":\"Pixel - Афиша\",\"logo\":\"user_logos/2pfmtnPz4BvKjr5wtzWxxvbLYHVDwHS4YWR28oRO.png\"}}],\"first_page_url\":\"https://api.pixelafisha.com/api/events?page=1\",\"from\":1,\"last_page\":1,\"last_page_url\":\"https://api.pixelafisha.com/api/events?page=1\",\"links\":[{\"url\":null,\"label\":\"&laquo; Previous\",\"active\":false},{\"url\":\"https://api.pixelafisha.com/api/events?page=1\",\"label\":\"1\",\"active\":true},{\"url\":null,\"label\":\"Next &raquo;\",\"active\":false}],\"next_page_url\":null,\"path\":\"https://api.pixelafisha.com/api/events\",\"per_page\":1000,\"prev_page_url\":null,\"to\":157,\"total\":157}";

        public EventsDataAdapter(IOptions<EDAdapterSettings> adapterSettings)
        {
            this.adapterSettings = adapterSettings.Value;
        }

        public async Task<List<EventDataModel>> GetEvents()
        {
            // Temporal solution: Just for not overload a eventData web service
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Local";
            string bodyResult;

            if (environment.Equals("Local"))
                bodyResult = testString;
            else
            {
                var httpClient = new HttpClient();
                var uri = new Uri(new Uri(adapterSettings.RootApiUrl), adapterSettings.GetEventsApi);
                var response = await httpClient.GetAsync(uri) ?? throw new Exception("Adapter returns null response");
                bodyResult = await response.Content.ReadAsStringAsync();
            }

            var result = JsonConvert.DeserializeObject<EventsFullPageModel>(bodyResult)
                ?? throw new Exception($"Get result of {adapterSettings.GetEventsApi}/{adapterSettings.GetEventsApi} was empty");
            return result?.data;
        }
    }
}
