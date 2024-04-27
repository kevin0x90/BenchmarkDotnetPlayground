using System.Runtime.CompilerServices;

namespace TrieImplementation
{
  public sealed class WknTrie
  {
    private readonly WknTrie?[] _childNodes = new WknTrie?[36];

    private readonly bool _isWordEnd;

    public WknTrie(bool isWordEnd)
    {
      _isWordEnd = isWordEnd;
    }

    public void AddWkn(WknTrie root, ReadOnlySpan<char> wkn)
    {
      WknTrie currentNode = root;

      unsafe
      {
        for (int i = 0, index, length = wkn.Length, lastIndex = length - 1; i < length; ++i)
        {
          index = GetChildNodeIndex(wkn[i]);

          if (currentNode._childNodes[index] is null)
          {
            currentNode._childNodes[index] = new WknTrie(i == lastIndex);
          }

          currentNode = currentNode._childNodes[index]!;
        }
      }
    }

    public bool Contains(WknTrie root, ReadOnlySpan<char> wkn)
    {
      WknTrie currentNode = root;

      unsafe
      {
        for (int i = 0, length = wkn.Length; i < length; ++i)
        {
          currentNode = currentNode._childNodes[GetChildNodeIndex(wkn[i])]!;

          if (currentNode is null)
          {
            return false;
          }
        }
      }

      return currentNode._isWordEnd;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe int GetChildNodeIndex(char currentChar)
    {
      int isDigit = BoolToInt(char.IsDigit(currentChar));

      return (currentChar - 'A') * isDigit + (currentChar - '0' + 25) * isDigit;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe int BoolToInt(bool b) => *(Byte*)&b;
  }
}
