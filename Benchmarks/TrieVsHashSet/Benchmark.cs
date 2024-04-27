using BenchmarkDotNet.Attributes;
using TrieImplementation;

namespace Benchmarks.TrieVsDictionary
{
  [MemoryDiagnoser(true)]
  public class Benchmark
  {
    private readonly HashSet<string> _hashSet = new();

    private readonly WknTrie _trie = new(false);

    [GlobalSetup]
    public void SetupTestData()
    {
      using var filestream = File.OpenRead("./TrieVsHashSet/Testdata/wkn.txt");
      using var streamReader = new StreamReader(filestream);

      while (!streamReader.EndOfStream)
      {
        string? wkn = streamReader.ReadLine();
        if (wkn != null)
        {
          _hashSet.Add(wkn);
          _trie.AddWkn(_trie, wkn);
        }
      }
    }

    [Benchmark]
    public void HashSetLookup()
    {
      _hashSet.Contains("A0MRNP");
      _hashSet.Contains("A2DYXV");
      _hashSet.Contains("755096");
      _hashSet.Contains("A0JJTJ");
      _hashSet.Contains("A0DKXK");
    }

    [Benchmark]
    public void TrieLookup()
    {
      _trie.Contains(_trie, "A0MRNP");
      _trie.Contains(_trie, "A2DYXV");
      _trie.Contains(_trie, "755096");
      _trie.Contains(_trie, "A0JJTJ");
      _trie.Contains(_trie, "A0DKXK");
    }
  }
}
