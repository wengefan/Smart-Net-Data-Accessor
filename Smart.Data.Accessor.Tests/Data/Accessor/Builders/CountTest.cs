namespace Smart.Data.Accessor.Builders;

using Smart.Data.Accessor.Attributes;
using Smart.Mock;
using Xunit;

public class CountTest
{
    //--------------------------------------------------------------------------------
    // Argument
    //--------------------------------------------------------------------------------

    [DataAccessor]
    public interface ICountByArgumentAccessor
    {
        [Count(typeof(MultiKeyEntity))]
        long Count(long key1, [Condition(Operand.GreaterEqualThan)] long key2);
    }

    [Fact]
    public void TestCountByArgument()
    {
        using (TestDatabase.Initialize()
            .SetupMultiKeyTable()
            .InsertMultiKey(new MultiKeyEntity { Key1 = 1, Key2 = 1, Type = "A", Name = "Data-1" })
            .InsertMultiKey(new MultiKeyEntity { Key1 = 1, Key2 = 2, Type = "B", Name = "Data-2" })
            .InsertMultiKey(new MultiKeyEntity { Key1 = 1, Key2 = 3, Type = "A", Name = "Data-3" }))
        {
            var generator = new TestFactoryBuilder()
                .UseFileDatabase()
                .Build();
            var accessor = generator.Create<ICountByArgumentAccessor>();

            var count = accessor.Count(1L, 2L);

            Assert.Equal(2, count);
        }
    }

    //--------------------------------------------------------------------------------
    // Parameter
    //--------------------------------------------------------------------------------

    public class Parameter
    {
        public long Key1 { get; set; }

        [Condition(Operand.GreaterEqualThan)]
        public long Key2 { get; set; }
    }

    [DataAccessor]
    public interface ICountByParameterAccessor
    {
        [Count("MultiKey")]
        long Count(Parameter parameter);
    }

    [Fact]
    public void TestCountByParameter()
    {
        using (TestDatabase.Initialize()
            .SetupMultiKeyTable()
            .InsertMultiKey(new MultiKeyEntity { Key1 = 1, Key2 = 1, Type = "A", Name = "Data-1" })
            .InsertMultiKey(new MultiKeyEntity { Key1 = 1, Key2 = 2, Type = "B", Name = "Data-2" })
            .InsertMultiKey(new MultiKeyEntity { Key1 = 1, Key2 = 3, Type = "A", Name = "Data-3" }))
        {
            var generator = new TestFactoryBuilder()
                .UseFileDatabase()
                .Build();
            var accessor = generator.Create<ICountByParameterAccessor>();

            var count = accessor.Count(new Parameter { Key1 = 1L, Key2 = 2L });

            Assert.Equal(2, count);
        }
    }

    //--------------------------------------------------------------------------------
    // Invalid
    //--------------------------------------------------------------------------------

    [DataAccessor]
    public interface ICountInvalidAccessor
    {
        [Count("")]
        long Count();
    }

    [Fact]
    public void TestCountInvalid()
    {
        var generator = new TestFactoryBuilder()
            .UseFileDatabase()
            .Build();

        Assert.Throws<BuilderException>(() => generator.Create<ICountInvalidAccessor>());
    }
}
