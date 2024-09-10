using System.ComponentModel.DataAnnotations;

namespace CDRLibrary.Models;

public class CallDetailRecord
{
    public int ID { get; set; }
    public required string CallerId { get; set; }
    public required string Recipient { get; set; }
    public DateTime CallDate { get; set; } 
    public TimeSpan EndTime { get; set; }
    public int Duration { get; set; }
    public decimal Cost { get; set; }
    public required string Reference { get; set; }
    public required string Currency { get; set; }
    public required CallType Type { get; set; }
}
