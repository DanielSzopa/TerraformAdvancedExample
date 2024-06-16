namespace NewsletterSubscriberPublisher.Exceptions;

internal class InvalidEmailException : Exception
{
    internal InvalidEmailException(string invalidEmail) : base($"Email: {invalidEmail} is not correct!!!!") { }
}
