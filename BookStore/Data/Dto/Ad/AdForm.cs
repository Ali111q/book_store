using black_follow.Entity;
using FluentValidation;

namespace BookStore.Data.Dto.Ad;

public class AdForm
{
    public Guid? RefId { get; set; }
    public AdType Type { get; set; }
    public string? AdLink { get; set; }
    public string? Image { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public int Priority { get; set; } = 2;
}

public class AdFormValidator : AbstractValidator<AdForm>
{
    public AdFormValidator()
    {
        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid AdType.");

        When(x => x.Type == AdType.RANDOM_ADD, () =>
        {
            RuleFor(x => x.AdLink)
                .NotEmpty().WithMessage("AdLink is required when Type is RANDOM_ADD.");
        });

        When(x => x.Type != AdType.RANDOM_ADD, () =>
        {
            RuleFor(x => x.RefId)
                .NotEmpty().WithMessage("RefId is required when Type is not RANDOM_ADD.");
        });

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

        RuleFor(x => x.Image)
            .NotEmpty()
          
            .WithMessage("Image must be not empty.");


     
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

      
        RuleFor(x => x.Priority)
            .InclusiveBetween(1, 10).WithMessage("Priority must be between 1 and 10.");
    }
}