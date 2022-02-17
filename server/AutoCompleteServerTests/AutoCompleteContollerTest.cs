using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Server.Controllers;
using Server.Interfaces;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCompleteServerTests
{
    [TestClass]
    public class AutoCompleteTest
    {
        private static Random random = new Random();
        HttpClient httpClient = new HttpClient();

        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnop";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public class GetSuggestionsWrapper
        {
            public GetSuggestionsWrapper(string prefix, HttpClient httpClient)
            {
                _prefix = prefix;
                _httpClient = httpClient;
            }

            string _prefix;
            HttpClient _httpClient;
            public HttpResponseMessage Response { get; set; }

            public async Task<HttpResponseMessage> CallToGetSuggestionsAsync()
            {
                HttpResponseMessage res = new HttpResponseMessage();
                try
                {
                    res = await _httpClient.GetAsync($"http://localhost:8888/api/auto-complete/get-suggestions?prefix={_prefix}");
                }
                catch(Exception ex)
                {
                    return new HttpResponseMessage();
                }
                Response = res;
                return res;
            }
        }

        [TestMethod]
        public async Task Get_Suggestions_Route_Load_Test()
        {
            //// prepare for dummy DI
            //ILogger<AutoCompleteService> logger = Mock.Of<ILogger<AutoCompleteService>>();
            //IAutoCompleteService autoCompleteService = new AutoCompleteService(logger);

            //// dummy initialize
            //var ct = new CancellationToken();
            //autoCompleteService.StartAsync(ct).Wait();

            // create the dummy controller
            ServicePointManager.DefaultConnectionLimit = 200;
            List<GetSuggestionsWrapper> autoCompleteWrappers = new List<GetSuggestionsWrapper>();

            for (int i = 0; i < 10000; i++)
            {
                string rand = RandomString(3);
                var wrapper = new GetSuggestionsWrapper(rand, httpClient);
                autoCompleteWrappers.Add(wrapper);
            }

            // count the time takes to all the tasks to finished
            var st = new Stopwatch();
            st.Start();
            await Task.WhenAll(autoCompleteWrappers.Select(wr => wr.CallToGetSuggestionsAsync()));
            st.Stop();
            httpClient.Dispose();

            var totalSeconds = st.ElapsedMilliseconds / 1000;
            var singleRequestAverageTimeSeconds = totalSeconds / 10000;

            Console.WriteLine("Total seconds for 1000000 requests:", totalSeconds);
            Console.WriteLine($"average time for single request: {singleRequestAverageTimeSeconds}s");
            Assert.IsTrue(autoCompleteWrappers.All(v => v.Response.IsSuccessStatusCode));
            Assert.IsTrue(totalSeconds <= 10);
        }
    }
}
