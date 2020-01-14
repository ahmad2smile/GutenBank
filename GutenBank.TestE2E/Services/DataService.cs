using GutenBank.Domain;
using GutenBank.Models;
using GutenBank.TestE2E.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GutenBank.TestE2E.Services
{
    public class DataService
    {
        private readonly HttpClient _client;

        public DataService()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:49966/account/")
            };
        }

        public async Task<HttpStatusCode> GetBalance(int? accountNumber = 1)
        {
            var response = await _client.GetAsync($"balance?accountNumber={accountNumber}");

            return response.StatusCode;
        }

        public async Task<HttpStatusCode> PostDeposit(int accountNumber = 1, decimal amount = 100)
        {
            var transaction = new TransactionDTO
            {
                AccountNumber = accountNumber,
                Amount = amount,
                Currency = Currency.Dollar.ToString()
            };

            var json = JsonConvert.SerializeObject(transaction);

            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("deposit", data);

            return response.StatusCode;
        }

        public async Task<HttpStatusCode> PostWithdraw(int accountNumber = 1, decimal amount = 100)
        {
            var transaction = new TransactionDTO
            {
                AccountNumber = accountNumber,
                Amount = amount,
                Currency = Currency.Dollar.ToString()
            };

            var json = JsonConvert.SerializeObject(transaction);

            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("withdraw", data);

            return response.StatusCode;
        }

        public async Task<RequestResult> SetupGetRequests(int requests)
        {
            var tasks = new List<Task<HttpStatusCode>>();

            for (int i = 0; i < requests; i++)
            {
                tasks.Add(GetBalance());
            }

            var results = await Task.WhenAll(tasks.ToArray());

            var (success, failed) = CalculateResults(results);

            return new RequestResult
            {
                SuccessRequest = success,
                FailedRequest = failed
            };
        }

        public async Task<RequestResult> SetupUpdateRequests(int requests)
        {
            var tasks = new List<Task<HttpStatusCode>>();

            for (int i = 0; i < requests; i++)
            {
                tasks.Add(PostDeposit());
            }

            var results = await Task.WhenAll(tasks.ToArray());

            var (success, failed) = CalculateResults(results);

            return new RequestResult
            {
                SuccessRequest = success,
                FailedRequest = failed
            };
        }

        private (int success, int failed) CalculateResults(HttpStatusCode[] results)
        {
            int success = 0;
            int failed = 0;

            foreach (var res in results)
            {
                if (res == HttpStatusCode.OK)
                {
                    success += 1;
                }
                else
                {
                    failed += 1;
                }
            }

            return (success, failed);
        }
    }
}
