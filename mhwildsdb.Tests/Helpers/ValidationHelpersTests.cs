using FluentAssertions;
using mhwildsdb.Helpers;

namespace mhwildsdb.Tests.Helpers;

public class ValidationHelpersTests
{
    [Theory]
    [InlineData("Attack Boost")]        // letters and space
    [InlineData("D'Angelo")]            // apostrophe
    [InlineData("Doshaguma")]           // single word
    [InlineData("Conga Set α")]         // unicode letter
    public void BeValidName_WithValidInput_ShouldReturnTrue(string name)
    {
        ValidationHelpers.BeValidName(name).Should().BeTrue();
    }

    [Theory]
    [InlineData("Attack123")]           // contains numbers
    [InlineData("Conga-Set")]           // contains hyphen
    [InlineData("Helm!")]               // contains punctuation
    [InlineData("Attack_Boost")]        // contains underscore
    public void BeValidName_WithInvalidInput_ShouldReturnFalse(string name)
    {
        ValidationHelpers.BeValidName(name).Should().BeFalse();
    }

    [Fact]
    public void BeUnique_WithUniqueElements_ShouldReturnTrue()
    {
        var ids = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

        ValidationHelpers.BeUnique(ids, id => id).Should().BeTrue();
    }

    [Fact]
    public void BeUnique_WithDuplicateElements_ShouldReturnFalse()
    {
        var duplicate = Guid.NewGuid();
        var ids = new List<Guid> { duplicate, Guid.NewGuid(), duplicate };

        ValidationHelpers.BeUnique(ids, id => id).Should().BeFalse();
    }

    [Fact]
    public void BeUnique_WithSingleElement_ShouldReturnTrue()
    {
        var ids = new List<Guid> { Guid.NewGuid() };

        ValidationHelpers.BeUnique(ids, id => id).Should().BeTrue();
    }

    [Fact]
    public void BeUnique_WithDuplicatesBySelector_ShouldReturnFalse()
    {
        // duplicates detected via selector, not reference equality
        var items = new List<(int Level, string Name)>
        {
            (1, "Rank I"),
            (2, "Rank II"),
            (1, "Rank III")     // duplicate level
        };

        ValidationHelpers.BeUnique(items, x => x.Level).Should().BeFalse();
    }

    [Fact]
    public void BeSequential_WithSequentialLevels_ShouldReturnTrue()
    {
        var items = new List<(int Level, string Name)>
        {
            (1, "Rank I"),
            (2, "Rank II"),
            (3, "Rank III")
        };

        ValidationHelpers.BeSequential(items, x => x.Level).Should().BeTrue();
    }

    [Fact]
    public void BeSequential_WithOutOfOrderButSequentialLevels_ShouldReturnTrue()
    {
        // order in the collection shouldn't matter, only that 1..n are all present
        var items = new List<(int Level, string Name)>
        {
            (3, "Rank III"),
            (1, "Rank I"),
            (2, "Rank II")
        };

        ValidationHelpers.BeSequential(items, x => x.Level).Should().BeTrue();
    }

    [Fact]
    public void BeSequential_WithGapInLevels_ShouldReturnFalse()
    {
        var items = new List<(int Level, string Name)>
        {
            (1, "Rank I"),
            (3, "Rank III")     // skips level 2
        };

        ValidationHelpers.BeSequential(items, x => x.Level).Should().BeFalse();
    }

    [Fact]
    public void BeSequential_NotStartingAtOne_ShouldReturnFalse()
    {
        var items = new List<(int Level, string Name)>
        {
            (2, "Rank II"),
            (3, "Rank III")
        };

        ValidationHelpers.BeSequential(items, x => x.Level).Should().BeFalse();
    }

    [Fact]
    public void BeSequential_WithSingleElement_ShouldReturnTrue()
    {
        var items = new List<(int Level, string Name)> { (1, "Rank I") };

        ValidationHelpers.BeSequential(items, x => x.Level).Should().BeTrue();
    }
}
