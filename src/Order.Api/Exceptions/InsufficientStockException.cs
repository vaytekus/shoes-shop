namespace Order.Api.Exceptions;

public class InsufficientStockException(string message) : Exception(message);
