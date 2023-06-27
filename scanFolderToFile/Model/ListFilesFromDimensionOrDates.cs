using System;

namespace ScanFolderToFile.Model
{
    public class ListFilesFromDimensionOrDates
    {
        public Dimensions Dimensions { get; set; }
        public Dates Dates { get; set; }

        public ListFilesFromDimensionOrDates()
        {
            Dates = null;
            Dimensions = null;
        }
    }

    public class Dimensions
    {
        public decimal? DimensionMin { get; set; } = null;
        public decimal? DimensionMax { get; set; } = null;
    }

    public class Dates
    {
        public DateTime? StartDate { get; set; } = null;
        public DateTime? EndDate { get; set; } = null;
    }
}
