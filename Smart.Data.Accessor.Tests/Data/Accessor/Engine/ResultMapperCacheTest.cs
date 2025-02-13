namespace Smart.Data.Accessor.Engine;

using Smart.Mock.Data;

using Xunit;

public class ResultMapperCacheTest
{
    [Fact]
    public void TestResultMapperCache()
    {
        var engine = new ExecuteEngineConfig().ToEngine();

        var columns = new[]
        {
            new MockColumn(typeof(long), "Id"),
            new MockColumn(typeof(string), "Name")
        };

        var cmd = new MockDbCommand();
        cmd.SetupResult(new MockDataReader(columns, new List<object[]>()));
        cmd.SetupResult(new MockDataReader(columns, new List<object[]>()));

        var info = new QueryInfo<CacheEntity>(engine, GetType().GetMethod(nameof(TestResultMapperCache))!, false);

        Assert.Equal(0, info.MapperCount);

        engine.QueryBuffer(info, cmd);

        Assert.Equal(1, info.MapperCount);

        engine.QueryBuffer(info, cmd);

        Assert.Equal(1, info.MapperCount);
    }

    [Fact]
    public void TestResultMapperCacheOptimized()
    {
        var engine = new ExecuteEngineConfig().ToEngine();

        var columns = new[]
        {
            new MockColumn(typeof(long), "Id"),
            new MockColumn(typeof(string), "Name")
        };

        var cmd = new MockDbCommand();
        cmd.SetupResult(new MockDataReader(columns, new List<object[]>()));
        cmd.SetupResult(new MockDataReader(columns, new List<object[]>()));

        var info = new QueryInfo<CacheEntity>(engine, GetType().GetMethod(nameof(TestResultMapperCacheOptimized))!, true);

        Assert.Equal(0, info.MapperCount);

        engine.QueryBuffer(info, cmd);

        Assert.Equal(1, info.MapperCount);

        engine.QueryBuffer(info, cmd);

        Assert.Equal(1, info.MapperCount);
    }

    [Fact]
    public void TestResultMapperCacheForSameTypeDifferentResult()
    {
        var engine = new ExecuteEngineConfig().ToEngine();

        var columns1 = new[]
        {
            new MockColumn(typeof(long), "Id")
        };
        var columns2 = new[]
        {
            new MockColumn(typeof(string), "Name")
        };
        var columns3 = new[]
        {
            new MockColumn(typeof(long), "Id"),
            new MockColumn(typeof(string), "Name")
        };
        var columns4 = new[]
        {
            new MockColumn(typeof(long), "Id2"),
            new MockColumn(typeof(string), "Name")
        };

        var cmd = new MockDbCommand();
        cmd.SetupResult(new MockDataReader(columns1, new List<object[]>()));
        cmd.SetupResult(new MockDataReader(columns2, new List<object[]>()));
        cmd.SetupResult(new MockDataReader(columns3, new List<object[]>()));
        cmd.SetupResult(new MockDataReader(columns4, new List<object[]>()));

        var info = new QueryInfo<CacheEntity>(engine, GetType().GetMethod(nameof(TestResultMapperCacheForSameTypeDifferentResult))!, false);

        Assert.Equal(0, info.MapperCount);

        engine.QueryBuffer(info, cmd);

        Assert.Equal(1, info.MapperCount);

        engine.QueryBuffer(info, cmd);

        Assert.Equal(2, info.MapperCount);

        engine.QueryBuffer(info, cmd);

        Assert.Equal(3, info.MapperCount);

        engine.QueryBuffer(info, cmd);

        Assert.Equal(4, info.MapperCount);
    }

    public class CacheEntity
    {
        public long Id { get; set; }

        public string Name { get; set; } = default!;
    }
}
