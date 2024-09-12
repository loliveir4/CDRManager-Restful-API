using Microsoft.AspNetCore.Mvc;

namespace CDRManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CDRController : ControllerBase
{
    private readonly IRegisterCallService _registerService;
    private readonly IDbService _dbService;
    private readonly ILogger<CDRController> _logger;

    public CDRController(IRegisterCallService registerService, IDbService dbService, ILogger<CDRController> logger)
    {
        _registerService = registerService;
        _dbService = dbService;
        _logger = logger;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("The provided file is null or empty.");

        if (!file.FileName.EndsWith(".csv"))
            return BadRequest("Invalid file format. Please upload a .csv file.");

        try
        {
            var (cdrRecords, errorLines) = await _registerService.ProcessFileAsync(file);

            if (cdrRecords == null || cdrRecords.Count == 0)
            {
                return BadRequest("File processing resulted in no valid CDR records.");
            }

            await _dbService.AddCdrRecordsAsync(cdrRecords);

            var response = new
            {
                message = "CDR file uploaded and processed successfully.",
                recordCount = cdrRecords.Count,
                errorCount = errorLines.Count,
                errors = errorLines.Count > 0 ? errorLines : null
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during file upload");
            return StatusCode(500, "An internal error occurred while processing the file. Please try again later.");
        }
    }

    [HttpGet("search-ref")]
    public async Task<IActionResult> FilterByRef([FromQuery] string reference)
    {
        if (string.IsNullOrWhiteSpace(reference))
        {
            return BadRequest("Reference cannot be null or empty.");
        }
        try
        {
            var cdrResult = await _dbService.SearchByReferenceAsync(reference);

            if (cdrResult == null)
            {
                return NotFound($"No CDR found with reference: {reference}");
            }

            return Ok(cdrResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while searching CDR with reference: {reference}");
            return StatusCode(500, "An error occurred while searching for the CDR.");
        }
    }

    [HttpGet("call-summary")]
    public async Task<IActionResult> GetCallSummary([FromQuery] CallSumary callSumaryFilter)
    {
        if (callSumaryFilter == null)
        {
            return BadRequest("Invalid search parameters.");
        }
        try
        {

            var monthResult = await _dbService.GetCallSummary(callSumaryFilter);

            return Ok(monthResult);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid search period.");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving call summary.");
            return StatusCode(500, "An error occurred while retrieving the call summary.");
        }
    }

    [HttpGet("cdr-per-caller")]
    public async Task<IActionResult> SearchCdrsForCaller([FromQuery] CallerFilter searchPerMonth)
    {
        if (searchPerMonth == null)
        {
            return BadRequest("Invalid search parameters.");
        }

        try
        {

            var monthResult = await _dbService.GetCdrsForCaller(searchPerMonth);

            if (monthResult == null || monthResult.Count == 0)
            {
                return NotFound("No CDR records found for the given period.");
            }

            return Ok(monthResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving call summary.");
            return StatusCode(500, "An error occurred while retrieving the call summary.");
        }
    }

    [HttpGet("most-expensive-calls")]
    public async Task<IActionResult> GetMostExpensiveCalls([FromQuery] CallerFilter callSumaryFilter, [FromQuery] int N)
    {
        if (callSumaryFilter == null)
        {
            return BadRequest("Invalid search parameters.");
        }
        if (N <= 0)
        {
            return BadRequest("N must be a positive number.");
        }
        try
        {

            var callerSumaryFilter = new CallerFilter
            {
                CallerId = callSumaryFilter.CallerId,
                InicialDate = callSumaryFilter.InicialDate,
                FinalDate = callSumaryFilter.FinalDate,
                CallType = callSumaryFilter.CallType
            };

            var result = await _dbService.GetMostExpensiveCalls(callerSumaryFilter, N);

            if (result == null || result.Count == 0)
            {
                return NotFound("No calls found for the given criteria.");
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

}
