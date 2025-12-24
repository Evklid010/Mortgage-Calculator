using System;
using System.Collections.Generic;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class MortgageService
    {
        private readonly List<MortgageCalculation> _calculations = new();

        public MortgageCalculation Calculate(MortgageRequest request)
        {
            decimal loan = request.PropertyPrice - request.DownPayment;
            int months = request.Years * 12;
            double rate = request.InterestRate / 100 / 12;

            decimal total = 0;
            decimal monthly;

            if (request.PaymentType == "diff")
            {
                // Дифференцированный
                decimal principalPart = loan / months;

                for (int i = 0; i < months; i++)
                {
                    decimal remaining = loan - principalPart * i;
                    decimal interest = remaining * (decimal)rate;
                    total += principalPart + interest;
                }

                monthly = principalPart + loan * (decimal)rate; // первый платёж
            }
            else
            {
                // Аннуитетный
                double k = (rate * Math.Pow(1 + rate, months)) /
                           (Math.Pow(1 + rate, months) - 1);

                monthly = loan * (decimal)k;
                total = monthly * months;
            }

            var result = new MortgageResult
            {
                MonthlyPayment = Math.Round(monthly, 2),
                TotalPayment = Math.Round(total, 2),
                Overpayment = Math.Round(total - loan, 2)
            };

            var calc = new MortgageCalculation
            {
                Request = request,
                Result = result
            };

            _calculations.Add(calc);
            return calc;
        }

        public IEnumerable<MortgageCalculation> GetAll() => _calculations;
    }
}