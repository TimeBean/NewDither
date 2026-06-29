using Dapper;
using Microsoft.Data.SqlClient;
using Service.Quote.Domain.Exception;
using Service.Quote.Domain.Model;
using Service.Quote.Domain.Model.Numeric;
using Service.Quote.Domain.Repository;

namespace Service.Quote.Infrastructure.Repository;

public class DapperQuoteRepository : IQuoteRepository
{
    private readonly string _connectionString;

    public DapperQuoteRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<Domain.Model.Quote> Get(int id)
    {
        await using var connection = new SqlConnection(_connectionString);
    
        const string sql = """
                           SELECT 
                               q.id AS Id,
                               q.content AS Content,
                               
                               a.id AS Id, 
                               a.first_name AS FirstName, 
                               a.second_name AS SecondName, 
                               a.middle_name AS MiddleName,
                               
                               y.id AS Id,
                               y.years_from_median AS YearFromMedian,
                               y.is_anno_domini AS IsAnnoDomini
                           FROM Quote q
                           INNER JOIN Author a ON q.author_id = a.id
                           INNER JOIN Year y ON q.year_id = y.id
                           WHERE q.id = @Id;
                           """;

        var quotes = await connection.QueryAsync<QuoteDto, Author, Year, Domain.Model.Quote>(
            sql,
            (quoteDto, author, year) => 
            {
                return new Domain.Model.Quote(author, quoteDto.Content, year);
            },
            new { Id = id },
            splitOn: "Id,Id"
        );

        var quote = quotes.FirstOrDefault();

        if (quote == null)
        {
            throw new QuoteNotFoundException(id);
        }
        
        return quote;
    }

    public async Task<int> Count()
    {
        await using var connection = new SqlConnection(_connectionString);
        const string sql = "SELECT COUNT(1) FROM Quote;";
        return await connection.ExecuteScalarAsync<int>(sql);
    }

    public async Task<IEnumerable<Domain.Model.Quote>> GetAllOfAuthor(int authorId)
    {
        await using var connection = new SqlConnection(_connectionString);

        const string sql = """
                           SELECT 
                               q.id AS Id, q.content AS Content,
                               a.id AS Id, a.first_name AS FirstName, a.second_name AS SecondName, a.middle_name AS MiddleName,
                               y.id AS Id, y.years_from_median AS YearFromMedian, y.is_anno_domini AS IsAnnoDomini
                           FROM Quote q
                           INNER JOIN Author a ON q.author_id = a.id
                           INNER JOIN Year   y ON q.year_id = y.id
                           WHERE q.author_id = @AuthorId;
                           """;

        return await connection.QueryAsync<QuoteDto, Author, Year, Domain.Model.Quote>(
            sql,
            (dto, author, year) => new Domain.Model.Quote(author, dto.Content, year),
            new { AuthorId = authorId },
            splitOn: "Id,Id"
        );
    }
}