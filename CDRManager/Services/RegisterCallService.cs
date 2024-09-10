using System.Globalization;

namespace CDRManager.Services;

public class RegisterCallService : IRegisterCallService
{
    private readonly List<CallDetailRecord> _cdrRecords = new List<CallDetailRecord>();

    public async Task<List<CallDetailRecord>> ProcessFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Invalid file.");

        using (var stream = new StreamReader(file.OpenReadStream()))
        {
            string? line;

            while ((line = await stream.ReadLineAsync()) != null)
            {
                var values = line.Split(',');

                if (values.Length != 8) //It needs to contain 8 parameters
                    throw new ArgumentException("Incorrect number of columns in the file.");

                var cdr = new CallDetailRecord
                {
                    CallerId = values[0],
                    Recipient = values[1],
                    CallDate = DateTime.ParseExact(NormalizeDate(values[2]), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    EndTime = TimeSpan.Parse(values[3]),
                    Duration = int.Parse(values[4]),
                    Cost = decimal.Parse(values[5]),
                    Reference = values[6],
                    Currency = values[7],
                    Type = VerifyDDD(values[1])
                };

                _cdrRecords.Add(cdr);
            }
        }

        return _cdrRecords;
    }

    //Method to fix wrong dates
    private static string NormalizeDate(string date)
    {
        var parts = date.Split('/');

        // Normalize day and month by adding leading zeros if necessary
        string day = parts[0].Length == 1 ? "0" + parts[0] : parts[0];
        string month = parts[1].Length == 1 ? "0" + parts[1] : parts[1];
        string year = parts[2];

        return $"{day}/{month}/{year}";
    }

    //Method to set if is Domestic or International bases on the DDD 44
    private static CallType VerifyDDD(string Number)
    {
        if (Number.Length >= 2)
        {
            string ddd = Number.Substring(0, 2);

            if (ddd == "44")
            {
                return CallType.Domestic;
            }
        }

        return CallType.International;
    }
}
