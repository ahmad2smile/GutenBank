using GutenBank.TestE2E.Models;
using Konsole;
using System;
using System.Timers;

namespace GutenBank.TestE2E.Services
{
    public class UiService
    {
        private IConsole _headline;
        private IConsole _status;
        private IConsole _menu;
        private IConsole _content;

        public EventHandler<Timer> OnRefresh { get; set; }

        public void Init()
        {
            SetupConsole();

            var timer = new Timer(7000);
            timer.Elapsed += delegate (object sender, ElapsedEventArgs e) { OnRefresh?.Invoke(this, timer); };
            timer.Enabled = true;
            timer.Start();
        }

        private void SetupConsole()
        {
            var konsoleWindow = new Window();
            var consoles = konsoleWindow.SplitRows(
                    new Split(4, "Guten Bank", LineThickNess.Single),
                    new Split(0),
                    new Split(4, "Status", LineThickNess.Single)
            );

            _headline = consoles[0];
            _status = consoles[2];

            var contents = consoles[1].SplitColumns(
                    new Split(20),
                    new Split(0, "Content") { Foreground = ConsoleColor.White, Background = ConsoleColor.DarkGray }
            );
            _menu = contents[0];
            _content = contents[1];

            _headline.Write("Running E2E on Random Inputs...");
            _content.WriteLine("Results:");
            _menu.WriteLine("Concurrent Requests");
        }

        public void Update(TestStatus testOptions)
        {
            BalanceRequestResults(testOptions.BalanceResult);
            UpdateRequestResults(testOptions.UpdateResult);

            _menu.WriteLine($"GET Balance: {testOptions.ConcurrentBalanceRequest}");
            _menu.WriteLine($"UPDATE Balance: {testOptions.ConcurrentBalanceRequest}");

            _status.Write(testOptions.Message);
        }

        private void BalanceRequestResults(RequestResult result)
        {
            if (result.SuccessRequest > 0 || result.FailedRequest > 0)
            {
                var assertion = result.FailedRequest == 0;
                var resultText = assertion ? "SUCCESS" : "FAILED";
                var color = assertion ? ConsoleColor.Green : ConsoleColor.Red;

                _content.WriteLine($"Balance Success: {result.SuccessRequest}");
                _content.WriteLine($"Balance Failed: {result.FailedRequest}");
                _content.WriteLine(color, $"Balance Test: {resultText}");
            }
        }

        private void UpdateRequestResults(RequestResult result)
        {
            if (result.SuccessRequest > 0 || result.FailedRequest > 0)
            {
                var assertion = result.SuccessRequest > 0;
                var resultText = assertion ? "SUCCESS" : "FAILED";
                var color = assertion ? ConsoleColor.Green : ConsoleColor.Red;

                _content.WriteLine($"Balance Success: {result.SuccessRequest}");
                _content.WriteLine($"Balance Failed: {result.FailedRequest}");
                _content.WriteLine(color, $"Balance Test: {resultText}");
            }
        }
    }
}
