namespace TrieImplementation
{
  [TestClass]
  public class TrieTest
  {
    [TestMethod]
    public void TrieSearch()
    {
      // Arrange
      var trie = new WknTrie(false);
      trie.AddWkn(trie, "A2DYXV");
      trie.AddWkn(trie, "904318");
      trie.AddWkn(trie, "A0RFDZ");

      // Act
      bool resultFound = trie.Contains(trie, "904318");
      bool resultNotFound = trie.Contains(trie, "2435");

      // Assert
      Assert.IsTrue(resultFound);
      Assert.IsFalse(resultNotFound);
    }
  }
}