using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace Lab4.Pages
{
    public class IndexModel : PageModel
    {
        private readonly Random _random = new Random();

        public IndexModel(IOptionsMonitor<Game> game)
        {
            Game = game.CurrentValue;
        }

        public Game Game { get; set; }

        public List<int> OptimalStrategyIndices { get; set; }

        public bool Changed { get; set; }

        public int Strategy { get; set; }

        public int OpponentStrategy { get; set; }

        public List<double> State { get; set; }

        public void OnGet()
        {
            OptimalStrategyIndices = FindOptimalStrategies();
        }

        public void OnPost(int strategy)
        {
            Changed = true;

            Strategy = strategy;
            OpponentStrategy = _random.Next(Game.Columns.Count);
            State = Game.Matrix[Strategy][OpponentStrategy];
            OptimalStrategyIndices = FindOptimalStrategies();
        }

        private List<int> FindOptimalStrategies()
        {
            var minimums = Game.Matrix.Select((r, i) => new { Strategy = i, Min = r.Select(c => c.First()).Min() }).ToList();
            var max = minimums.Select(m => m.Min).Max();

            return minimums.Where(m => m.Min == max).Select(m => m.Strategy).ToList();
        }
    }
}
