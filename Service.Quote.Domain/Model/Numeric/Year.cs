using System;

namespace Service.Quote.Domain.Model.Numeric;

/// <summary>
/// Represents a specific year structure with a relative offset and an era indicator.
/// </summary>
public class Year
{
    /// <summary>
    /// Gets the unique identifier of the year record.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Gets the number of years relative to a designated median or baseline year.
    /// </summary>
    public int YearFromMedian { get; }

    /// <summary>
    /// Gets a value indicating whether the year is in the Anno Domini (A.D.) era.
    /// </summary>
    /// <remarks>
    /// Note: According to the current <see cref="ToString"/> implementation, 
    /// <see langword="true"/> appends "BC" and <see langword="false"/> appends "AD".
    /// </remarks>
    public bool IsAnnoDomini { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Year"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the year record.</param>
    /// <param name="yearFromMedian">The number of years relative to a designated median or baseline year.</param>
    /// <param name="isAnnoDomini">Indicates whether the year is in the Anno Domini era.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="yearFromMedian"/> is zero.</exception>
    public Year(int id, int yearFromMedian, bool isAnnoDomini)
    {
        if (yearFromMedian == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(yearFromMedian), "Year from median cannot be zero.");
        }

        Id = id;
        YearFromMedian = yearFromMedian;
        IsAnnoDomini = isAnnoDomini;
    }

    /// <summary>
    /// Returns a string representation of the current year, including its era suffix.
    /// </summary>
    /// <returns>A string combining the offset value and the era indicator ("BC" or "AD").</returns>
    public override string ToString()
    {
        var output = YearFromMedian.ToString();
        if (IsAnnoDomini) output += " BC";
        else output += " AD";
        return output;
    }
}