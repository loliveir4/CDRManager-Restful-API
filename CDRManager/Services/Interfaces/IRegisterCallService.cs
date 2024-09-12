using CDR.Core.Models;

namespace CDRManager.Services.Interfaces;

public interface IRegisterCallService
{
    Task<(List<CallDetailRecord>, List<string>)> ProcessFileAsync(IFormFile file);
}
