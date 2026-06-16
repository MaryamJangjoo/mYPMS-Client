namespace mYPMS.Models;

// Maps 1:1 to FastAPI JSON response:
// { "success": true, "plate": "12ب34567", "confidence": 0.91, "ocr_chars": 8 }
public class AlprResponse
{
    public bool   success    { get; set; }
    public string plate      { get; set; } = string.Empty;
    public double confidence { get; set; }
    public int    ocr_chars  { get; set; }
}