namespace Smart.Data.Accessor.Generator.Metadata
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Smart.Data.Accessor.Attributes;
    using Smart.Data.Accessor.Generator;
    using Smart.Data.Accessor.Nodes;

    internal sealed class MethodMetadata
    {
        public int No { get; }

        public MethodInfo MethodInfo { get; }

        public CommandType CommandType { get; }

        public MethodType MethodType { get; }

        public bool ReturnValueAsResult { get; }

        public IReadOnlyList<INode> Nodes { get; }

        public IReadOnlyList<ParameterEntry> Parameters { get; }

        public IReadOnlyList<DynamicParameterEntry> DynamicParameters { get; }

        // Helper

        public bool IsAsync { get; }

        public Type EngineResultType { get; }

        // Method attribute

        public ProviderAttribute Provider { get; }

        public TimeoutAttribute Timeout { get; }

        public SqlSizeAttribute SqlSize { get; }

        // Parameter

        public ParameterInfo TimeoutParameter { get; }

        public ParameterInfo CancelParameter { get; }

        public ParameterInfo ConnectionParameter { get; }

        public ParameterInfo TransactionParameter { get; }

        // Helper

        public bool HasConnectionParameter => ConnectionParameter != null || TransactionParameter != null;

        public MethodMetadata(
            int no,
            MethodInfo mi,
            CommandType commandType,
            MethodType memberType,
            bool returnValueAsResult,
            IReadOnlyList<INode> nodes,
            IReadOnlyList<ParameterEntry> parameters,
            IReadOnlyList<DynamicParameterEntry> dynamicParameters)
        {
            No = no;
            MethodInfo = mi;
            CommandType = commandType;
            MethodType = memberType;
            ReturnValueAsResult = returnValueAsResult;
            Nodes = nodes;
            Parameters = parameters;
            DynamicParameters = dynamicParameters;

            IsAsync = mi.ReturnType.GetMethod(nameof(Task.GetAwaiter)) != null;
            EngineResultType = IsAsync
                ? (mi.ReturnType.IsGenericType
                    ? mi.ReturnType.GetGenericArguments()[0]
                    : typeof(void))
                : mi.ReturnType;

            Provider = mi.GetCustomAttribute<ProviderAttribute>();
            Timeout = mi.GetCustomAttribute<TimeoutAttribute>();
            SqlSize = mi.GetCustomAttribute<SqlSizeAttribute>();

            foreach (var pmi in mi.GetParameters())
            {
                if (ParameterHelper.IsTimeoutParameter(pmi))
                {
                    TimeoutParameter = pmi;
                }

                if (ParameterHelper.IsCancellationTokenParameter(pmi))
                {
                    CancelParameter = pmi;
                }

                if (ParameterHelper.IsConnectionParameter(pmi))
                {
                    ConnectionParameter = pmi;
                }

                if (ParameterHelper.IsTransactionParameter(pmi))
                {
                    TransactionParameter = pmi;
                }
            }
        }

        public ParameterEntry FindParameterByName(string name)
        {
            return Parameters.FirstOrDefault(x => x.Name == name);
        }

        public DynamicParameterEntry FindDynamicParameterByName(string name)
        {
            return DynamicParameters.FirstOrDefault(x => x.Name == name);
        }
    }
}
