using CDRLibrary.Models;

namespace CDRManager.Services.Interfaces;

public interface IRegisterCallService
{
    Task<List<CallDetailRecord>> ProcessFileAsync(IFormFile file);
}
