
using FluentValidation;
using Estoque.Domain.Entities;

namespace Estoque.Service.Validators
{
    public class VendaValidator : AbstractValidator<Venda>
    {
        public VendaValidator()
        {
            RuleFor(c => c.Fornecedor)
                .NotEmpty().WithMessage("Por favor informe o cliente.")
                .NotNull().WithMessage("Por favor informe o cliente.");
            RuleFor(c => c.Usuario)
                .NotEmpty().WithMessage("Por favor informe o usuario.")
                .NotNull().WithMessage("Por favor informe o usuario.");
            RuleFor(c => c.Items)
                .NotEmpty().WithMessage("Por favor informe os produtos.")
                .NotNull().WithMessage("Por favor informe os produtos.");
        }
    }
}
