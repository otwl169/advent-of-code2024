using System.Text.RegularExpressions;
using Common;

namespace aoc2024.Days;

public partial class Day05 : IDay
{
    [GeneratedRegex(@"(\d+)\|(\d+)")]
    private static partial Regex OrderingRuleRegex();
    private static readonly Regex PageOrderingRegex = OrderingRuleRegex();
    
    [GeneratedRegex(@"\d+(,\d+)+")]
    private static partial Regex PageEntriesRegex();
    private static readonly Regex PageEntryRegex = PageEntriesRegex();

    private Dictionary<int, HashSet<int>> pagesThatCantGoBefore = [];
    private List<List<int>> pageUpdates = [];

    public Tuple<string, string> Solve()
    {
        var text = File.ReadAllText("Inputs\\Day05.txt");

        // parse page orders
        foreach (Match match in PageOrderingRegex.Matches(text))
        {
            var firstPage = int.Parse(match.Groups[1].Value);
            var secondPage = int.Parse(match.Groups[2].Value);
            if (pagesThatCantGoBefore.TryGetValue(firstPage, out var value))
            {
                value.Add(secondPage);
            }
            else
            {
                pagesThatCantGoBefore[firstPage] = [secondPage];
            }
        }

        // parse page updates
        foreach (Match match in PageEntryRegex.Matches(text))
        {
            pageUpdates.Add(match.Value.Split(',').Select(int.Parse).ToList());
        }

        return new Tuple<string, string>(Part1(), Part2());
    }

    private string Part1()
    {
        var total = 0;

        foreach (var entry in pageUpdates)
        {
            if (IsValidEntry(entry)) total += entry[entry.Count / 2];
        }

        return total.ToString();
    }
    
    private string Part2()
    {
        var total = 0;

        foreach (var entry in pageUpdates.Where(entry => !IsValidEntry(entry)))
        {
            var ordered = new List<int>();
            var contents = new HashSet<int>(entry);
            var requirements = new Dictionary<int, HashSet<int>>();

            foreach (var update in entry)
            {
                var relevantRules = new HashSet<int>(pagesThatCantGoBefore[update].Where(x => contents.Contains(x)));
                requirements.Add(update, relevantRules);
            }
            
            // there must be a page which has no requirements otherwise there will be a loop
            while (ordered.Count < entry.Count)
            {
                // select a page with no requirements and add it to the list
                var page = requirements.First(kv => kv.Value.Count == 0);
                ordered.Add(page.Key);
                requirements.Remove(page.Key);

                foreach (var update in requirements)
                {
                    update.Value.Remove(page.Key);
                }
            }

            total += ordered[ordered.Count / 2];
        }

        return total.ToString();
    }

    private bool IsValidEntry(IReadOnlyList<int> pageUpdates)
    {
        var before = new HashSet<int>();

        foreach (var update in pageUpdates)
        {
            if (pagesThatCantGoBefore.ContainsKey(update) && before.Intersect(pagesThatCantGoBefore[update]).Any()) return false;
            before.Add(update);
        }

        return true;
    }
}