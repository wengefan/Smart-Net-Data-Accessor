namespace Smart.Data.Accessor.Attributes;

using System.Data;
using System.Reflection;

using Smart.Data.Accessor.Builders.Helpers;
using Smart.Data.Accessor.Generator;
using Smart.Data.Accessor.Nodes;

public sealed class ProcedureAttribute : MethodAttribute, IReturnValueBehavior
{
    private readonly string procedure;

    public bool ReturnValueAsResult { get; }

    public ProcedureAttribute(string procedure)
        : this(procedure, MethodType.Execute)
    {
    }

    public ProcedureAttribute(string procedure, MethodType methodType)
        : this(procedure, methodType, true)
    {
    }

    public ProcedureAttribute(string procedure, bool returnValueAsResult)
        : this(procedure, MethodType.Execute, returnValueAsResult)
    {
    }

    private ProcedureAttribute(string procedure, MethodType methodType, bool returnValueAsResult)
        : base(CommandType.StoredProcedure, methodType)
    {
        this.procedure = procedure;
        ReturnValueAsResult = returnValueAsResult;
    }

    public override IReadOnlyList<INode> GetNodes(ISqlLoader loader, MethodInfo mi)
    {
        var nodes = new List<INode>
        {
            new SqlNode(procedure)
        };

        nodes.AddRange(BuildHelper.GetParameters(mi).Select(x => new ParameterNode(x.Name, x.ParameterName)));

        return nodes;
    }
}
