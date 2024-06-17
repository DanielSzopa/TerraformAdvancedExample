using FluentAssertions;
using NewsletterSubscriberPublisher.Exceptions;
using NewsletterSubscriberPublisher.Models;

namespace NewsletterSubscriberPublisher.UnitTests;

public class EmailTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    [InlineData("testgmail.com")]
    [InlineData("test@gmail.gmail.com")]
    public void Create_WhenValueInputIsInvalid_ShouldThrowInvalidEmailException(string? email)
    {
        //act
        var result = () => { new Email(email); };

        //assert
        result.Should().Throw<InvalidEmailException>();
    }

    [Theory]
    [InlineData("test@gmail.com")]
    [InlineData("test123@gmail.com")]
    public void Create_WhenValueInputIsValid_ShouldNotThrowInvalidEmailException(string email)
    {
        //act
        var result = () => { new Email(email); };

        //assert
        result.Should().NotThrow<InvalidEmailException>();
    }
}
