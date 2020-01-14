using GutenBank.Domain;
using GutenBank.Models;
using Newtonsoft.Json;
using System;
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
    }
}
