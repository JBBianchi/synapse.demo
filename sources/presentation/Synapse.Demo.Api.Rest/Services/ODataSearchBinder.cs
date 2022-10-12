namespace Synapse.Demo.Api.Rest.Services;

/// <summary>
/// Represents the default <see cref="ISearchBinder"/> implementation
/// </summary>
public partial class ODataSearchBinder
    : QueryBinder, ISearchBinder
{

    private static readonly Dictionary<BinaryOperatorKind, ExpressionType> BinaryOperatorMapping = new Dictionary<BinaryOperatorKind, ExpressionType>
        {
            { BinaryOperatorKind.And, ExpressionType.AndAlso },
            { BinaryOperatorKind.Or, ExpressionType.OrElse },
        };

    private static readonly MethodInfo FilterDeviceMethod = typeof(ODataSearchBinder).GetMethod(nameof(FilterDevice), BindingFlags.Static | BindingFlags.NonPublic)!;

    /// <inheritdoc/>
    public Expression BindSearch(SearchClause searchClause, QueryBinderContext context)
    {
        return Expression.Lambda(this.BindSingleValueNode(searchClause.Expression, context), context.CurrentParameter);
    }

    /// <summary>
    /// Binds the specified <see cref="SingleValueCastNode"/>
    /// </summary>
    /// <param name="node">The <see cref="SingleValueCastNode"/> to bind</param>
    /// <param name="context">The current <see cref="QueryBinderContext"/></param>
    /// <returns>A new <see cref="Expression"/></returns>
    public override Expression BindSingleValueNode(SingleValueNode node, QueryBinderContext context)
    {
        return node switch
        {
            BinaryOperatorNode binaryOperatorNode => this.BindBinaryOperatorNode(binaryOperatorNode, context),
            SearchTermNode searchTermNode => this.BindSearchTerm(searchTermNode, context),
            UnaryOperatorNode unaryOperatorNode => this.BindUnaryOperatorNode(unaryOperatorNode, context),
            _ => throw new NotSupportedException($"The specified {nameof(SingleValueNode)} type '{node.GetType().Name}' is not supported")
        };
    }

    /// <summary>
    /// Binds the specified <see cref="BinaryOperatorNode"/>
    /// </summary>
    /// <param name="binaryOperatorNode">The <see cref="BinaryOperatorNode"/> to bind</param>
    /// <param name="context">The current <see cref="QueryBinderContext"/></param>
    /// <returns>A new <see cref="Expression"/></returns>
    public override Expression BindBinaryOperatorNode(BinaryOperatorNode binaryOperatorNode, QueryBinderContext context)
    {
        var left = this.Bind(binaryOperatorNode.Left, context);
        var right = this.Bind(binaryOperatorNode.Right, context);
        if (!BinaryOperatorMapping.TryGetValue(binaryOperatorNode.OperatorKind, out ExpressionType binaryExpressionType))
            throw new NotImplementedException($"Binary operator '{binaryOperatorNode.OperatorKind}' is not supported!");
        return Expression.MakeBinary(binaryExpressionType, left, right);
    }

    /// <summary>
    /// Binds the specified <see cref="SearchTermNode"/>
    /// </summary>
    /// <param name="term">The <see cref="SearchTermNode"/> to bind</param>
    /// <param name="context">The current <see cref="QueryBinderContext"/></param>
    /// <returns>A new <see cref="Expression"/></returns>
    public Expression BindSearchTerm(SearchTermNode term, QueryBinderContext context)
    {
        if (term == null)
            throw new ArgumentNullException(nameof(term));
        if (context.ElementClrType == typeof(Integration.Models.Device))
            return this.BindDeviceSearchTerm(term, context);
        else
            throw new NotSupportedException($"Search is not allowed on element type '{context.ElementClrType.Name}'");
    }

    /// <summary>
    /// Binds the specified <see cref="Integration.Models.Device"/> <see cref="SearchTermNode"/>
    /// </summary>
    /// <param name="searchTermNode">The <see cref="Integration.Models.Device"/> <see cref="SearchTermNode"/> to bind</param>
    /// <param name="context">The current <see cref="QueryBinderContext"/></param>
    /// <returns>A new <see cref="Expression"/></returns>
    protected virtual Expression BindDeviceSearchTerm(SearchTermNode searchTermNode, QueryBinderContext context)
    {
        var searchTerm = searchTermNode.Text.ToLowerInvariant();
        var searchQuery = Expression.IsTrue(Expression.Call(null, FilterDeviceMethod, context.CurrentParameter, Expression.Constant(searchTerm)));
        return searchQuery;
    }

    static bool FilterDevice(Integration.Models.Device device, string searchTerm)
    {
        return device.Label.ToLowerInvariant().Contains(searchTerm)
            || device.Type.ToLowerInvariant().Contains(searchTerm)
            || device.Location.ToString().ToLowerInvariant().Contains(searchTerm)
            || JsonConvert.SerializeObject(device.State).Contains(searchTerm)
            ;
    }

}