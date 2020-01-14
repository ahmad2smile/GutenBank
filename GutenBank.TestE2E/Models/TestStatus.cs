using GutenBank.TestE2E.Models;

namespace GutenBank.TestE2E
{
    public class TestStatus
    {
        public int ConcurrentBalanceRequest { get; set; }
        public RequestResult BalanceResult { get; set; } = new RequestResult();

        public int ConcurrentUpdateRequest { get; set; }
        public RequestResult UpdateResult { get; set; } = new RequestResult();

        public string Message { get; set; } = "In Progress...";
    }
}
