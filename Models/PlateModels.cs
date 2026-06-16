using System;
using System.Collections.Generic;

namespace mYPMS.Models
{
    public class PlateResponse
    {
        public bool Success { get; set; }
        public List<PlateInfo> Data { get; set; } = new();
        public int Total { get; set; }
    }

    public class PlateInfo
    {
        public string Plate { get; set; } = string.Empty;
        public double Confidence { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CameraIdentifier { get; set; } = string.Empty;
        public string SnapshotPath { get; set; } = string.Empty;
    }

    public class WebSocketPlateMessage
    {
        public string Type { get; set; } = string.Empty;
        public PlateData Data { get; set; } = new();
    }

    public class PlateData
    {
        public string Plate { get; set; } = string.Empty;
        public double Confidence { get; set; }
        public DateTime Timestamp { get; set; }
        public string CameraId { get; set; } = string.Empty;
        public string SnapshotPath { get; set; } = string.Empty;
    }
}