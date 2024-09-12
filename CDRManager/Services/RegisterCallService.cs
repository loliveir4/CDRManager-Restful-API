using System.Globalization;

namespace CDRManager.Services;

public class RegisterCallService : IRegisterCallService
{
    private const int ExpectedColumnCount = 9; // Constant to avoid magic number

    /// <summary>
    /// Reads the provided file, parses its content, and returns a list of Call Detail Records (CDRs) and List of the records with error.
    /// The file is expected to have 9 columns per line, each representing a specific call detail.
    /// </summary>
    public async Task<(List<CallDetailRecord>, List<string>)> ProcessFileAsync(IFormFile file)
    {
        var successfulRecords = new List<CallDetailRecord>();
        var errorLines = new List<string>();

        using (var stream = new StreamReader(file.OpenReadStream()))
        {
            string? line;
            int lineNumber = 0; // Para rastrear a linha atual

            while ((line = await stream.ReadLineAsync()) != null)
            {
                lineNumber++;
                var values = line.Split(',');

                if (values.Length != ExpectedColumnCount)
                {
                    errorLines.Add($"Line {lineNumber}: Incorrect number of columns (expected {ExpectedColumnCount}).");
                    continue;
                }

                try
                {
                    var cdr = new CallDetailRecord
                    {
                        CallerId = values[0],
                        Recipient = values[1],
                        CallDate = DateTime.ParseExact(NormalizeDate(values[2]), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        EndTime = TimeSpan.Parse(values[3]),
                        Duration = int.Parse(values[4]),
                        Cost = decimal.Parse(values[5], CultureInfo.InvariantCulture),
                        Reference = values[6],
                        Currency = values[7],
                        Type = (CallType)int.Parse(values[8])
                    };

                    successfulRecords.Add(cdr);
                }
                catch (FormatException ex)
                {
                    errorLines.Add($"Line {lineNumber}: Error parsing line: {line} - {ex.Message}");
                }
            }
        }

        // Retornar as linhas válidas e as linhas que geraram erros
        return (successfulRecords, errorLines);
    }

    /// <summary>
    /// Ensures that the provided date string is in the correct format by normalizing
    /// the day and month to always have two digits (i.e., adding leading zeros if necessary).
    /// </summary>
    private static string NormalizeDate(string date)
    {
        var parts = date.Split('/');

        // Normalize day and month by adding leading zeros if necessary
        string day = parts[0].Length == 1 ? "0" + parts[0] : parts[0];
        string month = parts[1].Length == 1 ? "0" + parts[1] : parts[1];
        string year = parts[2];

        return $"{day}/{month}/{year}";
    }

    /// <summary>
    /// Determines the call type (Domestic or International) based on the DDD (area code)
    /// extracted from the phone number.
    /// </summary>
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
