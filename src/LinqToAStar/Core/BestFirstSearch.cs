using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LinqToAStar.Core
{
    internal static class BestFirstSearch
    {
        #region Override

        public static IEnumerable<TFactor> Run<TFactor, TStep>(HeuristicSearchBase<TFactor, TStep> source)
        {
            var nexts = new List<Node<TFactor, TStep>>(source.ConvertToNodes(source.From, 0));

            if (nexts.Count == 0)
                return Enumerable.Empty<TFactor>();

            var visited = new HashSet<TStep>(source.StepComparer);
            var sortAt = 0;

            while (nexts.Count > 0)
            {
                var best = nexts[sortAt]; // nexts.First();
                var hasNext = false;

                if (source.StepComparer.Equals(best.Step, source.To))
                    return best.TraceBack();

                sortAt++; // nexts.RemoveAt(0);

                foreach (var next in source.Expands(best.Step, best.Level, visited.Add))
                {
                    Debug.WriteLine($"{best.Step}\t{best.Level} -> {next.Step}\t{next.Level}");

                    next.Previous = best;
                    nexts.Add(next);
                    hasNext = true;
                }
                if (hasNext)
                    nexts.Sort(sortAt, nexts.Count - sortAt, source.NodeComparer.FactorOnlyComparer);
            }
            return Enumerable.Empty<TFactor>();
        }

        #endregion
    }
}