using Microsoft.AspNetCore.Http.HttpResults;

namespace SeatGuard.Api.Infrastructure;

public static class ApiProblems
{
    public static ProblemHttpResult NotFound(string code, string detail) =>
    Create(StatusCodes.Status404NotFound, "Not found", code, detail);

    public static ProblemHttpResult Conflict(string code, string detail) =>
    Create(StatusCodes.Status409Conflict, "Conflict", code, detail);

    public static ProblemHttpResult TooManyRequests(string code, string detail) =>
    Create(StatusCodes.Status429TooManyRequests, "Too many requests", code, detail);

    public static ProblemHttpResult BadRequest(string code, string detail) =>
    Create(StatusCodes.Status400BadRequest, "Bad request", code, detail);

    public static ProblemHttpResult Unauthorized(string code, string detail) =>
    Create(StatusCodes.Status401Unauthorized, "Unauthorized", code, detail);

    private static ProblemHttpResult Create(int status, string title, string code, string detail) =>
    TypedResults.Problem(
    statusCode: status,
    title: title,
    detail: detail,
    extensions: new Dictionary<string, object?> { ["code"] = code });
}
