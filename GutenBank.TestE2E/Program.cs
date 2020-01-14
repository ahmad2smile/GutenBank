using GutenBank.TestE2E.Services;
using Konsole;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace GutenBank.TestE2E
{
    class Program
    {
        private static DataService _dataService = new DataService();

        static async Task Main(string[] args)
        {
            var totalRequests = 3;

            var tasks = new List<Task<HttpStatusCode>>();
            var bar = new ProgressBar(PbStyle.DoubleLine, totalRequests);

            for (int i = 0; i < totalRequests; i++)
            {
                // tasks.Add(_dataService.GetBalance());
                tasks.Add(_dataService.PostDeposit());
                //tasks.Add(_dataService.PostDeposit());
            }

            bar.Refresh(0, "");

            var result = await Task.WhenAll(tasks.ToArray());

            var success = 0;
            var failed = 0;

            foreach (var res in result)
            {
                if (res == HttpStatusCode.OK)
                {
                    success += 1;
                    bar.Refresh(success, $"{success} Success of 300");
                }
                else
                {
                    Console.WriteLine(res.ToString());

                    failed += 1;
                }
            }

            Console.WriteLine($"{failed} Failed of 300");
            Console.ReadLine();
        }
    }
}
