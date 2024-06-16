using NewsletterSubscriberPublisher.Exceptions;
using System.Text.RegularExpressions;

namespace NewsletterSubscriberPublisher.Models;

internal record Email
{
    internal string Value { get; }

    internal Email(string value)
    {
        if (!isValidEmail(value))
        {
            throw new InvalidEmailException(value);
        }

        Value = value;
    }

    private bool isValidEmail(string email)
    {
        if(string.IsNullOrWhiteSpace(email))
            return false;

        var regex = new Regex(@"^[a-z0-9]+\.?[a-z0-9]+@[a-z]+\.[a-z]{2,3}$", RegexOptions.IgnoreCase);
        return regex.IsMatch(email);
    } 

    public static implicit operator Email(string email) => new Email(email);

    public static implicit operator string(Email email) => email.Value;
}
