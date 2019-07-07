namespace Smart.Data.Accessor.Attributes
{
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;

    using Smart.Data.Accessor.Loaders;
    using Smart.Data.Accessor.Nodes;
    using Smart.Data.Accessor.Parser;
    using Smart.Data.Accessor.Tokenizer;

    public abstract class LoaderMethodAttribute : MethodAttribute
    {
        protected LoaderMethodAttribute(MethodType methodType)
            : base(CommandType.Text, methodType)
        {
        }

        public override IReadOnlyList<INode> GetNodes(ISqlLoader loader, MethodInfo mi)
        {
            var sql = loader.Load(mi);
            var tokenizer = new SqlTokenizer(sql);
            var tokens = tokenizer.Tokenize();
            var parser = new NodeParser(tokens);
            return parser.Parse();
        }
    }
}
