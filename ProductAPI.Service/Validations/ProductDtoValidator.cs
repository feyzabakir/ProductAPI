using FluentValidation;
using ProductAPI.Core.DTOs;

namespace ProductAPI.Service.Validations
{
    public class ProductDtoValidator : AbstractValidator<ProductDto>
    {
        public ProductDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ürün adı boş olamaz.")
                .MaximumLength(200).WithMessage("Ürün adı 200 karakterden uzun olamaz.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Ürün açıklaması 500 karakterden uzun olamaz.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Ürün fiyatı 0'dan büyük olmalıdır.");

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Stok miktarı negatif olamaz.");
        }
    }
}
