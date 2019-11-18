using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab1.DataAccess.Models;

namespace Lab1.Services
{
    public class DecisionService
    {
        public List<Alternative> Normalize(List<Alternative> alternatives)
        {
            if (!alternatives.Any())
            {
                return alternatives;
            }

            List<Guid> criteriaIds = alternatives.First().Criteria.Keys.ToList();

            foreach (Guid criterionId in criteriaIds)
            {
                List<double> values = alternatives.Select(a => a.Criteria[criterionId].Value).ToList();
                double max = values.Max();
                double min = values.Min();

                alternatives = alternatives.Select(a => a.NormalizeCriterion(criterionId, min, max)).ToList();
            }

            return alternatives;
        }

        public List<Alternative> GetParetoOptimal(List<Alternative> alternatives)
        {
            if (alternatives.Count < 2)
            {
                return alternatives;
            }

            var optimal = alternatives.ToDictionary(a => a.Id, _ => true);
            List<Guid> criteriaIds = alternatives.First().Criteria.Keys.ToList();

            foreach (Alternative alternative in alternatives)
            {
                if (!optimal[alternative.Id])
                {
                    continue;
                }

                foreach (Alternative other in alternatives.Where(a => a.Id != alternative.Id && optimal[a.Id]))
                {
                    bool isBetter = true;

                    foreach (Guid criterionId in criteriaIds)
                    {
                        var value = alternative.Criteria[criterionId].Value;
                        var otherValue = other.Criteria[criterionId].Value;

                        if (value < otherValue)
                        {
                            isBetter = false;
                            break;
                        }
                    }

                    if (isBetter)
                    {
                        optimal[other.Id] = false;
                    }
                }
            }

            return alternatives.Where(a => optimal[a.Id]).ToList();
        }

        public Alternative GetOptimalByLinearAdditiveConvolution(List<Alternative> alternatives)
        {
            List<Guid> criteriaIds = alternatives.First().Criteria.Keys.ToList();
            Dictionary<Guid, int> z = alternatives.ToDictionary(a => a.Id, _ => 0);

            foreach (Alternative alternative in alternatives)
            {
                foreach (Guid criterionId in criteriaIds)
                {
                    var value = alternative.Criteria[criterionId].Value;

                    bool isBest = alternatives.Select(a => a.Criteria[criterionId].Value).All(v => value >= v);

                    if (isBest)
                    {
                        z[alternative.Id]++;
                    }
                }
            }

            Guid optimalAlternativeId = z.OrderByDescending(p => p.Value).First().Key;

            return alternatives.Single(a => a.Id == optimalAlternativeId);
        }
    }

    public class Alternative
    {
        public Guid Id { get; set; }

        public Dictionary<Guid, Criterion> Criteria { get; set; }

        public Alternative NormalizeCriterion(Guid criterionId, double min, double max)
        {
            Criterion c = Criteria[criterionId];
            Criteria[criterionId] = c.Normalize(min, max);
            return this;
        }
    }

    public class Criterion
    {
        public Guid Id { get; set; }

        public double Value { get; set; }

        public Optimality Optimality { get; set; }

        public Criterion Normalize(double min, double max)
        {
            double divisor = max - min;
            double normalized = Math.Abs(divisor) < 1e-6
                ? (Math.Abs(Value) < 1e-6 ? 0 : 1)
                : (Value - min) / (max - min);

            if (Optimality == Optimality.Min)
            {
                normalized = 1 - normalized;
            }

            return new Criterion
            {
                Id = Id,
                Value = normalized,
                Optimality = Optimality
            };
        }
    }
}
