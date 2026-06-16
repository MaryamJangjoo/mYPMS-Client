namespace mYPMS.Models;

public class AlprOptions
{
    public string BaseUrl        { get; set; } = "http://192.168.88.90:8000";
    public double MinConfidence  { get; set; } = 0.4;
    public int    TimeoutSeconds { get; set; } = 10;
}
