using FluentValidation;

namespace Application.Modules.Users.SetAvatar;

public class SetAvatarCommandValidator
    : AbstractValidator<SetAvatarCommand>
{
    private const int MaxFileSize = 10 * 1024 * 1024;
    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png"];

    public SetAvatarCommandValidator()
    {
        RuleFor(x => x.Content)
            .Must(x => x.Length > 0)
            .WithMessage("File cannot be empty");

        RuleFor(x => x.Content.Length)
            .LessThanOrEqualTo(MaxFileSize)
            .WithMessage($"The file size must not exceed {MaxFileSize / (1024 * 1024)} MB.");

        RuleFor(file => Path.GetExtension(file.FileName).ToLower())
            .Must(ext => AllowedExtensions.Contains(ext))
            .WithMessage("Invalid file format. Only JPG and PNG are allowed.");
    }
}
