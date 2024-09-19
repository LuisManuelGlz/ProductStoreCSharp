namespace ProductStoreCSharp.TestSource
{
    public class LoginTestSource
    {
        private static readonly string csvDirectoryPath = $"{TestContext.CurrentContext.WorkDirectory}/TestData";
        
        public static IEnumerable<TestCaseData> SuccessfulLoginShowsWelcomeMessageTestData()
        {
            var csvFilePath = Path.Combine(csvDirectoryPath, "TC-2-Data.csv");
            var csvLines = File.ReadAllLines(csvFilePath);
            foreach (var line in csvLines.Skip(1))
            {
                var values = line.Split(',');
                yield return new TestCaseData(values[0], values[1])
                    .SetName($"TC-2: Text \"Welcome {values[0]}\" should appear after successful login");
            }
        }

        public static IEnumerable<TestCaseData> FailedLoginShowsWarning()
        {
            var csvFilePath = Path.Combine(csvDirectoryPath, "TC-3-Data.csv");
            var csvLines = File.ReadAllLines(csvFilePath);
            foreach (var line in csvLines.Skip(1))
            {
                var values = line.Split(',');
                yield return new TestCaseData(values[0], values[1])
                    .SetName($"TC-3: Warning message is displayed after a failed login (Password: {values[1]})");
            }
        }
    }
}
