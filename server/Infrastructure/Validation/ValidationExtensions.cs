using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using VitalityBuilder.Api.Models.DTOs;
using VitalityBuilder.Api.Models.Validations;

namespace VitalityBuilder.Api.Infrastructure;

/// <summary>
/// Extension methods for registering validation services
/// </summary>
public static class ValidationExtensions
{
    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        // Register core character validators
        services.AddScoped<IValidator<CreateCharacterDto>, CreateCharacterDtoValidator>();
        services.AddScoped<IValidator<CombatAttributesDto>, CombatAttributesDtoValidator>();
        services.AddScoped<IValidator<UtilityAttributesDto>, UtilityAttributesDtoValidator>();

        // Register archetype validators
        services.AddScoped<IValidator<CharacterArchetypesDto>, CharacterArchetypesDtoValidator>();
        services.AddScoped<IValidator<MovementArchetypeDto>, MovementArchetypeDtoValidator>();
        services.AddScoped<IValidator<AttackTypeArchetypeDto>, AttackTypeArchetypeDtoValidator>();
        services.AddScoped<IValidator<EffectTypeArchetypeDto>, EffectTypeArchetypeDtoValidator>();
        services.AddScoped<IValidator<UniqueAbilityArchetypeDto>, UniqueAbilityArchetypeDtoValidator>();
        services.AddScoped<IValidator<SpecialAttackArchetypeDto>, SpecialAttackArchetypeDtoValidator>();
        services.AddScoped<IValidator<UtilityArchetypeDto>, UtilityArchetypeDtoValidator>();

        // Register FluentValidation
        services.AddFluentValidationAutoValidation();

        return services;
    }
}