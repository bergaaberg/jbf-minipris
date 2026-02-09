using Minipris.Bilforsikring.Models;
using Minipris.Bilforsikring.Requests;

namespace Minipris.Bilforsikring;

public class CarService(CarInfoRepository carInfoRepository)
{
    public Task<CarInsuranceQuote> GetQuote(PriceRequest request)
    {
        var basePrice = request.RegNumber is not null
            ? carInfoRepository.GetBasePrice(request.RegNumber) ?? GenerateBasePrice()
            : GenerateBasePrice();

        return Task.FromResult(BuildQuote(request, basePrice));
    }

    public Task<CarInsuranceQuote> GetEstimate(PriceRequest request)
    {
        var basePrice = GenerateBasePrice();
        return Task.FromResult(BuildQuote(request, basePrice));
    }

    private static CarInsuranceQuote BuildQuote(PriceRequest request, int basePrice)
    {
        var price = CalculatePremium(basePrice, request.Bonus);

        return new CarInsuranceQuote
        {
            RegNumber = request.RegNumber is not null
                ? CarInfoRepository.FormatRegNumber(request.RegNumber)
                : "ESTIMAT",
            Make = request.Make,
            Model = request.Model,
            Year = request.Year,
            InsurancePrice = price,
            Coverage = "Kasko",
            Bonus = request.Bonus,
            Deductible = 4000,
            CoverageOptions = GenerateCoverageOptions(basePrice, request.Bonus)
        };
    }

    private static int GenerateBasePrice()
    {
        return 1500 + Random.Shared.Next(3000);
    }

    private static int CalculatePremium(int basePrice, int bonus = 0)
    {
        var multiplier = 1.0m - bonus / 100.0m;
        var price = basePrice * multiplier;
        return (int)price;
    }

    private static List<CoverageOption> GenerateCoverageOptions(int basePrice, int bonus)
    {
        var kaskoPrice = CalculatePremium(basePrice, bonus);

        return
        [
            new()
            {
                Name = "Ansvar",
                Price = (int)(kaskoPrice * 0.6),
                Description = "Dekker skade på andres kjøretøy, eiendom og personer."
            },
            new()
            {
                Name = "Delkasko",
                Price = (int)(kaskoPrice * 0.8),
                Description = "Inkluderer ansvar, pluss tyveri, brann, og glasskade."
            },
            new()
            {
                Name = "Kasko",
                Price = kaskoPrice,
                Description = "Full dekning inkludert skade på eget kjøretøy ved kollisjon/utforkjøring."
            }
        ];
    }
}