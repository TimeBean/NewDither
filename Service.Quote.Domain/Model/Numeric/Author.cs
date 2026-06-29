namespace Service.Quote.Domain.Model.Numeric;

/// <summary>
/// Represents an author of a quote.
/// </summary>
public class Author
{
    /// <summary>
    /// Gets the unique identifier of the author.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Gets the first name of the author.
    /// </summary>
    public string FirstName { get; }

    /// <summary>
    /// Gets the second name (last name or surname) of the author.
    /// </summary>
    public string SecondName { get; }

    /// <summary>
    /// Gets the optional middle name of the author.
    /// </summary>
    public string? MiddleName { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Author"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the author.</param>
    /// <param name="firstName">The first name of the author.</param>
    /// <param name="secondName">The second name (last name or surname) of the author.</param>
    /// <param name="middleName">The optional middle name of the author.</param>
    public Author(int id, string firstName, string secondName, string? middleName = null)
    {
        Id = id;
        FirstName = firstName;
        SecondName = secondName;
        MiddleName = middleName;
    }

    /// <summary>
    /// Returns the full name of the author as a string.
    /// </summary>
    /// <returns>
    /// A string representing the author's full name, including the middle name if it is provided.
    /// </returns>
    public override string ToString()
    {
        return string.IsNullOrWhiteSpace(MiddleName)
            ? $"{FirstName} {SecondName}"
            : $"{FirstName} {MiddleName} {SecondName}";
    }
}