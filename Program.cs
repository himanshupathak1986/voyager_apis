using System;
using WireMock.Server;
using WireMock.Settings;
using System.Web;
using System.Net;
using System.Collections.Generic;
using WireMock.Matchers.Request;
using WireMock.Matchers;
using WireMock.Models;
using WireMock.RequestBuilders;
using WireMock.Types;
using WireMock.Util;
using WireMock.ResponseBuilders;
using WireMock.Settings;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;


namespace Voyager_test
{
    class Program
    {
        //stub fo the git api GET /codes_of_conduct
        //URL of the above api - "https://api.github.com/codes_of_conduct/citizen_code_of_conduct",
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var stub = get_server();
            set_data_on_home_page(stub);

            //Test 1
            test_api_returns_200_for_good_input();

            //Test 2
            test_api_returns_404_for_extended_url();

            //Test 3
            test_api_returns_400_on_making_post_call();

            Console.WriteLine("Press any key to stop the server");
            Console.ReadLine();
            stub.Stop();

        }

        private static void set_data_on_home_page(WireMockServer server)
        {
            var responseData = new List<test_response> {
                new test_response {key = "citizen_code_of_conduct", url = "http://citizencodeofconduct.org/"},
                new test_response {key = "contributor_covenant", url = "https://www.contributor-covenant.org/version/2/0/code_of_conduct/"},
            };

            var jsonObject = JsonConvert.SerializeObject(responseData);

                server
                .Given(
                    Request.Create()
                        .WithPath("/codes_of_conduct")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithBody(jsonObject)
                );

                server.Given(
                    Request.Create()
                        .WithPath("/codes_of_conduct")
                        .UsingPost()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(400)
                        .WithBody(JsonConvert.SerializeObject("Post is not allowed"))
                );
        }

        private static WireMockServer get_server()
        {
            return WireMockServer.Start(new WireMockServerSettings
            {
                Urls = new[] { "http://+:5001" },
                StartAdminInterface = true
            });
        }

        private class test_response
        {
            public string key {get;set;}
            public string url {get;set;}
        }


        //This test is passing
        public static void test_api_returns_200_for_good_input()
        {
            HttpClient client = new HttpClient();
                 HttpResponseMessage response =  client.GetAsync("http://localhost:5001/codes_of_conduct").Result;
                 //ASSERT. status code . This is console application, but we can put assertions in unit test project
                 response.EnsureSuccessStatusCode();
                if(response.StatusCode != HttpStatusCode.OK)
                 {
                     Console.WriteLine("/n test_api_returns_200_for_good_input FAILED Test");
                 }
                 else{
                        Console.WriteLine("/n test_api_returns_200_for_good_input -Passed Test");
                 }
        }

        //This test is passing
        public static void test_api_returns_404_for_extended_url()
        {
            HttpClient client = new HttpClient();
                 HttpResponseMessage response =  client.GetAsync("http://localhost:5001/codes_of_conduct/ExtendedURK").Result;
                 //ASSERT. status code . This is console application, but we can put assertions in unit test project
                 if(response.StatusCode != HttpStatusCode.NotFound)
                 {
                     Console.WriteLine("/n test_api_returns_404_for_extended_url - FAILED Test");
                 }
                 else
                 {
                     Console.WriteLine("/n test_api_returns_404_for_extended_url -Passed Test");
                 }
        }

        //This test is passing
        public static void test_api_returns_400_on_making_post_call()
        {
            var person = "name:coder";
            var json = JsonConvert.SerializeObject(person);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();

                 HttpResponseMessage response =  client.PostAsync("http://localhost:5001/codes_of_conduct", data).Result;
                 //ASSERT. status code . This is console application, but we can put assertions in unit test project
                 if(response.StatusCode != HttpStatusCode.BadRequest)
                 {
                     Console.WriteLine("/n test_api_returns_400_on_making_post_call - FAILED Test");
                 }
                 else
                 {
                     Console.WriteLine("/n test_api_returns_400_on_making_post_call -Passed Test");
                 }
        }
    }
}
