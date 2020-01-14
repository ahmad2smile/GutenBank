using GutenBank.TestE2E.Services;
using System;
using System.Timers;

namespace GutenBank.TestE2E
{
    class Program
    {
        private static DataService _dataService = new DataService();
        private static UiService _uiService = new UiService();
        private static Random _randomNumber = new Random();

        static void Main(string[] args)
        {
            _uiService.Init();

            _uiService.OnRefresh += RefreshHandler;

            Console.ReadLine();
        }

        private static async void RefreshHandler(object sender, Timer timer)
        {
            timer.Stop();

            var testStatus = new TestStatus
            {
                ConcurrentBalanceRequest = _randomNumber.Next(300),
                ConcurrentUpdateRequest = _randomNumber.Next(10),
                Message = "Starting new requests..."
            };

            _uiService.Update(testStatus);

            var getResults = await _dataService.SetupGetRequests(testStatus.ConcurrentBalanceRequest);

            var updateResults = await _dataService.SetupUpdateRequests(testStatus.ConcurrentUpdateRequest);

            testStatus.BalanceResult = getResults;
            testStatus.UpdateResult = updateResults;
            testStatus.Message = "Requests Completed...";

            _uiService.Update(testStatus);

            timer.Start();
        }
    }
}
